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

   



    private static void Before_MonsterDrop(GameLocation __instance, Monster monster, int x, int y, Farmer who)
        {

        

        NetCollection<Debris> debris = new NetCollection<Debris>();
        IList<int> objects = monster.objectsToDrop;
        Vector2 playerPosition = new Vector2(Game1.player.GetBoundingBox().Center.X, Game1.player.GetBoundingBox().Center.Y);
        List<Item> extraDrops = monster.getExtraDropItems();
        if (Game1.player.isWearingRing(526))
        {
            // Add this logic to original code to run code to run loot 20 times and adds item to list of items to drop
           for (int i = 0; i < 24; i++) { 
                string result1 = "";
                Game1.content.Load<Dictionary<string, string>>("Data\\Monsters").TryGetValue(monster.Name, out result1);
                if (result1 != null && result1.Length > 0)
                {
                    string[] objectsSplit = result1.Split('/')[6].Split(' ');
                    for (int l = 0; l < objectsSplit.Length; l += 2)
                    {
                        if (Game1.random.NextDouble() < Convert.ToDouble(objectsSplit[l + 1]))
                        {
                            objects.Add(Convert.ToInt32(objectsSplit[l]));
 

                        }
                    }
                }
            }
        }

        else {
            //Add this logic to original code to run code to run loot 10 times and adds item to list of items to drop
            for (int i = 0; i < 12; i++)
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
                            objects.Add(Convert.ToInt32(objectsSplit[l]));


                        }
                    }
                }
            }
        }


        // code creates the items and drops onto screen.
        for (int k = 0; k < objects.Count; k++)
        {
            int objectToAdd = objects[k];
            if (objectToAdd < 0)
            {
                debris.Add(monster.ModifyMonsterLoot(new Debris(Math.Abs(objectToAdd), Game1.random.Next(1, 4), new Vector2(x, y), playerPosition)));

            }
            else
            {
                debris.Add(monster.ModifyMonsterLoot(new Debris(objectToAdd, new Vector2(x, y), playerPosition)));

            }
        }


        return;



    }



    }


