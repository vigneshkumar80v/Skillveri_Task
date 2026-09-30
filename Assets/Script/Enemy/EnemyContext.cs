using System.Collections.Generic;
using UnityEngine;

namespace EnemyAI
{
    /// <summary>
    /// What a state is allowed to see and do. States receive this instead of a reference
    /// to the concrete controller, which keeps them decoupled and easy to reuse.
    /// </summary>
    public sealed class EnemyContext
    {
        private readonly float _turnSpeed;

        public Transform Self { get; }
        public Transform Target { get; }
        public IReadOnlyList<Transform> PatrolPoints { get; }

        public EnemyContext(Transform self, Transform target, IReadOnlyList<Transform> patrolPoints, float turnSpeed)
        {
            Self = self;
            Target = target;
            PatrolPoints = patrolPoints ?? new Transform[0];
            _turnSpeed = turnSpeed;
        }

        /// <summary>Distance on the ground plane (ignores height).</summary>
        public float FlatDistanceTo(Vector3 point)
        {
            Vector3 delta = point - Self.position;
            delta.y = 0f;
            return delta.magnitude;
        }

        /// <summary>Turns toward and moves at <paramref name="speed"/> toward a point, staying at the same height.</summary>
        public void MoveTowards(Vector3 destination, float speed)
        {
            Vector3 direction = destination - Self.position;
            direction.y = 0f;
            if (direction.sqrMagnitude < 0.0001f) return;

            Self.rotation = Quaternion.RotateTowards(
                Self.rotation, Quaternion.LookRotation(direction), _turnSpeed * Time.deltaTime);

            float step = Mathf.Min(speed * Time.deltaTime, direction.magnitude);
            Self.position += direction.normalized * step;
        }

        public int ClosestPatrolPointIndex()
        {
            int best = 0;
            float bestDistance = float.MaxValue;

            for (int i = 0; i < PatrolPoints.Count; i++)
            {
                if (PatrolPoints[i] == null) continue;
                float distance = FlatDistanceTo(PatrolPoints[i].position);
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    best = i;
                }
            }
            return best;
        }
    }
}
