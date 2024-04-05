using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley.Monsters;
using StardewValley;
using System.Collections.Generic;
using System;
using HarmonyLib;
using StardewValley.Locations;
using Spacechase.Shared.Patching;
using Netcode;
using static HarmonyLib.Code;
using System.Reflection;
using System.Text.Json.Nodes;
using System.IO;
using System.Xml.Linq;
using Newtonsoft.Json;
using StardewValley.Network;
using Newtonsoft.Json.Linq;

/// <summary>Applies Harmony patches to <see cref="GameLocation"/>.</summary>


internal class ObjectPatches : BasePatcher
    {

        public override void Apply(Harmony harmony, IMonitor monitor)
        {

            harmony.Patch(
                original: this.RequireMethod<GameLocation>(nameof(GameLocation.monsterDrop)),
                prefix: this.GetHarmonyMethod(nameof(Before_MonsterDrop))

            );

        }


    public class RootObject
    {
        public string url_short { get; set; }
        public string url_long { get; set; }
        public int type { get; set; }
    }


    private static void Before_MonsterDrop(GameLocation __instance, Monster monster, int x, int y, Farmer who)
        {

        //Add code for reading config file
        int NumberOfDrops = 12;
        int NumberOfDropsWithRing = 24;
        int counter = 0;
        // Get the current directory
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
               
                if (counter == 0)
                {
                    NumberOfDrops = (int)((JProperty)item).Value;

                }
                if (counter == 1)
                {
                    NumberOfDropsWithRing = (int)((JProperty)item).Value;
                }
                counter++;
                
              


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


        //Updated original code
        NetCollection<Debris> debris = new NetCollection<Debris>();
        IList<String> objects = monster.objectsToDrop;
        Vector2 playerPosition = new Vector2(Game1.player.GetBoundingBox().Center.X, Game1.player.GetBoundingBox().Center.Y);
        List<Item> extraDrops = monster.getExtraDropItems();
        if (who.isWearingRing("526") && DataLoader.Monsters(Game1.content).TryGetValue(monster.Name, out var result))
        {
            // Add this logic to original code to run code to run loot 24 times and adds item to list of items to drop
            for (int k = 0; k < NumberOfDropsWithRing; k++)
            {
                string[] objectsSplit = ArgUtility.SplitBySpace(result.Split('/')[6]);
                for (int i = 0; i < objectsSplit.Length; i += 2)
                {
                    if (Game1.random.NextDouble() < Convert.ToDouble(objectsSplit[i + 1]))
                    {
                        objects.Add(objectsSplit[i]);
                    }
                }
            }
        }


        else {
            //Add this logic to original code to run code to run loot 10 times and adds item to list of items to drop
            for (int j = 0; j < NumberOfDrops; j++)
            {
                string result1 = "";
                Game1.content.Load<Dictionary<string, string>>("Data\\Monsters").TryGetValue(monster.Name, out result1);
                if (result1 != null && result1.Length > 0)
                {
                    string[] objectsSplit = result1.Split('/')[6].Split(' ');
                    for (int l = 0; l < objectsSplit.Length; l += 2)
                    {
                        if (Game1.random.NextDouble() < Convert.ToDouble(objectsSplit[l + 1]))
                        {
                            objects.Add(objectsSplit[l]);


                        }
                    }
                }
            }
        }


        // code creates the items and drops onto screen.
        List<Debris> debrisToAdd = new List<Debris>();
        for (int i = 0; i < objects.Count; i++)
        {
            string objectToAdd = objects[i];
            if (objectToAdd != null && objectToAdd.StartsWith('-') && int.TryParse(objectToAdd, out var parsedIndex))
            {
                debrisToAdd.Add(monster.ModifyMonsterLoot(new Debris(Math.Abs(parsedIndex), Game1.random.Next(1, 4), new Vector2(x, y), playerPosition)));
            }
            else
            {
                debrisToAdd.Add(monster.ModifyMonsterLoot(new Debris(objectToAdd, new Vector2(x, y), playerPosition)));
            }
        }


        return;



    }



    }


