using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetection : MonoBehaviour
{
    [SerializeField][Range(1f, 15f)] private float height;
    [SerializeField][Range(1f, 15f)] private float range;
    [SerializeField][Range(0f, 1f)] private float degree;
    [SerializeField] private Transform testObject;

    void OnDrawGizmos() {
        if (testObject == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.right * 100f);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.up * 100f);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * 100f);
        //check the height first
        Vector3 currObject = transform.InverseTransformPoint(testObject.position);
        Vector2 currPlace = new Vector2(currObject.x, currObject.z);
        Vector2 turrForward = new Vector2(0f, 1f); 
        // do this instead of using the transform.forward because that vector
          //is based on world orientation but we're operating on local space.
        Vector2 center = new Vector2(0f, 0f);
      
        // we only care about the angle between the z and x axis and not the y since we capped the height;
        float dotProduct = Vector2.Dot(currPlace.normalized, turrForward.normalized); 
        
        // calculating distance on the z x plane to factor in the range;
        float distance = Vector2.Distance(currPlace, center);

        // Debug.Log($"Curr place: {currObject}");
        // Debug.Log($"currObject local position : {currPlace}");
        // Debug.Log($"Forward: {turrForward}");
        // Debug.Log($"Dot Product: {dotProduct}");
        // Debug.Log($"Distance: {distance}");

        // Correct way to get the local transform coordinate through matrix multiplication
        // Vector3 testCurr = transform.worldToLocalMatrix * new Vector4(testObject.position.x, testObject.position.y, testObject.position.z, 1f);

        //Not using distance because we're just looking for range

        Vector3 lookAt = new Vector3(testObject.position.x, 0f, testObject.position.z);

        if (currObject.y <= height && currObject.y >= 0f){
            if (distance <= range && currObject.z >= 0f){
                if (dotProduct >= degree){
                    Gizmos.color = Color.white;
                    Gizmos.DrawLine(testObject.position, transform.position);
                    transform.LookAt(lookAt);
                }
            }
        }
    }

}
