﻿using HarmonyLib;
using System.Diagnostics.CodeAnalysis;

namespace SolastaCommunityExpansion.Patches.GameUiLevelUp
{
    [HarmonyPatch(typeof(CharacterStageClassSelectionPanel), "Compare")]
    [SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Patch")]
    internal static class CharacterStageClassSelectionPanel_Compare
    {
        internal static void Postfix(CharacterClassDefinition left, CharacterClassDefinition right, ref int __result)
        {
            if (!Main.Settings.EnableSortingClasses)
            {
                return;
            }

            __result = left.FormatTitle().CompareTo(right.FormatTitle());
        }
    }
}
