using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarGrid : MonoBehaviour
{
    //TODO: Write it so that the grid takes into account any rotations!
    public LayerMask unWalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

    public Transform detectableObj;

    int gridSizeX, gridSizeY;
    public bool debug = false;
    float nodeDiameter;

    public void Start() {
        nodeDiameter = nodeRadius*2;
        gridSizeX = Mathf.FloorToInt(gridWorldSize.x/nodeDiameter);
        gridSizeY = Mathf.FloorToInt(gridWorldSize.y/nodeDiameter);
        grid = new Node[gridSizeX,gridSizeY];
        MakeGrid();
    } 

    public void MakeGrid() {
        float bottomLeftX = transform.position.x - (gridWorldSize.x/2f);
        float bottomLeftY = transform.position.z - (gridWorldSize.y/2f);
        Vector3 bottomLeft = new Vector3(bottomLeftX, 0f, bottomLeftY);
        for(int row = 0; row < gridSizeX ; row++){
            for(int column = 0; column < gridSizeY ; column++){
               float offsetX = row * nodeDiameter + nodeRadius;
               float offsetY = column* nodeDiameter + nodeRadius;
               Vector3 nodeCenter = new Vector3(bottomLeft.x + offsetX, 0f, bottomLeftY + offsetY);
               bool isObstacle = Physics.CheckBox(nodeCenter, new Vector3(nodeRadius, nodeRadius, nodeRadius), Quaternion.identity, unWalkableMask);
               grid[row,column] = new Node(isObstacle, nodeCenter);
            }
        }
    }

    public Vector2 WorldToNode(Vector3 NodeLocation){
        float bottomLeftX = transform.position.x - (gridWorldSize.x/2f);
        float bottomLeftY = transform.position.z - (gridWorldSize.y/2f);
        Vector3 bottomLeft = new Vector3(bottomLeftX, 0f, bottomLeftY);
        Vector3 diff = NodeLocation - bottomLeft;
        int cordX = Mathf.FloorToInt(Mathf.Clamp((diff.x/nodeDiameter), 0f, gridSizeX-1));
        int cordY = Mathf.FloorToInt(Mathf.Clamp(diff.z/nodeDiameter, 0f, gridSizeY-1));
        return new Vector2(cordX, cordY);
    }

    void OnDrawGizmos()
    {
        if(debug){
            Gizmos.color = Color.blue;
            // would have to rotate it according to the plane and not! to the a* script
            // Gizmos.matrix = transform.worldToLocalMatrix;
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1f, gridWorldSize.y));
            Gizmos.color = Color.black;
            if(grid!= null){
                foreach(Node n in grid){
                    Gizmos.color = n.isObstacle? Color.red: Color.black;
                    Gizmos.DrawWireCube(n.worldPosition, Vector3.one * (nodeDiameter-.1f));
                }
                if(detectableObj!= null){
                    Vector2 location = WorldToNode(detectableObj.position);
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawCube(grid[Mathf.FloorToInt(location.x),Mathf.FloorToInt(location.y)].worldPosition, Vector3.one*(nodeDiameter-.1f));
                }
            }
            }
        }
}
