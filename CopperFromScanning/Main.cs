﻿using System.Reflection;
using HarmonyLib;
using QModManager.API.ModLoading;

namespace CopperFromScanning
{
    [QModCore]
    public class Main
    {
        [QModPatch]
        public static void Load()
        {
            var assembly = Assembly.GetExecutingAssembly();
            new Harmony($"MrPurple6411_{assembly.GetName().Name}").PatchAll(assembly);
        }
    }
}