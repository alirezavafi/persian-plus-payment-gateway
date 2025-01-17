﻿// Copyright (c) Persian.Plus.PaymentGateway.Core. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC License, Version 3.0. See License.txt in the project root for license information.

using System;
using Persian.Plus.PaymentGateway.Core.Gateway;
using Persian.Plus.PaymentGateway.Core.Invoice;

namespace Persian.Plus.PaymentGateway.Facilitators.PayPing
{
    public static class PayPingGatewayInvoiceBuilderExtensions
    {
        private const string PayPingRequestKey = "PayPingRequest";

        /// <summary>
        /// The invoice will be sent to PayPing gateway.
        /// </summary>
        public static IInvoiceBuilder UsePayPing(this IInvoiceBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            return builder.SetGateway(PayPingGateway.Name);
        }

        /// <summary>
        /// Sets the additional data for PayPing Gateway.
        /// </summary>
        public static IInvoiceBuilder SetPayPingData(this IInvoiceBuilder builder, Action<PayPingRequest> configurePayPing)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configurePayPing == null) throw new ArgumentNullException(nameof(configurePayPing));

            var payPingRequest = new PayPingRequest();
            configurePayPing(payPingRequest);

            return SetPayPingData(builder, payPingRequest);
        }

        /// <summary>
        /// Sets the additional data for PayPing Gateway.
        /// </summary>
        public static IInvoiceBuilder SetPayPingData(this IInvoiceBuilder builder, PayPingRequest payPingRequest)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (payPingRequest == null) throw new ArgumentNullException(nameof(payPingRequest));

            builder.AddOrUpdateProperty(PayPingRequestKey, payPingRequest);

            return builder;
        }

        internal static PayPingRequest GetPayPingRequest(this Invoice invoice)
        {
            if (invoice == null) throw new ArgumentNullException(nameof(invoice));

            if (invoice.Properties.ContainsKey(PayPingRequestKey))
            {
                return (PayPingRequest)invoice.Properties[PayPingRequestKey];
            }

            return null;
        }
    }
}
