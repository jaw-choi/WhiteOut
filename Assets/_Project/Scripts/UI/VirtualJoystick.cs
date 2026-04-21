using System;
using UnityEngine;
using UnityEngine.EventSystems;
using WhiteOut.Systems;

namespace WhiteOut.UI
{
    [DisallowMultipleComponent]
    public sealed class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform background;
        [SerializeField] private RectTransform handle = null;
        [SerializeField] private float handleRange = 1f;
        [SerializeField] private GameFlowController gameFlowController = null;
        [SerializeField] private GameObject visualRoot = null;
        [SerializeField] private bool hideVisualsOnGameOver = true;

        public Vector2 CurrentValue { get; private set; }

        public event Action<Vector2> ValueChanged;

        private bool inputEnabled = true;
        private RectTransform ActiveBackground => background != null ? background : transform as RectTransform;

        private void Awake()
        {
            if (background == null)
            {
                background = transform as RectTransform;
            }

            if (gameFlowController == null)
            {
                gameFlowController = FindFirstObjectByType<GameFlowController>();
            }
        }

        private void OnEnable()
        {
            if (gameFlowController != null)
            {
                gameFlowController.GameOverStateChanged += HandleGameOverStateChanged;
                HandleGameOverStateChanged(gameFlowController.IsGameOver);
            }
        }

        private void OnDisable()
        {
            if (gameFlowController != null)
            {
                gameFlowController.GameOverStateChanged -= HandleGameOverStateChanged;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!inputEnabled)
            {
                return;
            }

            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!inputEnabled)
            {
                return;
            }

            var activeBackground = ActiveBackground;
            if (activeBackground == null)
            {
                return;
            }

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    activeBackground,
                    eventData.position,
                    eventData.pressEventCamera,
                    out var localPoint))
            {
                return;
            }

            var radius = Mathf.Min(activeBackground.rect.width, activeBackground.rect.height) * 0.5f;
            if (radius <= 0f)
            {
                return;
            }

            var normalized = localPoint / radius;
            SetValue(Vector2.ClampMagnitude(normalized, 1f));
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            SetValue(Vector2.zero);
        }

        public void SetValue(Vector2 value)
        {
            CurrentValue = Vector2.ClampMagnitude(value, 1f);
            UpdateHandleVisual();
            ValueChanged?.Invoke(CurrentValue);
        }

        private void UpdateHandleVisual()
        {
            if (handle == null)
            {
                return;
            }

            var activeBackground = ActiveBackground;
            if (activeBackground == null)
            {
                handle.anchoredPosition = Vector2.zero;
                return;
            }

            var radius = Mathf.Min(activeBackground.rect.width, activeBackground.rect.height) * 0.5f;
            handle.anchoredPosition = CurrentValue * radius * handleRange;
        }

        private void HandleGameOverStateChanged(bool isGameOver)
        {
            SetInputEnabled(!isGameOver);
        }

        private void SetInputEnabled(bool isEnabled)
        {
            inputEnabled = isEnabled;

            if (!inputEnabled)
            {
                SetValue(Vector2.zero);
            }

            if (hideVisualsOnGameOver && visualRoot != null)
            {
                visualRoot.SetActive(inputEnabled);
            }
        }
    }
}
