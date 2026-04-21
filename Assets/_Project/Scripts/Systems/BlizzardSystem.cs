using System;
using UnityEngine;

namespace WhiteOut.Systems
{
    [DisallowMultipleComponent]
    public sealed class BlizzardSystem : MonoBehaviour
    {
        [SerializeField] private float maxSeverity = 1f;
        [SerializeField] private float severityIncreasePerSecond = 0.02f;
        [SerializeField] private GameFlowController gameFlowController;

        public float CurrentSeverity { get; private set; }
        public float MaxSeverity => maxSeverity;
        public float SeverityIncreasePerSecond => severityIncreasePerSecond;

        public event Action<float> SeverityChanged;

        private void Awake()
        {
            if (gameFlowController == null)
            {
                gameFlowController = FindFirstObjectByType<GameFlowController>();
            }
        }

        private void OnValidate()
        {
            maxSeverity = Mathf.Max(0f, maxSeverity);
            severityIncreasePerSecond = Mathf.Max(0f, severityIncreasePerSecond);
            CurrentSeverity = Mathf.Clamp(CurrentSeverity, 0f, maxSeverity);
        }

        private void Update()
        {
            if (gameFlowController != null && gameFlowController.IsGameOver)
            {
                return;
            }

            if (CurrentSeverity >= maxSeverity)
            {
                return;
            }

            SetSeverity(CurrentSeverity + (severityIncreasePerSecond * Time.deltaTime));
        }

        public void ResetSeverity()
        {
            SetSeverity(0f);
        }

        public void SetSeverity(float newSeverity)
        {
            var clamped = Mathf.Clamp(newSeverity, 0f, maxSeverity);
            if (Mathf.Approximately(clamped, CurrentSeverity))
            {
                return;
            }

            CurrentSeverity = clamped;
            SeverityChanged?.Invoke(CurrentSeverity);
        }
    }
}
