﻿// Copyright (c) Persian.Plus.PaymentGateway.Core. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC License, Version 3.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Persian.Plus.PaymentGateway.Storage.Cache.Abstractions;
using Persian.Plus.PaymentGateway.Storage.Cache.Internal;

namespace Persian.Plus.PaymentGateway.Storage.Cache.MemoryCache
{
    /// <summary>
    /// Memory cache implementation of Persian.Plus.PaymentGateway.Core storage.
    /// </summary>
    public class MemoryCachePaymentStorage : CachePaymentStorage
    {
        private readonly IMemoryCache _memoryCache;
        private readonly MemoryCacheStorageOptions _options;

        /// <summary>
        /// Initializes an instance of <see cref="MemoryCachePaymentStorage"/>.
        /// </summary>
        /// <param name="memoryCache"></param>
        /// <param name="options"></param>
        public MemoryCachePaymentStorage(IMemoryCache memoryCache, IOptions<MemoryCacheStorageOptions> options)
        {
            _memoryCache = memoryCache;
            _options = options.Value;
            Collection = memoryCache.Get<ICacheStorageCollection>(_options.CacheKey) ?? new CacheStorageCollection();
        }

        /// <inheritdoc />
        protected override ICacheStorageCollection Collection { get; }

        /// <inheritdoc />
        protected override Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            _memoryCache.Set(_options.CacheKey, Collection, _options.CacheEntryOptions);

            return Task.CompletedTask;
        }
    }
}
