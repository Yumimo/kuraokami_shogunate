using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Kuraokami
{
    [CreateAssetMenu(fileName = "Input", menuName = "Scriptable Objects/Input")]
    public class InputReader : ScriptableObject, InputSystem_Actions.IPlayerActions
    {
        public event UnityAction<Vector2> Move = delegate { };
        public event UnityAction<bool> Jump = delegate { };

        private InputSystem_Actions inputAction;
        public bool IsJumpPressed { get; private set; }

        public Vector3 Direction => inputAction.Player.Move.ReadValue<Vector2>();
        private void OnEnable()
        {
            if (inputAction == null )
            {
                inputAction = new InputSystem_Actions();
                inputAction.Player.SetCallbacks(this);
            }
            inputAction.Enable();
        }

        private void OnDisable()
        {
            inputAction.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Move?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {

        }

        public void OnAttack(InputAction.CallbackContext context)
        {

        }

        public void OnInteract(InputAction.CallbackContext context)
        {

        }

        public void OnCrouch(InputAction.CallbackContext context)
        {

        }

        public void OnJump(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    IsJumpPressed = true;
                    Jump?.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    IsJumpPressed = false;
                    Jump?.Invoke(false);
                    break;
            }
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {

        }

        public void OnNext(InputAction.CallbackContext context)
        {

        }

        public void OnSprint(InputAction.CallbackContext context)
        {

        }
    }
}
