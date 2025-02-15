﻿// --------------------------------------------------------------------------
//  <copyright file="GetGatewaysResponse.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------

using System.Collections.Generic;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common.Templates.Gateway;
using Newtonsoft.Json;

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common.API.Clients.Gateway.Responses
{
    public class GetGatewaysResponse
    {
        [JsonProperty("value")]
        public List<GatewayTemplateResource> Gateways { get; set; }

        public int Count { get; set; }
    }
}
