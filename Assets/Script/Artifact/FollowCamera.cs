using UnityEngine;

namespace ArtifactInspector
{
    /// <summary>Simple smoothed third-person follow camera.</summary>
    public sealed class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0f, 9f, -8f);
        [SerializeField] private float smoothTime = 0.15f;

        private Vector3 _velocity;

        private void LateUpdate()
        {
            if (target == null) return;

            transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref _velocity, smoothTime);
            transform.LookAt(target.position + Vector3.up);
        }
    }
}
