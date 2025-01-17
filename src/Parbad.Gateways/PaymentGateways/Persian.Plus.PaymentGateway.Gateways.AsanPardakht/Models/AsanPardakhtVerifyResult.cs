﻿// Copyright (c) Persian.Plus.PaymentGateway.Core. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC License, Version 3.0. See License.txt in the project root for license information.

using Persian.Plus.PaymentGateway.Core.Internal;

namespace Persian.Plus.PaymentGateway.Gateways.AsanPardakht.Models
{
    public class AsanPardakhtVerifyResult
    {
        public bool IsSucceed { get; internal set; }
        public PaymentVerifyResult Result { get; internal set; }
    }
}
