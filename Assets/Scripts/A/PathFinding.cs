using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.Diagnostics;
using TMPro;

public class PathFinding : MonoBehaviour
{
    public AStarGrid grid;
    private Input input;
    public TextMeshProUGUI pathFoundText;
    private PathRequestManager pathRequestManager;
    private List<Node> path;

    public void Awake()
    {
        grid = GetComponent<AStarGrid>();
        pathRequestManager = GetComponent<PathRequestManager>();
        Assert.IsNotNull(pathRequestManager, $"PathRequestManager is not attached to {transform.name}");
        Assert.IsNotNull(grid, $"Grid is not attached to {transform.name}");
        Assert.IsNotNull(pathFoundText);

        pathFoundText.enabled = false;
        input = new Input();
        input.Enable();
        input.Grid.Enable();
        input.Grid.Path.Enable();
    }

    void OnEnable()
    {
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

    public void StartFindPath(Vector3 start, Vector3 end)
    {
        StartCoroutine(ShortestPath(start, end));
    }

    void OnPath()
    {
       
    }
    public IEnumerator ShortestPath(Vector3 startPosition, Vector3 endPosition)
    {
        bool isPathFound = false;
        Vector3[] wayPoints = new Vector3[0];

        Heap<Node> Open = new Heap<Node>(grid.MaxSize);
        HashSet<Node> Closed = new HashSet<Node>();
        Node startNode = grid.WorldToNode(startPosition);
        Node endNode = grid.WorldToNode(endPosition);
        if (!endNode.isObstacle && !startNode.isObstacle)
        {
            Node currentNode;
            Open.Add(startNode);
            while (Open.Length > 0)
            {
                currentNode = Open.getTop();
                Closed.Add(currentNode);

                if (currentNode == endNode)
                {
                    pathFoundText.enabled = false;
                    isPathFound = true;
                    break;
                }

                List<Node> neighbors = grid.GetNeighbor(currentNode);

                foreach (Node n in neighbors)
                {
                    if (n.isObstacle || Closed.Contains(n))
                    {
                        continue;
                    }
                    int distCurrToNeighbor = getDistance(currentNode, n) + currentNode.gCost;
                    if (distCurrToNeighbor < n.gCost || !Open.Contains(n))
                    {
                        n.gCost = distCurrToNeighbor;
                        n.hCost = getDistance(n, endNode);
                        n.parent = currentNode;
                        if (!Open.Contains(n))
                        {
                            Open.Add(n);
                        }
                    }

                }

            }
        }
        yield return null;
        if(isPathFound){
            wayPoints = getPath(endNode, startNode);   
        } 
        pathRequestManager.FinishedPath(wayPoints, isPathFound); 
        
        
    }

    //what if the start and end node are one and the same?
    // Well then the path is just that one node ain't it?

    //I guess I'll leave the getPath to be normal and mhave the wayPointSlim take care 
    //of the whole array vector3 business
    public Vector3[] getPath(Node endNode, Node startNode)
    {
        List<Node> path = new List<Node>();
        Node currNode = endNode;
        path.Add(currNode);
        while (currNode != startNode)
        {
            currNode = currNode.parent;
            path.Add(currNode);
        }
        path.Reverse();
        return wayPointSlim(path);
    }

    private Vector3[] wayPointSlim(List<Node> points)
    {
        path = points;
        List<Vector3> waypoints = new List<Vector3>();
        Vector3 currDir = Vector3.zero;
        waypoints.Add(points[0].worldPosition);
       
        for (int i = 1; i < points.Count - 1; i++)
        {
            float xDir = points[i].xCoord - points[i - 1].xCoord;
            float yDir = points[i].yCoord - points[i - 1].yCoord;
            Vector3 nextDir = new Vector3(xDir, yDir);
            if (nextDir != currDir)
            {
                waypoints.Add(points[i].worldPosition);
            }
            currDir = nextDir;
        }
        waypoints.Add(points[points.Count-1].worldPosition);
      //add line to add in last item
        return waypoints.ToArray();
    }

    private Vector3[] AlternateWayPoint(List<Node> points){
        path = points;
        List<Vector3> waypoints = new List<Vector3>();
        Vector3 currDir = Vector3.zero;
        
        for (int i = 0; i < points.Count - 1; i++)
        {
            float xDir = points[i + 1].xCoord - points[i].xCoord;
            float yDir = points[i + 1].yCoord - points[i].yCoord;
            Vector3 nextDir = new Vector3(xDir, yDir);
            if (nextDir != currDir)
            {
                waypoints.Add(points[i].worldPosition);
            }
            currDir = nextDir;
        }
        waypoints.Add(points[points.Count-1].worldPosition);
      //add line to add in last item
        return waypoints.ToArray();
    }

    public int getDistance(Node start, Node end)
    {
        int diffX = Mathf.Abs(start.xCoord - end.xCoord);
        int diffY = Mathf.Abs(start.yCoord - end.yCoord);

        if (diffX < diffY)
        {
            return 14 * diffX + 10 * (diffY - diffX);
        }
        else
        {
            return 14 * diffY + 10 * (diffX - diffY);
        }
    }

    // void OnDrawGizmos()
    // {
    //     if(path != null){
    //         for(int i = 0 ; i < path.Count ; i++){
    //             Gizmos.color = Color.green;
    //             Gizmos.DrawCube(path[i].worldPosition, new Vector3(3f,3f,3f));
    //         }
    //     }
    // }
}
