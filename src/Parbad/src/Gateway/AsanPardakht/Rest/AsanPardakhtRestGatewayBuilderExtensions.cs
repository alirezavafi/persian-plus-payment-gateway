// Copyright (c) Parbad. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC License, Version 3.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Parbad.Gateway.AsanPardakht.Internal;
using Parbad.GatewayBuilders;

namespace Parbad.Gateway.AsanPardakht
{
    public static class AsanPardakhtRestGatewayBuilderExtensions
    {
        /// <summary>
        /// Adds AsanPardakht gateway to Parbad services.
        /// </summary>
        /// <param name="builder"></param>
        public static IGatewayConfigurationBuilder<AsanPardakhtRestGateway> AddAsanPardakhtRest(this IGatewayBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            return builder
                .AddGateway<AsanPardakhtRestGateway>()
                .WithOptions(options => { })
                .WithHttpClient(clientBuilder => clientBuilder.ConfigureHttpClient(client => { }));
        }

        /// <summary>
        /// Configures the accounts for <see cref="AsanPardakhtSoapGateway"/>.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configureAccounts">Configures the accounts.</param>
        public static IGatewayConfigurationBuilder<AsanPardakhtRestGateway> WithAccounts(
            this IGatewayConfigurationBuilder<AsanPardakhtRestGateway> builder,
            Action<IGatewayAccountBuilder<AsanPardakhtRestGatewayAccount>> configureAccounts)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            return builder.WithAccounts(configureAccounts);
        }

        /// <summary>
        /// Configures the options for AsanPardakht Gateway.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configureOptions">Configuration</param>
        public static IGatewayConfigurationBuilder<AsanPardakhtRestGateway> WithOptions(
            this IGatewayConfigurationBuilder<AsanPardakhtRestGateway> builder,
            Action<AsanPardakhtSoapGatewayOptions> configureOptions)
        {
            builder.Services.Configure(configureOptions);

            return builder;
        }
    }
}