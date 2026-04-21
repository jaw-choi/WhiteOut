using System;
using UnityEngine;
using WhiteOut.Config;
using WhiteOut.Inventory;

namespace WhiteOut.Systems
{
    [DisallowMultipleComponent]
    public sealed class CraftingSystem : MonoBehaviour
    {
        [SerializeField] private GameBalanceConfig balanceConfig = null;
        [SerializeField] private PlayerInventory playerInventory = null;
        [SerializeField] private SurvivalSystem survivalSystem = null;
        [SerializeField] private GameFlowController gameFlowController = null;

        private float nextCraftCheckTime;

        public event Action ToolCrafted;
        public event Action FoodCreated;
        public event Action FoodConsumedForHealth;
        public event Action FoodConvertedToMoney;

        private void Awake()
        {
            if (playerInventory == null)
            {
                playerInventory = FindFirstObjectByType<PlayerInventory>();
            }

            if (survivalSystem == null)
            {
                survivalSystem = FindFirstObjectByType<SurvivalSystem>();
            }

            if (gameFlowController == null)
            {
                gameFlowController = FindFirstObjectByType<GameFlowController>();
            }
        }

        private void Update()
        {
            if (gameFlowController != null && gameFlowController.IsGameOver)
            {
                return;
            }

            if (Time.time < nextCraftCheckTime)
            {
                return;
            }

            nextCraftCheckTime = Time.time + GetAutoCraftCheckInterval();
            TryRunSingleCraftingStep();
        }

        public bool TryRunSingleCraftingStep()
        {
            if (playerInventory == null)
            {
                return false;
            }

            if (!playerInventory.HasTool)
            {
                return TryCraftTool();
            }

            return TryCreateAndResolveFood();
        }

        private bool TryCraftTool()
        {
            if (playerInventory == null)
            {
                return false;
            }

            if (!playerInventory.TryCraftPermanentTool(GetToolWoodCost()))
            {
                return false;
            }

            ToolCrafted?.Invoke();
            return true;
        }

        private bool TryCreateAndResolveFood()
        {
            if (playerInventory == null || survivalSystem == null)
            {
                return false;
            }

            if (!survivalSystem.IsInCampfireHeat)
            {
                return false;
            }

            if (!playerInventory.HasTool)
            {
                return false;
            }

            if (!playerInventory.TrySpendWood(GetFoodWoodCost()))
            {
                return false;
            }

            playerInventory.AddFood(1);
            FoodCreated?.Invoke();
            TryAutoConsumeFood();
            return true;
        }

        private bool TryAutoConsumeFood()
        {
            if (playerInventory == null || survivalSystem == null)
            {
                return false;
            }

            if (!playerInventory.TrySpendFood(1))
            {
                return false;
            }

            if (survivalSystem.CurrentHealth < survivalSystem.MaxHealth)
            {
                survivalSystem.ApplyHealthDelta(GetFoodHealAmount());
                FoodConsumedForHealth?.Invoke();
            }
            else
            {
                playerInventory.AddMoney(GetMoneyPerOverflowFood());
                FoodConvertedToMoney?.Invoke();
            }

            return true;
        }

        private int GetToolWoodCost()
        {
            return balanceConfig != null ? balanceConfig.ToolWoodCost : 5;
        }

        private int GetFoodWoodCost()
        {
            return balanceConfig != null ? balanceConfig.FoodWoodCost : 5;
        }

        private int GetFoodHealAmount()
        {
            return balanceConfig != null ? balanceConfig.FoodHealAmount : 1;
        }

        private int GetMoneyPerOverflowFood()
        {
            return balanceConfig != null ? balanceConfig.MoneyPerOverflowFood : 10;
        }

        private float GetAutoCraftCheckInterval()
        {
            return balanceConfig != null ? balanceConfig.AutoCraftCheckInterval : 0.25f;
        }
    }
}
