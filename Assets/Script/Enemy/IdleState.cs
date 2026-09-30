using UnityEngine;

namespace EnemyAI
{
    [CreateAssetMenu(menuName = "Enemy AI/States/Idle", fileName = "IdleState")]
    public sealed class IdleState : EnemyState
    {
        public override void Tick(EnemyContext context)
        {
            // Intentionally does nothing.
        }
    }
}
