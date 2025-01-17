// Copyright (c) Persian.Plus.PaymentGateway.Core. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC License, Version 3.0. See License.txt in the project root for license information.

using System;
using Persian.Plus.PaymentGateway.Core.Builder;
using Persian.Plus.PaymentGateway.Core.Internal;

namespace Persian.Plus.PaymentGateway.Core.PaymentTokenProviders
{
    public static class PaymentTokenProviderExtensions
    {
        /// <summary>
        /// Configures the <see cref="IPaymentTokenProvider"/> required by Persian.Plus.PaymentGateway.Core to create
        /// a unique token which will be used to load the payment data when the client comes
        /// back from the gateway.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="tokenBuilder">A builder to configure the <see cref="IPaymentTokenProvider"/></param>
        public static IPaymentGatewayBuilder ConfigurePaymentToken(this IPaymentGatewayBuilder builder,
            Action<IPaymentTokenBuilder> tokenBuilder)
        {
            tokenBuilder(new PaymentTokenBuilder(builder.Services));

            return builder;
        }
    }
}
