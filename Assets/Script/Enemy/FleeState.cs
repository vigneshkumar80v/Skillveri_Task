using UnityEngine;

namespace EnemyAI
{
    [CreateAssetMenu(
        menuName = "Enemy AI/States/Flee",
        fileName = "FleeState"
    )]
    public sealed class FleeState : EnemyState
    {
        [SerializeField] private float speed = 4f;

        public override void Tick(EnemyContext context)
        {
            if (context.Target == null)
                return;

            Vector3 away =
                context.Self.position +
                (context.Self.position - context.Target.position);

            context.MoveTowards(away, speed);
        }
    }
}