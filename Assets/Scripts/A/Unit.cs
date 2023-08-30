using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;
public class Unit : MonoBehaviour
{
   [SerializeField] private Transform destination;
   [SerializeField][Range(0,100)] private float speed;
   private Vector3[] path;
   private int pointIndex = 0;
   
   void Start(){
    Assert.IsNotNull(destination, "You have not assigned a destination to the Unit");
    PathRequestManager.Request(transform.position, destination.position, Move);
   }


   public void Move (Vector3[] wayPoints, bool isComplete){
    if(!isComplete) {
        return;
    } else {
        path = wayPoints;
        StopCoroutine("FollowPath");
        StartCoroutine("FollowPath");
    }
            
    }
    
    private IEnumerator FollowPath(){
        Vector3 point = path[0];
        while(true){
            if(transform.position == path[pointIndex]){
                pointIndex++;
                if(pointIndex >= path.Length){
                    yield break;
                }
                point = path[pointIndex];
            }
             transform.position =  Vector3.MoveTowards(transform.position, point, speed * Time.deltaTime);
             yield return null;
        }
       

    }

    void OnDrawGizmos()
    {
        if(path!=null){
            for(int i = pointIndex ; i < path.Length; i++){
                Gizmos.color = Color.white;
                Gizmos.DrawCube(path[i], new Vector3(5f, 5f, 5f));
                Gizmos.color = Color.black;
                if(i == pointIndex){
                    Gizmos.DrawLine(transform.position, path[i]);
                } else {
                    Gizmos.DrawLine(path[i], path[i-1]);
                }
                
                
                

            }
        }
    }
   }
