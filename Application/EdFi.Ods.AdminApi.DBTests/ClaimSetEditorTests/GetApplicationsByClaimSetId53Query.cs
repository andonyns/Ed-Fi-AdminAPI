// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using EdFi.Admin.DataAccess.Contexts;
using EdFi.Ods.AdminApi.Infrastructure.ClaimSetEditor;
using EdFi.SecurityCompatiblity53.DataAccess.Contexts;

namespace EdFi.Ods.AdminApi.DBTests.ClaimSetEditorTests;

/// <summary>
/// Compatibility copy of EdFi.Ods.AdminApi.Infrastructure.ClaimSetEditor.GetApplicationsByClaimSetIdQuery
///
/// Since the projected ClaimSet does not include the new columns from v6.1, this query does not
/// require multiple versions. However, in order to preserve consistent test results, we must
/// construct dependent services using a query against the same database.
/// </summary>
internal class GetApplicationsByClaimSetId53Query : IGetApplicationsByClaimSetIdQuery
{
    private readonly ISecurityContext _securityContext;
    private readonly IUsersContext _usersContext;

    public GetApplicationsByClaimSetId53Query(ISecurityContext securityContext, IUsersContext usersContext)
    {
        _securityContext = securityContext;
        _usersContext = usersContext;
    }

    public IEnumerable<Application> Execute(int securityContextClaimSetId)
    {
        var claimSetName = GetClaimSetNameById(securityContextClaimSetId);

        return GetApplicationsByClaimSetName(claimSetName);
    }

    private string GetClaimSetNameById(int claimSetId)
    {
        return _securityContext.ClaimSets
            .Select(x => new { x.ClaimSetId, x.ClaimSetName })
            .Single(x => x.ClaimSetId == claimSetId).ClaimSetName;
    }

    private IEnumerable<Application> GetApplicationsByClaimSetName(string claimSetName)
    {
        return _usersContext.Applications
            .Where(x => x.ClaimSetName == claimSetName)
            .OrderBy(x => x.ClaimSetName)
            .Select(x => new Application
            {
                Name = x.ApplicationName,
                VendorName = x.Vendor.VendorName
            })
            .ToList();
    }

    public int ExecuteCount(int claimSetId)
    {
        return Execute(claimSetId).Count();
    }
}

