// Copyright (c) Persian.Plus.PaymentGateway.Core. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC License, Version 3.0. See License.txt in the project root for license information.

using System;

namespace Persian.Plus.PaymentGateway.Core.Exceptions
{
    [Serializable]
    public class InvalidTrackingNumberException : Exception
    {
        public InvalidTrackingNumberException(long trackingNumber) :
            base($"The provided tracking number {trackingNumber} is not valid. A valid tracking number must be greater than zero.")
        {
            TrackingNumber = trackingNumber;
        }

        public long TrackingNumber { get; }
    }
}
