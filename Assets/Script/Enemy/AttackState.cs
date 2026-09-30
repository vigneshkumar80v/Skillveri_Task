using UnityEngine;

namespace EnemyAI
{
    [CreateAssetMenu(menuName = "Enemy AI/States/Attack", fileName = "AttackState")]
    public sealed class AttackState : EnemyState
    {
        [SerializeField, Min(0.1f)] private float speed = 4.5f;
        [SerializeField, Min(0.1f)] private float stopDistance = 1.5f;

        public override void Tick(EnemyContext context)
        {
            if (context.Target == null) return;

            if (context.FlatDistanceTo(context.Target.position) > stopDistance)
                context.MoveTowards(context.Target.position, speed);
        }
    }
}
