using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using Nebuli.Events.EventArguments.SCPs.Scp939;
using Nebuli.Events.Handlers;
using NorthwoodLib.Pools;
using PlayerRoles.PlayableScps.Scp939.Mimicry;
using static HarmonyLib.AccessTools;

namespace Nebuli.Events.Patches.SCPs.Scp939;

[HarmonyPatch(typeof(MimicryRecorder), nameof(MimicryRecorder.SaveRecording))]
internal class SaveVoice
{
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> OnSaveVoice(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        List<CodeInstruction> newInstructions = EventManager.CheckPatchInstructions<SaveVoice>(37, instructions);
        
        Label retLabel = generator.DefineLabel();
        
        newInstructions.InsertRange(0, new CodeInstruction[]
        {
            new(OpCodes.Ldarg_0),
            new(OpCodes.Callvirt, PropertyGetter(typeof(MimicryRecorder), nameof(MimicryRecorder.Owner))),
            new(OpCodes.Ldarg_1),
            new(OpCodes.Newobj, GetDeclaredConstructors(typeof(Scp939SavePlayerVoiceEvent))[0]),
            new(OpCodes.Dup),
            new(OpCodes.Call, Method(typeof(Scp939Handlers), nameof(Scp939Handlers.OnSaveVoice))),
            new(OpCodes.Callvirt, PropertyGetter(typeof(Scp939SavePlayerVoiceEvent), nameof(Scp939SavePlayerVoiceEvent.IsCancelled))),
            new(OpCodes.Brtrue_S, retLabel),
        });
        
        newInstructions[newInstructions.Count - 1].labels.Add(retLabel);
        
        foreach (CodeInstruction instruction in newInstructions)
            yield return instruction;
        
        ListPool<CodeInstruction>.Shared.Return(newInstructions);
    }
}