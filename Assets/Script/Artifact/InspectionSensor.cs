using UnityEngine;

namespace ArtifactInspector
{
    /// <summary>
    /// Attach to the player. Periodically finds the nearest IInspectable within range
    /// and publishes focus changes through InspectionEvents.
    /// </summary>
    public sealed class InspectionSensor : MonoBehaviour
    {
        [SerializeField, Min(0.5f)] private float radius = 3f;
        [SerializeField, Min(0.02f)] private float scanInterval = 0.1f;
        [SerializeField] private LayerMask layers = ~0;

        private readonly Collider[] _buffer = new Collider[32]; // reused: no GC allocations
        private IInspectable _current;
        private float _nextScanTime;

        private void Update()
        {
            if (Time.time < _nextScanTime) return;
            _nextScanTime = Time.time + scanInterval;
            Scan();
        }

        private void OnDisable()
        {
            if (_current == null) return;
            _current = null;
            InspectionEvents.RaiseCleared();
        }

        private void Scan()
        {
            int count = Physics.OverlapSphereNonAlloc(transform.position, radius, _buffer, layers,
                QueryTriggerInteraction.Collide);

            IInspectable nearest = null;
            float nearestSqr = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                var candidate = _buffer[i].GetComponentInParent<IInspectable>();
                if (candidate == null) continue;

                float sqr = (candidate.Position - transform.position).sqrMagnitude;
                if (sqr < nearestSqr)
                {
                    nearestSqr = sqr;
                    nearest = candidate;
                }
            }

            if (ReferenceEquals(nearest, _current)) return;

            _current = nearest;
            if (_current != null) InspectionEvents.RaiseFocused(_current);
            else InspectionEvents.RaiseCleared();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
