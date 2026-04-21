using UnityEngine;
using WhiteOut.Inventory;

namespace WhiteOut.World
{
    [DisallowMultipleComponent]
    public sealed class WoodPickup : MonoBehaviour
    {
        [SerializeField] private int woodAmount = 1;
        [SerializeField] private bool destroyOnCollect = false;

        private bool collected;

        private void OnValidate()
        {
            woodAmount = Mathf.Max(1, woodAmount);

            if (TryGetComponent<Collider>(out var pickupCollider))
            {
                pickupCollider.isTrigger = true;
            }
        }

        private void Awake()
        {
            if (TryGetComponent<Collider>(out var pickupCollider))
            {
                pickupCollider.isTrigger = true;
            }

            if (!TryGetComponent<Rigidbody>(out var pickupRigidbody))
            {
                pickupRigidbody = gameObject.AddComponent<Rigidbody>();
            }

            pickupRigidbody.isKinematic = true;
            pickupRigidbody.useGravity = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            TryCollect(other);
        }

        private void OnTriggerStay(Collider other)
        {
            TryCollect(other);
        }

        private void TryCollect(Collider other)
        {
            if (collected)
            {
                return;
            }

            var inventory = other.GetComponentInParent<PlayerInventory>();
            if (inventory == null)
            {
                return;
            }

            Collect(inventory);
        }

        public void Collect(PlayerInventory inventory)
        {
            if (collected || inventory == null)
            {
                return;
            }

            collected = true;
            inventory.AddWood(woodAmount);

            if (destroyOnCollect)
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
