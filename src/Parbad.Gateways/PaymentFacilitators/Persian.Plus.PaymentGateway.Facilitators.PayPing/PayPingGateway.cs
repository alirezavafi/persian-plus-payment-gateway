﻿// Copyright (c) Persian.Plus.PaymentGateway.Core. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC License, Version 3.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
using Persian.Plus.PaymentGateway.Facilitators.PayPing.Internal;

namespace Persian.Plus.PaymentGateway.Facilitators.PayPing
{
    /// <summary>
    /// PayPing Gateway.
    /// </summary>
    [Gateway(Name)]
    public class PayPingGateway : GatewayBase<PayPingGatewayAccount>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;
        private readonly PayPingGatewayOptions _pingGatewayOptions;
        private readonly ParbadOptions _options;

        public const string Name = "PayPing";

        /// <summary>
        /// Initializes an instance of <see cref="PayPingGateway"/>.
        /// </summary>
        public PayPingGateway(
            IGatewayAccountProvider<PayPingGatewayAccount> accountProvider,
            IHttpContextAccessor httpContextAccessor,
            IHttpClientFactory httpClientFactory,
            IOptions<PayPingGatewayOptions> gatewayOptions,
            IOptions<ParbadOptions> options) : base(accountProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClientFactory.CreateClient(this);
            _pingGatewayOptions = gatewayOptions.Value;
            _options = options.Value;
        }

        /// <inheritdoc />
        public override async Task<PaymentRequestResult> RequestAsync(Invoice invoice, CancellationToken cancellationToken = default)
        {
            if (invoice == null) throw new ArgumentNullException(nameof(invoice));

            var account = await GetAccountAsync(invoice).ConfigureAwaitFalse();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", account.AccessToken);

            var body = new CreatePayRequestModel
            {
                Amount = invoice.Amount,
                Description = invoice.GetPayPingRequest()?.Description,
                PayerIdentity = invoice.GetPayPingRequest()?.Mobile,
                PayerName = invoice.GetPayPingRequest()?.PayerName,
                ReturnUrl = invoice.CallbackUrl,
                ClientRefId = invoice.TrackingNumber.ToString()
            };

            //Send Create pay Request
            var response = await _httpClient.PostJsonAsync(_pingGatewayOptions.ApiRequestUrl, body, cancellationToken);

            //Check if we ran into an Issue
            response.EnsureSuccessStatusCode();

            //Get Response data 
            var responseBody = await response.Content.ReadAsStringAsync();

            //Convert Response data to Model and get PayCode
            var createPayResult = JsonConvert.DeserializeObject<CreatePayResponseModel>(responseBody);

            //Redirect User to gateway with the Code

            var url = _pingGatewayOptions.PaymentPageUrl.ToggleStringAtEnd("/", shouldHave: true) + createPayResult.Code;

            return PaymentRequestResult.SucceedWithRedirect(account.Name, url, createPayResult);
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

        private async Task<PayPingCallbackResult> GetCallbackResult(InvoiceContext context, CancellationToken cancellationToken)
        {
            var callBackTransaction = context.Transactions.SingleOrDefault(x => x.Type == TransactionType.Callback);

            PayPingCallbackResult callbackResult;
            if (callBackTransaction == null)
            {
                callbackResult = await PayPingGatewayHelper.GetCallbackResult(
                    _httpContextAccessor.HttpContext.Request,
                    context,
                    _options.Messages,
                    cancellationToken);
            }
            else
            {
                callbackResult =
                    JsonConvert.DeserializeObject<PayPingCallbackResult>(callBackTransaction.AdditionalData);
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

            var verificationModel = PayPingGatewayHelper.CreateVerificationModel(context, callbackResult);

            var account = await GetAccountAsync(context.Payment).ConfigureAwaitFalse();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", account.AccessToken);

            //Send Verify pay Request
            var response = await _httpClient.PostJsonAsync(_pingGatewayOptions.ApiVerificationUrl, verificationModel, cancellationToken);

            //Check if we ran into an Issue
            if (!response.IsSuccessStatusCode)
            {
                return PaymentVerifyResult.Failed(_options.Messages.PaymentFailed);
            }

            //Get Response data 
            var responseBody = await response.Content.ReadAsStringAsync();

            var responseModel = JsonConvert.DeserializeObject<VerifyPayResponseModel>(responseBody);

            if (responseModel.Amount != (long)context.Payment.Amount)
            {
                var message = $"{_options.Messages.PaymentFailed} Amount is not valid.";

                return PaymentVerifyResult.Failed(message);
            }

            return PaymentVerifyResult.Succeed(callbackResult.RefId, _options.Messages.PaymentSucceed);
        }

        /// <inheritdoc />
        public override Task<PaymentRefundResult> RefundAsync(InvoiceContext context, Money amount, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(PaymentRefundResult.Failed("The Refund operation is not supported by this gateway."));
        }
    }
}
