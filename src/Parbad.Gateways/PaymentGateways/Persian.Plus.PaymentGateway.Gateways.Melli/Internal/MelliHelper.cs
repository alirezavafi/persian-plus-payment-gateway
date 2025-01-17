// Copyright (c) Persian.Plus.PaymentGateway.Core. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC License, Version 3.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Persian.Plus.PaymentGateway.Core;
using Persian.Plus.PaymentGateway.Core.Gateway;
using Persian.Plus.PaymentGateway.Core.Http;
using Persian.Plus.PaymentGateway.Core.Internal;
using Persian.Plus.PaymentGateway.Core.Options;
using Persian.Plus.PaymentGateway.Gateways.Melli.Internal.Models;
using Persian.Plus.PaymentGateway.Gateways.Melli.Internal.ResultTranslator;

namespace Persian.Plus.PaymentGateway.Gateways.Melli.Internal
{
    internal static class MelliHelper
    {
        private const int SuccessCode = 0;
        private const int DuplicateTrackingNumberCode = 1011;

        public static object CreateRequestData(Invoice invoice, MelliGatewayAccount account, IMelliGatewayCrypto crypto)
        {
            var data = $"{account.TerminalId};{invoice.TrackingNumber};{(long)invoice.Amount}";

            var signedData = crypto.Encrypt(account.TerminalKey, data);

            return CreateRequestObject(
                account.TerminalId,
                account.MerchantId,
                invoice.Amount,
                signedData,
                invoice.CallbackUrl,
                invoice.TrackingNumber);
        }

        public static PaymentRequestResult CreateRequestResult(
            MelliApiRequestResult result,
            HttpContext httpContext,
            MelliGatewayAccount account,
            MelliGatewayOptions gatewayOptions,
            MessagesOptions messagesOptions)
        {
            if (result == null)
            {
                return PaymentRequestResult.Failed(messagesOptions.UnexpectedErrorText);
            }

            var isSucceed = result.ResCode == SuccessCode;

            if (!isSucceed)
            {
                string message;

                if (result.ResCode == DuplicateTrackingNumberCode)
                {
                    message = messagesOptions.DuplicateTrackingNumber;
                }
                else
                {
                    message = !result.Description.IsNullOrEmpty()
                        ? result.Description
                        : MelliRequestResultTranslator.Translate(result.ResCode, messagesOptions);
                }

                return PaymentRequestResult.Failed(message, account.Name);
            }

            var paymentPageUrl = $"{gatewayOptions.PaymentPageUrl}/Index?token={result.Token}";

            return PaymentRequestResult.SucceedWithRedirect(account.Name, paymentPageUrl, result);
        }

        public static async Task<MelliCallbackResult> CreateCallbackResultAsync(
            InvoiceContext context,
            HttpRequest httpRequest,
            MelliGatewayAccount account,
            IMelliGatewayCrypto crypto,
            MessagesOptions messagesOptions,
            CancellationToken cancellationToken)
        {
            var apiResponseCode = await httpRequest.TryGetParamAsAsync<int>("ResCode", cancellationToken).ConfigureAwaitFalse();

            if (!apiResponseCode.Exists || apiResponseCode.Value != SuccessCode)
            {
                return new MelliCallbackResult
                {
                    IsSucceed = false,
                    Message = messagesOptions.PaymentFailed
                };
            }

            var apiToken = await httpRequest.TryGetParamAsync("Token", cancellationToken).ConfigureAwaitFalse();
            var apiOrderId = await httpRequest.TryGetParamAsAsync<long>("OrderId", cancellationToken).ConfigureAwaitFalse();

            if (!apiOrderId.Exists || apiOrderId.Value != context.Payment.TrackingNumber)
            {
                return new MelliCallbackResult
                {
                    IsSucceed = false,
                    Token = apiToken.Value,
                    Message = messagesOptions.InvalidDataReceivedFromGateway
                };
            }

            var signedData = crypto.Encrypt(account.TerminalKey, apiToken.Value);

            var dataToVerify = CreateVerifyObject(apiToken.Value, signedData);

            return new MelliCallbackResult
            {
                IsSucceed = true,
                Token = apiToken.Value,
                JsonDataToVerify = dataToVerify
            };
        }

        public static PaymentVerifyResult CreateVerifyResult(MelliApiVerifyResult result, MessagesOptions messagesOptions)
        {
            if (result == null)
            {
                return PaymentVerifyResult.Failed(messagesOptions.UnexpectedErrorText);
            }

            string message;

            if (!result.Description.IsNullOrEmpty())
            {
                message = result.Description;
            }
            else
            {
                message = MelliVerifyResultTranslator.Translate(result.ResCode, messagesOptions);
            }

            var status = result.ResCode == SuccessCode
                ? PaymentVerifyResultStatus.Succeed
                : PaymentVerifyResultStatus.Failed;

            return new PaymentVerifyResult
            {
                Status = status,
                TransactionCode = result.RetrivalRefNo,
                Message = message
            };
        }

        private static object CreateRequestObject(string terminalId, string merchantId, long amount, string signedData, string callbackUrl, long orderId)
        {
            return new
            {
                TerminalId = terminalId,
                MerchantId = merchantId,
                Amount = amount,
                SignData = signedData,
                ReturnUrl = callbackUrl,
                LocalDateTime = DateTime.Now,
                OrderId = orderId.ToString()
            };
        }

        private static object CreateVerifyObject(string apiToken, string signedData)
        {
            return new
            {
                token = apiToken,
                SignData = signedData
            };
        }
    }
}
