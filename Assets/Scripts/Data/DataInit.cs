namespace Data {
    public static class DataInit {
        public static bool IsInit { get; set; }

        public static void Init() {
            if (IsInit) return;
            Animator.Init();
            Res.Init();
            Settings.Init();
            IsInit = true;
            if (Tracker.IsInit) Tracker.Instance.Log("All data was successfully initialized");
        }
    }
}
