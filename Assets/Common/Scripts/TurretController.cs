using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class TurretController : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [FormerlySerializedAs("turretHeadTransform")]
    [SerializeField] private Transform turretIKTargetTransform;
    
    [SerializeField] private Transform turretHeadTransform;
    
    private float initialLaserHandleScaleY;

    public void OnHandleTouch(Vector2 touchPosition)
    {
        RotateTowardsScreenPoint(touchPosition);
    }

    public void OnFireTurret(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started || context.phase == InputActionPhase.Performed)
        {
            RotateTowardsScreenPoint(Input.mousePosition);
        }
    }

    private void RotateTowardsScreenPoint(Vector3 targetPoint)
    {
        Ray screenRay = targetCamera.ScreenPointToRay(targetPoint);

        if (Physics.Raycast(screenRay, out RaycastHit hitInfo, 1000f))
        {
            RotateTowardsTargetPoint(hitInfo.point);
        }
        else
        {
            Debug.Log("[WARNING]: screen ray didn't hit anything");
        }
    }

    private void RotateTowardsTargetPoint(Vector3 targetPoint)
    {
        Vector3 leveledHitPoint = new Vector3(targetPoint.x, turretIKTargetTransform.position.y, targetPoint.z);
        Vector3 directionToHitPoint = leveledHitPoint - turretIKTargetTransform.position;
        Vector3 boneRotatedDirection = Vector3.Cross(turretIKTargetTransform.up, directionToHitPoint.normalized);
        Quaternion targetRotation = Quaternion.LookRotation(boneRotatedDirection, Vector3.up);
        turretIKTargetTransform.rotation = targetRotation;
        
        Vector3 headPos = turretHeadTransform.position;
        Vector3 headToTargetDirection = targetPoint - headPos;
        Quaternion headRotation = Quaternion.FromToRotation(turretHeadTransform.up, headToTargetDirection.normalized) * turretHeadTransform.rotation;
        turretHeadTransform.rotation = headRotation;
    }
}
