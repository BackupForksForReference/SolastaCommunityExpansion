﻿using ModKit;
using SolastaCommunityExpansion.Models;

namespace SolastaCommunityExpansion.Viewers.Displays
{
    internal static class CampaignsAndLocationsDisplay
    {
        internal static void DisplayCampaignsAndLocations()
        {
            bool toggle;
            int intValue;

            UI.Label("");

            toggle = Main.Settings.EnableSaveByLocation;
            if (UI.Toggle("Enable save by campaigns / locations", ref toggle, UI.AutoWidth()))
            {
                Main.Settings.EnableSaveByLocation = toggle;
            }

            toggle = Main.Settings.EnableTelemaCampaign;
            if (UI.Toggle("Enable the Telema Kickstarter demo location", ref toggle))
            {
                Main.Settings.EnableTelemaCampaign = toggle;
            }

            UI.Label("");

            toggle = Main.Settings.FollowCharactersOnTeleport;
            if (UI.Toggle("Camera follows teleported character(s)", ref toggle, UI.AutoWidth()))
            {
                Main.Settings.FollowCharactersOnTeleport = toggle;
            }

            UI.Label("");

            toggle = Main.Settings.OverrideMinMaxLevel;
            if (UI.Toggle("Override required min / max level", ref toggle))
            {
                Main.Settings.OverrideMinMaxLevel = toggle;
            }

            UI.Label("");

            intValue = Main.Settings.OverridePartySize;
            if (UI.Slider("Override party size ".white() + "[only in custom dungeons]".italic().yellow(), ref intValue, DungeonMakerContext.MIN_PARTY_SIZE, DungeonMakerContext.MAX_PARTY_SIZE, DungeonMakerContext.GAME_PARTY_SIZE, "", UI.AutoWidth()))
            {
                Main.Settings.OverridePartySize = intValue;
            }

            UI.Label("");

            intValue = Main.Settings.MaxBackupFilesPerLocationCampaign;
            if (UI.Slider("Max. backup files per location or campaign".white(), ref intValue, 0, 20, 10))
            {
                Main.Settings.MaxBackupFilesPerLocationCampaign = intValue;
            }

            UI.Label("");
            UI.Label(". Backup files are saved under " + "GAME_FOLDER/Mods/SolastaCommunityExpansion/DungeonMakerBackups".italic().yellow());

            UI.Label("");
        }
    }
}
