using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
   public bool isObstacle;
   public Vector3 worldPosition;
   public int xCoord;
   public int yCoord;
   public Node parent; 
   public int gCost;
   public int hCost;
   
   public Node(bool _isObstacle, Vector3 _worldPosition, int _xCoord, int _yCoord){
    isObstacle = _isObstacle;
    worldPosition = _worldPosition;
    xCoord = _xCoord;
    yCoord = _yCoord;
   }

   public int fCost {
      get{
         return gCost + hCost;
      }
   }
}
