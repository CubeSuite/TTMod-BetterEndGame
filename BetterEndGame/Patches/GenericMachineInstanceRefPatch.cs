using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BetterEndGame.Patches
{
    internal class GenericMachineInstanceRefPatch
    {
        [HarmonyPatch(typeof(GenericMachineInstanceRef), "get_CurPowerConsumption")]
        [HarmonyPostfix]
        static void ApplyPowerMultplier(GenericMachineInstanceRef __instance, ref int __result) {
            __result = Mathf.FloorToInt(InfiniteUnlocks.powerEfficiency.multiplier * __instance.GetPowerInfo().curPowerConsumption);
        }
    }
}
