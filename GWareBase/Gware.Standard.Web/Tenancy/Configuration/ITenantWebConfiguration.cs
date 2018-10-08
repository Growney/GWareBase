﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Standard.Web.Tenancy.Configuration
{
    public interface ITenantWebConfiguration : ITenantConfiguration
    {
        IActionResult CreateNewResult { get; set; }
        IActionResult NotFoundResult { get; set; }
        IActionResult TenantHome { get; set; }
        IActionResult Upgrading { get; set; }

        Task<bool> CreateTenant(string name, string displayName, int entityType, long entityID);
        bool IsValidTenantName(string name);
    }
}