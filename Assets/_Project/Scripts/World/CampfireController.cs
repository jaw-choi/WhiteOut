using System;
using UnityEngine;
using WhiteOut.Config;
using WhiteOut.Inventory;

namespace WhiteOut.World
{
    [DisallowMultipleComponent]
    public sealed class CampfireController : MonoBehaviour
    {
        private const int WoodPerFuelAction = 1;

        [Header("Balance")]
        [SerializeField] private GameBalanceConfig balanceConfig = null;

        [Header("State")]
        [SerializeField] private bool startLit = false;
        [SerializeField] private bool isLit;
        [SerializeField] private float remainingBurnTime;

        [Header("Automation")]
        [SerializeField] private float autoIgniteRadius = 2f;
        [SerializeField] private float heatRadius = 3f;
        [SerializeField] private float autoFuelCheckInterval = 0.25f;
        [SerializeField] private float autoRefuelBelowSeconds = 1f;
        [SerializeField] private LayerMask playerSearchMask = ~0;

        private readonly Collider[] playerHits = new Collider[12];
        private float nextAutoFuelCheckTime;

        public bool IsLit => isLit;
        public float RemainingBurnTime => remainingBurnTime;
        public float AutoIgniteRadius => autoIgniteRadius;
        public float HeatRadius => heatRadius;

        public event Action<bool> LitStateChanged;
        public event Action<float> BurnTimeChanged;

        private void Awake()
        {
            isLit = startLit;

            if (isLit && remainingBurnTime <= 0f)
            {
                remainingBurnTime = GetStartingBurnSeconds();
            }

            if (!isLit)
            {
                remainingBurnTime = 0f;
            }
        }

        private void OnValidate()
        {
            remainingBurnTime = Mathf.Max(0f, remainingBurnTime);
            autoIgniteRadius = Mathf.Max(0f, autoIgniteRadius);
            heatRadius = Mathf.Max(0f, heatRadius);
            autoFuelCheckInterval = Mathf.Max(0.05f, autoFuelCheckInterval);
            autoRefuelBelowSeconds = Mathf.Max(0f, autoRefuelBelowSeconds);
        }

        private void Update()
        {
            TickBurnTime(Time.deltaTime);

            if (Time.time < nextAutoFuelCheckTime)
            {
                return;
            }

            nextAutoFuelCheckTime = Time.time + autoFuelCheckInterval;
            TryAutoFuelFromNearbyPlayer();
        }

        public bool TryIgnite(PlayerInventory inventory)
        {
            if (isLit)
            {
                return false;
            }

            if (!TryConsumeWoodForFuel(inventory))
            {
                return false;
            }

            AddBurnTime(GetBurnSecondsPerWood());
            SetLit(true);
            return true;
        }

        public bool TryAddFuel(PlayerInventory inventory)
        {
            if (!isLit)
            {
                return TryIgnite(inventory);
            }

            if (!TryConsumeWoodForFuel(inventory))
            {
                return false;
            }

            AddBurnTime(GetBurnSecondsPerWood());
            return true;
        }

        public void Extinguish()
        {
            remainingBurnTime = 0f;
            BurnTimeChanged?.Invoke(remainingBurnTime);
            SetLit(false);
        }

        private void TickBurnTime(float deltaTime)
        {
            if (!isLit)
            {
                return;
            }

            var previousBurnTime = remainingBurnTime;
            remainingBurnTime = Mathf.Max(0f, remainingBurnTime - deltaTime);

            if (!Mathf.Approximately(previousBurnTime, remainingBurnTime))
            {
                BurnTimeChanged?.Invoke(remainingBurnTime);
            }

            if (remainingBurnTime <= 0f)
            {
                SetLit(false);
            }
        }

        private void TryAutoFuelFromNearbyPlayer()
        {
            var inventory = FindNearbyInventory();
            if (inventory == null)
            {
                return;
            }

            if (!isLit)
            {
                TryIgnite(inventory);
                return;
            }

            if (remainingBurnTime <= autoRefuelBelowSeconds)
            {
                TryAddFuel(inventory);
            }
        }

        private PlayerInventory FindNearbyInventory()
        {
            if (autoIgniteRadius <= 0f)
            {
                return null;
            }

            var hitCount = Physics.OverlapSphereNonAlloc(
                transform.position,
                autoIgniteRadius,
                playerHits,
                playerSearchMask,
                QueryTriggerInteraction.Collide);

            for (var i = 0; i < hitCount; i++)
            {
                var hit = playerHits[i];
                if (hit == null)
                {
                    continue;
                }

                var inventory = hit.GetComponentInParent<PlayerInventory>();
                if (inventory != null)
                {
                    return inventory;
                }
            }

            return null;
        }

        private bool TryConsumeWoodForFuel(PlayerInventory inventory)
        {
            return inventory != null && inventory.TrySpendWood(WoodPerFuelAction);
        }

        private void AddBurnTime(float seconds)
        {
            if (seconds <= 0f)
            {
                return;
            }

            remainingBurnTime += seconds;
            BurnTimeChanged?.Invoke(remainingBurnTime);
        }

        private void SetLit(bool lit)
        {
            if (isLit == lit)
            {
                return;
            }

            isLit = lit;
            LitStateChanged?.Invoke(isLit);
        }

        private float GetStartingBurnSeconds()
        {
            return balanceConfig != null ? balanceConfig.StartingCampfireBurnSeconds : 30f;
        }

        private float GetBurnSecondsPerWood()
        {
            return balanceConfig != null ? balanceConfig.BurnSecondsPerWood : 3f;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, autoIgniteRadius);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, heatRadius);
        }
    }
}
