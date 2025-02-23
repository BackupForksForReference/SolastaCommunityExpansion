﻿using HarmonyLib;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using static SolastaModApi.DatabaseHelper.QuestTreeDefinitions;
using SolastaCommunityExpansion.Helpers;
using System.Collections.Generic;

namespace SolastaCommunityExpansion.Patches.Cheats
{
    // use this patch to enable the No Experience on Level up cheat
    [HarmonyPatch(typeof(RulesetCharacterHero), "CanLevelUp", MethodType.Getter)]
    [SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Patch")]
    internal static class RulesetCharacterHero_CanLevelUp
    {
        internal static bool Prefix(RulesetCharacterHero __instance, ref bool __result)
        {
            if (Main.Settings.NoExperienceOnLevelUp)
            {
                var levelCap = Main.Settings.EnableLevel20 ? Models.Level20Context.MOD_MAX_LEVEL : Models.Level20Context.GAME_MAX_LEVEL - 1;

                __result = __instance.ClassesHistory.Count < levelCap;

                return false;
            }
            if (Main.Settings.EnableLevel20)
            {
                var levelCap = Main.Settings.EnableLevel20 ? Models.Level20Context.MOD_MAX_LEVEL : Models.Level20Context.GAME_MAX_LEVEL - 1;
                // If the game doesn't know how much XP to reach the next level it uses -1 to determine if the character can level up.
                // When a character is level 20, this ends up meaning the character can now level up forever unless we stop it here.
                if (__instance.ClassesHistory.Count >= levelCap)
                {
                    __result = false;
                    return false;
                }
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(RulesetCharacterHero), "GrantExperience")]
    [SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Patch")]
    internal static class RulesetCharacterHero_GrantExperience
    {
        internal static void Prefix(ref int experiencePoints)
        {
            if (Main.Settings.MultiplyTheExperienceGainedBy != 100 && Main.Settings.MultiplyTheExperienceGainedBy > 0)
            {
                var original = experiencePoints;

                experiencePoints = (int)Math.Round(experiencePoints * Main.Settings.MultiplyTheExperienceGainedBy / 100.0f, MidpointRounding.AwayFromZero);

                Main.Log($"GrantExperience: Multiplying experience gained by {Main.Settings.MultiplyTheExperienceGainedBy}%. Original={original}, modified={experiencePoints}.");
            }
        }
    }

    /// <summary>
    /// This is *only* called from FunctorGrantExperience as of 1.1.12. 
    /// By default don't modify the return value from this method.  This means requests to level up will be scaled by MultiplyTheExperienceGainedBy.
    /// At certain quest specific points the level up must not be scaled.
    /// </summary>
    [HarmonyPatch(typeof(RulesetCharacterHero), "ComputeNeededExperienceToReachLevel")]
    [SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Patch")]
    internal static class RulesetCharacterHero_ComputeNeededExperienceToReachLevel
    {
        internal static void Postfix(ref int __result)
        {
            if (Main.Settings.MultiplyTheExperienceGainedBy != 100 && Main.Settings.MultiplyTheExperienceGainedBy > 0)
            {
                var gameQuestService = ServiceRepository.GetService<IGameQuestService>();

#if DEBUG
                gameQuestService?.ActiveQuests.ForEach(x => Main.Log($"Quest: {x.QuestTreeDefinition.Name}"));
#endif

                // Level up essential for Caer_Cyflen_Quest_AfterTutorial.
                bool levelupRequired = gameQuestService?.ActiveQuests?.Any(x => x.QuestTreeDefinition == Caer_Cyflen_Quest_AfterTutorial) == true;

                if (levelupRequired)
                {
                    // Adjust the amount of XP required in order to cancel the adjustment made in RulesetCharacterHero_GrantExperience_Patch.
                    // This results in a call from FunctorGrantExperience with GrantExperienceMode.ReachLevel working as expected and 
                    // the relevant quest step is then not blocked.
                    var original = __result;

                    __result = (int)Math.Round(__result / (Main.Settings.MultiplyTheExperienceGainedBy / 100.0f), MidpointRounding.AwayFromZero);

                    Main.Log($"ComputeNeededExperienceToReachLevel: Dividing experience gained by {Main.Settings.MultiplyTheExperienceGainedBy}%. Original={original}, modified={__result}.");
                }
            }
        }
    }

    [HarmonyPatch(typeof(RulesetCharacterHero), "EnumerateUsableRitualSpells")]
    [SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Patch")]
    internal static class RestModuleHitDice_EnumerateUsableRitualSpells_Patch
    {
        internal static void Postfix(RulesetCharacterHero __instance, RuleDefinitions.RitualCasting ritualType, List<SpellDefinition> ritualSpells)
        {
            if ((ExtraRitualCasting)ritualType != ExtraRitualCasting.Known) { return; }

            var spellRepertoire = __instance.SpellRepertoires
                .Where(r => r.SpellCastingFeature.SpellReadyness == RuleDefinitions.SpellReadyness.AllKnown)
                .FirstOrDefault(r => r.SpellCastingFeature.SpellKnowledge == RuleDefinitions.SpellKnowledge.Selection);

            if (spellRepertoire == null) { return; }

            ritualSpells.AddRange(spellRepertoire.KnownSpells
                .Where(s => s.Ritual)
                .Where(s => spellRepertoire.MaxSpellLevelOfSpellCastingLevel >= s.SpellLevel));

            if (spellRepertoire.AutoPreparedSpells == null) { return; }

            ritualSpells.AddRange(spellRepertoire.AutoPreparedSpells
                .Where(s => s.Ritual)
                .Where(s => spellRepertoire.MaxSpellLevelOfSpellCastingLevel >= s.SpellLevel));

        }
    }
}
