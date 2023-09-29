using UnityEngine;

namespace LivingBeings.Player.CharacterMovement.MovementStateMachine.States
{
    public class JumpingState : BaseState
    {
        public JumpingState (PlayerController characterMovement, StateMachine stateMachine) : base(characterMovement, stateMachine)
        {
            StateName = "JumpingState";
        }



        public override void Enter()
        {
            base.Enter();
            
            // EventSystem.TriggerEvent("OnJump");

            _characterController.RigidBody.velocity = new Vector2(
                _characterController.RigidBody.velocity.x,
                _characterController.JumpHeight);
        }

        public override void Exit() {}



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
            bool isStartFalling = _characterController.RigidBody.velocity.y < 1f;


            if (isUpButtonPressed)
            {
                _characterController.PressButtonTimer = _characterController.PressBeforeGroundTime;
            }
            else
            {
                _characterController.RigidBody.velocity = new Vector2(
                    _characterController.RigidBody.velocity.x,
                    _characterController.RigidBody.velocity.y * _characterController.CutJumpHeight);
            }


            if (isStartFalling)
            {
                _stateMachine.TransitionToState(_characterController.Falling);
            }

            _characterController.AfterGroundTouchTimer -= Time.deltaTime;
        }
    }
} 