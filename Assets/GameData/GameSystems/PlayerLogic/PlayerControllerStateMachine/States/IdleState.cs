using UnityEngine;

namespace LivingBeings.Player.CharacterMovement.MovementStateMachine.States
{
    public class IdleState : BaseState
    {
        public IdleState (PlayerController characterMovement, StateMachine stateMachine) 
        : base(characterMovement, stateMachine) 
        {
            StateName = "Idle";
        }



        public override void Enter()
        {
            base.Enter();
            
            // EventSystem.TriggerEvent("OnStop");
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
            
            bool isStartRunning = (_characterController.RigidBody.velocity.x > 1) ||
                                  (_characterController.RigidBody.velocity.x < -1);
            bool isStartFalling = _characterController.RigidBody.velocity.y < -1f;
            bool isUpButtonPressed = _characterController.IsJumpButtonPressed;

            if (_characterController.SurfaceCheck.IsCharacterIsOnInclinedSurface())
            {
                _characterController.RigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            }
            else
            {
                _characterController.RigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            }

            if (isStartFalling)
            {
                _stateMachine.TransitionToState(_characterController.Falling);
            }

            if (isStartRunning)
            {
                _stateMachine.TransitionToState(_characterController.Running);
            }

            if (isUpButtonPressed)
            {
                _stateMachine.TransitionToState(_characterController.Jumping);
            }
        }


        public override void Exit()
        {
            _characterController.AfterGroundTouchTimer = _characterController.AfterGroundTouchJumpTime;
            _characterController.RigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
} 