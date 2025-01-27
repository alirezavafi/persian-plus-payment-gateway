// Copyright (c) Persian.Plus.PaymentGateway.Core. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC License, Version 3.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Persian.Plus.PaymentGateway.Core;
using Persian.Plus.PaymentGateway.Core.Gateway;
using Persian.Plus.PaymentGateway.Core.Internal;
using Persian.Plus.PaymentGateway.Core.Net;
using Persian.Plus.PaymentGateway.Core.Options;
using Persian.Plus.PaymentGateway.Core.Storage.Abstractions.Models;
using Persian.Plus.PaymentGateway.Gateways.Saman.Internal;
using Persian.Plus.PaymentGateway.Gateways.Saman.Internal.Models;

namespace Persian.Plus.PaymentGateway.Gateways.Saman
{
    [Gateway(Name)]
    public class SamanGateway : GatewayBase<SamanGatewayAccount>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;
        private readonly SamanGatewayOptions _gatewayOptions;
        private readonly MessagesOptions _messageOptions;

        public const string Name = "Saman";

        public SamanGateway(
            IHttpContextAccessor httpContextAccessor,
            IHttpClientFactory httpClientFactory,
            IGatewayAccountProvider<SamanGatewayAccount> accountProvider,
            IOptions<SamanGatewayOptions> gatewayOptions,
            IOptions<MessagesOptions> messageOptions) : base(accountProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClientFactory.CreateClient(this);
            _gatewayOptions = gatewayOptions.Value;
            _messageOptions = messageOptions.Value;
        }

        /// <inheritdoc />
        public override async Task<PaymentRequestResult> RequestAsync(Invoice invoice, CancellationToken cancellationToken = default)
        {
            if (invoice == null) throw new ArgumentNullException(nameof(invoice));

            var account = await GetAccountAsync(invoice).ConfigureAwaitFalse();

            var httpContext = _httpContextAccessor.HttpContext;

            var result = await SamanHelper.CreateRequest(
                invoice,
                httpContext,
                account,
                _httpClient,
                _gatewayOptions,
                _messageOptions,
                cancellationToken);

            return result;
        }

        /// <inheritdoc />
        public override async Task<PaymentFetchResult> FetchAsync(InvoiceContext context, CancellationToken cancellationToken = default)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var callbackResult = await GetCallbackResult(context, cancellationToken);

            if (callbackResult.IsSucceed)
            {
                return PaymentFetchResult.ReadyForVerifying(callbackResult);
            }

            return PaymentFetchResult.Failed(callbackResult, callbackResult.Message);
        }

        private async Task<SamanCallbackResult> GetCallbackResult(InvoiceContext context, CancellationToken cancellationToken)
        {
            var callBackTransaction = context.Transactions.SingleOrDefault(x => x.Type == TransactionType.Callback);

            SamanCallbackResult callbackResult;
            if (callBackTransaction == null)
            {
                callbackResult = await SamanHelper.CreateCallbackResultAsync(
                        _httpContextAccessor.HttpContext.Request,
                        _messageOptions,
                        cancellationToken)
                    .ConfigureAwaitFalse();
            }
            else
            {
                callbackResult =
                    JsonConvert.DeserializeObject<SamanCallbackResult>(callBackTransaction.AdditionalData);
            }

            return callbackResult;
        }


        /// <inheritdoc />
        public override async Task<PaymentVerifyResult> VerifyAsync(InvoiceContext context, CancellationToken cancellationToken = default)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var callbackResult = await GetCallbackResult(context, cancellationToken);

            if (!callbackResult.IsSucceed)
            {
                return PaymentVerifyResult.Failed(callbackResult.Message);
            }

            var account = await GetAccountAsync(context.Payment).ConfigureAwaitFalse();

            var resp = await _httpClient.PostJsonAsync(_gatewayOptions.VerificationUrl, new
            {
                RefNum = callbackResult.TransactionId,
                TerminalNumber = account.MerchantId
            }, cancellationToken: cancellationToken);
            resp.EnsureSuccessStatusCode();
            var result = await resp.Content.ReadFromJsonAsync<SamanVerifyTransactionResult>(cancellationToken: cancellationToken);

            return SamanHelper.CreateVerifyResult(result, context, callbackResult, _messageOptions);
        }

        /// <inheritdoc />
        public override async Task<PaymentRefundResult> RefundAsync(InvoiceContext context, Money amount, CancellationToken cancellationToken = default)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var account = await GetAccountAsync(context.Payment).ConfigureAwaitFalse();

            var data = SamanHelper.CreateRefundData(context, amount, account);

            var responseMessage = await _httpClient
                .PostXmlAsync(_gatewayOptions.VerificationUrl, data, cancellationToken)
                .ConfigureAwaitFalse();

            var response = await responseMessage.Content.ReadAsStringAsync().ConfigureAwaitFalse();

            return SamanHelper.CreateRefundResult(response, _messageOptions);
        }
    }
}
