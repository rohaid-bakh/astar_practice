using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>{
    [Header("Parent Node")]
    public Node parent; // shouldn't I encapsulate this?

    [Header("Obstacle?")]
    public bool isObstacle;

    [Header("Position")]
    public int xCoord;
    public int yCoord;
    public Vector3 worldPosition;

    [Header("Travel Cost")]
    public int gCost;
    public int hCost;
    private int _heapIndex;
    public int terrainCost;

    public Node(bool _isObstacle, Vector3 _worldPosition, int _xCoord, int _yCoord, int _terrainCost){
        isObstacle = _isObstacle;
        worldPosition = _worldPosition;
        xCoord = _xCoord;
        yCoord = _yCoord;
        terrainCost = _terrainCost;
    }


    /// <summary> The Fcost of the Node. Calculated by adding Gcost + Hcost </summary>
    public int fCost{
        get{
            return gCost + hCost;
        }
    }

    /// <summary> Position on the Heap </summary>
    public int HeapIndex{
        get{
            return _heapIndex;
        }
        set{
            _heapIndex = value;
        }
    }

    /// <summary> 
    /// <para> Returns 1 if the current node has a lower Fcost or 
    /// if the Fcosts are the same,
    /// the lower hCost to the node being compared against.
    /// </para>
    ///  </summary>
    public int CompareTo(Node nodeToCompare){
        int CostCompareResult = fCost.CompareTo(nodeToCompare.fCost);
        if (CostCompareResult == 0){
            CostCompareResult = hCost.CompareTo(nodeToCompare.hCost);
        }
        // inverting the result because we want the node 
        // the lowest f cost/h cost
        return CostCompareResult * -1;
    }
}
