﻿using HarmonyLib;
using InventorySystem.Items.Firearms.Modules;
using Nebuli.Events.EventArguments.Player;
using Nebuli.Events.Handlers;
using NorthwoodLib.Pools;
using System.Collections.Generic;
using System.Reflection.Emit;
using static HarmonyLib.AccessTools;

namespace Nebuli.Events.Patches.Player;

[HarmonyPatch(typeof(SingleBulletHitreg), nameof(SingleBulletHitreg.ServerPerformShot))]
internal class PlayerShot
{
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> OnShot(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        List<CodeInstruction> newInstructions = EventManager.CheckPatchInstructions<PlayerShot>(58, instructions);

        Label retLabel = generator.DefineLabel();

        newInstructions.InsertRange(0, new CodeInstruction[]
        {
            new(OpCodes.Ldarg_0),
            new(OpCodes.Callvirt, PropertyGetter(typeof(SingleBulletHitreg), nameof(StandardHitregBase.Hub))),
            new(OpCodes.Ldarg_0),
            new(OpCodes.Callvirt, PropertyGetter(typeof(SingleBulletHitreg), nameof(SingleBulletHitreg.Firearm))),
            new(OpCodes.Newobj, GetDeclaredConstructors(typeof(PlayerShotEventArgs))[0]),
            new(OpCodes.Dup),
            new(OpCodes.Call, Method(typeof(PlayerHandlers), nameof(PlayerHandlers.OnShot))),
            new(OpCodes.Callvirt, PropertyGetter(typeof(PlayerShotEventArgs), nameof(PlayerShotEventArgs.IsCancelled))),
            new(OpCodes.Brtrue_S, retLabel)
        });

        newInstructions[newInstructions.Count - 1].labels.Add(retLabel);

        foreach (CodeInstruction instruction in newInstructions)
            yield return instruction;

        ListPool<CodeInstruction>.Shared.Return(newInstructions);
    }
}