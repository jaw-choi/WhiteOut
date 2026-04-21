using System;
using UnityEngine;
using UnityEngine.InputSystem;
using WhiteOut.Core;
using WhiteOut.UI;

namespace WhiteOut.Player
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(PlayerInput))]
    public sealed class PlayerInputReader : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private VirtualJoystick virtualJoystick;
        [SerializeField] private string moveActionName = GameConstants.MoveAction;

        private InputAction moveAction;
        private Vector2 actionMove;
        private Vector2 virtualMove;

        public Vector2 CurrentMoveVector => Vector2.ClampMagnitude(actionMove + virtualMove, 1f);
        public Vector2 ActionMoveVector => actionMove;
        public Vector2 VirtualMoveVector => virtualMove;

        public event Action<Vector2> MoveChanged;

        private void Awake()
        {
            if (playerInput == null)
            {
                playerInput = GetComponent<PlayerInput>();
            }
        }

        private void OnEnable()
        {
            CacheMoveAction();
            SubscribeMoveAction();
            SubscribeJoystick(virtualJoystick);
            NotifyMoveChanged();
        }

        private void OnDisable()
        {
            UnsubscribeMoveAction();
            SubscribeJoystick(null);
            actionMove = Vector2.zero;
            virtualMove = Vector2.zero;
            NotifyMoveChanged();
        }

        public void SetVirtualJoystick(VirtualJoystick joystick)
        {
            if (virtualJoystick == joystick)
            {
                return;
            }

            SubscribeJoystick(null);
            virtualJoystick = joystick;

            if (isActiveAndEnabled)
            {
                SubscribeJoystick(virtualJoystick);
                NotifyMoveChanged();
            }
        }

        private void CacheMoveAction()
        {
            moveAction = playerInput != null && playerInput.actions != null
                ? playerInput.actions.FindAction(moveActionName, false)
                : null;

            if (moveAction == null)
            {
                Debug.LogWarning($"PlayerInputReader could not find move action '{moveActionName}' on {name}.", this);
            }
        }

        private void SubscribeMoveAction()
        {
            if (moveAction == null)
            {
                return;
            }

            moveAction.performed += OnMovePerformed;
            moveAction.canceled += OnMovePerformed;
            actionMove = moveAction.ReadValue<Vector2>();
        }

        private void UnsubscribeMoveAction()
        {
            if (moveAction == null)
            {
                return;
            }

            moveAction.performed -= OnMovePerformed;
            moveAction.canceled -= OnMovePerformed;
            moveAction = null;
        }

        private void SubscribeJoystick(VirtualJoystick joystick)
        {
            if (virtualJoystick != null)
            {
                virtualJoystick.ValueChanged -= OnVirtualJoystickChanged;
            }

            virtualJoystick = joystick;

            if (virtualJoystick == null)
            {
                virtualMove = Vector2.zero;
                return;
            }

            virtualJoystick.ValueChanged += OnVirtualJoystickChanged;
            virtualMove = virtualJoystick.CurrentValue;
        }

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            actionMove = context.ReadValue<Vector2>();
            NotifyMoveChanged();
        }

        private void OnVirtualJoystickChanged(Vector2 value)
        {
            virtualMove = value;
            NotifyMoveChanged();
        }

        private void NotifyMoveChanged()
        {
            MoveChanged?.Invoke(CurrentMoveVector);
        }
    }
}
