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
        public const string API = "SandlingAPI";
        public const string VERSION = "0.1.0";
        public const string TEXT_COLOR = "#FFD97F";





        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
        /// .
        /// .                                              Static Properties
        /// .
        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
        public static bool Initialized { get; set; }





        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
        /// .
        /// .                                               Public Methods
        /// .
        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
        public void Start()
        {
            ETGModMainBehaviour.WaitForGameManagerStart(GMStart);
        }

        public void GMStart(GameManager g)
        {
            try
            {
                RegisterAll();
            }
            catch (Exception e)
            {
                Log($"{NAME} v{VERSION} was NOT loaded successfully! See the exception:\n" + e.Message, "#FF1212");
            }
            finally
            {
                Log($"{NAME} v{VERSION} started successfully.", TEXT_COLOR);
            }
        }





        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
        /// .
        /// .                                               Static Methods
        /// .
        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
        public static void RegisterAll()
        {
            if (Initialized) return;

            // Registers items:
            Sandling.Register();

            // Should it be moved to the beginning of the registration?
            Initialized = true;
        }

        public static void Log(string text, string color="#FFFFFF")
        {
            if (text.Contains('\n'))
            {
                foreach (var line in text.Split('\n', '\r'))
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    else ETGModConsole.Log($"<color={color}>{line}</color>");
                }
            }
            else ETGModConsole.Log($"<color={color}>{text}</color>");
        }
    }
}
