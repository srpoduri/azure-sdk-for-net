﻿// -----------------------------------------------------------------------------------------
// <copyright file="RequestMessageExtensions.cs" company="Microsoft">
//    Copyright 2012 Microsoft Corporation
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//      http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
// -----------------------------------------------------------------------------------------

namespace Microsoft.WindowsAzure.Storage.Shared.Protocol
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Microsoft.WindowsAzure.Storage.Core;
    using Microsoft.WindowsAzure.Storage.Core.Util;

    internal static class RequestMessageExtensions
    {
        /// <summary>
        /// Adds the lease id.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="leaseId">The lease id.</param>
        internal static void AddLeaseId(this HttpRequestMessage request, string leaseId)
        {
            if (leaseId != null)
            {
                request.AddOptionalHeader("x-ms-lease-id", leaseId);
            }
        }

        /// <summary>
        /// Adds the optional header.
        /// </summary>
        /// <param name="request">The web request.</param>
        /// <param name="name">The metadata name.</param>
        /// <param name="value">The metadata value.</param>
        internal static void AddOptionalHeader(this HttpRequestMessage request, string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                request.Headers.Add(name, value);
            }
        }

        /// <summary>
        /// Adds an optional header to a request.
        /// </summary>
        /// <param name="request">The web request.</param>
        /// <param name="name">The header name.</param>
        /// <param name="value">The header value.</param>
        internal static void AddOptionalHeader(this HttpRequestMessage request, string name, int? value)
        {
            if (value.HasValue)
            {
                request.Headers.Add(name, value.Value.ToString());
            }
        }

        /// <summary>
        /// Applies the condition to the web request.
        /// </summary>
        /// <param name="request">The request to be modified.</param>
        /// <param name="accessCondition">Access condition to be added to the request.</param>
        internal static void ApplyAccessCondition(this HttpRequestMessage request, AccessCondition accessCondition)
        {
            if (accessCondition != null)
            {
                if (!string.IsNullOrEmpty(accessCondition.IfMatchETag))
                {
                    request.Headers.IfMatch.Add(EntityTagHeaderValue.Parse(accessCondition.IfMatchETag));
                }

                if (!string.IsNullOrEmpty(accessCondition.IfNoneMatchETag))
                {
                    if (accessCondition.IfNoneMatchETag.Equals(EntityTagHeaderValue.Any.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        request.Headers.IfNoneMatch.Add(EntityTagHeaderValue.Any);
                    }
                    else
                    {
                        request.Headers.IfNoneMatch.Add(EntityTagHeaderValue.Parse(accessCondition.IfNoneMatchETag));
                    }
                }

                request.Headers.IfModifiedSince = accessCondition.IfModifiedSinceTime;
                request.Headers.IfUnmodifiedSince = accessCondition.IfNotModifiedSinceTime;

                if (!string.IsNullOrEmpty(accessCondition.LeaseId))
                {
                    request.Headers.Add(Constants.HeaderConstants.LeaseIdHeader, accessCondition.LeaseId);
                }
            }
        }

        /// <summary>
        /// Applies the condition for a source blob to the web request.
        /// </summary>
        /// <param name="request">The request to be modified.</param>
        /// <param name="accessCondition">Access condition to be added to the request.</param>
        internal static void ApplyAccessConditionToSource(this HttpRequestMessage request, AccessCondition accessCondition)
        {
            if (accessCondition != null)
            {
                if (!string.IsNullOrEmpty(accessCondition.IfMatchETag))
                {
                    request.Headers.Add(Constants.HeaderConstants.SourceIfMatchHeader, accessCondition.IfMatchETag);
                }

                if (!string.IsNullOrEmpty(accessCondition.IfNoneMatchETag))
                {
                    request.Headers.Add(Constants.HeaderConstants.SourceIfNoneMatchHeader, accessCondition.IfNoneMatchETag);
                }

                if (accessCondition.IfModifiedSinceTime.HasValue)
                {
                    request.Headers.Add(
                        Constants.HeaderConstants.SourceIfModifiedSinceHeader,
                        HttpUtility.ConvertDateTimeToHttpString(accessCondition.IfModifiedSinceTime.Value));
                }

                if (accessCondition.IfNotModifiedSinceTime.HasValue)
                {
                    request.Headers.Add(
                        Constants.HeaderConstants.SourceIfUnmodifiedSinceHeader,
                        HttpUtility.ConvertDateTimeToHttpString(accessCondition.IfNotModifiedSinceTime.Value));
                }

                if (!string.IsNullOrEmpty(accessCondition.LeaseId))
                {
                    throw new InvalidOperationException(SR.LeaseConditionOnSource);
                }
            }
        }
    }
}
