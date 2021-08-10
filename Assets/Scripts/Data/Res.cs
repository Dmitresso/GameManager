using TMPro;
using UnityEngine;

namespace Data {
    public struct Res {
        public static void Init() {
            Skins.Init();
            Fonts.Init();
            Music.Init();
        }


        public struct Skins {
            private static string path = "ScriptableObjects/Skins/";
            private static readonly string lightSkinPath = path + "Light UISkin";
            private static readonly string darkSkinPath = path + "Dark UISkin";

            public static UISkin lightSkin;
            public static UISkin darkSkin;
            
            public static void Init() {
                lightSkin = (UISkin) Resources.Load(lightSkinPath);
                darkSkin = (UISkin) Resources.Load(darkSkinPath);
            }
        }
        
        public struct Prefabs {
            public static readonly string testPrefab = "Prefabs/Test";
        }

        public struct Sprites {
            public static readonly string testSprite = "Sprites/Test";
        }

        public struct Fonts {
            public static TMP_FontAsset arialSDF;
            public static TMP_FontAsset quicksandSDF;
            public static TMP_FontAsset ptSansRegularSDF;
            public static TMP_FontAsset robotoSlabRegularSDF;

            private static readonly string arialSDFPath = "Fonts/Arial SDF";
            private static readonly string quicksandSDFPath = "Fonts/Quicksand SDF";
            private static readonly string ptSansRegularPath = "Fonts/PTSans-Regular SDF";
            private static readonly string robotoSlabRegularPath = "Fonts/RobotoSlab-Regular SDF";

            public static void Init() {
                arialSDF = (TMP_FontAsset) Resources.Load(arialSDFPath);
                quicksandSDF = Resources.Load(quicksandSDFPath, typeof(TMP_FontAsset)) as TMP_FontAsset;
                ptSansRegularSDF = Resources.Load(ptSansRegularPath, typeof(TMP_FontAsset)) as TMP_FontAsset;
                robotoSlabRegularSDF = Resources.Load(robotoSlabRegularPath, typeof(TMP_FontAsset)) as TMP_FontAsset;
            }
        }

        public struct Music {
            public static AudioClip MainSoundtrack;
            public static AudioClip ButtonOnHoverSound, ButtonOnClickSound;

            private static readonly string mainSoundtrackPath = "Sounds/Music/Main soundtrack";
            private static readonly string buttonOnHoverSoundPath = "Sounds/SFX/01021";
            private static readonly string buttonOnClickSoundPath = "Sounds/SFX/computer-keyboard-button-press-release_m1pp3tnd";

            public static void Init() {
                MainSoundtrack = Resources.Load(mainSoundtrackPath, typeof(AudioClip)) as AudioClip;
                ButtonOnHoverSound = (AudioClip) Resources.Load(buttonOnHoverSoundPath);
                ButtonOnClickSound = (AudioClip) Resources.Load(buttonOnClickSoundPath);
            }
        }
    }
}