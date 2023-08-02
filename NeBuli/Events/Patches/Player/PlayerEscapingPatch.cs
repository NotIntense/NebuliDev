﻿using HarmonyLib;
using Nebuli.Events.EventArguments.Player;
using Nebuli.Events.Handlers;
using NorthwoodLib.Pools;
using System.Collections.Generic;
using System.Reflection.Emit;
using static HarmonyLib.AccessTools;

namespace Nebuli.Events.Patches.Player;

[HarmonyPatch(typeof(Escape), nameof(Escape.ServerHandlePlayer))]
public class PlayerEscapingPatch
{
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> OnEscape(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        List<CodeInstruction> newInstructions = EventManager.CheckPatchInstructions<PlayerEscapingPatch>(71, instructions);

        Label retLabel = generator.DefineLabel();

        int index = newInstructions.FindLastIndex(i => i.opcode == OpCodes.Ldloc_2);

        newInstructions.InsertRange(index, new CodeInstruction[]
        {
            new CodeInstruction(OpCodes.Ldarg_0).MoveLabelsFrom(newInstructions[index]),
            new(OpCodes.Ldloc_0),
            new(OpCodes.Ldloc_1),
            new(OpCodes.Newobj, GetDeclaredConstructors(typeof(PlayerEscaping))[0]),
            new(OpCodes.Dup),
            new(OpCodes.Call, Method(typeof(PlayerHandlers), nameof(PlayerHandlers.OnEscaping))),
            new(OpCodes.Callvirt, PropertyGetter(typeof(PlayerEscaping), nameof(PlayerEscaping.IsCancelled))),
            new(OpCodes.Brtrue_S, retLabel)
        });

        newInstructions[newInstructions.Count - 1].labels.Add(retLabel);

        foreach (CodeInstruction instruction in newInstructions)
            yield return instruction;

        ListPool<CodeInstruction>.Shared.Return(newInstructions);
    }
}