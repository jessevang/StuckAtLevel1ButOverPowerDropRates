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
        private uint realCurrentDay;
        /*********
       ** Public methods
       *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.DayStarted += onStartOfDay;
            helper.Events.GameLoop.DayEnding += onEndOfDay;

            HarmonyPatcher.Apply(this,
                new ObjectPatches()
           
            );


        }

        //Code to change day from 1-4 days to day 5 to open up the mines.
        private void onStartOfDay(object sender, EventArgs e)
        {
            Console.WriteLine("CurrentDaysPlayed : " + Game1.stats.DaysPlayed);
            this.realCurrentDay = (uint)Game1.stats.DaysPlayed;
            // Check the number of days played
            if (Game1.stats.DaysPlayed <= 5)
                {
                    Game1.stats.DaysPlayed = 5; // Set the days played to 5
                    Console.WriteLine("Days played set :" + Game1.stats.DaysPlayed);
                }

        }

        private void onEndOfDay(object sender, EventArgs e)
        {
            Console.WriteLine("CurrentDaysPlayed : " + Game1.stats.DaysPlayed);
            Game1.stats.DaysPlayed = this.realCurrentDay;
            Console.WriteLine("Current Day has been set to : " + Game1.stats.DaysPlayed);

        }







    }
}

