using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarGrid : MonoBehaviour
{
    public LayerMask unWalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

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
        float topLeftX = transform.position.x - (gridWorldSize.x/2f);
        float topLeftY = transform.position.z + (gridWorldSize.y/2f);
        Vector3 topLeft = new Vector3(topLeftX, 0f, topLeftY);
        for(int row = 0; row < gridSizeX ; row++){
            for(int column = 0; column < gridSizeY ; column++){
               float offsetX = row * nodeDiameter + nodeRadius;
               float offsetY = column* nodeDiameter + nodeRadius;
               Vector3 nodeCenter = new Vector3(topLeft.x + offsetX, 0f, topLeftY - offsetY);
               bool isObstacle = Physics.CheckBox(nodeCenter, new Vector3(nodeRadius, nodeRadius, nodeRadius), Quaternion.identity, unWalkableMask);
               grid[row,column] = new Node(isObstacle, nodeCenter);
            }
        }
    }

    public void WorldToNode(Vector3 NodeLocation){
        
    }

    void OnDrawGizmos()
    {
        if(debug){
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position,10f);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1f, gridWorldSize.y));
            Gizmos.color = Color.red; 
            float topLeftX = transform.position.x - (gridWorldSize.x/2f);
            float topLeftY = transform.position.z + (gridWorldSize.y/2f);
            Vector3 topLeft = new Vector3(topLeftX, 0f , topLeftY);
            Gizmos.DrawSphere(topLeft, 10f);
            Gizmos.color = Color.black;
            foreach(Node n in grid){
                    Gizmos.color = n.isObstacle? Color.red: Color.black;
                    Gizmos.DrawWireCube(n.worldPosition, Vector3.one * (nodeDiameter-.1f));
                }
            }
        }
}
