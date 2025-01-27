// Copyright (c) Persian.Plus.PaymentGateway.Core. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC License, Version 3.0. See License.txt in the project root for license information.

using Persian.Plus.PaymentGateway.Core;

namespace Persian.Plus.PaymentGateway.Gateways.VirtualGateway.MiddlewareInvoker
{
    internal class VirtualGatewayCommandDetails
    {
        public VirtualGatewayCommandDetails(VirtualGatewayCommandType commandType, long trackingNumber, Money amount, string redirectUrl)
        {
            CommandType = commandType;
            TrackingNumber = trackingNumber;
            Amount = amount;
            RedirectUrl = redirectUrl;
        }

        public VirtualGatewayCommandType CommandType { get; }

        public long TrackingNumber { get; }

        public Money Amount { get; }

        public string RedirectUrl { get; }
    }
}
