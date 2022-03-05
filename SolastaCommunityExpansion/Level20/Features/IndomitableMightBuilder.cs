﻿using System.Collections.Generic;
using SolastaCommunityExpansion.Builders;
using SolastaCommunityExpansion.Builders.Features;
using SolastaCommunityExpansion.CustomFeatureDefinitions;
using SolastaModApi.Extensions;

namespace SolastaCommunityExpansion.Level20.Features
{
    internal sealed class IndomitableMightBuilder : FeatureDefinitionBuilder<IndomitableMight, IndomitableMightBuilder>
    {
        private const string IndomitableMightName = "ZSBarbarianIndomitableMight";
        private const string IndomitableMightGuid = "2a0e9082-c81d-4d02-800a-92f04fbe85dc";

        private IndomitableMightBuilder(string name, string guid) : base(name, guid)
        {
            var guiPresentationBuilder = new GuiPresentationBuilder(
                "Feature/&BarbarianIndomitableMightTitle",
                "Feature/&BarbarianIndomitableMightDescription");

            Definition.SetGuiPresentation(guiPresentationBuilder.Build());
        }

        private static FeatureDefinition CreateAndAddToDB(string name, string guid)
        {
            return new IndomitableMightBuilder(name, guid).AddToDB();
        }

        internal static readonly FeatureDefinition IndomitableMight =
            CreateAndAddToDB(IndomitableMightName, IndomitableMightGuid);
    }

    internal sealed class IndomitableMight : FeatureDefinition, IChangeAbilityCheck
    {
        public int MinRoll(
            RulesetCharacter character,
            int baseBonus,
            int rollModifier,
            string abilityScoreName,
            string proficiencyName,
            List<RuleDefinitions.TrendInfo> advantageTrends,
            List<RuleDefinitions.TrendInfo> modifierTrends)
        {
            if (character == null || abilityScoreName != AttributeDefinitions.Strength)
            {
                return 1;
            }

            return character.GetAttribute(AttributeDefinitions.Strength).CurrentValue;
        }
    }
}
