namespace WhiteOut.Core
{
    public static class GameConstants
    {
        public const string ProjectRoot = "Assets/_Project";
        public const string ScriptsRoot = ProjectRoot + "/Scripts";
        public const string PrefabsRoot = ProjectRoot + "/Prefabs";
        public const string ScenesRoot = ProjectRoot + "/Scenes";
        public const string InputRoot = ProjectRoot + "/Input";
        public const string ArtRoot = ProjectRoot + "/Art";
        public const string AudioRoot = ProjectRoot + "/Audio";
        public const string UIRoot = ProjectRoot + "/UI";

        public const string GameplayActionMap = "Gameplay";
        public const string UIActionMap = "UI";
        public const string MoveAction = "Move";
        public const string RestartAction = "Restart";

        // Temporary defaults until a GameBalanceConfig asset owns tuning.
        public static readonly float DefaultStartingHealth = 100f;
        public static readonly float DefaultMaxHealth = 100f;
        public static readonly float DefaultStartingTemperature = 100f;
        public static readonly float DefaultMaxTemperature = 100f;
        public static readonly float DefaultTemperatureDecayPerSecond = 1f;
        public static readonly float DefaultCampfireTemperatureRecoveryPerSecond = 5f;
        public static readonly float DefaultHealthDecayAtZeroTemperaturePerSecond = 2f;
        public static readonly float DefaultCampfireBurnSecondsPerWood = 3f;
        public static readonly int DefaultMoneyPerFoodOverflow = 10;
    }
}
