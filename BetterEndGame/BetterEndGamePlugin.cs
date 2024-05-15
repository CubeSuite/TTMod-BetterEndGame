using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BetterEndGame.Patches;
using EquinoxsModUtils;
using HarmonyLib;
using Rewired.UI.ControlMapper;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using UnityEngine;

namespace BetterEndGame
{
    [BepInPlugin(MyGUID, PluginName, VersionString)]
    public class BetterEndGamePlugin : BaseUnityPlugin
    {
        private const string MyGUID = "com.equinox.BetterEndGame";
        private const string PluginName = "BetterEndGame";
        private const string VersionString = "1.1.0";

        private static readonly Harmony Harmony = new Harmony(MyGUID);
        public static ManualLogSource Log = new ManualLogSource(PluginName);

        // Objects & Variables

        private const string electricEngine = "Electric Engine";
        private const string heatsink = "Heatsink";
        private const string motherboard = "Motherboard";
        private const string sturdyFrame = "Sturdy Frame";

        private const string primeComponents = "Prime Components";
        private bool updatedQuestText = false;

        // Unity Functions

        private void Awake() {
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loading...");
            Harmony.PatchAll();

            ModUtils.GameDefinesLoaded += OnGameDefinesLoaded;
            ModUtils.GameLoaded += OnGameLoaded;
            ModUtils.GameSaved += OnGameSaved;

            ApplyPatches();
            ModUtils.AddNewSchematicsSubHeader(primeComponents, "Intermediates", 8, false);
            CreateNewItems();
            InfiniteUnlocks.CreateUnlocks();

            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loaded.");
            Log = Logger;
        }

        private void FixedUpdate() {
            if (!ModUtils.hasGameLoaded) return;

            if (TechTreeState.instance.IsUnlockActive(InfiniteUnlocks.miningSpeed.id)) {
                InfiniteUnlocks.UptickUnlockTier(InfiniteUnlocks.miningSpeed.id);
            }

            if (TechTreeState.instance.IsUnlockActive(InfiniteUnlocks.oreConsumption.id)) {
                InfiniteUnlocks.UptickUnlockTier(InfiniteUnlocks.oreConsumption.id);
            }

            if (TechTreeState.instance.IsUnlockActive(InfiniteUnlocks.fuelEfficiency.id)) {
                InfiniteUnlocks.UptickUnlockTier(InfiniteUnlocks.fuelEfficiency.id);
            }

            if (TechTreeState.instance.IsUnlockActive(InfiniteUnlocks.powerEfficiency.id)) {
                InfiniteUnlocks.UptickUnlockTier(InfiniteUnlocks.powerEfficiency.id);
            }

            if (TechTreeState.instance.IsUnlockActive(InfiniteUnlocks.walkSpeed.id)) {
                InfiniteUnlocks.UptickUnlockTier(InfiniteUnlocks.walkSpeed.id);
            }

            if (TechTreeState.instance.IsUnlockActive(InfiniteUnlocks.moleSpeed.id)) {
                InfiniteUnlocks.UptickUnlockTier(InfiniteUnlocks.moleSpeed.id);
            }

            if (updatedQuestText) return;
            updatedQuestText = true;

            Dictionary<int, QuestData> quests = (Dictionary<int, QuestData>)ModUtils.GetPrivateField("_questsToIds", PlayerQuestSystem.instance);
            foreach(QuestData data in quests.Values) {
                if(data.GetDisplayText().Contains("1950 MJ Accumulated Power")) {
                    string displayText = "Supply and maximise Production Terminal VICTOR by depositing:<br><br>1000 Electrical Engines<br>1000 Heatsinks<br>1000 Motherboards<br>1000 Sturdy Frames<br>2000 MJ Accumulated Power<rsc: Accumulator>";
                    string hash = LocsUtility.GetHashString(displayText);
                    ModUtils.hashTranslations.Add(hash, displayText);
                    data.displayTextHash = hash;
                }
            }

            ModUtils.SetPrivateField("_questsToIds", PlayerQuestSystem.instance, quests);
        }

        // Events

        private void OnGameDefinesLoaded(object sender, EventArgs e) {
            InfiniteUnlocks.FetchUnlocks();
            InfiniteUnlocks.UpdateUnlockDetails();
            UpdateNewItems();

            foreach (GatedDoorConfiguration config in GameDefines.instance.gatedDoorConfigurations) {
                if (config.displayName == "Tech Tier 10") {
                    config.reqTypes = new ResourceInfo[] {
                        ModUtils.GetResourceInfoByName(electricEngine),
                        ModUtils.GetResourceInfoByName(heatsink),
                        ModUtils.GetResourceInfoByName(motherboard),
                        ModUtils.GetResourceInfoByName(sturdyFrame)
                    };
                    config.reqQuantities = new int[] { 1000, 1000, 1000, 1000 };
                    config.energyReqInMJ = 2000;
                }
            }
        }

        private void OnGameSaved(object sender, EventArgs e) {
            InfiniteUnlocks.Save(sender.ToString());
        }

        private void OnGameLoaded(object sender, EventArgs e) {
            InfiniteUnlocks.Load(SaveState.instance.metadata.worldName);
        }

        // Private Functions

        private void ApplyPatches() {
            Harmony.CreateAndPatchAll(typeof(GameStatePatch));
            Harmony.CreateAndPatchAll(typeof(GenericMachineInstanceRefPatch));
            Harmony.CreateAndPatchAll(typeof(PendingVoxelChangesPatch));
            Harmony.CreateAndPatchAll(typeof(QuestDataPatch));
            Harmony.CreateAndPatchAll(typeof(TechTreeStatePatch));
            Harmony.CreateAndPatchAll(typeof(TechTreeNodePatch));
        }

        private void CreateNewItems() {
            ModUtils.AddNewResource(new NewResourceDetails() {
                name = electricEngine,
                craftingMethod = CraftingMethod.Assembler,
                craftTierRequired = 0,
                description = "Drives vehicles and machines with greater power than Electric Motors",
                fuelAmount = 0,
                maxStackCount = 500,
                miningTierRequired = 0,
                parentName = ResourceNames.RelayCircuit,
                sprite = ModUtils.LoadSpriteFromFile("BetterEndGame.Images.Items.Engine.png"),
                sortPriority = 0,
                subHeaderTitle = primeComponents,
                unlockName = UnlockNames.AdvancedOptimization
            });
            
            ModUtils.AddNewResource(new NewResourceDetails() {
                name = heatsink,
                craftingMethod = CraftingMethod.Assembler,
                craftTierRequired = 0,
                description = "Dissipating excess heat generated by electronic components.",
                fuelAmount = 0,
                maxStackCount = 500,
                miningTierRequired = 0,
                parentName = ResourceNames.RelayCircuit,
                sprite = ModUtils.LoadSpriteFromFile("BetterEndGame.Images.Items.Heatsink.png"),
                sortPriority = 1,
                subHeaderTitle = primeComponents,
                unlockName = UnlockNames.AdvancedOptimization
            });
            ModUtils.AddNewResource(new NewResourceDetails() {
                name = motherboard,
                craftingMethod = CraftingMethod.Assembler,
                craftTierRequired = 0,
                description = "A central hub connecting circuits and enabling seamless communication between them.",
                fuelAmount = 0,
                maxStackCount = 500,
                miningTierRequired = 0,
                parentName = ResourceNames.RelayCircuit,
                sprite = ModUtils.LoadSpriteFromFile("BetterEndGame.Images.Items.Motherboard.png"),
                sortPriority = 2,
                subHeaderTitle = primeComponents,
                unlockName = UnlockNames.AdvancedOptimization
            });
            ModUtils.AddNewResource(new NewResourceDetails() {
                name = sturdyFrame,
                craftingMethod = CraftingMethod.Assembler,
                craftTierRequired = 0,
                description = "A robust and durable structural component for stabilising machines.",
                fuelAmount = 0,
                maxStackCount = 500,
                miningTierRequired = 0,
                parentName = ResourceNames.RelayCircuit,
                sprite = ModUtils.LoadSpriteFromFile("BetterEndGame.Images.Items.SturdyFrame.png"),
                sortPriority = 3,
                subHeaderTitle = primeComponents,
                unlockName = UnlockNames.AdvancedOptimization
            });

            ModUtils.AddNewRecipe(new NewRecipeDetails() {
                craftingMethod = CraftingMethod.Assembler,
                craftTierRequired = 0,
                duration = 60,
                ingredients = new List<RecipeResourceInfo>() {
                    new RecipeResourceInfo() {
                        name = ResourceNames.ElectricMotor,
                        quantity = 2
                    },
                    new RecipeResourceInfo() {
                        name = ResourceNames.WireSpindle,
                        quantity = 8
                    },
                    new RecipeResourceInfo() {
                        name = ResourceNames.Gearbox,
                        quantity = 8
                    }
                },
                outputs = new List<RecipeResourceInfo>() {
                    new RecipeResourceInfo() {
                        name = electricEngine,
                        quantity = 1
                    }
                },
                sortPriority = 0,
                unlockName = UnlockNames.AdvancedOptimization
            });
            ModUtils.AddNewRecipe(new NewRecipeDetails() {
                craftingMethod = CraftingMethod.Assembler,
                craftTierRequired = 0,
                duration = 60,
                ingredients = new List<RecipeResourceInfo>() {
                    new RecipeResourceInfo() {
                        name = ResourceNames.IronMechanism,
                        quantity = 10
                    },
                    new RecipeResourceInfo() {
                        name = ResourceNames.CopperMechanism,
                        quantity = 10
                    },
                    new RecipeResourceInfo() {
                        name = ResourceNames.CeramicTiles,
                        quantity = 10
                    }
                },
                outputs = new List<RecipeResourceInfo>() {
                    new RecipeResourceInfo() {
                        name = heatsink,
                        quantity = 1
                    }
                },
                sortPriority = 1,
                unlockName = UnlockNames.AdvancedOptimization
            });
            ModUtils.AddNewRecipe(new NewRecipeDetails() {
                craftingMethod = CraftingMethod.Assembler,
                craftTierRequired = 0,
                duration = 60,
                ingredients = new List<RecipeResourceInfo>() {
                    new RecipeResourceInfo() {
                        name = ResourceNames.RelayCircuit,
                        quantity = 2
                    },
                    new RecipeResourceInfo() {
                        name = ResourceNames.ElectricalSet,
                        quantity = 2
                    },
                    new RecipeResourceInfo() {
                        name = ResourceNames.ProcessorArray,
                        quantity = 2
                    }
                },
                outputs = new List<RecipeResourceInfo>() {
                    new RecipeResourceInfo() {
                        name = motherboard,
                        quantity = 1
                    }
                },
                sortPriority = 2,
                unlockName = UnlockNames.AdvancedOptimization
            });
            ModUtils.AddNewRecipe(new NewRecipeDetails() {
                craftingMethod = CraftingMethod.Assembler,
                craftTierRequired = 0,
                duration = 60,
                ingredients = new List<RecipeResourceInfo>() {
                    new RecipeResourceInfo() {
                        name = ResourceNames.SteelFrame,
                        quantity = 8
                    },
                    new RecipeResourceInfo() {
                        name = ResourceNames.ReinforcedIronFrame,
                        quantity = 4
                    },
                    new RecipeResourceInfo() {
                        name = ResourceNames.ReinforcedCopperFrame,
                        quantity = 4
                    }
                },
                outputs = new List<RecipeResourceInfo>() {
                    new RecipeResourceInfo() {
                        name = sturdyFrame,
                        quantity = 1
                    }
                },
                sortPriority = 3,
                unlockName = UnlockNames.AdvancedOptimization
            });
        }

        private void UpdateNewItems() {
            ResourceInfo engine = ModUtils.GetResourceInfoByName("Electric Engine");
            ResourceInfo heatsink = ModUtils.GetResourceInfoByName("Heatsink");
            ResourceInfo motherboard = ModUtils.GetResourceInfoByName("Motherboard");
            ResourceInfo sturdyFrame = ModUtils.GetResourceInfoByName("Sturdy Frame");

            engine.headerType = ModUtils.GetSchematicsSubHeaderByTitle("Prime Components");
            heatsink.headerType = ModUtils.GetSchematicsSubHeaderByTitle("Prime Components");
            motherboard.headerType = ModUtils.GetSchematicsSubHeaderByTitle("Prime Components");
            sturdyFrame.headerType = ModUtils.GetSchematicsSubHeaderByTitle("Prime Components");
        }
    }
}
