using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using HarmonyLib;
using Spacechase.Shared.Patching;

using StardewValley.Characters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using Mono.CompilerServices.SymbolWriter;
using StardewValley.Monsters;
using System.ComponentModel;

namespace StuckAtLevel1ButOverPowerDropRates
{
    
   

    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {
        public uint NumberOfDrops;
        public uint NumberOfDropsWithRing;
        public const string ConfigFileName = "config.json";

        private class Config
        {

            public int withoutBurglarRing { get; set; } = 12; 
            public int withBurglarRing { get; set; } = 24; 

        }

        /*********
       ** Public methods
       *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            var config = helper.ReadConfig<Config>() ?? new Config();

            //This Harmony Patcher turns on more monster drops
            HarmonyPatcher.Apply(this,
            new ObjectPatches() 
            );

        }






    }
}

