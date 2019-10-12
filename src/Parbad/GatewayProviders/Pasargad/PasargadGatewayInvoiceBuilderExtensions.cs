﻿using System;
using Parbad.GatewayProviders.Pasargad;
using Parbad.InvoiceBuilder;

namespace Parbad
{
    public static class PasargadGatewayInvoiceBuilderExtensions
    {
        /// <summary>
        /// The invoice will be sent to Pasargad gateway.
        /// </summary>
        /// <param name="builder"></param>
        public static IInvoiceBuilder UsePasargad(this IInvoiceBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            return builder.UseGateway(PasargadGateway.Name);
        }
    }
}
