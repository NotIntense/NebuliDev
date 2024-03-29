﻿using Footprinting;
using HarmonyLib;
using Nebuli.Events.EventArguments.Server;
using Nebuli.Events.Handlers;
using NorthwoodLib.Pools;
using System.Collections.Generic;
using System.Reflection.Emit;
using static HarmonyLib.AccessTools;

namespace Nebuli.Events.Patches.Warhead;

[HarmonyPatch(typeof(AlphaWarheadController), nameof(AlphaWarheadController.Detonate))]
internal class WarheadDetonatingPatch
{
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> OnDetonating(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        List<CodeInstruction> newInstructions = EventManager.CheckPatchInstructions<WarheadDetonatingPatch>(184, instructions);

        Label retLabel = generator.DefineLabel();

        newInstructions.InsertRange(0, new CodeInstruction[]
        {
            new(OpCodes.Callvirt, PropertyGetter(typeof(AlphaWarheadController), nameof(AlphaWarheadController.Singleton))),
            new(OpCodes.Ldfld, Field(typeof(AlphaWarheadController), nameof(AlphaWarheadController._triggeringPlayer))),
            new(OpCodes.Ldfld, Field(typeof(Footprint), nameof(Footprint.Hub))),
            new(OpCodes.Newobj, GetDeclaredConstructors(typeof(WarheadDetonatingEvent))[0]),
            new(OpCodes.Dup),
            new(OpCodes.Call, Method(typeof(ServerHandler), nameof(ServerHandler.OnWarheadDetonated))),
            new(OpCodes.Callvirt, PropertyGetter(typeof(WarheadDetonatingEvent), nameof(WarheadDetonatingEvent.IsCancelled))),
            new(OpCodes.Brtrue_S, retLabel)
        });

        newInstructions[newInstructions.Count - 1].labels.Add(retLabel);

        foreach (CodeInstruction instruction in newInstructions)
            yield return instruction;
        ListPool<CodeInstruction>.Shared.Return(newInstructions);
    }
}