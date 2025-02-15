﻿// --------------------------------------------------------------------------
//  <copyright file="GetAllGroupsLinkedToProductResponse.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------

using System.Collections.Generic;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common.Templates.Groups;
using Newtonsoft.Json;

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common.API.Clients.Groups.Responses
{
    public class GetGroupsResponse
    {
        [JsonProperty("value")]
        public List<GroupTemplateResource> Groups { get; set; }

        public int Count { get; set; }
    }
}
