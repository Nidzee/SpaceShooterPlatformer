using System;
using UnityEngine;

public class SurfaceCheck : MonoBehaviour
{
    [SerializeField] GroundCheckPoints _groundCheckPoints;          // A collection of points that define from where will be thrown rays that check if character is grounded.
    [SerializeField] float _groundCheckDepth = 0;                   // How deep we must check to define is character standing on ground now.
    [SerializeField] LayerMask _whatIsGround = Physics2D.AllLayers; // A mask determine what is the ground for the character.

    // Additional data to check on what surface we are standing
    public GameObject OnWhatIsStanding => _onWhatIsStanding;
    private GameObject _onWhatIsStanding = null;



    public bool IsCharacterIsOnSurface()
    {
        //Throw two rays from groundCheck points(left and right).
        RaycastHit2D leftRay = ThrowRayFromPoint(_groundCheckPoints._leftGroundCheckPoint.position);
        RaycastHit2D rightRay = ThrowRayFromPoint(_groundCheckPoints._rightGroundCheckPoint.position);
        
        //If one or both rays is hitting ground, we say that character is grounded.
        bool isLeftRayHitGround = IsRayCollidedWithGround(leftRay);
        bool isRightRayHitGround = IsRayCollidedWithGround(rightRay);

        if (isRightRayHitGround)
        {
            _onWhatIsStanding = rightRay.collider.gameObject;
        }
        else if (isLeftRayHitGround)
        {
            _onWhatIsStanding = leftRay.collider.gameObject;
        }
        else
        {
            _onWhatIsStanding = null;
        }

        return isLeftRayHitGround || isRightRayHitGround;
    }

    public bool IsCharacterIsOnInclinedSurface()
    {
        //Throw two rays from groundCheck points(left and right).
        RaycastHit2D leftRay = ThrowRayFromPoint(_groundCheckPoints._leftGroundCheckPoint.position);
        RaycastHit2D rightRay = ThrowRayFromPoint(_groundCheckPoints._rightGroundCheckPoint.position);

        //If only one ray is hitting ground we say that character is on stairs.
        bool isLeftRayHitGround = IsRayCollidedWithGround(leftRay);
        bool isRightRayHitGround = IsRayCollidedWithGround(rightRay);

        if (isRightRayHitGround)
        {
            _onWhatIsStanding = rightRay.collider.gameObject;
        }
        else if (isLeftRayHitGround)
        {
            _onWhatIsStanding = leftRay.collider.gameObject;
        }
        else
        {
            _onWhatIsStanding = null;
        }
        
        return isLeftRayHitGround ^ isRightRayHitGround;
    }



    // Helper functions
    RaycastHit2D ThrowRayFromPoint(Vector2 throwFormPoint)
    {
        return Physics2D.Raycast(throwFormPoint, -transform.up, _groundCheckDepth, _whatIsGround);
    }

    bool IsRayCollidedWithGround(RaycastHit2D thorwnRay)
    {
        if (thorwnRay.collider != null)
        {
            return ((1 << thorwnRay.collider.gameObject.layer) & _whatIsGround) != 0;   
        }
        else
        {
            return false;
        }
    }
}

[Serializable]
public struct GroundCheckPoints
{
    public Transform _leftGroundCheckPoint;
    public Transform _rightGroundCheckPoint;
}