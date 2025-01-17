// Copyright (c) Persian.Plus.PaymentGateway.Core. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC License, Version 3.0. See License.txt in the project root for license information.

using System;
using Persian.Plus.PaymentGateway.Core.Builder;
using Persian.Plus.PaymentGateway.Core.Internal;

namespace Persian.Plus.PaymentGateway.Core.Gateway
{
    public static class GatewayBuilderExtensions
    {
        /// <summary>
        /// Configures the gateways.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configure"></param>
        public static IPaymentGatewayBuilder ConfigureGateways(this IPaymentGatewayBuilder builder, Action<IGatewayBuilder> configure)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configure == null) throw new ArgumentNullException(nameof(configure));

            configure(new GatewayBuilder(builder.Services));

            return builder;
        }
    }
}
