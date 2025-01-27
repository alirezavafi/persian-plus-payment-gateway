﻿using System;
using Persian.Plus.PaymentGateway.Core.Invoice;

namespace Persian.Plus.PaymentGateway.Gateways.Melli
{
    public static class MelliGatewayInvoiceBuilderExtensions
    {
        /// <summary>
        /// The invoice will be sent to Melli gateway.
        /// </summary>
        /// <param name="builder"></param>
        public static IInvoiceBuilder UseMelli(this IInvoiceBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            return builder.SetGateway(MelliGateway.Name);
        }
    }
}
