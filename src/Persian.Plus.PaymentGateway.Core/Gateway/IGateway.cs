// Copyright (c) Persian.Plus.PaymentGateway.Core. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC License, Version 3.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Persian.Plus.PaymentGateway.Core.Internal;

namespace Persian.Plus.PaymentGateway.Core.Gateway
{
    /// <summary>
    /// Represents a gateway (Bank).
    /// </summary>
    public interface IGateway
    {
        /// <summary>
        /// Performs a payment request using the given <paramref name="invoice"/>.
        /// </summary>
        /// <param name="invoice">The invoice which must be pay.</param>
        /// <param name="cancellationToken"></param>
        Task<PaymentRequestResult> RequestAsync(Invoice invoice, CancellationToken cancellationToken = default);

        /// <summary>
        /// Fetches the invoice from current request.
        /// This method must be called when the users come back from gateways to your website.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        Task<PaymentFetchResult> FetchAsync(InvoiceContext context, CancellationToken cancellationToken = default);

        /// <summary>
        /// Verifies the requested payment to check whether or not the invoice has was paid in the gateway by the client.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        Task<PaymentVerifyResult> VerifyAsync(InvoiceContext context, CancellationToken cancellationToken = default);

        /// <summary>
        /// Performs a refund request for the given invoice.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="amount"></param>
        /// <param name="cancellationToken"></param>
        Task<PaymentRefundResult> RefundAsync(InvoiceContext context, Money amount, CancellationToken cancellationToken = default);
    }
}
