using System;
using BepInEx;

namespace DataLogger
{
    // Token: 0x02000004 RID: 4
    [BepInPlugin("ShinyGorilla.DataLogger", "DataLogger", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        // Token: 0x06000008 RID: 8 RVA: 0x0000207F File Offset: 0x0000027F
        private void OnEnable()
        {
            HarmonyPatches.ApplyHarmonyPatches();
        }
    }
}
