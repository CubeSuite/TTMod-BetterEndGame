using BepInEx;
using EquinoxsModUtils;
using FluffyUnderware.DevTools.Extensions;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BetterEndGame.Patches
{
    internal class GameStatePatch
    {
        [HarmonyPatch(typeof(GameState), "PrepForStateRefresh")]
        [HarmonyPostfix]
        static void ApplyBiofuelBoost() {
            float combined = GameState.instance.upgradeStates[38].floatVal + InfiniteUnlocks.fuelEfficiency.multiplier;
            foreach(ResourceInfo info in GameDefines.instance.upgradeableFuel) {
                GameState.instance.fuelMultipliers[info.uniqueId] = combined;
            }
        }

        [HarmonyPatch(typeof(GameState), "get_walkSpeed")]
        [HarmonyPostfix]
        static void ApplyWalkSpeedBoost(ref float __result) {
            __result = (InfiniteUnlocks.walkSpeed.multiplier - 1) + GameState.instance.upgradeStates[4].floatVal;
        }

        [HarmonyPatch(typeof(GameState), "get_terraformSpeed")]
        [HarmonyPostfix]
        static void ApplyMOLESpeedBoost(ref float __result) {
            __result = (InfiniteUnlocks.moleSpeed.multiplier - 1) + GameState.instance.upgradeStates[12].floatVal;
        }
    }

    // ToDo: Uncomment if trying to fix custom conveyor prefabs
    //internal class GameDefinesPatch {
    //    [HarmonyPatch(typeof(GameDefines), "InitRuntimeData")]
    //    [HarmonyPostfix]
    //    static void AddSprites() {
    //        GameDefines.instance.resourceSpriteArray.sprites = GameDefines.instance.resourceSpriteArray.sprites.Add(BetterEndGamePlugin.engineSprite);
    //        BetterEndGamePlugin.Log.LogWarning("Added sprite");
    //        //GameDefines.instance.resourceSpriteArray.textureArraySheet.
    //    }
    //}
}
