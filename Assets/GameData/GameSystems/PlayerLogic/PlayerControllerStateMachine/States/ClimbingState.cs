using UnityEngine;
using UnityEngine.EventSystems;

namespace LivingBeings.Player.CharacterMovement.MovementStateMachine.States
{
    public class ClimbingState : BaseState
    {
        public ClimbingState (PlayerController characterMovement, StateMachine stateMachine) : base(characterMovement, stateMachine)
        {
            StateName = "ClimbingState";
        }

        public override void Enter()
        {
            base.Enter();
            
            // EventSystem.TriggerEvent("OnStartClimb");
        }


        public override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);
        }


        public override void OnTriggerExit2D(Collider2D other)
        {
            base.OnTriggerExit2D(other);

            bool isStartFalling = _characterController.RigidBody.velocity.y < -1;
            bool isStartJumping = _characterController.RigidBody.velocity.y > 1;
            bool isStartRunning = (_characterController.RigidBody.velocity.x > 1) ||
                                       (_characterController.RigidBody.velocity.x < -1);
            
            if (other.CompareTag("Ladder"))
            {
                if (isStartFalling)
                {
                    _stateMachine.TransitionToState(_characterController.Falling);
                }
                else if (isStartJumping)
                {
                    _stateMachine.TransitionToState(_characterController.Jumping);
                }
                else if (isStartRunning)
                {
                    _stateMachine.TransitionToState(_characterController.Running);
                } 
                else 
                {
                    _stateMachine.TransitionToState(_characterController.Idle);
                }
            }
        }


        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

        
            bool isUpButtonPressed = _characterController.IsJumpButtonPressed;

            if (isUpButtonPressed)
            {
                _characterController.RigidBody.velocity = new Vector2(
                    _characterController.RigidBody.velocity.x,
                    _characterController.ClimbUpSpeed);
            }
            else
            {
                if (_characterController.RigidBody.velocity.y != 0)
                {
                    _characterController.RigidBody.velocity = new Vector2(
                    _characterController.RigidBody.velocity.x, 
                    -_characterController.ClimbDownSpeed);
                }
            }
        }


        public override void Exit()
        {
            // EventSystem.TriggerEvent("OnStopClimb");
        }
    }
}