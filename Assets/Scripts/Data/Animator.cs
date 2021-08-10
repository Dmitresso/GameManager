using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data {
    public struct Animator {
        public static void Init() {
            Events.Init();
            Parameters.Init(); 
        }

        public struct Events {
            public static Dictionary<string, Action> Functions => functions;
            private static Dictionary<string, Action> functions;

            public static void Init() {
                functions = new Dictionary<string, Action>(StringComparer.OrdinalIgnoreCase) {
                    {nameof(GoToMainMenu), GoToMainMenu},
                    {nameof(Test), Test}
                };
                if (Tracker.IsInit) Tracker.Instance.Log("Events data was successfully initialized.");
            }
            
            private static void GoToMainMenu() {
                GameManager.Instance.UpdateState(GameState.Menu.InMain);
                if (Tracker.IsInit) Tracker.Instance.Log("GoToMainMenu() event was called.");
            }

            private static void Test() {
                Debug.Log("TEST");
            }
        }
        
        public struct States {
            public const string
                SplashScreenFadeIn = "SplashScreenFadeIn",
                SplashScreenFadeOut = "SplashScreenFadeOut",
                InMainMenu = "InMainMenu",
                InSettingsMenu = "InSettingsMenu",
                InPauseMenu = "InPauseMenu",
                InGame = "InGame";

            public const string
                Wait = "Wait",
                CurrentSkyboxFadeIn = "CurrentSkyboxFadeIn",
                NextSkyboxFadeOut = "NextSkyboxFadeOut";
        }


        public struct Layers { }


        public struct Parameters {
            public static void Init() {
                Id.Float.SkyboxSwapRate = UnityEngine.Animator.StringToHash(String.Float.SkyboxSwapRate);
                Id.Float.SkyboxSwapFadeTime = UnityEngine.Animator.StringToHash(String.Float.SkyboxSwapFadeTime);

                Id.Bool.InGame = UnityEngine.Animator.StringToHash(String.Bool.InGame);
                Id.Bool.InMenu = UnityEngine.Animator.StringToHash(String.Bool.InMenu);
                Id.Bool.GameState.Menu.InSplashScreen = UnityEngine.Animator.StringToHash(String.Bool.GameState.Menu.InSplashScreen);
                Id.Bool.GameState.Menu.InSettings = UnityEngine.Animator.StringToHash(String.Bool.GameState.Menu.InSettings);
                Id.Bool.GameState.Menu.InMain = UnityEngine.Animator.StringToHash(String.Bool.GameState.Menu.InMain);
                Id.Bool.GameState.Menu.InPause = UnityEngine.Animator.StringToHash(String.Bool.GameState.Menu.InPause);
                Id.Bool.GameState.Game.Running = UnityEngine.Animator.StringToHash(String.Bool.GameState.Game.Running);
                Id.Bool.SwapSkyboxes = UnityEngine.Animator.StringToHash(String.Bool.SwapSkyboxes);
                Id.Bool.UseLightSkyboxes = UnityEngine.Animator.StringToHash(String.Bool.UseLightSkyboxes);
                Id.Bool.UseDarkSkyboxes = UnityEngine.Animator.StringToHash(String.Bool.UseDarkSkyboxes);
                
                
                
                Id.Trigger.SpaceBarClick = UnityEngine.Animator.StringToHash(String.Trigger.SpaceBarClicked);
                
                Id.Trigger.StartSwap = UnityEngine.Animator.StringToHash(String.Trigger.StartSwap);
                Id.Trigger.SwapCompleted = UnityEngine.Animator.StringToHash(String.Trigger.SwapCompleted);
                Id.Trigger.SkyboxFadeInCompleted = UnityEngine.Animator.StringToHash(String.Trigger.SkyboxFadeInCompleted);
                Id.Trigger.SkyboxFadeOutCompleted = UnityEngine.Animator.StringToHash(String.Trigger.SkyboxFadeOutCompleted);
                
                Id.Trigger.TransitionFadeIn = UnityEngine.Animator.StringToHash(String.Trigger.TransitionFadeIn);
                Id.Trigger.TransitionFadeOut = UnityEngine.Animator.StringToHash(String.Trigger.TransitionFadeOut);
                Id.Trigger.TransitionSlideIn = UnityEngine.Animator.StringToHash(String.Trigger.TransitionSlideIn);
                Id.Trigger.TransitionSlideOut = UnityEngine.Animator.StringToHash(String.Trigger.TransitionSlideOut);
                if (Tracker.IsInit) Tracker.Instance.Log("Parameters data successfully initialized");
            }



            public struct Id {
                public struct Float {
                    public static int
                        SkyboxSwapRate,
                        SkyboxSwapFadeTime,
                        SkyboxSwapOffset;
                }
    
                public struct Int { }
    
                public struct Bool {
                    public static int InGame;
                    public static int InMenu;
                    
                    public struct GameState {
                        public struct Game {
                            public static int
                                Running;
                        }

                        public struct Menu {
                            public static int
                                InSplashScreen,
                                InSettings,
                                InMain,
                                InPause;
                        }
                    }

                    public static int SwapSkyboxes;
                    public static int UseLightSkyboxes;   
                    public static int UseDarkSkyboxes;   
                }
    
                public struct Trigger {
                    public static int
                        SpaceBarClick,
                        StartSwap,
                        SwapCompleted,
                        SkyboxFadeInCompleted,
                        SkyboxFadeOutCompleted,
                        TransitionFadeIn,
                        TransitionFadeOut,
                        TransitionSlideIn,
                        TransitionSlideOut;
                }            
            }
            
            private struct String {
                public struct Float {
                    public const string
                        SkyboxSwapRate = "SkyboxSwapRate",
                        SkyboxSwapFadeTime = "SkyboxSwapFadeTime";
                }
    
                public struct Int { }
    
                public struct Bool {
                    public const string
                        InGame = "InGame",
                        InMenu = "InMenu";
                    
                    public struct GameState {
                        public struct Game {
                            public const string
                                Running = "GameState.Game.Running";
                        }
                        public struct Menu {
                            public const string
                                InSplashScreen = "GameState.Menu.InSplashScreen",
                                InSettings = "GameState.Menu.InSettings",
                                InMain = "GameState.Menu.InMain",
                                InPause = "GameState.Menu.InPause";                            
                        }
                    }
                    
                    public const string
                        SwapSkyboxes = "SwapSkyboxes",
                        UseLightSkyboxes = "UseLightSkyboxes",
                        UseDarkSkyboxes = "UseDarkSkyboxes";
                }
    
                public struct Trigger {
                    public static readonly string
                        SpaceBarClicked = "SpaceBarClicked",
                        StartSwap = "StartSwap",
                        SwapCompleted = "SwapCompleted",
                        SkyboxFadeInCompleted = "SkyboxFadeInCompleted",
                        SkyboxFadeOutCompleted = "SkyboxFadeOutCompleted",
                        TransitionFadeIn = "TransitionFadeIn",
                        TransitionFadeOut = "TransitionFadeOut",
                        TransitionSlideIn = "TransitionSlideIn",
                        TransitionSlideOut = "TransitionSlideOut";
                }            
            }
        }
    }
}