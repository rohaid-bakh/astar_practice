using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;
/// <summary>
/// <para>
/// Class <c>Unit</c> requests a path to its destination and moves the Unit on a 
/// path towards it if a path is found.
/// </para>
/// </summary>
public class Unit : MonoBehaviour {

   [Header("Destination")]
   [SerializeField] private Transform destination;

   [Header("Unit Speed")]
   [SerializeField][Range(1,100)] private float speed;

   private int pathIndex = 0;
   private Vector3[] path;
   
   void Start(){
        Assert.IsNotNull(destination, "You have not assigned a destination to the Unit");
        PathRequestManager.Request(transform.position, destination.position, Move);
   }

    /// <summary><para>
    /// Moves the Unit object along a set path of <c><paramref name="wayPoints"/></c> 
    /// if a path is possible.
    /// </para></summary>
    /// <param name="wayPoints">Path unit follows.</param>
    /// <param name="isComplete">If a path was found.</param>
   public void Move (Vector3[] wayPoints, bool isComplete){
        if(!isComplete) return;
    
        path = wayPoints;
        StopCoroutine("FollowPath");
        StartCoroutine("FollowPath");   
    }

    /// <summary><para>
    /// Coroutine that moves a Unit along a <c> path </c> 
    /// </para></summary>
    private IEnumerator FollowPath(){
        Assert.IsNotNull(path, "The array passed in Unit.Move() was null");
        Vector3 point = path[0];
        while(true){
            if(transform.position == path[pathIndex]){
                pathIndex++;
                if(pathIndex >= path.Length) yield break;
                point = path[pathIndex];
            }
            //breaking this line so I don't have to scroll.
            transform.position =  Vector3.MoveTowards(transform.position, point, speed * Time.deltaTime);
            yield return null;
        }
    }

    // Draws a cube for every waypoint and draws the path
    // from current position till the end.
    void OnDrawGizmos(){
        if(path!=null){
            for(int i = pathIndex ; i < path.Length; i++){
                Gizmos.color = Color.green;
                Gizmos.DrawCube(path[i], new Vector3(5f, 5f, 5f));
                Gizmos.color = Color.black;
                if(i == pathIndex){
                    Gizmos.DrawLine(transform.position, path[i]);
                } else {
                    Gizmos.DrawLine(path[i], path[i-1]);
                }
            }
        }
    }
   }
