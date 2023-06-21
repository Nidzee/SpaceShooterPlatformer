using UnityEngine;

namespace LivingBeings.Player.CharacterMovement.MovementStateMachine
{
    public abstract class BaseState
    {
        public string StateName;
        protected PlayerController _characterController = null;                    // Reference to character movement script.
        protected StateMachine _stateMachine = null;                              // Reference to state machine.
        private Vector2 _currentVelocity = Vector2.zero;                          // Current speed of change in characters velocity.



        protected BaseState(PlayerController characterMovement, StateMachine stateMachine)
        {
            _characterController = characterMovement;
            _stateMachine = stateMachine;
        }


        public virtual void Enter() {}
        
        public virtual void Exit() {}

        public virtual void OnTriggerEnter2D(Collider2D other)
        {
            // Collectible item logic
            if (other.tag == TagConstraintsConfig.COLLECTIBLE_ITEM_TAG)
            {
                BasicDropItem itemData = other.gameObject.GetComponent<BasicDropItem>();
                if (itemData != null)
                {
                    _characterController.ItemCollectorHandler.CollectItem(itemData);
                }
            }
        

            // Interactible zone logic
            if (other.tag == TagConstraintsConfig.INTERACTIBLE_ZONE_TAG)
            {
                Debug.Log("ENTER INTERRACTIBLE");
                IInteractible data = other.gameObject.GetComponent<IInteractible>();
                if (data != null)
                {
                    _characterController.InterractionHandler.RegisterInteractible(data);                
                }
            }

            
            // Effect zone logic
            if (other.tag == TagConstraintsConfig.EFFECT_ZONE_TAG)
            {
                BasicEffectZone environment = other.gameObject.GetComponent<BasicEffectZone>();
                if (environment != null)
                {
                    _characterController.PlayerEffectsHandler.ApplyEffect(environment);
                }
            }




            if (other.CompareTag("Ladder"))
            {
                _stateMachine.TransitionToState(_characterController.Climbing);
            }
        }

        public virtual void OnTriggerExit2D(Collider2D other) 
        {
            // Interactible zone logic
            if (other.tag == TagConstraintsConfig.INTERACTIBLE_ZONE_TAG)
            {
                IInteractible data = other.gameObject.GetComponent<IInteractible>();
                if (data != null)
                {
                    _characterController.InterractionHandler.UnregisterInteractible(data);                
                }
            }

            
            if (other.tag == TagConstraintsConfig.EFFECT_ZONE_TAG)
            {
                BasicEffectZone environment = other.gameObject.GetComponent<BasicEffectZone>();
                if (environment != null)
                {
                    _characterController.PlayerEffectsHandler.RemoveEffect(environment);
                }
            }
        }


        public virtual void PhysicsUpdate()
        {
            Vector2 targetVelocity = new Vector2(0, _characterController.RigidBody.velocity.y);
            bool isLeftButtonPressed = _characterController.IsLeftButtonPressed;
            bool isRightButtonPressed = _characterController.IsRightButtonPressed;

            // If player press left movement button and release right movement button.
            if (isLeftButtonPressed && !isRightButtonPressed)
            {
                // Set tarhet velocity.
                targetVelocity = new Vector2(-_characterController.HorizontalSpeed, _characterController.RigidBody.velocity.y);

                if(_characterController.IsFacingRight)
                {
                    Flip();
                }
            }
            // If player press right movement button and release left movement button.
            else if (isRightButtonPressed && !isLeftButtonPressed)
            {
                // Set tarhet velocity.
                targetVelocity = new Vector2(_characterController.HorizontalSpeed, _characterController.RigidBody.velocity.y);

                if(!_characterController.IsFacingRight)
                {
                    Flip();
                }
            }


            // Set smoothed velocity to the character.
            _characterController.RigidBody.velocity = Vector2.SmoothDamp(
                _characterController.RigidBody.velocity,
                targetVelocity, ref _currentVelocity,
                _characterController.MovementSmoothing);
        }


        private void Flip()
        {
            _characterController.IsFacingRight = !_characterController.IsFacingRight;
            _characterController.VisualsContainer.Rotate(0f, 180f, 0f);
        }
    }
}