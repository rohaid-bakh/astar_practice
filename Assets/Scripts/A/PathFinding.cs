using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PathFinding : MonoBehaviour
{
   public AStarGrid grid;
   public Transform startPoint;
   public Transform endPoint;


   public void Awake(){
    grid = GetComponent<AStarGrid>();
    Assert.IsNotNull(grid, "Can't find AStarGrid Script");
    Assert.IsNotNull(startPoint);
    Assert.IsNotNull(endPoint);
   }   

   public void Update(){
    ShortestPath(startPoint.position, endPoint.position);
   }
   public void ShortestPath(Vector3 startPosition, Vector3 endPosition){
    List<Node> Open = new List<Node>();
    List<Node> Closed = new List<Node>();
    Node startNode = grid.WorldToNode(startPosition);
    Node endNode = grid.WorldToNode(endPosition);
    Node currentNode;
    Open.Add(startNode);
        while(Open.Count > 0){
            currentNode = Open[0];
            for(int i = 1; i < Open.Count ; i++){
               if(Open[i].fCost < currentNode.fCost || 
                (Open[i].fCost == currentNode.fCost && Open[i].hCost < currentNode.hCost)){
                    currentNode = Open[i];
                }
            }
            Open.Remove(currentNode);
            Closed.Add(currentNode);
            if(currentNode == endNode){
                getPath(endNode, startNode);
                return;
            }
           
            List<Node> neighbors = grid.GetNeighbor(currentNode);

            foreach (Node n in neighbors){
                if(n.isObstacle || Closed.Contains(n)){
                    continue;
                }
                int distCurrToNeighbor = getDistance(currentNode, n) + currentNode.gCost;
                if(distCurrToNeighbor < n.gCost || !Open.Contains(n)){
                    n.gCost = distCurrToNeighbor;
                    n.hCost = getDistance(n, endNode);
                    n.parent = currentNode;
                    if(!Open.Contains(n)){
                        Open.Add(n);
                    }
                }

            }

        }
   }

    //what if the start and end node are one and the same?
   public void getPath(Node endNode, Node startNode){
    List<Node> path = new List<Node>();
    Node currNode = endNode;
    path.Add(currNode);
        while(currNode != startNode){
            currNode = currNode.parent;
            path.Add(currNode);
        }
        path.Reverse();
        grid.path = path;
   }
   public int getDistance(Node start , Node end){
        int diffX = Mathf.Abs(start.xCoord - end.xCoord);
        int diffY = Mathf.Abs(start.yCoord - end.yCoord);

        if(diffX < diffY){
            return 14 * diffX + 10 * (diffY - diffX);
        } else {
            return 14 * diffY + 10 * (diffX - diffY);
        }
   }
}
