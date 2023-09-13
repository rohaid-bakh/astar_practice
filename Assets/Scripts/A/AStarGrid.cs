using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TMPro;
using UnityEditor;
using OpenCover.Framework.Model;
using System;
public class AStarGrid : MonoBehaviour{
    public Transform gridTransform;
    public LayerMask unWalkableMask;
    private LayerMask walkableMask;
    public TerrainTypes[] terrainTypes;
    //should I store the terraintype classes or just store the terrain cost?
    //I guess it depends on if more info will be added to this class
    public Dictionary<int, TerrainTypes> terrainDict = new Dictionary<int, TerrainTypes>();
    public Vector2 gridWorldSize;
    public float nodeRadius;
    float nodeDiameter;
    public Node[,] grid;
    int gridSizeX, gridSizeY;
    public int MaxSize{
        get{
            return gridSizeX * gridSizeY;
        }
    }
    public bool debug = false;
    public Transform UnitObject;
    public TextMeshProUGUI debugText1, debugText2, debugText3, debugText4;

    public void Awake() {
        Assert.IsNotNull(gridTransform);
        Assert.IsNotNull(UnitObject);
        Assert.IsNotNull(terrainTypes, $"The terrain cost array in {gameObject.name} is empty");

        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.FloorToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.FloorToInt(gridWorldSize.y / nodeDiameter);
        grid = new Node[gridSizeX, gridSizeY];

        for(int i = 0; i< terrainTypes.Length; i++){
            walkableMask |= terrainTypes[i].layer.value;
            int layerNumber = (int)Math.Log(terrainTypes[i].layer.value, 2.0);
            terrainDict.Add(layerNumber, terrainTypes[i]);
        }
        MakeGrid();
    }
    
    public void MakeGrid(){
        Vector3 bottomLeft = gridTransform.position - (gridTransform.forward * (gridWorldSize.y / 2)) - (gridTransform.right * (gridWorldSize.x / 2));
        for (int row = 0; row < gridSizeX; row++) {
            for (int column = 0; column < gridSizeY; column++) {
                Vector3 nodeCenter = bottomLeft +
                                      (gridTransform.right * row * nodeDiameter + gridTransform.right * nodeRadius) +
                                      (gridTransform.forward * column * nodeDiameter + gridTransform.forward * nodeRadius);
                bool isObstacle = Physics.CheckBox(nodeCenter, new Vector3(nodeRadius, nodeRadius, nodeRadius),
                                    Quaternion.identity, unWalkableMask);
                int layerInfo = 0;
                if(!isObstacle){
                  layerInfo = DetectLayer(nodeCenter);
                }
                grid[row, column] = new Node(isObstacle, nodeCenter, row, column, layerInfo);
            }
        }
    }

    private int DetectLayer(Vector3 nodeCenter){
        Vector3 gridDown = gridTransform.up * -1;
        Vector3 startingPoint = nodeCenter + 50 * gridTransform.up;
        Ray birdsEye = new Ray(startingPoint, gridDown);
        RaycastHit hit;
        Physics.Raycast(birdsEye, out hit, 100f, walkableMask);
        int layerOrder = hit.transform.gameObject.layer;
        return getTerrainCost(layerOrder);
    }

    private int getTerrainCost(int LayerOrder){
        if(terrainDict.ContainsKey(LayerOrder)){
            return terrainDict[LayerOrder].terrainCost;
        } else {
            return 0;
        }
        
    }

    public Node WorldToNode(Vector3 NodeLocation){
        float bottomLeftX = transform.position.x - (gridWorldSize.x / 2f);
        float bottomLeftY = transform.position.z - (gridWorldSize.y / 2f);
        Vector3 bottomLeft = new Vector3(bottomLeftX, 0f, bottomLeftY);
        Vector3 diff = NodeLocation - bottomLeft;
        int cordX = Mathf.FloorToInt(Mathf.Clamp((diff.x / nodeDiameter), 0f, gridSizeX - 1));
        int cordY = Mathf.FloorToInt(Mathf.Clamp(diff.z / nodeDiameter, 0f, gridSizeY - 1));
        return grid[cordX, cordY];
    }

    public List<Node> GetNeighbor(Node curr){
        Assert.IsNotNull(curr, "The Node being passed into GetNeighbor is null");
        int xCoord = curr.xCoord;
        int yCoord = curr.yCoord;
        List<Node> neighbors = new List<Node>();
        for (int x = -1; x < 2; x++){
            for (int y = -1; y < 2; y++){
                if (x == 0 && y == 0){
                    continue;
                }
                int neighX = xCoord + x;
                int neighY = yCoord + y;
                if (neighX < 0 || neighY < 0){
                    continue;
                }
                if (neighX > gridSizeX - 1 || neighY > gridSizeY - 1){
                    continue;
                }
                neighbors.Add(grid[neighX, neighY]);
            }
        }
        return neighbors;
    }

    void OnDrawGizmos(){
        if (grid == null) return;

        if (debug) Gizmos.color = Color.blue;
        foreach (Node n in grid){
            Vector3[] points = DrawGridSquare(n);
            Gizmos.color = n.isObstacle ? Color.red : Color.blue;
            // if (n == WorldToNode(UnitObject.position)){
            //     Gizmos.color = Color.green;
            // }
            if(n.terrainCost == 7){
                Gizmos.color = Color.green;
            }
            if(n.terrainCost == 8){
                Gizmos.color = Color.yellow;
            }
            if(n.terrainCost == 9){
                Gizmos.color = Color.magenta;
            }
            if(n.terrainCost == 10){
                Gizmos.color = Color.white;
            }
            Gizmos.DrawLineList(points);
        }
    }

    private Vector3[] DrawGridSquare(Node n){
        Vector3 nodeCenter = n.worldPosition;
        Vector3 BLCorner = nodeCenter - (gridTransform.forward * nodeRadius) - (gridTransform.right * nodeRadius);
        Vector3 ULCorner = BLCorner + (gridTransform.forward * nodeDiameter);
        Vector3 URCorner = ULCorner + (gridTransform.right * nodeDiameter);
        Vector3 BRCorner = URCorner - (gridTransform.forward * nodeDiameter);
        Vector3[] points = new Vector3[8]{
            BLCorner, ULCorner,
            ULCorner, URCorner,
            URCorner, BRCorner,
            BRCorner, BLCorner
        };
        return points;
    }

    [System.Serializable]
    public class TerrainTypes{
        public int terrainCost;
        public LayerMask layer;
    }
}
