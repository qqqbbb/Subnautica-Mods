﻿using Harmony;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Options;
using SMLHelper.V2.Utility;
using System;
using System.Reflection;
using UnityEngine;
using Story;

namespace Time_Eternal
{
    public static class Main
    {
        public static void Load()
        {
            Config.Load();
            OptionsPanelHandler.RegisterModOptions(new Options());
            try
            {
                HarmonyInstance.Create("MrPurple6411.Eternal_Sunshine").PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }

    [HarmonyPatch(typeof(DayNightCycle))]
    [HarmonyPatch("GetDayNightCycleTime")]
    internal class DayNightCycle_GetDayNightCycleTime_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(DayNightCycle __instance)
        {
            if (Time_Eternal.Config.freezeTimeChoice == 1)
            {
                //always day
                __instance.sunRiseTime = -1000.0f;
                __instance.sunSetTime = 1000.0f;
                return true;
            }
            else if (Time_Eternal.Config.freezeTimeChoice == 2)
            {
                //always night
                __instance.sunRiseTime = 1000.0f;
                __instance.sunSetTime = -1000.0f;
                return true;
            }
            else
            { 
                //9pm to 3am game default
                __instance.sunRiseTime = 0.125f;
                __instance.sunSetTime = 0.875f;
                return true;
            }
        }
    }

    public static class Config
    {
        public static int freezeTimeChoice;

        public static void Load()
        {
            freezeTimeChoice = PlayerPrefs.GetInt("DayNightToggle", 0);
        }
    }

    public class Options : ModOptions
    {
        public Options() : base("Time Eternal")
        {
            ChoiceChanged += Options_DayToggleChanged;
        }

        public void Options_DayToggleChanged(object sender, ChoiceChangedEventArgs e)
        {
            if (e.Id != "DayNightToggle") return;
            Config.freezeTimeChoice = e.Index;
            PlayerPrefs.SetInt("DayNightToggle", e.Index);
        }

        public override void BuildModOptions()
        {
            AddChoiceOption("DayNightToggle", "Freeze Lighting", new string[] { "Normal", "Noon", "MidNight" }, Config.freezeTimeChoice);
        }
    }
}
