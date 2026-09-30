using UnityEngine;
using UnityEngine.InputSystem;

namespace ArtifactInspector
{
    /// <summary>
    /// WASD / Arrow movement, camera-relative movement,
    /// Space to jump, gravity, and smooth turning.
    /// Uses Unity's New Input System.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float turnSpeed = 720f;

        [Header("Jump")]
        [SerializeField] private float jumpHeight = 1.2f;
        [SerializeField] private float gravity = -20f;

        private CharacterController _controller;
        private Transform _camera;
        private float _verticalVelocity;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();

            if (Camera.main != null)
                _camera = Camera.main.transform;
        }

        private void Update()
        {
            // -------------------------
            // MOVEMENT INPUT
            // -------------------------
            Vector2 moveInput = Vector2.zero;

            if (Keyboard.current != null)
            {
                // WASD
                if (Keyboard.current.wKey.isPressed)
                    moveInput.y += 1f;

                if (Keyboard.current.sKey.isPressed)
                    moveInput.y -= 1f;

                if (Keyboard.current.aKey.isPressed)
                    moveInput.x -= 1f;

                if (Keyboard.current.dKey.isPressed)
                    moveInput.x += 1f;

                // Arrow keys
                if (Keyboard.current.upArrowKey.isPressed)
                    moveInput.y += 1f;

                if (Keyboard.current.downArrowKey.isPressed)
                    moveInput.y -= 1f;

                if (Keyboard.current.leftArrowKey.isPressed)
                    moveInput.x -= 1f;

                if (Keyboard.current.rightArrowKey.isPressed)
                    moveInput.x += 1f;
            }

            moveInput = Vector2.ClampMagnitude(moveInput, 1f);

            Vector3 input = new Vector3(
                moveInput.x,
                0f,
                moveInput.y
            );

            // Camera-relative movement
            Vector3 horizontalMove = CameraRelative(input);

            // -------------------------
            // GROUND CHECK
            // -------------------------
            if (_controller.isGrounded && _verticalVelocity < 0f)
            {
                _verticalVelocity = -2f;
            }

            // -------------------------
            // JUMP
            // -------------------------
            if (_controller.isGrounded &&
                Keyboard.current != null &&
                Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                _verticalVelocity =
                    Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            // -------------------------
            // GRAVITY
            // -------------------------
            _verticalVelocity += gravity * Time.deltaTime;

            Vector3 finalMove = horizontalMove * moveSpeed;
            finalMove.y = _verticalVelocity;

            // -------------------------
            // MOVE CHARACTER
            // -------------------------
            _controller.Move(finalMove * Time.deltaTime);

            // Only use horizontal movement for rotation
            FaceDirection(horizontalMove);
        }

        private Vector3 CameraRelative(Vector3 input)
        {
            if (_camera == null)
                return input;

            Vector3 forward =
                Vector3.ProjectOnPlane(_camera.forward, Vector3.up).normalized;

            Vector3 right =
                Vector3.ProjectOnPlane(_camera.right, Vector3.up).normalized;

            return forward * input.z + right * input.x;
        }

        private void FaceDirection(Vector3 direction)
        {
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.01f)
                return;

            Quaternion targetRotation =
                Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                turnSpeed * Time.deltaTime
            );
        }
    }
}