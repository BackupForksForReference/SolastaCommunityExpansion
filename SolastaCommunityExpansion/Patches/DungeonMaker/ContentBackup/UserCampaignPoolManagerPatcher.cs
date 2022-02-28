﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection.Emit;
using HarmonyLib;

namespace SolastaCommunityExpansion.Patches.DungeonMaker.ContentBackup
{
    // this patch allows the last X campaign files to be backed up in the mod folder
    //
    // this patch shouldn't be protected
    //
    [HarmonyPatch(typeof(UserCampaignPoolManager), "SaveUserCampaign")]
    [SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Patch")]
    internal static class UserCampaignPoolManager_SaveUserCampaign
    {
        internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var deleteMethod = typeof(File).GetMethod("Delete");
            var backupAndDeleteMethod = typeof(Models.DungeonMakerContext).GetMethod("BackupAndDelete");

            foreach (CodeInstruction instruction in instructions)
            {
                if (instruction.Calls(deleteMethod))
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_1);
                    yield return new CodeInstruction(OpCodes.Call, backupAndDeleteMethod);
                }
                else
                {
                    yield return instruction;
                }
            }
        }
    }
}
