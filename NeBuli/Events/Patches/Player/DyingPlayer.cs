﻿using HarmonyLib;
using Nebuli.Events.EventArguments.Player;
using Nebuli.Events.Handlers;
using NorthwoodLib.Pools;
using PlayerStatsSystem;
using System.Collections.Generic;
using System.Reflection.Emit;
using static HarmonyLib.AccessTools;

namespace Nebuli.Events.Patches.Player;

[HarmonyPatch(typeof(PlayerStats), nameof(PlayerStats.KillPlayer))]
internal class DyingPlayer
{
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> OnDying(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        List<CodeInstruction> newInstructions = EventManager.CheckPatchInstructions<DyingPlayer>(37, instructions);

        Label retLabel = generator.DefineLabel();
        LocalBuilder @event = generator.DeclareLocal(typeof(PlayerDyingEvent));

        newInstructions.InsertRange(0, new CodeInstruction[]
        {
            new(OpCodes.Ldarg_0),
            new(OpCodes.Ldfld, Field(typeof(PlayerStats), nameof(PlayerStats._hub))),
            new(OpCodes.Ldarg_1),
            new(OpCodes.Newobj, GetDeclaredConstructors(typeof(PlayerDyingEvent))[0]),
            new(OpCodes.Dup),
            new(OpCodes.Dup),
            new(OpCodes.Stloc, @event.LocalIndex),
            new(OpCodes.Call, Method(typeof(PlayerHandlers), nameof(PlayerHandlers.OnDying))),
            new(OpCodes.Ldloc, @event.LocalIndex),
            new(OpCodes.Callvirt, PropertyGetter(typeof(PlayerDyingEvent), nameof(PlayerDyingEvent.DamageHandlerBase))),
            new(OpCodes.Starg, 1),
            new(OpCodes.Callvirt, PropertyGetter(typeof(PlayerHurtEvent), nameof(PlayerHurtEvent.IsCancelled))),
            new(OpCodes.Brtrue_S, retLabel)
        }) ;    

        newInstructions[newInstructions.Count - 1].labels.Add(retLabel);

        foreach (CodeInstruction instruction in newInstructions)
            yield return instruction;

        ListPool<CodeInstruction>.Shared.Return(newInstructions);
    }
}