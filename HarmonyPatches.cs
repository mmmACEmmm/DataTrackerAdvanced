using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace DataLogger
{
    // Token: 0x02000002 RID: 2
    public class HarmonyPatches
    {
        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        // (set) Token: 0x06000002 RID: 2 RVA: 0x00002057 File Offset: 0x00000257
        public static bool IsPatched { get; private set; }

        // Token: 0x06000003 RID: 3 RVA: 0x00002100 File Offset: 0x00000300
        internal static void ApplyHarmonyPatches()
        {
            Debug.Log("ShinyGorilla.DataLogger patching...");
            bool flag = !HarmonyPatches.IsPatched;
            if (flag)
            {
                if (HarmonyPatches.instance == null)
                {
                    HarmonyPatches.instance = new Harmony("ShinyGorilla.DataLogger");
                }
                HarmonyPatches.instance.PatchAll(Assembly.GetExecutingAssembly());
                HarmonyPatches.IsPatched = true;
            }
        }

        // Token: 0x06000004 RID: 4 RVA: 0x00002154 File Offset: 0x00000354
        internal static void RemoveHarmonyPatches()
        {
            bool flag = HarmonyPatches.instance != null && HarmonyPatches.IsPatched;
            if (flag)
            {
                Debug.Log("Trying");
                HarmonyPatches.instance.UnpatchSelf();
                HarmonyPatches.IsPatched = false;
            }
        }

        // Token: 0x04000001 RID: 1
        private static Harmony instance;
    }
}
