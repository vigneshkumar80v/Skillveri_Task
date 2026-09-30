using System;

namespace EnemyAI
{
    /// <summary>Plain C# state machine: owns the current state and runs Enter/Tick/Exit.</summary>
    public sealed class EnemyStateMachine
    {
        private readonly EnemyContext _context;

        public EnemyState Current { get; private set; }
        public event Action<EnemyState> StateChanged;

        public EnemyStateMachine(EnemyContext context)
        {
            _context = context;
        }

        public void ChangeState(EnemyState next)
        {
            if (next == null || next == Current) return;

            Current?.Exit(_context);
            Current = next;
            Current.Enter(_context);
            StateChanged?.Invoke(Current);
        }

        public void Tick() => Current?.Tick(_context);
    }
}
