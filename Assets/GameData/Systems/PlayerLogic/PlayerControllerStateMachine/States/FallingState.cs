using System.Runtime.InteropServices;
using UnityEngine;

namespace LivingBeings.Player.CharacterMovement.MovementStateMachine.States
{
    public class FallingState : BaseState
    {
        public FallingState (PlayerController characterMovement, StateMachine stateMachine) : base (characterMovement, stateMachine)
        {
            StateName = "Falling";
        }


        public override void Enter()
        {
            base.Enter();

            _characterController.StartFallingPosition = _characterController.Transform.position;

            // EventSystem.TriggerEvent("OnFall");
        }


        public override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);
        }


        public override void OnTriggerExit2D(Collider2D other)
        {
            base.OnTriggerExit2D(other);
        }


        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            
            bool isUpButtonPressed = _characterController.IsJumpButtonPressed;

            _characterController.PressButtonTimer -= Time.deltaTime;

            if (isUpButtonPressed)
            {
                _characterController.PressButtonTimer = _characterController.PressBeforeGroundTime;

                if (_characterController.AfterGroundTouchTimer > 0f)
                {
                    _stateMachine.TransitionToState(_characterController.Jumping);
                }
            }

            if (_characterController.SurfaceCheck.IsCharacterIsOnSurface())
            {
                _stateMachine.TransitionToState(_characterController.Landing);
            }

            _characterController.AfterGroundTouchTimer -= Time.deltaTime;
        }


        public override void Exit()
        {

        }
    }
}
