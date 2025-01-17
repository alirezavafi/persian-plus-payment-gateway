// Copyright (c) Persian.Plus.PaymentGateway.Core. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC License, Version 3.0. See License.txt in the project root for license information.

using System;

namespace Persian.Plus.PaymentGateway.Core.PaymentTokenProviders
{
    public class InvalidPaymentTokenProviderException : Exception
    {
        public string PaymentToken { get; }

        public InvalidPaymentTokenProviderException(string paymentToken, string message) : base(message)
        {
            PaymentToken = paymentToken;
        }
    }
}
