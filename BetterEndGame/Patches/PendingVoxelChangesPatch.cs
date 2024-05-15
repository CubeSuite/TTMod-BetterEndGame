using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BetterEndGame.Patches
{
    public class PendingVoxelChangesPatch
    {
        public static int maxIntegrity = 10000;

        [HarmonyPatch(typeof(PendingVoxelChanges), "TryDig")]
        [HarmonyPostfix]
        static void ReduceVeinDamage(PendingVoxelChanges __instance, Vector3Int coord, int digStrength, int miningTier, ref int numResourcesTaken, bool __result) {
            if (__result || numResourcesTaken <= 0) return;

            int chunkId = VoxelManager.GetChunkId(coord.x, coord.y, coord.z);
            int orCreateIndexForChunk = __instance.GetOrCreateIndexForChunk(chunkId);
            ref ChunkPendingVoxelChanges chunkChanges = ref __instance.chunkData[orCreateIndexForChunk];
            int voxelIndex = chunkChanges.GetIndex(coord.x, coord.y, coord.z);
            ref ModifiedCoordData voxelData = ref chunkChanges.GetOrAdd(voxelIndex);

            int reducedAmount = Mathf.CeilToInt(numResourcesTaken * (1f - InfiniteUnlocks.oreConsumption.multiplier));
            int restoreAmount = Math.Min(numResourcesTaken - reducedAmount, maxIntegrity - voxelData.integrity);
            voxelData.integrity += restoreAmount;
        }
    }
}
