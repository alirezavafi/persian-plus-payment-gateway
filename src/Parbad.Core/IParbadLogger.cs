﻿// Copyright (c) Parbad.Core. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC License, Version 3.0. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;

namespace Parbad
{
    /// <summary>
    /// Parbad.Core Logger is a Proxy for Microsoft Logging.
    /// </summary>
    /// <typeparam name="TCategoryName"></typeparam>
    public interface IParbadLogger<out TCategoryName> : ILogger<TCategoryName>
    {
    }
}