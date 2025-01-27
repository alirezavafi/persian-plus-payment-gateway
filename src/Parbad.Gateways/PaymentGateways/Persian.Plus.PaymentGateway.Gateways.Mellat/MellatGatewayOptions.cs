﻿// Copyright (c) Persian.Plus.PaymentGateway.Core. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC License, Version 3.0. See License.txt in the project root for license information.

namespace Persian.Plus.PaymentGateway.Gateways.Mellat
{
    public class MellatGatewayOptions
    {
        public string PaymentPageUrl { get; set; } = "https://bpm.shaparak.ir/pgwchannel/startpay.mellat";

        public string ApiUrl { get; set; } = "https://bpm.shaparak.ir/pgwchannel/services/pgw";
    }
}
