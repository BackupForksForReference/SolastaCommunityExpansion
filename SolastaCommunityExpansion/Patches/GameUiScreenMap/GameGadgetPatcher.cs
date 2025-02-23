﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using HarmonyLib;
using SolastaCommunityExpansion.Helpers;
using TA;

namespace SolastaCommunityExpansion.Patches.GameUiScreenMap
{
    // hides certain element from the map on custom dungeons unless already discovered
    [HarmonyPatch(typeof(GameGadget), "ComputeIsRevealed")]
    [SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Patch")]
    internal static class GameGadget_ComputeIsRevealed
    {


        internal static void Postfix(GameGadget __instance, ref bool ___revealed, ref bool __result)
        {
            if (!__instance.Revealed || Gui.GameLocation.UserLocation == null || !Main.Settings.EnableAdditionalIconsOnLevelMap)
            {
                return;
            }

            var userGadget = Gui.GameLocation.UserLocation.UserRooms
                .SelectMany(a => a.UserGadgets)
                .FirstOrDefault(b => b.UniqueName == __instance.UniqueNameId);

            if (userGadget == null || !Models.GameUiContext.IsGadgetExit(userGadget.GadgetBlueprint))
            {
                return;
            }

            // reverts the revealed state and recalculates it
            ___revealed = false;
            __result = false;

            var x = (int)__instance.FeedbackPosition.x;
            var y = (int)__instance.FeedbackPosition.z;

            var feedbackPosition = new int3(x, 0, y);
            var referenceBoundingBox = new BoxInt(feedbackPosition, feedbackPosition);

            var gridAccessor = GridAccessor.Default;

            foreach (var position in referenceBoundingBox.EnumerateAllPositionsWithin())
            {
                if (gridAccessor.Visited(position))
                {
                    var gameLocationService = ServiceRepository.GetService<IGameLocationService>();
                    var worldGadgets = gameLocationService.WorldLocation.WorldSectors.SelectMany(ws => ws.WorldGadgets);
                    var worldGadget = worldGadgets.FirstOrDefault(wg => wg.GameGadget == __instance);

                    var isInvisible = __instance.IsInvisible();
                    var isEnabled = __instance.IsEnabled();

                    GameLocationManager_ReadyLocation.SetTeleporterGadgetActiveAnimation(worldGadget, isEnabled && !isInvisible);

                    ___revealed = true;
                    __result = true;

                    break;
                }
            }
        }
    }

    [HarmonyPatch(typeof(GameGadget), "SetCondition")]
    [SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Patch")]
    internal static class GameGadget_SetCondition
    {
        internal static void Postfix(GameGadget __instance, int conditionIndex, bool state, List<string> ___conditionNames)
        {
            if (!Main.Settings.HideExitAndTeleporterGizmosIfNotDiscovered)
            {
                return;
            }

            if (conditionIndex >= 0 && conditionIndex < ___conditionNames.Count)
            {
                var param = ___conditionNames[conditionIndex];

                Main.Log($"GameGadget_SetCondition {__instance.UniqueNameId}: {param} state = {state}");

#if DEBUG
                //Main.Log("GameGadget_SetCondition: " + string.Join(",", ___conditionNames.Select(n => $"{n}={__instance.CheckConditionName(n, true, false)}")));
#endif

                if ((param == GameGadgetExtensions.Enabled || param == GameGadgetExtensions.ParamEnabled)
                    && __instance.UniqueNameId.StartsWith("Teleport"))
                {
                    var service = ServiceRepository.GetService<IGameLocationService>();

                    if (service != null)
                    {
                        var worldGadget = service.WorldLocation.WorldSectors
                            .SelectMany(ws => ws.WorldGadgets)
                            .FirstOrDefault(wg => wg.GameGadget == __instance);

                        if (worldGadget != null)
                        {
                            Main.Log($"GameGadget_SetCondition-setting-animation {__instance.UniqueNameId}: {state}");

                            GameLocationManager_ReadyLocation.SetTeleporterGadgetActiveAnimation(worldGadget, state);
                        }
                    }
                }
            }
            else
            {
                Main.Log($"GameGadget_SetCondition {__instance.UniqueNameId}: condition index out of range.");
            }
        }
    }
}
