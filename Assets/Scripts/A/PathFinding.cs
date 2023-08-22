using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.Diagnostics;
using TMPro;

public class PathFinding : MonoBehaviour
{
   public AStarGrid grid;
   public Transform startPoint;
   public Transform endPoint;
   private Input input; 
   public TextMeshProUGUI pathFoundText;

   public void Awake(){
    grid = GetComponent<AStarGrid>();
    Assert.IsNotNull(grid, "Can't find AStarGrid Script");
    Assert.IsNotNull(startPoint);
    Assert.IsNotNull(endPoint);
    Assert.IsNotNull(pathFoundText);

    pathFoundText.enabled = false;
    input = new Input();
    input.Enable();
    input.Grid.Enable();
    input.Grid.Path.Enable();
   } 

   void OnEnable(){
    input.Enable();
    input.Grid.Enable();
    input.Grid.Path.Enable();
   }  
    void OnDisable()
   {
    input.Grid.Path.Disable();
    input.Grid.Disable();
    input.Disable();
   }

   void OnPath(){
    ShortestPath(startPoint.position, endPoint.position);
   }
   public void ShortestPath(Vector3 startPosition, Vector3 endPosition){
    Stopwatch S = new Stopwatch();
    S.Start();
    Heap<Node> Open = new Heap<Node>(grid.MaxSize);
    HashSet<Node> Closed = new HashSet<Node>();
    Node startNode = grid.WorldToNode(startPosition);
    Node endNode = grid.WorldToNode(endPosition);
    Node currentNode;
    Open.Add(startNode);
        while(Open.Length > 0){
            currentNode = Open.getTop();
            Closed.Add(currentNode);

            if(currentNode == endNode){
                S.Stop();
                UnityEngine.Debug.Log(S.ElapsedMilliseconds + " ms");
                getPath(endNode, startNode);
                pathFoundText.enabled = false;
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

        pathFoundText.enabled = true;
        
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
