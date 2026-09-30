using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EnemyAI
{
    /// <summary>
    /// Wires everything together.
    /// It knows nothing about Idle / Patrol / Attack.
    /// It runs whichever EnemyState assets are in its list
    /// and switches based on each state's ActivationKey.
    /// </summary>
    public sealed class EnemyController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform target;
        [SerializeField] private Transform[] patrolPoints;
        [SerializeField] private Renderer bodyRenderer;

        [Header("Settings")]
        [SerializeField, Min(0f)] private float turnSpeed = 360f;

        [Header("Behaviours (first one is the starting state)")]
        [SerializeField] private List<EnemyState> stateAssets =
            new List<EnemyState>();

        private readonly List<EnemyState> _states =
            new List<EnemyState>();

        private EnemyStateMachine _machine;

        public event Action<EnemyState> StateChanged;

        public IReadOnlyList<EnemyState> States => _states;

        public EnemyState CurrentState => _machine?.Current;

        private void Awake()
        {
            // Clone assets so each enemy has its own runtime state data.
            foreach (var asset in stateAssets)
            {
                if (asset == null)
                    continue;

                var instance = Instantiate(asset);

                instance.name = asset.name;

                _states.Add(instance);
            }

            var context = new EnemyContext(
                transform,
                target,
                patrolPoints,
                turnSpeed
            );

            _machine = new EnemyStateMachine(context);

            _machine.StateChanged += OnStateChanged;
        }

        private void Start()
        {
            if (_states.Count > 0)
            {
                _machine.ChangeState(_states[0]);
            }
        }

        private void Update()
        {
            HandleInput();

            _machine?.Tick();
        }

        private void HandleInput()
        {
            if (Keyboard.current == null)
                return;

            foreach (var state in _states)
            {
                if (IsActivationKeyPressed(state.ActivationKey))
                {
                    _machine.ChangeState(state);
                    return;
                }
            }
        }

     private bool IsActivationKeyPressed(KeyCode key)
{
    if (Keyboard.current == null)
        return false;

    switch (key)
    {
        case KeyCode.Keypad1:
            return Keyboard.current.numpad1Key.wasPressedThisFrame ||
                   Keyboard.current.digit1Key.wasPressedThisFrame;

        case KeyCode.Keypad2:
            return Keyboard.current.numpad2Key.wasPressedThisFrame ||
                   Keyboard.current.digit2Key.wasPressedThisFrame;

        case KeyCode.Keypad3:
            return Keyboard.current.numpad3Key.wasPressedThisFrame ||
                   Keyboard.current.digit3Key.wasPressedThisFrame;

        case KeyCode.Keypad4:
            return Keyboard.current.numpad4Key.wasPressedThisFrame ||
                   Keyboard.current.digit4Key.wasPressedThisFrame;

        case KeyCode.Keypad5:
            return Keyboard.current.numpad5Key.wasPressedThisFrame ||
                   Keyboard.current.digit5Key.wasPressedThisFrame;

        default:
            return false;
    }
}

        private void OnStateChanged(EnemyState state)
        {
            if (bodyRenderer != null)
            {
                bodyRenderer.material.color = state.Tint;
            }

            StateChanged?.Invoke(state);
        }

        private void OnDestroy()
        {
            if (_machine != null)
            {
                _machine.StateChanged -= OnStateChanged;
            }

            foreach (var state in _states)
            {
                if (state != null)
                Destroy(state);
            }
        }
    }
}