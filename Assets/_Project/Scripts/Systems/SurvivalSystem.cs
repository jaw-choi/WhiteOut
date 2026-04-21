using System;
using UnityEngine;
using WhiteOut.Config;

namespace WhiteOut.Systems
{
    [DisallowMultipleComponent]
    public sealed class SurvivalSystem : MonoBehaviour
    {
        [SerializeField] private GameBalanceConfig balanceConfig = null;
        [SerializeField] private BlizzardSystem blizzardSystem;

        private int activeHeatSourceCount;
        private bool manualHeatSourceActive;

        public float CurrentHealth { get; private set; }
        public float CurrentTemperature { get; private set; }
        public bool IsInCampfireHeat => activeHeatSourceCount > 0 || manualHeatSourceActive;
        public bool IsFrozen => CurrentTemperature <= 0f;
        public bool IsAlive => CurrentHealth > 0f;

        public float MaxHealth => balanceConfig != null ? balanceConfig.MaxHealth : 100f;
        public float MaxTemperature => balanceConfig != null ? balanceConfig.MaxTemperature : 100f;

        public event Action<float, float> HealthChanged;
        public event Action<float, float> TemperatureChanged;
        public event Action<bool> HeatStateChanged;
        public event Action Died;

        private void Awake()
        {
            if (blizzardSystem == null)
            {
                blizzardSystem = FindFirstObjectByType<BlizzardSystem>();
            }

            ResetState();
        }

        private void Update()
        {
            if (!IsAlive)
            {
                return;
            }

            UpdateTemperature(Time.deltaTime);

            if (IsFrozen)
            {
                ApplyHealthDelta(-(GetHealthLossWhenFrozenPerSecond() * Time.deltaTime));
            }
        }

        public void ResetState()
        {
            activeHeatSourceCount = 0;
            manualHeatSourceActive = false;
            CurrentHealth = Mathf.Clamp(GetStartingHealth(), 0f, MaxHealth);
            CurrentTemperature = Mathf.Clamp(GetStartingTemperature(), 0f, MaxTemperature);
            HealthChanged?.Invoke(CurrentHealth, MaxHealth);
            TemperatureChanged?.Invoke(CurrentTemperature, MaxTemperature);
            HeatStateChanged?.Invoke(false);
        }

        public void EnterCampfireHeatZone()
        {
            var wasInHeat = IsInCampfireHeat;
            activeHeatSourceCount++;
            NotifyHeatStateIfChanged(wasInHeat);
        }

        public void ExitCampfireHeatZone()
        {
            var wasInHeat = IsInCampfireHeat;
            if (activeHeatSourceCount <= 0)
            {
                activeHeatSourceCount = 0;
                NotifyHeatStateIfChanged(wasInHeat);
                return;
            }

            activeHeatSourceCount--;
            NotifyHeatStateIfChanged(wasInHeat);
        }

        public void SetCampfireHeatActive(bool isActive)
        {
            var wasInHeat = IsInCampfireHeat;
            manualHeatSourceActive = isActive;
            NotifyHeatStateIfChanged(wasInHeat);
        }

        public void ApplyHealthDelta(float delta)
        {
            if (!IsAlive && delta <= 0f)
            {
                return;
            }

            var previousHealth = CurrentHealth;
            CurrentHealth = Mathf.Clamp(CurrentHealth + delta, 0f, MaxHealth);

            if (!Mathf.Approximately(previousHealth, CurrentHealth))
            {
                HealthChanged?.Invoke(CurrentHealth, MaxHealth);
            }

            if (previousHealth > 0f && CurrentHealth <= 0f)
            {
                Died?.Invoke();
            }
        }

        public void ApplyTemperatureDelta(float delta)
        {
            var previousTemperature = CurrentTemperature;
            CurrentTemperature = Mathf.Clamp(CurrentTemperature + delta, 0f, MaxTemperature);

            if (!Mathf.Approximately(previousTemperature, CurrentTemperature))
            {
                TemperatureChanged?.Invoke(CurrentTemperature, MaxTemperature);
            }
        }

        private void UpdateTemperature(float deltaTime)
        {
            if (IsInCampfireHeat)
            {
                ApplyTemperatureDelta(GetTemperatureRecoveryPerSecond() * deltaTime);
                return;
            }

            var temperatureLoss = GetBaseTemperatureLossPerSecond();
            if (blizzardSystem != null)
            {
                temperatureLoss += blizzardSystem.CurrentSeverity;
            }

            ApplyTemperatureDelta(-(temperatureLoss * deltaTime));
        }

        private void NotifyHeatStateIfChanged(bool wasInHeat)
        {
            if (wasInHeat != IsInCampfireHeat)
            {
                HeatStateChanged?.Invoke(IsInCampfireHeat);
            }
        }

        private float GetStartingHealth()
        {
            return balanceConfig != null ? balanceConfig.StartingHealth : 100f;
        }

        private float GetStartingTemperature()
        {
            return balanceConfig != null ? balanceConfig.StartingTemperature : 100f;
        }

        private float GetBaseTemperatureLossPerSecond()
        {
            return balanceConfig != null ? balanceConfig.BaseTemperatureLossPerSecond : 1f;
        }

        private float GetTemperatureRecoveryPerSecond()
        {
            return balanceConfig != null ? balanceConfig.TemperatureRecoveryPerSecond : 5f;
        }

        private float GetHealthLossWhenFrozenPerSecond()
        {
            return balanceConfig != null ? balanceConfig.HealthLossWhenFrozenPerSecond : 2f;
        }
    }
}
