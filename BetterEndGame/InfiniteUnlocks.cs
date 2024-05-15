using EquinoxsModUtils;
using Mirror;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TriangleNet;
using UnityEngine;
using Voxeland5;

namespace BetterEndGame
{
    public static class InfiniteUnlocks
    {
        // Objects & Variables
        public static InfiniteUnlock miningSpeed = new InfiniteUnlock() {
            id = -1,
            name = "MDR Speed ∞",
            tier = 0,
            boost = 5,
            increaseModifier = true
        };
        public static InfiniteUnlock oreConsumption = new InfiniteUnlock() {
            id = -1,
            name = "Ore Consumption ∞",
            tier = 0,
            boost = 10,
            increaseModifier = false
        };
        public static InfiniteUnlock fuelEfficiency = new InfiniteUnlock() {
            id = -1,
            name = "BioDense ∞",
            tier = 0,
            boost = 10,
            increaseModifier = true
        };
        public static InfiniteUnlock powerEfficiency = new InfiniteUnlock() {
            id = -1,
            name = "Power Trim ∞",
            tier = 0,
            boost = 5,
            increaseModifier = false
        };
        public static InfiniteUnlock walkSpeed = new InfiniteUnlock() {
            id = -1,
            name = "Suit Speed ∞",
            tier = 0,
            boost = 5,
            increaseModifier = true
        };
        public static InfiniteUnlock moleSpeed = new InfiniteUnlock() {
            id = -1,
            name = "M.O.L.E. Speed ∞",
            tier = 0,
            boost = 5,
            increaseModifier = true
        };

        private static Dictionary<int, InfiniteUnlock> infiniteUnlocks = new Dictionary<int, InfiniteUnlock>();

        private static string dataFolder = $"{Application.persistentDataPath}/BetterEndGame";

        // Public Functions
     
        public static void CreateUnlocks() {
            SetDescriptions();
            CreateMinerSpeedUnlock();
            CreateOreConsumptionUnlock();
            CreateFuelEfficiencyUnlock();
            CreatePowerEfficiencyUnlock();
            CreateWalkSpeedUnlock();
            CreateMoleSpeedUnlock();
        }

        public static void FetchUnlocks() {
            miningSpeed.id = ModUtils.GetUnlockByName(miningSpeed.name).uniqueId;
            oreConsumption.id = ModUtils.GetUnlockByName(oreConsumption.name).uniqueId;
            fuelEfficiency.id = ModUtils.GetUnlockByName(fuelEfficiency.name).uniqueId;
            powerEfficiency.id = ModUtils.GetUnlockByName(powerEfficiency.name).uniqueId;
            walkSpeed.id = ModUtils.GetUnlockByName(walkSpeed.name).uniqueId;
            moleSpeed.id = ModUtils.GetUnlockByName(moleSpeed.name).uniqueId;

            infiniteUnlocks.Add(miningSpeed.id, miningSpeed);
            infiniteUnlocks.Add(oreConsumption.id, oreConsumption);
            infiniteUnlocks.Add(fuelEfficiency.id, fuelEfficiency);
            infiniteUnlocks.Add(powerEfficiency.id, powerEfficiency);
            infiniteUnlocks.Add(walkSpeed.id, walkSpeed);
            infiniteUnlocks.Add(moleSpeed.id, moleSpeed);
        }

        public static void UpdateUnlockDetails() {
            // Tier
            Unlock moleSpeed4 = ModUtils.GetUnlockByName(UnlockNames.MOLESpeedIV);

            // Miner Speed TreePos
            Unlock mdrSpeed3 = ModUtils.GetUnlockByName(UnlockNames.MDRSpeedIII);
            miningSpeed.unlock.treePosition = mdrSpeed3.treePosition;
            miningSpeed.unlock.requiredTier = moleSpeed4.requiredTier;
            miningSpeed.unlock.sprite = ModUtils.LoadSpriteFromFile("BetterEndGame.Images.MinerInfUnlock.png");

            // Ore Consumption
            Unlock drillMk2 = ModUtils.GetUnlockByName(UnlockNames.MiningDrillMKII);
            oreConsumption.unlock.treePosition = drillMk2.treePosition;
            oreConsumption.unlock.requiredTier = moleSpeed4.requiredTier;
            oreConsumption.unlock.sprite = ModUtils.LoadSpriteFromFile("BetterEndGame.Images.OreInfUnlock.png");

            // Fuel Efficiency
            Unlock biodense5 = ModUtils.GetUnlockByName(UnlockNames.BioDenseV);
            fuelEfficiency.unlock.treePosition = biodense5.treePosition;
            fuelEfficiency.unlock.requiredTier = moleSpeed4.requiredTier;
            fuelEfficiency.unlock.sprite = ModUtils.LoadSpriteFromFile("BetterEndGame.Images.FuelInfUnlock.png");

            // Power Efficiency
            Unlock crankSpan = ModUtils.GetUnlockByName(UnlockNames.CrankSpan);
            powerEfficiency.unlock.treePosition = crankSpan.treePosition;
            powerEfficiency.unlock.requiredTier = moleSpeed4.requiredTier;
            powerEfficiency.unlock.sprite = ModUtils.LoadSpriteFromFile("BetterEndGame.Images.PowerInfUnlock.png");

            // Walk Speed
            Unlock suitSpeed4 = ModUtils.GetUnlockByName(UnlockNames.SuitSpeedIV);
            walkSpeed.unlock.treePosition = suitSpeed4.treePosition;
            walkSpeed.unlock.requiredTier = moleSpeed4.requiredTier;
            walkSpeed.unlock.sprite = ModUtils.LoadSpriteFromFile("BetterEndGame.Images.WalkInfUnlock.png");

            // MOLE Speed
            Unlock moleT99 = ModUtils.GetUnlockByName(UnlockNames.MOLET99Tunneling);
            moleSpeed.unlock.treePosition = moleT99.treePosition;
            moleSpeed.unlock.requiredTier = moleSpeed4.requiredTier;
            moleSpeed.unlock.sprite = ModUtils.LoadSpriteFromFile("BetterEndGame.Images.MoleInfUnlock.png");
        }

        public static void UptickUnlockTier(int id) {
            infiniteUnlocks[id].IncreaseTier();
            TechTreeState.instance.unlockStates[id].isActive = false;
            UIManager.instance.techTreeMenu.SetSidebarInfo(infiniteUnlocks[id].unlock);
        }

        // Private Functions

        private static void SetDescriptions() {
            miningSpeed.description = $"Boosts Miner Drill's ore production rate by {miningSpeed.boost}% per level.";
            oreConsumption.description = $"Reduces the vein destruction of Miner Drills by {oreConsumption.boost}% per level.";
            fuelEfficiency.description = $"Increase each fuel's burn time by {fuelEfficiency.boost}% per level.";
            powerEfficiency.description = $"Reduces each machine's power usage by {powerEfficiency.boost}% per level.";
            walkSpeed.description = $"Increases walk and fly speed by {walkSpeed.boost}% per level.";
            moleSpeed.description = $"Increases the M.O.L.E's dig speed by {moleSpeed.boost}% per level.";
        }

        private static void CreateMinerSpeedUnlock() {
            ModUtils.AddNewUnlock(new NewUnlockDetails() {
                category = Unlock.TechCategory.Terraforming,
                coreTypeNeeded = ResearchCoreDefinition.CoreType.Green,
                coreCountNeeded = 100,
                description = miningSpeed.description,
                displayName = miningSpeed.name,
                numScansNeeded = 0,
                requiredTier = TechTreeState.ResearchTier.Tier0,
                treePosition = 0
            });
        }

        private static void CreateOreConsumptionUnlock() {
            ModUtils.AddNewUnlock(new NewUnlockDetails() {
                category = Unlock.TechCategory.Terraforming,
                coreTypeNeeded = ResearchCoreDefinition.CoreType.Green,
                coreCountNeeded = 100,
                description = oreConsumption.description,
                displayName = oreConsumption.name,
                numScansNeeded = 0,
                requiredTier = TechTreeState.ResearchTier.Tier0,
                treePosition = 0
            });
        }

        private static void CreateFuelEfficiencyUnlock() {
            ModUtils.AddNewUnlock(new NewUnlockDetails() {
                category = Unlock.TechCategory.Synthesis,
                coreTypeNeeded = ResearchCoreDefinition.CoreType.Green,
                coreCountNeeded = 100,
                description = fuelEfficiency.description,
                displayName = fuelEfficiency.name,
                numScansNeeded = 0,
                requiredTier = TechTreeState.ResearchTier.Tier0,
                treePosition = 0
            });
        }

        private static void CreatePowerEfficiencyUnlock() {
            ModUtils.AddNewUnlock(new NewUnlockDetails() {
                category = Unlock.TechCategory.Energy,
                coreTypeNeeded = ResearchCoreDefinition.CoreType.Green,
                coreCountNeeded = 100,
                description = powerEfficiency.description,
                displayName = powerEfficiency.name,
                numScansNeeded = 0,
                requiredTier = TechTreeState.ResearchTier.Tier0,
                treePosition = 0
            });
        }

        private static void CreateWalkSpeedUnlock() {
            ModUtils.AddNewUnlock(new NewUnlockDetails() {
                category = Unlock.TechCategory.Transportation,
                coreTypeNeeded = ResearchCoreDefinition.CoreType.Green,
                coreCountNeeded = 100,
                description = walkSpeed.description,
                displayName = walkSpeed.name,
                numScansNeeded = 0,
                requiredTier = TechTreeState.ResearchTier.Tier0,
                treePosition = 0
            });
        }

        private static void CreateMoleSpeedUnlock() {
            ModUtils.AddNewUnlock(new NewUnlockDetails() {
                category = Unlock.TechCategory.Terraforming,
                coreTypeNeeded = ResearchCoreDefinition.CoreType.Green,
                coreCountNeeded = 100,
                description = moleSpeed.description,
                displayName = moleSpeed.name,
                numScansNeeded = 0,
                requiredTier = TechTreeState.ResearchTier.Tier0,
                treePosition = 0
            });
        }

        public static int GetCostForTier(int tier) {
            return Mathf.FloorToInt(100 * Mathf.Pow(1.1f, tier));
        }

        // Data Functions

        public static void Save(string worldName) {
            Directory.CreateDirectory(dataFolder);
            
            string saveFile = $"{dataFolder}/{worldName}.txt";
            string data = $"{miningSpeed.tier}|{oreConsumption.tier}|{fuelEfficiency.tier}|" +
                          $"{powerEfficiency.tier}|{walkSpeed.tier}|{moleSpeed.tier}";

            File.WriteAllText(saveFile, data);
        }

        public static void Load(string worldName) {
            string saveFile = $"{dataFolder}/{worldName}.txt";
            if (!File.Exists(saveFile)) {
                BetterEndGamePlugin.Log.LogInfo($"Could not find save file '{saveFile}'");
                return;
            }

            BetterEndGamePlugin.Log.LogInfo("Loading Data...");

            string data = File.ReadAllText(saveFile);
            string[] parts = data.Split('|');

            miningSpeed.SetTier(int.Parse(parts[0]));
            oreConsumption.SetTier(int.Parse(parts[1]));
            fuelEfficiency.SetTier(int.Parse(parts[2]));
            powerEfficiency.SetTier(int.Parse(parts[3]));
            walkSpeed.SetTier(int.Parse(parts[4]));
            moleSpeed.SetTier(int.Parse(parts[5]));

            int coresSpent = 0;
            coresSpent += miningSpeed.GetCumulativeCost();
            coresSpent += oreConsumption.GetCumulativeCost();
            coresSpent += fuelEfficiency.GetCumulativeCost();
            coresSpent += powerEfficiency.GetCumulativeCost();
            coresSpent += walkSpeed.GetCumulativeCost();
            coresSpent += moleSpeed.GetCumulativeCost();

            BetterEndGamePlugin.Log.LogInfo($"coresSpent: {coresSpent}");

            TechTreeState.instance.usedResearchCores[(int)ResearchCoreDefinition.CoreType.Green] += coresSpent;

            BetterEndGamePlugin.Log.LogInfo("Loaded Data");
        }
    }

    public class InfiniteUnlock {
        public int id;
        public string name;
        public string description;
        public int tier;
        public int boost;
        public bool increaseModifier;
        public float multiplier = 1;

        public Unlock unlock => ModUtils.GetUnlockByID(id);

        public void SetMultiplier() {
            if (increaseModifier) {
                multiplier = Mathf.Pow(1f + (boost / 100f), tier);
            }
            else {
                multiplier = Mathf.Pow((100f - boost) / 100f, tier);
            }
        }

        public void SetTier(int newTier) {
            tier = newTier;
            unlock.coresNeeded = new List<Unlock.RequiredCores>() {
                new Unlock.RequiredCores() {
                    type = ResearchCoreDefinition.CoreType.Green,
                    number = InfiniteUnlocks.GetCostForTier(tier)
                }
            };
            
            SetMultiplier();
            
            string newDescription = $"{description}\nLevel: {tier}\nBoost: {multiplier * 100f:#.##}%";
            string hash = LocsUtility.GetHashString(newDescription);
            ModUtils.hashTranslations.Add(hash, newDescription);
            unlock.descriptionHash = hash;

            if(name == InfiniteUnlocks.fuelEfficiency.name) {
                for(int i = 0; i < GameState.instance.fuelMultipliers.Length; i++) {
                    GameState.instance.fuelMultipliers[i] += multiplier - 1;
                }
            }
        }

        public void IncreaseTier() {
            SetTier(tier + 1);
        }

        public int GetCumulativeCost() {
            int cost = 0;
            for(int i = 0; i < tier; i++) {
                cost += InfiniteUnlocks.GetCostForTier(i);
            }

            return cost;
        }
    }
}
