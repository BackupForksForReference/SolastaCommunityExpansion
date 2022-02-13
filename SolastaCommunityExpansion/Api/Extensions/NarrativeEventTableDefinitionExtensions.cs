using SolastaModApi.Infrastructure;
using AK.Wwise;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using System;
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;
using TA.AI;
using TA;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using  static  ActionDefinitions ;
using  static  TA . AI . DecisionPackageDefinition ;
using  static  TA . AI . DecisionDefinition ;
using  static  RuleDefinitions ;
using  static  BanterDefinitions ;
using  static  Gui ;
using  static  GadgetDefinitions ;
using  static  BestiaryDefinitions ;
using  static  CursorDefinitions ;
using  static  AnimationDefinitions ;
using  static  FeatureDefinitionAutoPreparedSpells ;
using  static  FeatureDefinitionCraftingAffinity ;
using  static  CharacterClassDefinition ;
using  static  CreditsGroupDefinition ;
using  static  SoundbanksDefinition ;
using  static  CampaignDefinition ;
using  static  GraphicsCharacterDefinitions ;
using  static  GameCampaignDefinitions ;
using  static  FeatureDefinitionAbilityCheckAffinity ;
using  static  TooltipDefinitions ;
using  static  BaseBlueprint ;
using  static  MorphotypeElementDefinition ;

namespace SolastaModApi.Extensions
{
    /// <summary>
    /// This helper extensions class was automatically generated.
    /// If you find a problem please report at https://github.com/SolastaMods/SolastaModApi/issues.
    /// </summary>
    [TargetType(typeof(NarrativeEventTableDefinition)), GeneratedCode("Community Expansion Extension Generator", "1.0.0")]
    public static partial class NarrativeEventTableDefinitionExtensions
    {
        public static T AddEventDescriptions<T>(this T entity,  params  NarrativeEventDescription [ ]  value)
            where T : NarrativeEventTableDefinition
        {
            AddEventDescriptions(entity, value.AsEnumerable());
            return entity;
        }

        public static T AddEventDescriptions<T>(this T entity, IEnumerable<NarrativeEventDescription> value)
            where T : NarrativeEventTableDefinition
        {
            entity.EventDescriptions.AddRange(value);
            return entity;
        }

        public static T ClearEventDescriptions<T>(this T entity)
            where T : NarrativeEventTableDefinition
        {
            entity.EventDescriptions.Clear();
            return entity;
        }

        public static T SetEventDescriptions<T>(this T entity,  params  NarrativeEventDescription [ ]  value)
            where T : NarrativeEventTableDefinition
        {
            SetEventDescriptions(entity, value.AsEnumerable());
            return entity;
        }

        public static T SetEventDescriptions<T>(this T entity, IEnumerable<NarrativeEventDescription> value)
            where T : NarrativeEventTableDefinition
        {
            entity.EventDescriptions.SetRange(value);
            return entity;
        }
    }
}