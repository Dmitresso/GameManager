public static class State {
    public enum Game {
        Loading = 0,
        Waiting = 1,
        Running = 2
    }

    public enum UI {
        Disabled = 3,
        InSplashscreen = 4,
        InMain = 5,
        InSettings = 6,
        InPause = 7
    }
}