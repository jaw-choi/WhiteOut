using System;
using UnityEngine;

namespace WhiteOut.Inventory
{
    [DisallowMultipleComponent]
    public sealed class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private int woodCount;
        [SerializeField] private int toolCount;
        [SerializeField] private int foodCount;
        [SerializeField] private int moneyCount;

        public int WoodCount => woodCount;
        public int ToolCount => toolCount;
        public int FoodCount => foodCount;
        public int MoneyCount => moneyCount;
        public bool HasTool => toolCount > 0;

        public event Action<PlayerInventory> InventoryChanged;
        public event Action<int> WoodChanged;
        public event Action<int> ToolChanged;
        public event Action<int> FoodChanged;
        public event Action<int> MoneyChanged;

        private void OnValidate()
        {
            woodCount = Mathf.Max(0, woodCount);
            toolCount = Mathf.Max(0, toolCount);
            foodCount = Mathf.Max(0, foodCount);
            moneyCount = Mathf.Max(0, moneyCount);
        }

        public void AddWood(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            woodCount += amount;
            WoodChanged?.Invoke(woodCount);
            InventoryChanged?.Invoke(this);
        }

        public bool TrySpendWood(int amount)
        {
            if (amount <= 0)
            {
                return true;
            }

            if (woodCount < amount)
            {
                return false;
            }

            woodCount -= amount;
            WoodChanged?.Invoke(woodCount);
            InventoryChanged?.Invoke(this);
            return true;
        }

        public void AddTool(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            toolCount += amount;
            ToolChanged?.Invoke(toolCount);
            InventoryChanged?.Invoke(this);
        }

        public bool TryCraftPermanentTool(int woodCost)
        {
            if (HasTool)
            {
                return false;
            }

            if (!TrySpendWood(woodCost))
            {
                return false;
            }

            AddTool(1);
            return true;
        }

        public void AddFood(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            foodCount += amount;
            FoodChanged?.Invoke(foodCount);
            InventoryChanged?.Invoke(this);
        }

        public bool TrySpendFood(int amount)
        {
            if (amount <= 0)
            {
                return true;
            }

            if (foodCount < amount)
            {
                return false;
            }

            foodCount -= amount;
            FoodChanged?.Invoke(foodCount);
            InventoryChanged?.Invoke(this);
            return true;
        }

        public void AddMoney(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            moneyCount += amount;
            MoneyChanged?.Invoke(moneyCount);
            InventoryChanged?.Invoke(this);
        }
    }
}
