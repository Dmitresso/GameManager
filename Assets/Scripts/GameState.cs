public static class GameState {
    public enum Game {
        Loading = 0,
        Waiting = 1,
        Running = 2
    }

    public enum Menu {
        InSplashscreen = 3,
        InMain = 4,
        InSettings = 5,
        InPause = 6
    }
}