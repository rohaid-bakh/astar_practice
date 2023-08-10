using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
   public bool isObstacle;
   public Vector3 worldPosition;
   public Vector2 gridCoordinates;
   public Node parent; 

   public Node(bool _isObstacle, Vector3 _worldPosition){
    isObstacle = _isObstacle;
    worldPosition = _worldPosition;
   }
}
