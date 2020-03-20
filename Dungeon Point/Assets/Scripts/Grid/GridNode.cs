using UnityEngine;

public class GridNode
{
    public int X, Y, Z;
    public Vector3 WorldPosition;
    public GridTile Tile;

    public bool IsExplored = false;
    public bool IsExplorable = false;
    public bool IsEmpty = true;
    public bool HasEnemy = false;
    public bool IsBlocked = false;


    //Pathfind node cost
    public float hCost;
    public float gCost;
    public float fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public GridNode(int x, int y, int z, Vector3 worldPos, GridTile tile, bool isexplored = true, bool isoccupied = false, bool isblocked = false)
    {
        X = x;
        Y = y;
        Z = z;
        WorldPosition = worldPos;
        IsExplored = isexplored;
        HasEnemy = isoccupied;
        Tile = tile;
        Tile.Node = this;
    }
}
