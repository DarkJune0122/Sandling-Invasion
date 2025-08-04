using BepInEx;
using Alexandria;
using Alexandria.ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SandlingInvasion
{
    [BepInDependency(Alexandria.Alexandria.GUID)] // this mod depends on the Alexandria API: https://enter-the-gungeon.thunderstore.io/package/Alexandria/Alexandria/
    [BepInDependency(ETGModMainBehaviour.GUID)]
    [BepInPlugin(GUID, NAME, VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public const string GUID = "sandlings-united.etg.sandling-invasion";
        public const string NAME = "Sandling Invasion!";
        public const string VERSION = "0.1.0";
        public const string TEXT_COLOR = "#FFD97F";

        public void Start()
        {
            ETGModMainBehaviour.WaitForGameManagerStart(GMStart);
        }

        public void GMStart(GameManager g)
        {
            ExamplePassive.Register();
            Log($"{NAME} v{VERSION} started successfully.", TEXT_COLOR);
        }

        public static void Log(string text, string color="#FFFFFF")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>");
        }
    }
}
