using BepInEx;
using Gungeon;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
                RegisterAll(g);
            }
            catch (Exception e)
            {
                Warning($"{NAME} v{VERSION} was NOT loaded successfully! See the exception:");
                Warning(e.Message);
                Warning(e.StackTrace);
            }
            finally
            {
                Log($"{NAME} v{VERSION} started successfully.", TEXT_COLOR);
            }
        }

        private void G_OnNewLevelFullyLoaded()
        {
            Log($"Loaded!", "#1221F1");
        }





        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
        /// .
        /// .                                               Static Methods
        /// .
        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
        public static void RegisterAll(GameManager manager)
        {
            if (Initialized) return;

            // Registers items:
            Sandling.Register(manager);

            // Should it be moved to the beginning of the registration?
            Initialized = true;
        }





        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
        /// .
        /// .                                                  Logging
        /// .
        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
        private static readonly char[] SplitSeparators = ['\n', '\r'];
        public static void Log() => Log(string.Empty);
        public static void Log<T>(ICollection<T> values, string color = "#FFFFFF") => Log(values, v => v.ToStringSafe(), color);
        public static void Log<T>(ICollection<T> values, Func<T, string> func, string color = "#FFFFFF")
        {
            foreach (T item in values)
            {
                Log(func.Invoke(item), color);
            }
        }

        public static void Log<T>(IEnumerable<T> values, string color = "#FFFFFF") => Log(values, v => v.ToStringSafe(), color);
        public static void Log<T>(IEnumerable<T> values, Func<T, string> func, string color = "#FFFFFF")
        {
            foreach (T item in values)
            {
                Log(func.Invoke(item), color);
            }
        }

        public static void Log(object obj, string color = "#FFFFFF") => Log(obj.ToStringSafe(), color);
        public static void Log(string text, string color = "#FFFFFF")
        {
            if (text.Contains('\n'))
            {
                foreach (var line in text.Split(SplitSeparators, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    else ETGModConsole.Log($"<color={color}>{line}</color>");
                }
            }
            else ETGModConsole.Log($"<color={color}>{text}</color>");
        }





        public const string WarningColor = "#FF5656";
        public static void Warning() => Warning(string.Empty);
        public static void Warning<T>(ICollection<T> values) => Warning(values, v => v.ToStringSafe());
        public static void Warning<T>(ICollection<T> values, Func<T, string> func)
        {
            foreach (T item in values)
            {
                Warning(func.Invoke(item));
            }
        }

        public static void Warning<T>(IEnumerable<T> values) => Warning(values, v => v.ToStringSafe());
        public static void Warning<T>(IEnumerable<T> values, Func<T, string> func)
        {
            foreach (T item in values)
            {
                Warning(func.Invoke(item));
            }
        }

        public static void Warning(object obj) => Warning(obj.ToStringSafe());
        public static void Warning(string text)
        {
            if (text.Contains('\n'))
            {
                foreach (var line in text.Split(SplitSeparators, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    else ETGModConsole.Log($"<color={WarningColor}>Warning: {line}</color>");
                }
            }
            else ETGModConsole.Log($"<color={WarningColor}>Warning: {text}</color>");
        }





        private static readonly Dictionary<string, int> counters = [];
        public static void ResetCount(string key = "") => counters[key] = 0;
        public static void Count(string key = "") => Count(string.Empty, key);
        public static void Count<T>(ICollection<T> values, string key = "", string color = "#FFFFFF") => Count(values, v => v.ToStringSafe(), key, color);
        public static void Count<T>(ICollection<T> values, Func<T, string> func, string key = "", string color = "#FFFFFF")
        {
            foreach (T item in values)
            {
                Count(func.Invoke(item), key, color);
            }
        }

        public static void Count<T>(IEnumerable<T> values, string color = "#FFFFFF") => Count(values, v => v.ToStringSafe(), color);
        public static void Count<T>(IEnumerable<T> values, Func<T, string> func, string color = "#FFFFFF")
        {
            foreach (T item in values)
            {
                Count(func.Invoke(item), color);
            }
        }

        public static void Count(object obj, string key = "", string color = "#FFFFFF") => Count(obj.ToStringSafe(), key, color);
        public static void Count(string text, string key = "", string color = "#FFFFFF")
        {
            int counter = counters[key] + 1;
            if (text.Contains('\n'))
            {
                foreach (var line in text.Split(SplitSeparators, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    ETGModConsole.Log($"[{counter}] <color={color}>{line}</color>");
                }
            }
            else
            {
                ETGModConsole.Log($"[{counter}] <color={color}>{text}</color>");
            }

            counters[key] = counter;
        }





        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
        /// .
        /// .                                                 UI Classes
        /// .
        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
        public static class UI
        {
            public class SpriteHolder(Canvas canvas)
            {
                public const int SpriteRowLimit = 8;
                public readonly Canvas canvas = canvas;
                public HorizontalLayoutGroup Layout
                {
                    get
                    {
                        int count = canvas.transform.childCount;
                        if (count == 0)
                        {
                            return new GameObject($"Layout - {count + 1}").AddComponent<HorizontalLayoutGroup>();
                        }

                        HorizontalLayoutGroup layout = canvas.transform.GetChild(count - 1).GetComponent<HorizontalLayoutGroup>();
                        if (layout.transform.childCount >= SpriteRowLimit)
                        {
                            layout = new GameObject($"Layout - {count + 1}").AddComponent<HorizontalLayoutGroup>();
                            layout.spacing = 10;
                        }

                        return layout;
                    }
                }

                public T AddComponent<T>() where T : Component
                {
                    T component = new GameObject(typeof(T).ToString()).AddComponent<T>();
                    component.transform.localPosition = Vector3.zero;
                    component.transform.SetParent(Layout.transform, false);
                    return component;
                }
            }

            private static SpriteHolder holder;
            public static SpriteHolder Holder
            {
                get
                {
                    if (holder == null)
                    {
                        Canvas canvas = CreateCanvas();
                        holder = new SpriteHolder(canvas);
                    }

                    return holder;
                }
            }





            /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
            /// .
            /// .                                               Static Methods
            /// .
            /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
            public static void ShowSprite(Sprite sprite)
            {
                Holder.AddComponent<Image>().sprite = sprite;
            }

            public static void ShowSprite(tk2dBaseSprite sprite)
            {
                // TODO: Debug and test.
                foreach (var texture in sprite.Collection.textures)
                {
                    Holder.AddComponent<RawImage>().texture = texture;
                }
            }

            public static Sprite ConvertTk2dToUISprite(tk2dBaseSprite tk2dSprite)
            {
                var collection = tk2dSprite.Collection;
                var spriteId = tk2dSprite.spriteId;
                var def = collection.spriteDefinitions[spriteId];
                var texture = def.material.mainTexture as Texture2D;

                // Convert pixel coords to Unity rect
                Rect spriteRect = new(
                    def.uvs[0].x * texture.width,
                    def.uvs[0].y * texture.height,
                    Mathf.Abs(def.uvs[1].x - def.uvs[0].x) * texture.width,
                    Mathf.Abs(def.uvs[3].y - def.uvs[0].y) * texture.height
                );

                // Create Unity Sprite
                return Sprite.Create(
                    texture,
                    spriteRect,
                    new Vector2(0.5f, 0.5f),  // pivot
                    tk2dSprite.Collection.invOrthoSize * 25f   // pixels per unit (adjust if needed)
                );
            }

            private static Canvas CreateCanvas()
            {
                // Creates canvas to hold all the sprites in a grid.
                Canvas canvas = new GameObject("Sprite Holder").AddComponent<Canvas>();
                canvas.transform.localPosition = new Vector3(canvas.transform.localPosition.x, canvas.transform.localPosition.y, -50);
                canvas.sortingOrder = -10000;
                VerticalLayoutGroup layout = canvas.gameObject.AddComponent<VerticalLayoutGroup>();
                layout.childAlignment = TextAnchor.MiddleCenter;
                layout.childControlWidth = true;
                layout.childForceExpandWidth = true;
                Image image = canvas.gameObject.AddComponent<Image>();
                image.color = new Color(1.0f, 1.0f, 1.0f, 0.6f);
                EventTrigger trigger = image.gameObject.AddComponent<EventTrigger>();
                var callback = new EventTrigger.TriggerEvent();
                callback.AddListener((e) => Destroy(canvas.gameObject));
                trigger.triggers.Add(new EventTrigger.Entry() { eventID = EventTriggerType.PointerClick, callback = callback });
                return canvas;
            }
        }





        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
        /// .
        /// .                                                 Debugging
        /// .
        /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
        public static class Debug
        {
            /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
            /// .
            /// .                                               Static Methods
            /// .
            /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
            public static void LogItems()
            {
                foreach (var pair in Game.Items.Pairs)
                {
                    Log($"{pair.Key}: {pair.Value}");
                }
            }

            public static void LogEnemies()
            {
                foreach (var pair in Game.Enemies.Pairs)
                {
                    Log($"{pair.Key}: {pair.Value}");
                }
            }
        }
    }
}
