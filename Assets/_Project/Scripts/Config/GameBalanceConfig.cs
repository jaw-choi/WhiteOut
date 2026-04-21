using UnityEngine;

namespace WhiteOut.Config
{
    [CreateAssetMenu(fileName = "GameBalanceConfig", menuName = "WhiteOut/Game Balance Config")]
    public sealed class GameBalanceConfig : ScriptableObject
    {
        [Header("Vitals")]
        [SerializeField] private float startingHealth = 100f;
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float startingTemperature = 100f;
        [SerializeField] private float maxTemperature = 100f;
        [SerializeField] private float baseTemperatureLossPerSecond = 1f;
        [SerializeField] private float temperatureRecoveryPerSecond = 5f;
        [SerializeField] private float healthLossWhenFrozenPerSecond = 2f;

        [Header("Campfire")]
        [SerializeField] private float startingCampfireBurnSeconds = 30f;
        [SerializeField] private float burnSecondsPerWood = 3f;

        [Header("Crafting")]
        [SerializeField] private int toolWoodCost = 5;
        [SerializeField] private int foodWoodCost = 5;
        [SerializeField] private int foodHealAmount = 1;
        [SerializeField] private int moneyPerOverflowFood = 10;
        [SerializeField] private float autoCraftCheckInterval = 0.25f;

        public float StartingHealth => startingHealth;
        public float MaxHealth => maxHealth;
        public float StartingTemperature => startingTemperature;
        public float MaxTemperature => maxTemperature;
        public float BaseTemperatureLossPerSecond => baseTemperatureLossPerSecond;
        public float TemperatureRecoveryPerSecond => temperatureRecoveryPerSecond;
        public float HealthLossWhenFrozenPerSecond => healthLossWhenFrozenPerSecond;
        public float StartingCampfireBurnSeconds => startingCampfireBurnSeconds;
        public float BurnSecondsPerWood => burnSecondsPerWood;
        public int ToolWoodCost => toolWoodCost;
        public int FoodWoodCost => foodWoodCost;
        public int FoodHealAmount => foodHealAmount;
        public int MoneyPerOverflowFood => moneyPerOverflowFood;
        public float AutoCraftCheckInterval => autoCraftCheckInterval;

        private void OnValidate()
        {
            maxHealth = Mathf.Max(1f, maxHealth);
            startingHealth = Mathf.Clamp(startingHealth, 0f, maxHealth);

            maxTemperature = Mathf.Max(1f, maxTemperature);
            startingTemperature = Mathf.Clamp(startingTemperature, 0f, maxTemperature);

            baseTemperatureLossPerSecond = Mathf.Max(0f, baseTemperatureLossPerSecond);
            temperatureRecoveryPerSecond = Mathf.Max(0f, temperatureRecoveryPerSecond);
            healthLossWhenFrozenPerSecond = Mathf.Max(0f, healthLossWhenFrozenPerSecond);
            startingCampfireBurnSeconds = Mathf.Max(0f, startingCampfireBurnSeconds);
            burnSecondsPerWood = Mathf.Max(0f, burnSecondsPerWood);
            toolWoodCost = Mathf.Max(0, toolWoodCost);
            foodWoodCost = Mathf.Max(0, foodWoodCost);
            foodHealAmount = Mathf.Max(0, foodHealAmount);
            moneyPerOverflowFood = Mathf.Max(0, moneyPerOverflowFood);
            autoCraftCheckInterval = Mathf.Max(0.05f, autoCraftCheckInterval);
        }
    }
}
