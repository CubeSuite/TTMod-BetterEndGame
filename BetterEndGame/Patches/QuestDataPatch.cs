using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BetterEndGame.Patches
{
    internal class QuestDataPatch
    {
        [HarmonyPatch(typeof(QuestData), "GetDisplayText")]
        public class DisplayTextPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(ref string __result, QuestData __instance) {
                if (string.IsNullOrEmpty(__instance.displayTextHash)) {
                    __result = "";
                    return false;
                }

                string text = LocsUtility.TranslateStringFromHash(__instance.displayTextHash, null, null);


                MethodInfo replaceMethod = typeof(QuestData).GetMethod("ReplaceProductionTerminalUpgradeCosts", BindingFlags.NonPublic | BindingFlags.Static);
                if (replaceMethod != null) {
                    text = (string)replaceMethod.Invoke(null, new object[] { text });
                }

                text = RemoveSpriteTags(text);
                text = GameDefines.instance.patternsList.Replace(text);

                __result = text;
                return false;
            }

            private static string RemoveSpriteTags(string input) {
                return System.Text.RegularExpressions.Regex.Replace(input, @"<sprite="""" index=0>", string.Empty);
            }
        }
    }
}
