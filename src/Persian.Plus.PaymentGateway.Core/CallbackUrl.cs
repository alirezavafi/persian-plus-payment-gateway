// Copyright (c) Persian.Plus.PaymentGateway.Core. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC License, Version 3.0. See License.txt in the project root for license information.

using System;
using System.Text;
using System.Text.Encodings.Web;
using Persian.Plus.PaymentGateway.Core.Exceptions;

namespace Persian.Plus.PaymentGateway.Core
{
    /// <summary>
    /// A complete URL of your website. It will be used by the gateway for redirecting
    /// the client again to your website.
    /// <para>Note: A complete URL would be like: "http://www.mywebsite.com/foo/bar/"</para>
    /// </summary>
    /// <exception cref="CallbackUrlFormatException"></exception>
    public readonly struct CallbackUrl : IComparable<CallbackUrl>
    {
        /// <summary>
        /// Initializes an instance of <see cref="CallbackUrl"/> class.
        /// </summary>
        /// <param name="url">
        /// A complete URL of your website. It will be used by the gateway for redirecting
        /// the client again to your website.
        /// <para>A complete URL would be like: "http://www.mywebsite.com/foo/bar/"</para>
        /// </param>
        /// <exception cref="CallbackUrlFormatException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public CallbackUrl(string url)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));

            if (!Uri.TryCreate(url, UriKind.Absolute, out _))
            {
                throw new CallbackUrlFormatException(url);
            }

            Url = url;
        }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        public string Url { get; }

        /// <summary>
        /// Adds a query string to the current URL.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public CallbackUrl AddQueryString(string name, string value)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (value == null) throw new ArgumentNullException(nameof(value));

            var anchorIndex = Url.IndexOf('#');
            var uriToBeAppended = Url;
            var anchorText = "";

            if (anchorIndex != -1)
            {
                anchorText = Url.Substring(anchorIndex);
                uriToBeAppended = Url.Substring(0, anchorIndex);
            }

            var queryIndex = uriToBeAppended.IndexOf('?');
            var hasQuery = queryIndex != -1;

            var generatedUrl = new StringBuilder();

            generatedUrl.Append(uriToBeAppended);
            generatedUrl.Append(hasQuery ? '&' : '?');
            generatedUrl.Append(UrlEncoder.Default.Encode(name));
            generatedUrl.Append('=');
            generatedUrl.Append(UrlEncoder.Default.Encode(value));

            generatedUrl.Append(anchorText);

            return new CallbackUrl(generatedUrl.ToString());
        }

        public int CompareTo(CallbackUrl other)
        {
            return string.Compare(Url, other.Url, StringComparison.OrdinalIgnoreCase);
        }

        public bool Equals(CallbackUrl other)
        {
            return string.Equals(Url, other.Url, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is CallbackUrl other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Url.GetHashCode();
        }

        public override string ToString()
        {
            return Url;
        }

        public static CallbackUrl Parse(string url) => new CallbackUrl(url);

        public static bool TryParse(string url, out CallbackUrl callbackUrl)
        {
            try
            {
                callbackUrl = new CallbackUrl(url);
                return true;
            }
            catch
            {
                callbackUrl = default;
                return false;
            }
        }

        public static implicit operator string(CallbackUrl callbackUrl)
        {
            return callbackUrl.ToString();
        }
    }
}
