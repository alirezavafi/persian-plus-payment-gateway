// Copyright (c) Persian.Plus.PaymentGateway.Core. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC License, Version 3.0. See License.txt in the project root for license information.

namespace Persian.Plus.PaymentGateway.Gateways.Saman.Internal.Models
{
    public class SamanCallbackResult
    {
        public bool IsSucceed { get; set; }

        public string ReferenceId { get; set; }

        public string TransactionId { get; set; }

        public string SecurePan { get; set; }

        public string Cid { get; set; }

        public string Rrn { get; set; }

        public string TraceNo { get; set; }

        public string Message { get; set; }
        public string State { get; set; }
    }
}
