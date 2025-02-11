// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using System.Net;
using EdFi.Ods.AdminApi.Infrastructure.ErrorHandling;
using EdFi.SecurityCompatiblity53.DataAccess.Contexts;

namespace EdFi.Ods.AdminApi.Infrastructure.ClaimSetEditor;

public class GetClaimSetByIdQueryV53Service
{
    private readonly ISecurityContext _securityContext;

    public GetClaimSetByIdQueryV53Service(ISecurityContext securityContext)
    {
        _securityContext = securityContext;
    }

    public ClaimSet Execute(int claimSetId)
    {
        var securityContextClaimSet = _securityContext.ClaimSets
            .SingleOrDefault(x => x.ClaimSetId == claimSetId);

        if (securityContextClaimSet != null)
        {
            return new ClaimSet
            {
                Id = securityContextClaimSet.ClaimSetId,
                Name = securityContextClaimSet.ClaimSetName,
                IsEditable = !Constants.DefaultClaimSets.Contains(securityContextClaimSet.ClaimSetName)
            };
        }

        throw new AdminApiException("No such claim set exists in the database.")
        {
            StatusCode = HttpStatusCode.NotFound
        };
    }
}
