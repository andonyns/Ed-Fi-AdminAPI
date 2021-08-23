// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using EdFi.Ods.AdminApp.Management.Configuration.Application;
using EdFi.Ods.AdminApp.Web.ActionFilters;
using EdFi.Ods.AdminApp.Web.Models.ViewModels;

namespace EdFi.Ods.AdminApp.Web.Controllers
{
    [BypassSetupRequiredFilter, BypassInstanceContextFilter]
    public class ProductImprovementController : ControllerBase
    {
        private readonly ApplicationConfigurationService _applicationConfigurationService;

        public ProductImprovementController(ApplicationConfigurationService applicationConfigurationService)
        {
            _applicationConfigurationService = applicationConfigurationService;
        }

        public ActionResult EditConfiguration()
        {
            return View(GetProductImprovementModel());
        }

        [HttpPost]
        public ActionResult EditConfiguration(ProductImprovementModel model)
        {
            SaveProductImprovementModel(model);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult EnableProductImprovementFirstTimeSetup()
        {
            return View(GetProductImprovementModel());
        }

        [HttpPost]
        public ActionResult EnableProductImprovementFirstTimeSetup(ProductImprovementModel model)
        {
            SaveProductImprovementModel(model);
            return RedirectToAction("PostSetup", "Home", new {setupCompleted = true});
        }

        private ProductImprovementModel GetProductImprovementModel()
        {
            var enableProductImprovement =
                _applicationConfigurationService.IsProductImprovementEnabled(out var productRegistrationId);

            return new ProductImprovementModel
            {
                EnableProductImprovement = enableProductImprovement,
                ProductRegistrationId = productRegistrationId
            };
        }

        private void SaveProductImprovementModel(ProductImprovementModel model)
        {
            _applicationConfigurationService.EnableProductImprovement(model.EnableProductImprovement, model.ProductRegistrationId);
        }
    }
}
