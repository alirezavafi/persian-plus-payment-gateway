// Copyright (c) Persian.Plus.PaymentGateway.Core. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC License, Version 3.0. See License.txt in the project root for license information.

namespace Persian.Plus.PaymentGateway.Core.PaymentTokenProviders
{
    public class QueryStringPaymentTokenOptions
    {

        public static readonly string DefaultQueryName = "paymentToken";

        /// <summary>
        /// The default value is "paymentToken".
        /// </summary>
        public string QueryName { get; set; } = DefaultQueryName;
    }
}
