using UnityEngine;
using WhiteOut.Systems;

namespace WhiteOut.Player
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerMover : MonoBehaviour
    {
        [SerializeField] private PlayerInputReader inputReader;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private GameFlowController gameFlowController;
        [SerializeField] private Transform movementReference = null;
        [SerializeField] private Transform visualRoot;
        [SerializeField] private float moveSpeed = 4.5f;
        [SerializeField] private float rotationSharpness = 12f;

        public Vector3 CurrentMoveDirection { get; private set; }

        private void Awake()
        {
            if (characterController == null)
            {
                characterController = GetComponent<CharacterController>();
            }

            if (inputReader == null)
            {
                inputReader = GetComponent<PlayerInputReader>();
            }

            if (gameFlowController == null)
            {
                gameFlowController = FindFirstObjectByType<GameFlowController>();
            }

            if (visualRoot == null)
            {
                visualRoot = transform;
            }
        }

        private void Update()
        {
            if (gameFlowController != null && gameFlowController.IsGameOver)
            {
                CurrentMoveDirection = Vector3.zero;
                return;
            }

            var moveInput = inputReader != null ? inputReader.CurrentMoveVector : Vector2.zero;
            CurrentMoveDirection = CalculateWorldMove(moveInput);

            if (CurrentMoveDirection.sqrMagnitude > 0f)
            {
                characterController.Move(CurrentMoveDirection * moveSpeed * Time.deltaTime);
                RotateVisualTowards(CurrentMoveDirection);
            }
        }

        private Vector3 CalculateWorldMove(Vector2 moveInput)
        {
            if (moveInput.sqrMagnitude <= 0f)
            {
                return Vector3.zero;
            }

            var reference = movementReference != null ? movementReference : Camera.main != null ? Camera.main.transform : null;

            var forward = reference != null ? reference.forward : Vector3.forward;
            var right = reference != null ? reference.right : Vector3.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            var move = (right * moveInput.x) + (forward * moveInput.y);
            return move.sqrMagnitude > 1f ? move.normalized : move;
        }

        private void RotateVisualTowards(Vector3 moveDirection)
        {
            if (visualRoot == null || moveDirection.sqrMagnitude <= 0f)
            {
                return;
            }

            var targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            var blend = 1f - Mathf.Exp(-rotationSharpness * Time.deltaTime);
            visualRoot.rotation = Quaternion.Slerp(visualRoot.rotation, targetRotation, blend);
        }
    }
}
