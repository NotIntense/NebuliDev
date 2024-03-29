﻿using Nebuli.API.Features.Player;
using System;

namespace Nebuli.Events.EventArguments.Player;

public class PlayerJoinEvent : EventArgs, IPlayerEvent
{
    public PlayerJoinEvent(ServerRoles serverRoles)
    {
        Player = new NebuliPlayer(serverRoles._hub);
    }

    /// <summary>
    /// The player calling the event.
    /// </summary>
    public NebuliPlayer Player { get; }
}