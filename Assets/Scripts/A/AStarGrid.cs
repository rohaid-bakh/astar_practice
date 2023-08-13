using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TMPro;
public class AStarGrid : MonoBehaviour
{
    public LayerMask unWalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public Node[,] grid;

    public Transform detectableObj;
    public Transform gridTransform;
    public TextMeshProUGUI debug1, debug2, debug3, debug4;

    int gridSizeX, gridSizeY;
    public bool debug = false;
    float nodeDiameter;

    public List<Node> path;

    public void Start() {
        Assert.IsNotNull(gridTransform);
        Assert.IsNotNull(detectableObj);
        nodeDiameter = nodeRadius*2;
        gridSizeX = Mathf.FloorToInt(gridWorldSize.x/nodeDiameter);
        gridSizeY = Mathf.FloorToInt(gridWorldSize.y/nodeDiameter);
        grid = new Node[gridSizeX,gridSizeY];
        path = new List<Node>();
        MakeGrid();
    } 

    public void Update(){
        MakeGrid();
    }

    public void MakeGrid() {
        Vector3 bottomLeft = gridTransform.position - (gridTransform.forward*(gridWorldSize.y/2)) - (gridTransform.right*(gridWorldSize.x/2));
        for(int row = 0; row < gridSizeX ; row++){
            for(int column = 0; column < gridSizeY ; column++){
              Vector3 nodeCenter = bottomLeft + 
                                    (gridTransform.right * row * nodeDiameter + gridTransform.right * nodeRadius) +
                                    (gridTransform.forward * column * nodeDiameter + gridTransform.forward * nodeRadius);
                bool isObstacle = Physics.CheckBox(nodeCenter, new Vector3(nodeRadius, nodeRadius, nodeRadius), 
                                    Quaternion.identity, unWalkableMask );
                grid[row, column] = new Node(isObstacle, nodeCenter, row, column);

            }
        }
    }

    public Node WorldToNode(Vector3 NodeLocation){
        float bottomLeftX = transform.position.x - (gridWorldSize.x/2f);
        float bottomLeftY = transform.position.z - (gridWorldSize.y/2f);
        Vector3 bottomLeft = new Vector3(bottomLeftX, 0f, bottomLeftY);
        Vector3 diff = NodeLocation - bottomLeft;
        int cordX = Mathf.FloorToInt(Mathf.Clamp((diff.x/nodeDiameter), 0f, gridSizeX-1));
        int cordY = Mathf.FloorToInt(Mathf.Clamp(diff.z/nodeDiameter, 0f, gridSizeY-1));
        return grid[cordX, cordY];
    }

    public List<Node> GetNeighbor(Node curr){
        int xCoord = curr.xCoord;
        int yCoord = curr.yCoord;
        List<Node> neighbors = new List<Node>();
        for(int x = -1; x < 2 ; x++){
            for(int y = -1; y < 2; y++){
                if (x == 0 && y == 0){
                    continue;
                }
                int neighX = xCoord+x;
                int neighY = yCoord+y;
                if(neighX < 0 || neighY < 0){
                    continue;
                }  
                if(neighX > gridSizeX - 1 || neighY > gridSizeY - 1){
                    continue;
                }
                neighbors.Add(grid[neighX,neighY]);
            }
        }

        return neighbors;
    }


    void OnDrawGizmos()
    {
        if(grid == null) return;

        Gizmos.color = Color.blue;
        foreach(Node n in grid){
            
            Vector3[] points = DrawGridSquare(n);

            Gizmos.color = n.isObstacle? Color.red: Color.blue;
            if(n == WorldToNode(detectableObj.position)){
                Gizmos.color = Color.green;
            }
            
            if(path.Contains(n)){
                Gizmos.color = Color.yellow;
                Gizmos.DrawLineList(points);
            } else {
                Gizmos.DrawLineList(points);
            }
        }
    }

    private Vector3[] DrawGridSquare(Node n){
        Vector3 nodeCenter = n.worldPosition;
        Vector3 BLCorner = nodeCenter - (gridTransform.forward*nodeRadius) - (gridTransform.right*nodeRadius);
        Vector3 ULCorner = BLCorner + (gridTransform.forward*nodeDiameter);
        Vector3 URCorner = ULCorner + (gridTransform.right*nodeDiameter);
        Vector3 BRCorner = URCorner - (gridTransform.forward*nodeDiameter);
        Vector3[] points = new Vector3[8]{
            BLCorner, ULCorner,
            ULCorner, URCorner,
            URCorner, BRCorner,
            BRCorner, BLCorner
        };

        return points;
        
    }
}
