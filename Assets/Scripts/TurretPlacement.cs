using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPlacement : MonoBehaviour
{
    [SerializeField] public Transform turret;
    void OnDrawGizmos()
    {
        if(turret == null) return;
        Ray eyeLine = new Ray(transform.position, transform.forward * 100);
        if(Physics.Raycast(eyeLine, out RaycastHit hitInfo)){
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, hitInfo.point);

            Gizmos.color = Color.green;
            Gizmos.DrawRay(hitInfo.point, hitInfo.normal * 100f);
            Vector3 newYDirection = hitInfo.normal; 
            // Vector3 testZ = new Vector3(transform.forward.x, transform.forward.y, 0f).normalized;
            Vector3 xDirection = Vector3.Cross(newYDirection, transform.forward).normalized;
             // transform.right is supposed to be used but it's always rotating 90 on the Y but transform.forward is 
             // used in the other answers so /shrug
            Gizmos.color = Color.red; 
            Gizmos.DrawRay(hitInfo.point, xDirection*100f);

            Vector3 zDirection = Vector3.Cross(xDirection, newYDirection);
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(hitInfo.point, zDirection * 100f);

            turret.position = hitInfo.point;
            turret.rotation = Quaternion.LookRotation(zDirection, newYDirection);

            /*
                Always has the object pointing forward in the same forward as the camera and if the camera looks down
                then the object will also face down.
            */
            // turret.forward = transform.forward;
        }   
       
    }
}
