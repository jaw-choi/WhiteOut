using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WhiteOut.Inventory;
using WhiteOut.Systems;

namespace WhiteOut.UI
{
    [Serializable]
    public sealed class UILabel
    {
        [SerializeField] private Text text = null;
        [SerializeField] private TMP_Text tmpText = null;

        public void SetText(string value)
        {
            if (tmpText != null)
            {
                tmpText.text = value;
            }

            if (text != null)
            {
                text.text = value;
            }
        }
    }

    public static class UIFormat
    {
        public static string Wood(int value) => $"Wood: {value}";
        public static string Tool(int value) => value > 0 ? "Tool: Owned" : "Tool: None";
        public static string Food(int value) => $"Food: {value}";
        public static string Money(int value) => $"Money: {value}";
        public static string SurvivalTime(float seconds) => $"Time: {FormatTime(seconds)}";
        public static string BestTime(float seconds) => $"Best: {FormatTime(seconds)}";
        public static string GameOver(float survivalTime, float bestTime) => $"Game Over\nTime: {FormatTime(survivalTime)}\nBest: {FormatTime(bestTime)}";

        public static string FormatTime(float seconds)
        {
            seconds = Mathf.Max(0f, seconds);
            var wholeSeconds = Mathf.FloorToInt(seconds);
            var minutes = wholeSeconds / 60;
            var remainingSeconds = wholeSeconds % 60;
            return $"{minutes:00}:{remainingSeconds:00}";
        }
    }

    [DisallowMultipleComponent]
    public sealed class HUDController : MonoBehaviour
    {
        [Header("Systems")]
        [SerializeField] private SurvivalSystem survivalSystem = null;
        [SerializeField] private PlayerInventory playerInventory = null;
        [SerializeField] private GameFlowController gameFlowController = null;

        [Header("Bars")]
        [SerializeField] private Slider temperatureBar = null;
        [SerializeField] private Slider healthBar = null;

        [Header("Texts")]
        [SerializeField] private UILabel woodCountText = new UILabel();
        [SerializeField] private UILabel toolCountText = new UILabel();
        [SerializeField] private UILabel foodCountText = new UILabel();
        [SerializeField] private UILabel moneyCountText = new UILabel();
        [SerializeField] private UILabel survivalTimeText = new UILabel();
        [SerializeField] private UILabel bestTimeText = new UILabel();

        private void Awake()
        {
            if (survivalSystem == null)
            {
                survivalSystem = FindFirstObjectByType<SurvivalSystem>();
            }

            if (playerInventory == null)
            {
                playerInventory = FindFirstObjectByType<PlayerInventory>();
            }

            if (gameFlowController == null)
            {
                gameFlowController = FindFirstObjectByType<GameFlowController>();
            }
        }

        private void OnEnable()
        {
            if (survivalSystem != null)
            {
                survivalSystem.HealthChanged += HandleHealthChanged;
                survivalSystem.TemperatureChanged += HandleTemperatureChanged;
            }

            if (playerInventory != null)
            {
                playerInventory.InventoryChanged += HandleInventoryChanged;
            }

            if (gameFlowController != null)
            {
                gameFlowController.SurvivalTimeChanged += HandleSurvivalTimeChanged;
                gameFlowController.BestSurvivalTimeChanged += HandleBestSurvivalTimeChanged;
            }

            RefreshAll();
        }

        private void OnDisable()
        {
            if (survivalSystem != null)
            {
                survivalSystem.HealthChanged -= HandleHealthChanged;
                survivalSystem.TemperatureChanged -= HandleTemperatureChanged;
            }

            if (playerInventory != null)
            {
                playerInventory.InventoryChanged -= HandleInventoryChanged;
            }

            if (gameFlowController != null)
            {
                gameFlowController.SurvivalTimeChanged -= HandleSurvivalTimeChanged;
                gameFlowController.BestSurvivalTimeChanged -= HandleBestSurvivalTimeChanged;
            }
        }

        private void RefreshAll()
        {
            if (survivalSystem != null)
            {
                HandleHealthChanged(survivalSystem.CurrentHealth, survivalSystem.MaxHealth);
                HandleTemperatureChanged(survivalSystem.CurrentTemperature, survivalSystem.MaxTemperature);
            }

            if (playerInventory != null)
            {
                HandleInventoryChanged(playerInventory);
            }

            if (gameFlowController != null)
            {
                HandleSurvivalTimeChanged(gameFlowController.SurvivalTime);
                HandleBestSurvivalTimeChanged(gameFlowController.BestSurvivalTime);
            }
        }

        private void HandleHealthChanged(float current, float max)
        {
            SetSlider(healthBar, current, max);
        }

        private void HandleTemperatureChanged(float current, float max)
        {
            SetSlider(temperatureBar, current, max);
        }

        private void HandleInventoryChanged(PlayerInventory inventory)
        {
            woodCountText.SetText(UIFormat.Wood(inventory.WoodCount));
            toolCountText.SetText(UIFormat.Tool(inventory.ToolCount));
            foodCountText.SetText(UIFormat.Food(inventory.FoodCount));
            moneyCountText.SetText(UIFormat.Money(inventory.MoneyCount));
        }

        private void HandleSurvivalTimeChanged(float survivalTime)
        {
            survivalTimeText.SetText(UIFormat.SurvivalTime(survivalTime));
        }

        private void HandleBestSurvivalTimeChanged(float bestTime)
        {
            bestTimeText.SetText(UIFormat.BestTime(bestTime));
        }

        private static void SetSlider(Slider slider, float current, float max)
        {
            if (slider == null)
            {
                return;
            }

            slider.minValue = 0f;
            slider.maxValue = Mathf.Max(1f, max);
            slider.value = Mathf.Clamp(current, slider.minValue, slider.maxValue);
        }
    }
}
