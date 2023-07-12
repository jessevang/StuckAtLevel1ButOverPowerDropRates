using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using HarmonyLib;
using Spacechase.Shared.Patching;

using StardewValley.Characters;

namespace StuckAtLevel1ButOverPowerDropRates
{
    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {

        public override void Entry(IModHelper helper)
        {
          

            
            HarmonyPatcher.Apply(this,
                new ObjectPatches()
           
            );

            
        }

       


    }
}

