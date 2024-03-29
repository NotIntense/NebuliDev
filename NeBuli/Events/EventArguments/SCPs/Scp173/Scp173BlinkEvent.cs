using System;
using Nebuli.API.Features.Player;
using UnityEngine;

namespace Nebuli.Events.EventArguments.SCPs.Scp173;

public class Scp173BlinkEvent : EventArgs, IPlayerEvent, ICancellableEvent
{
    public Scp173BlinkEvent(ReferenceHub player, Vector3 position)
    {
        Player = NebuliPlayer.Get(player);
        Position = position;
        IsCancelled = false;
    }
    
    /// <summary>
    /// The player triggering the event.
    /// </summary>
    public NebuliPlayer Player { get; }
    
    /// <summary>
    /// The position 173 will be teleported to.
    /// </summary>
    public Vector3 Position { get; set; }

    /// <summary>
    /// Gets or sets if the event is cancelled.
    /// </summary>
    public bool IsCancelled { get; set; }
}