using System.Collections.Generic;
using UnityEngine;
using WhiteOut.Inventory;
using WhiteOut.Systems;

namespace WhiteOut.World
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SphereCollider))]
    public sealed class CampfireHeatZone : MonoBehaviour
    {
        [SerializeField] private SurvivalSystem survivalSystem = null;
        [SerializeField] private CampfireController campfireController = null;
        [SerializeField] private SphereCollider triggerCollider = null;
        [SerializeField] private bool requireLitCampfire = true;
        [SerializeField] private float heatRadius = 3f;

        private readonly HashSet<PlayerInventory> playersInZone = new HashSet<PlayerInventory>();
        private bool heatApplied;

        private void Awake()
        {
            if (triggerCollider == null)
            {
                triggerCollider = GetComponent<SphereCollider>();
            }

            if (campfireController == null)
            {
                campfireController = GetComponentInParent<CampfireController>();
            }

            if (survivalSystem == null)
            {
                survivalSystem = FindFirstObjectByType<SurvivalSystem>();
            }

            SyncColliderRadius();
        }

        private void OnEnable()
        {
            if (campfireController != null)
            {
                campfireController.LitStateChanged += HandleCampfireLitStateChanged;
            }
        }

        private void OnDisable()
        {
            if (campfireController != null)
            {
                campfireController.LitStateChanged -= HandleCampfireLitStateChanged;
            }

            if (heatApplied && survivalSystem != null)
            {
                survivalSystem.ExitCampfireHeatZone();
            }

            heatApplied = false;
            playersInZone.Clear();
        }

        private void OnValidate()
        {
            heatRadius = Mathf.Max(0f, heatRadius);

            if (triggerCollider == null)
            {
                triggerCollider = GetComponent<SphereCollider>();
            }

            if (triggerCollider != null)
            {
                triggerCollider.isTrigger = true;
                triggerCollider.radius = heatRadius;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var inventory = other.GetComponentInParent<PlayerInventory>();
            if (inventory == null)
            {
                return;
            }

            if (playersInZone.Add(inventory))
            {
                RefreshHeatState();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var inventory = other.GetComponentInParent<PlayerInventory>();
            if (inventory == null)
            {
                return;
            }

            if (playersInZone.Remove(inventory))
            {
                RefreshHeatState();
            }
        }

        private void HandleCampfireLitStateChanged(bool isLit)
        {
            RefreshHeatState();
        }

        private void RefreshHeatState()
        {
            var shouldApplyHeat = survivalSystem != null
                && playersInZone.Count > 0
                && (!requireLitCampfire || campfireController == null || campfireController.IsLit);

            if (heatApplied == shouldApplyHeat)
            {
                return;
            }

            heatApplied = shouldApplyHeat;

            if (heatApplied)
            {
                survivalSystem.EnterCampfireHeatZone();
            }
            else
            {
                survivalSystem.ExitCampfireHeatZone();
            }
        }

        private void SyncColliderRadius()
        {
            if (campfireController != null)
            {
                heatRadius = campfireController.HeatRadius;
            }

            if (triggerCollider == null)
            {
                return;
            }

            triggerCollider.isTrigger = true;
            triggerCollider.radius = heatRadius;
        }
    }
}
