using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] [Range(1, 4)]public int bounceNumber;
    void OnDrawGizmos(){

        Vector3 lazerRay = Vector3.zero;
        Vector3 reflectRay = Vector3.zero;
        Vector3 lazerOrig = Vector3.zero;
        lazerRay = transform.right; 
        lazerOrig = transform.position;
        for(int i = 0 ; i < bounceNumber; i++){
            if(Physics.Raycast(lazerOrig, lazerRay, out RaycastHit info)){
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(info.point, .1f);
            Vector3 normal = info.normal;

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(lazerOrig, info.point);

            // Gizmos.color = Color.white;
            // Gizmos.DrawRay(info.point, info.normal * 15f);

            Vector3 dir = lazerRay; 
            float scalar = Vector3.Dot(normal, dir);
            Vector3 p = scalar * normal * 2 * -1 ;
            Vector3 reflection = p + dir ; 
            
            Gizmos.color = Color.green;
            Gizmos.DrawRay(info.point, reflection.normalized * 10f);
            lazerOrig = info.point;
            lazerRay = reflection.normalized;
        }
        }
        
        
    }
}
