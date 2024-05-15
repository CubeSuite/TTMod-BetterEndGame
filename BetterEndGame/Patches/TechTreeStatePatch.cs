using EquinoxsModUtils;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BetterEndGame.Patches
{
    internal class TechTreeStatePatch
    {
        [HarmonyPatch(typeof(TechTreeState), "HandleEndOfFrame")]
        [HarmonyPrefix]
        public static bool DisableClusters() {
            TechTreeState.instance.freeCores = 0;
            TechTreeState.instance.freeCoresAssembling = 0;
            TechTreeState.instance.freeCoresMining = InfiniteUnlocks.miningSpeed.multiplier - 1;
            TechTreeState.instance.freeCoresPowerOutput = 0;
            TechTreeState.instance.freeCoresSmelting = 0;
            TechTreeState.instance.freeCoresThreshing = 0;

            return false;
        }
    }
}
