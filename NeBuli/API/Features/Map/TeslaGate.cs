﻿using Nebuli.API.Features.Player;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MEC;

using TeslaGateBase = global::TeslaGate;

namespace Nebuli.API.Features.Map
{
    /// <summary>
    /// Wrapper for managing <see cref="TeslaGateBase"/> easier.
    /// </summary>
    public class TeslaGate
    {
        /// <summary>
        /// The dictionary of all the <see cref="TeslaGateBase"/> and their wrappers, <see cref="TeslaGate"/>, in the server.
        /// </summary>
        public static readonly Dictionary<TeslaGateBase, TeslaGate> Dictionary = new();
        internal TeslaGate(TeslaGateBase teslaGate)
        {
            Base = teslaGate;
            Dictionary.Add(teslaGate, this);
        }

        /// <summary>
        /// Gets the <see cref="TeslaGateController"/> base.
        /// </summary>
        public TeslaGateBase Base { get; }

        /// <summary>
        /// Gets a collection of all the tesla gates.
        /// </summary>
        public static IEnumerable<TeslaGate> Collection => Dictionary.Values;

        /// <summary>
        /// Gets a list of all the tesla gates on the server.
        /// </summary>
        public static List<TeslaGate> List => Collection.ToList();

        /// <summary>
        /// Gets the <see cref="TeslaGate"/> position.
        /// </summary>
        public Vector3 Position => Base.Position;

        /// <summary>
        /// Gets the <see cref="TeslaGate"/> <see cref="UnityEngine.Transform"/>.
        /// </summary>
        public Transform Transform => Base.transform;
        
        /// <summary>
        /// Gets the <see cref="TeslaGate"/> <see cref="UnityEngine.GameObject"/>.
        /// </summary>
        public GameObject GameObject => Base.gameObject;

        /// <summary>
        /// Gets the current <see cref="Map.Room"/> the <see cref="TeslaGate"/> is in.
        /// </summary>
        public Room Room => Room.Get(Base.Room);

        /// <summary>
        /// Gets if the specified player is in hurting range of the tesla gate.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool IsPlayerInHurtingRange(NebuliPlayer player) => Base.PlayerInRange(player.ReferenceHub);

        /// <summary>
        /// Gets if the specified player is in the idle range of the tesla gate.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool IsPlayerInIdleRange(NebuliPlayer player) => Base.IsInIdleRange(player.ReferenceHub);

        /// <summary>
        /// Gets if the Vector3 position is in range of the tesla gate.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool IsInRange(Vector3 position) => Base.InRange(position);

        /// <summary>
        /// Force triggers the <see cref="TeslaGate"/>, bypassing all cooldowns.
        /// </summary>
        public void ForceTrigger()
        {
            Base.RpcInstantBurst();
        }

        /// <summary>
        /// Makes the tesla gate trigger normally.
        /// </summary>
        public void Trigger()
        {
            Base.ServerSideCode();
        }

        /// <summary>
        /// Makes the tesla gate idle.
        /// </summary>
        /// <param name="shouldIdle"></param>
        public void Idle(bool shouldIdle = true)
        {
            Base.ServerSideIdle(shouldIdle);
        }

        /// <summary>
        /// Gets the nearest player to the tesla gate.
        /// </summary>
        public NebuliPlayer NearestPlayer
        {
            get
            {
                NebuliPlayer nearestPlayer = null;
                float shortestDistance = float.MaxValue;
                foreach (NebuliPlayer player in NebuliPlayer.List)
                {
                    float distance = Vector3.Distance(Position, player.Position);
                    if (distance < shortestDistance)
                    {
                        nearestPlayer = player;
                        shortestDistance = distance;
                    }
                }
                return nearestPlayer;
            }
        }
        /// <summary>
        /// Gets or creates a new tesla gate wrapper for the specified <see cref="TeslaGateBase"/>.
        /// </summary>
        /// <param name="teslaGate">The <see cref="TeslaGateBase"/> to use to find/create a wrapper.</param>
        /// <returns></returns>
        public static TeslaGate Get(TeslaGateBase teslaGate)
        {
            return Dictionary.TryGetValue(teslaGate, out TeslaGate tesla) ? tesla : new(teslaGate);
        }

        /// <summary>
        /// Triggers the specified tesla gate.
        /// </summary>
        /// <param name="gate"></param>
        public static void TriggerTeslaGate(TeslaGate gate) => gate.Trigger();

        /// <summary>
        /// Force triggers the specified tesla gate.
        /// </summary>
        public static void ForceTriggerTeslaGate(TeslaGate gate) => gate.ForceTrigger();
    }
}
