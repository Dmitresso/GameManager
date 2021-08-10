using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class ResolutionManager : Singleton<ResolutionManager> {
    public bool ResolutionsInit { get; set; }
    
    private static bool fixedAspectRatio = true;
    private static float targetAspectRatio = 4f / 3f;
    private static float windowedAspectRatio = 4f / 3f;
    private readonly int[] resolutions = { 600, 800, 1024, 1280, 1400, 1600, 1920 };

    private Resolution DisplayResolution;
    public List<Vector2> windowedResolutions, fullscreenResolutions;

    [HideInInspector] public int currWindowedRes, currFullscreenRes;
    
    private void Awake() {
        StartCoroutine(StartRoutine());
    }
    

    private void PrintResolution() {
        Debug.Log("Current resolution: " + Screen.currentResolution.width + "×" + Screen.currentResolution.height);
    }

    
    private IEnumerator StartRoutine() {
        if (Screen.fullScreen) {
            Resolution r = Screen.currentResolution;
            Screen.fullScreen = false;

            yield return null;
            yield return null;

            DisplayResolution = Screen.currentResolution;

            Screen.SetResolution(r.width, r.height, true);

            yield return null;
        }
        else {
            DisplayResolution = Screen.currentResolution;
        }

        InitResolutions();
        ResolutionsInit = true;
    }


    private void InitResolutions() {
        float screenAspect = (float) DisplayResolution.width / DisplayResolution.height;

        windowedResolutions = new List<Vector2>();
        fullscreenResolutions = new List<Vector2>();

        foreach (int w in resolutions) {
            if (w < DisplayResolution.width) {
                // Adding resolution only if it's 20% smaller than the screen
                if (w < DisplayResolution.width * 0.8f) {
                    Vector2 windowedResolution = new Vector2(w, Mathf.Round(w / (fixedAspectRatio ? targetAspectRatio : windowedAspectRatio)));
                    if (windowedResolution.y < DisplayResolution.height * 0.8f)
                        windowedResolutions.Add(windowedResolution);

                    fullscreenResolutions.Add(new Vector2(w, Mathf.Round(w / screenAspect)));
                }
            }
        }

        // Adding fullscreen native resolution
        fullscreenResolutions.Add(new Vector2(DisplayResolution.width, DisplayResolution.height));

        // Adding half fullscreen native resolution
        Vector2 halfNative = new Vector2(DisplayResolution.width * 0.5f, DisplayResolution.height * 0.5f);
        if (halfNative.x > resolutions[0] && fullscreenResolutions.IndexOf(halfNative) == -1)
            fullscreenResolutions.Add(halfNative);

        fullscreenResolutions = fullscreenResolutions.OrderBy(resolution => resolution.x).ToList();

        bool found = false;

        if (Screen.fullScreen) {
            currWindowedRes = windowedResolutions.Count - 1;

            for (int i = 0; i < fullscreenResolutions.Count; i++) {
                if (fullscreenResolutions[i].x == Screen.width && fullscreenResolutions[i].y == Screen.height) {
                    currFullscreenRes = i;
                    found = true;
                    break;
                }
            }

            if (!found)
                SetResolution(fullscreenResolutions.Count - 1, true);
        }
        else {
            currFullscreenRes = fullscreenResolutions.Count - 1;

            for (int i = 0; i < windowedResolutions.Count; i++) {
                if (windowedResolutions[i].x == Screen.width && windowedResolutions[i].y == Screen.height) {
                    found = true;
                    currWindowedRes = i;
                    break;
                }
            }

            if (!found)
                SetResolution(windowedResolutions.Count - 1, false);
        }
    }

    
    public void SetResolution(int index, bool fullscreen) {
        Vector2 r = new Vector2();
        if (fullscreen) {
            currFullscreenRes = index;
            r = fullscreenResolutions[currFullscreenRes];
        }
        else {
            currWindowedRes = index;
            r = windowedResolutions[currWindowedRes];
        }

        bool fullscreen2windowed = Screen.fullScreen & !fullscreen;

        //Debug.Log("Setting resolution to " + (int) r.x + "×" + (int) r.y);
        Screen.SetResolution((int) r.x, (int) r.y, fullscreen);
    }


    private IEnumerator SetResolutionAfterResize(Vector2 r) {
        int maxTime = 5;
        float time = Time.time;

        // Skipping a couple of frames during which the screen size will change
        yield return null;
        yield return null;

        int lastW = Screen.width;
        int lastH = Screen.height;

        // Waiting for another screen size change at the end of the transition animation
        while (Time.time - time < maxTime) {
            if (lastW != Screen.width || lastH != Screen.height) {
                Debug.Log("Resize! " + Screen.width + "×" + Screen.height);

                Screen.SetResolution((int) r.x, (int) r.y, Screen.fullScreen);
                yield break;
            }

            yield return null;
        }

        Debug.Log("End waiting");
    }

    public void ToggleFullscreen() {
        SetResolution(Screen.fullScreen ? currWindowedRes : currFullscreenRes, !Screen.fullScreen);
    }
}