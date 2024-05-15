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
    internal class TechTreeNodePatch
    {
        [HarmonyPatch(typeof(TechTreeNode), "RefreshState")]
        [HarmonyPostfix]
        static void UpdateCost(TechTreeNode __instance) {
            ref TechTreeState.UnlockState ptr = ref TechTreeState.instance.unlockStates[__instance.unlockId];
            Unlock unlockRef = ptr.unlockRef;
            __instance.coreSquare.Set(unlockRef.coresNeeded.First().type, unlockRef.coresNeeded.First().number);
        }
    }
}
