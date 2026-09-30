using UnityEngine;

namespace EnemyAI
{
    /// <summary>
    /// Base class for every enemy behaviour (State pattern). Each state is a ScriptableObject
    /// asset that also carries its own display name, activation key and tint colour, so the
    /// controller never needs to know which states exist.
    ///
    /// To add a new behaviour: subclass this, implement Tick(), create an asset, add it to
    /// the enemy's state list. No existing code changes.
    ///
    /// NOTE: the controller clones each asset at runtime, so it is safe to keep per-enemy
    /// runtime fields (timers, indices) inside a state.
    /// </summary>
    public abstract class EnemyState : ScriptableObject
    {
        [SerializeField] private string displayName = "State";
        [SerializeField] private KeyCode activationKey = KeyCode.None;
        [SerializeField] private Color tint = Color.white;

        public string DisplayName => displayName;
        public KeyCode ActivationKey => activationKey;
        public Color Tint => tint;

        public virtual void Enter(EnemyContext context) { }
        public abstract void Tick(EnemyContext context);
        public virtual void Exit(EnemyContext context) { }
    }
}
