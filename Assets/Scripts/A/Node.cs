using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
   public bool isObstacle;
   public Vector3 worldPosition;
   public int xCoord;
   public int yCoord;
   public Node parent; 
   public int gCost;
   public int hCost;
   private int _heapIndex;
   
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

   public int HeapIndex {
      get {
         return _heapIndex;
      }
      set {
         _heapIndex = value;
      }
   }

   public int CompareTo(Node nodeToCompare){
      int compare = fCost.CompareTo(nodeToCompare.fCost);
      if(compare == 0){
         compare = hCost.CompareTo(nodeToCompare.hCost);
      }
      // inverting the result because we want the node 
      // the lowest f cost/h cost
      return compare * -1 ;
   }
}
