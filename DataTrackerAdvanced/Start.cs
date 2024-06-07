using System;
using GorillaLocomotion;
using HarmonyLib;
using UnityEngine;

namespace DataLogger
{
    // Token: 0x02000003 RID: 3
    [HarmonyPatch(typeof(Player), "Awake")]
    internal class Start
    {
        // Token: 0x06000006 RID: 6 RVA: 0x00002068 File Offset: 0x00000268
        private static void Postfix()
        {
            Start.callbacks = new GameObject("Callbacks").AddComponent<PunCallbacks>();
        }

        // Token: 0x04000003 RID: 3
        public static PunCallbacks callbacks;
    }
}
