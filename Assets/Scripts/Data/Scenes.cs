using System;

namespace Data {
    public struct Scenes {
        public static readonly string Boot, Main;

        static Scenes() {
            Boot = GameManager.Instance.GetSceneNameByIndex(0);
            Main = GameManager.Instance.GetSceneNameByIndex(1);
        }

        private static string gameToLoad;

        public static string GameToLoad {
            get => gameToLoad;
            set {
                string scenesList = "";
                var scenes = typeof(Scenes).GetFields();
                for (int i = 0; i < scenes.Length; i++) {
                    string tmp = i == scenes.Length - 1 ? "." : ", ";
                    string.Concat(scenesList, (string) scenes[i].GetValue(null) + tmp);
                    if (value == (string) scenes[i].GetValue(null)) gameToLoad = value;
                }

                if (gameToLoad == null) {
                    throw new Exception("Attempt of setting \"GameToLoad\" string value to \"" +
                                        value + "\" didn't find matching scene name in \"Scenes\" struct. " +
                                        "Available scenes: " + scenes);
                }
            }
        }
    }
}