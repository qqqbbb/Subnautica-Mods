﻿using HarmonyLib;
using QModManager.API.ModLoading;
using BuilderArms.Configuration;
using SMLHelper.V2.Handlers;
using System.Reflection;

namespace BuilderArms
{
    [QModCore]
    public static class Main
    {
        internal static Config config { get; } = OptionsPanelHandler.RegisterModOptions<Config>();

        [QModPatch]
        public static void Load()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Harmony.CreateAndPatchAll(assembly, $"MrPurple6411_{assembly.GetName().Name}");
        }
    }
}