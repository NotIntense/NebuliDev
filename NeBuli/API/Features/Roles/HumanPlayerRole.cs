﻿using PlayerRoles;
using Respawning;

namespace Nebuli.API.Features.Roles;

public class HumanPlayerRole : FpcRoleBase
{
    internal HumanPlayerRole(HumanRole role) : base(role)
    {
        Base = role;
    }

    /// <summary>
    /// Gets the <see cref="HumanRole"/> base.
    /// </summary>
    public new HumanRole Base { get; }

    /// <summary>
    /// Gets the roles RoleTypeId.
    /// </summary>
    public override RoleTypeId RoleTypeId => Base.RoleTypeId;

    /// <summary>
    /// Gets the roles assigned <see cref="SpawnableTeamType"/>.
    /// </summary>
    public SpawnableTeamType AssingedSpawnAbleTeam => Base.AssignedSpawnableTeam;

    /// <summary>
    /// Gets if the role uses unit names.
    /// </summary>
    public bool UsesUnitName => Base.UsesUnitNames;
}