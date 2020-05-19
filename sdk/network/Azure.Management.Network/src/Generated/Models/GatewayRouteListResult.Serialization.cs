// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using System.Text.Json;
using Azure.Core;

namespace Azure.Management.Network.Models
{
    public partial class GatewayRouteListResult
    {
        internal static GatewayRouteListResult DeserializeGatewayRouteListResult(JsonElement element)
        {
            IReadOnlyList<GatewayRoute> value = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("value"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    List<GatewayRoute> array = new List<GatewayRoute>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        if (item.ValueKind == JsonValueKind.Null)
                        {
                            array.Add(null);
                        }
                        else
                        {
                            array.Add(GatewayRoute.DeserializeGatewayRoute(item));
                        }
                    }
                    value = array;
                    continue;
                }
            }
            return new GatewayRouteListResult(value);
        }
    }
}