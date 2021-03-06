﻿using System.Reflection;
using HarmonyLib;
using QModManager.API.ModLoading;
using SMLHelper.V2.Handlers;
using BetterACU.Configuration;

namespace BetterACU
{
    [QModCore]
    public static class Main
    {
        internal static Config config { get; private set; } 

        [QModPatch]
        public static void Load()
        {
            config = OptionsPanelHandler.RegisterModOptions<Config>();
            IngameMenuHandler.RegisterOnSaveEvent(config.Save);

            var assembly = Assembly.GetExecutingAssembly();
            new Harmony($"MrPurple6411_{assembly.GetName().Name}").PatchAll(assembly);
        }
    }
}