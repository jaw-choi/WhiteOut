using UnityEngine;

namespace WhiteOut.CameraSystem
{
    [DisallowMultipleComponent]
    public sealed class QuarterViewCameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0f, 10f, -8f);
        [SerializeField] private float followSmooth = 10f;
        [SerializeField] private float lookAtTargetHeight = 1.5f;

        public Transform Target
        {
            get => target;
            set => target = value;
        }

        private void OnEnable()
        {
            SnapToTarget();
        }

        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            var desiredPosition = target.position + offset;
            var blend = 1f - Mathf.Exp(-followSmooth * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, blend);

            var lookTarget = target.position + (Vector3.up * lookAtTargetHeight);
            var lookDirection = lookTarget - transform.position;

            if (lookDirection.sqrMagnitude > 0.0001f)
            {
                transform.rotation = Quaternion.LookRotation(lookDirection.normalized, Vector3.up);
            }
        }

        private void SnapToTarget()
        {
            if (target == null)
            {
                return;
            }

            transform.position = target.position + offset;

            var lookTarget = target.position + (Vector3.up * lookAtTargetHeight);
            var lookDirection = lookTarget - transform.position;

            if (lookDirection.sqrMagnitude > 0.0001f)
            {
                transform.rotation = Quaternion.LookRotation(lookDirection.normalized, Vector3.up);
            }
        }
    }
}
