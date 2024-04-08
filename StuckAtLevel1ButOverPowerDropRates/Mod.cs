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

namespace StuckAtLevel1ButOverPowerDropRates
{
    
   

    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {
        public uint realCurrentDay;
        public Boolean TurnOn_EarlyingMining = false;
        public Boolean TurnOn_MoreMonsterDrops = false;  //Will be handled in object patches .cs file as if added here the code will no longer trigger
        public uint NumberOfDrops;
        public uint NumberOfDropsWithRing;
        /*********
       ** Public methods
       *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.GameLaunched += onGameLaunch; //loads config file on game launch
            helper.Events.GameLoop.DayStarted += onStartOfDay;
            helper.Events.GameLoop.DayEnding += onEndOfDay;


            //This Harmony Patcher turns on more monster drops
            HarmonyPatcher.Apply(this,
            new ObjectPatches() 
            );




        }


        //reads config file and sets and turns on certain mod features
        private void onGameLaunch(object sender, GameLaunchedEventArgs e)
        {
            //read Config File
            string filePath = Directory.GetCurrentDirectory() + "\\Mods\\StuckAtLevel1ButOverPowerDropRates\\config.json";

            try
            {

                // Read the JSON file
                string jsonString = File.ReadAllText(filePath);

                // Deserialize the JSON string into a dynamic object
                dynamic jsonObj = JsonConvert.DeserializeObject(jsonString);

                // Access the properties of the JSON object
                foreach (var item in jsonObj)
                {
                    //Console.WriteLine($"{((JProperty)item).Name}: {((JProperty)item).Value}");

                    string propertyName = ((JProperty)item).Name.ToString();
                    if (propertyName == "NumberOfDrops")
                    {
                        NumberOfDrops = (uint)((JProperty)item).Value;
                        //Console.WriteLine("NumberOfDrops has been set to :" + NumberOfDrops);

                    }
                    else if (propertyName == "NumberOfDropsWithRing")
                    {
                        NumberOfDropsWithRing = (uint)((JProperty)item).Value;
                        //Console.WriteLine("NumberOfDropsWithRing has been set to :" + NumberOfDropsWithRing);
                    }
                    else if (propertyName == "TurnOnEarlyMining")
                    {
                        if ((uint)((JProperty)item).Value == 1)
                        {
                            TurnOn_EarlyingMining = true;
                            //Console.WriteLine("TurnOnEarlyMining has been set to :" + true);
                        }
                        else if ((uint)((JProperty)item).Value == 0)
                        {
                            TurnOn_EarlyingMining = false;
                            //Console.WriteLine("TurnOnEarlyMining has been set to :" + false);
                        }

                            
                    }
                    else if (propertyName == "TurnOnMoreMonsterDrops")
                    {
                        if ((uint)((JProperty)item).Value == 1)
                        {
                            TurnOn_MoreMonsterDrops = true;
                            //Console.WriteLine("TurnOnMoreMonsterDrops has been set to :" + true);
                        }
                        else if ((uint)((JProperty)item).Value == 0)
                        {
                            TurnOn_MoreMonsterDrops = false;
                            //Console.WriteLine("TurnOnMoreMonsterDrops has been set to :" + false);
                        }
                    }

                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"File '{filePath}' not found.");
            }
            catch (JsonException)
            {
                Console.WriteLine($"Invalid JSON format in '{filePath}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }


            
        }

        //Code to change day 1,2,3,4 to day 5 to open up the mines.
        private void onStartOfDay(object sender, EventArgs e)
        {
            if (!TurnOn_EarlyingMining)
                return;

            //Console.WriteLine("CurrentDaysPlayed : " + Game1.stats.DaysPlayed);
            this.realCurrentDay = (uint)Game1.stats.DaysPlayed;
            // Check the number of days played
            if (Game1.stats.DaysPlayed <= 5)
                {
                    Game1.stats.DaysPlayed = 5; // Set the days played to 5
                    //Console.WriteLine("Days played set :" + Game1.stats.DaysPlayed);
                }

        }

        //resets the day back to the original current day at the end of the day so calendar
        private void onEndOfDay(object sender, EventArgs e)
        {
            if (!TurnOn_EarlyingMining)
                return;
           // Console.WriteLine("CurrentDaysPlayed : " + Game1.stats.DaysPlayed);
            Game1.stats.DaysPlayed = this.realCurrentDay;
            //Console.WriteLine("Current Day has been set to : " + Game1.stats.DaysPlayed);

        }







    }
}

