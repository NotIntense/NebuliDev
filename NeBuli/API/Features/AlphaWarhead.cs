﻿using Nebuli.API.Features.Player;
using UnityEngine;

namespace Nebuli.API.Features;

/// <summary>
/// Allows easier control of the Alphawarhead by using the <see cref="AlphaWarheadController"/>.
/// </summary>
public static class AlphaWarhead
{
    /// <summary>
    /// Gets the <see cref="AlphaWarheadController"/> singleton.
    /// </summary>
    public static AlphaWarheadController Controller => AlphaWarheadController.Singleton;

    /// <summary>
    /// Gets the <see cref="AlphaWarheadController"/> <see cref="UnityEngine.GameObject"/>.
    /// </summary>
    public static GameObject GameObject => Controller.gameObject;

    /// <summary>
    /// Gets the <see cref="AlphaWarheadSyncInfo"/>.
    /// </summary>
    public static AlphaWarheadSyncInfo Info => Controller.Info;

    /// <summary>
    /// Forces the warhead to instantly prepare.
    /// </summary>
    public static void InstantPrepare() => Controller.InstantPrepare();

    /// <summary>
    /// Gets a value if the Alpha Warhead is in progress or not.
    /// </summary>
    public static bool IsInProgress => Controller.NetworkInfo.InProgress;

    /// <summary>
    /// Forces the warhead to dentonate.
    /// </summary>
    public static void Detonate() => Controller.Detonate();

    /// <summary>
    /// Cancels the warhead detonation sequence.
    /// </summary>
    public static void CancelDetonation(NebuliPlayer disabler = null) => Controller.CancelDetonation(disabler.ReferenceHub);

    /// <summary>
    /// Gets the total amount of deaths caused by the warhead.
    /// </summary>
    public static int WarheadKills => Controller.WarheadKills;

    /// <summary>
    /// Overrides the current warhead time with the new time.
    /// </summary>
    /// <param name="time">The time to override the current warhead time with.</param>
    public static void ForceTime(float time) => Controller.ForceTime(time);

    /// <summary>
    /// Forces the Alpha Warhead to start its detonation sequence.
    /// </summary>
    /// <param name="isAutomatic">If the detonation is automatic or not.</param>
    /// <param name="supressSubtitles">If C.A.S.S.I.E should supress the subtitles.</param>
    /// <param name="playerTriggering">The <see cref="ReferenceHub"/> triggering the detonation.</param>
    public static void StartDetonation(bool isAutomatic = false, bool supressSubtitles = false, ReferenceHub playerTriggering = null) => Controller.StartDetonation(isAutomatic, supressSubtitles, playerTriggering);

    /// <summary>
    /// Gets or sets the Alpha Warhead cooldown time.
    /// </summary>
    public static double CooldownTime
    {
        get => Controller.NetworkCooldownEndTime;
        set => Controller.NetworkCooldownEndTime = value;
    }
}