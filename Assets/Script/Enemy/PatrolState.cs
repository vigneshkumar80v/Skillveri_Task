using UnityEngine;

namespace EnemyAI
{
    [CreateAssetMenu(menuName = "Enemy AI/States/Patrol", fileName = "PatrolState")]
    public sealed class PatrolState : EnemyState
    {
        [SerializeField, Min(0.1f)] private float speed = 2.5f;
        [SerializeField, Min(0.05f)] private float arriveDistance = 0.2f;

        private int _index; // runtime data, safe because the asset is cloned per enemy

        public override void Enter(EnemyContext context)
        {
            _index = context.ClosestPatrolPointIndex();
        }

        public override void Tick(EnemyContext context)
        {
            int count = context.PatrolPoints.Count;
            if (count == 0 || context.PatrolPoints[_index] == null) return;

            Vector3 destination = context.PatrolPoints[_index].position;
            context.MoveTowards(destination, speed);

            if (context.FlatDistanceTo(destination) <= arriveDistance)
                _index = (_index + 1) % count; // works for 2 points or any number
        }
    }
}
