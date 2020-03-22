using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    #region Instance
    private static Pathfinder instance;
    public static Pathfinder Instance { get { return instance; } }
    #endregion

    private bool findNewPath = false;
    private bool findingPath = false;
    private bool IsDone = false;

    private GridNode startNode;
    private GridNode targetNode;

    public delegate void PathfindFinished(List<GridNode> nodes);
    public PathfindFinished OnPathfindFinished;


    public List<GridNode> Path = new List<GridNode>();

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (findNewPath)
        {
            findNewPath = false;
            findingPath = true;
            Thread jobThread = new Thread(FindPath);
            jobThread.Start();
        }

        if (IsDone)
        {
            IsDone = false;
            findingPath = false;
            OnPathfindFinished?.Invoke(Path);
        }
    }

    public void WantToFindNewPath(GridNode start, GridNode target)
    {
        if (findingPath)
        {
            Debug.Log("Cannot find new path because there is a current pathfinding request on going");
            return;
        }

        ClearPathfinder();
        findNewPath = true;
        startNode = start;
        targetNode = target;
    }

    private void FindPath()
    {
        Path = FindPathNow(startNode, targetNode);
        IsDone = true;
    }

    private List<GridNode> FindPathNow(GridNode start, GridNode target)
    {
        List<GridNode> ret = new List<GridNode>();

        List<GridNode> openNodes = new List<GridNode>();
        List<GridNode> closedNodes = new List<GridNode>();

        openNodes.Add(start);

        while (openNodes.Count > 0)
        {
            GridNode current = openNodes[0];

            for (int i = 0; i < openNodes.Count; ++i)
            {
                //if (!current.Equals(openNodes[i]))
                {
                    if (openNodes[i].fCost < current.fCost ||
                    openNodes[i].fCost == current.fCost && openNodes[i].hCost < current.hCost)
                    {
                        current = openNodes[i];
                    }
                }

            }

            openNodes.Remove(current);
            closedNodes.Add(current);

            if (current.Equals(target))
            {
                ret = GetPath(start, current);
                break;
            }

            foreach (GridNode neightbour in GetNeightbours(current))
            {
                if (!closedNodes.Contains(neightbour))
                {
                    float cost = current.gCost + GetDistance(current, neightbour);
                    if (cost < neightbour.gCost || !openNodes.Contains(neightbour))
                    {
                        neightbour.gCost = cost;
                        neightbour.hCost = GetDistance(neightbour, target);
                        neightbour.ParentNode = current;

                        if (!openNodes.Contains(neightbour))
                        {
                            openNodes.Add(neightbour);
                        }
                    }
                }
            }
        }

        return ret;
    }

    private List<GridNode> GetPath(GridNode start, GridNode end)
    {
        List<GridNode> ret = new List<GridNode>();
        GridNode current = end;

        while (current != start)
        {
            ret.Add(current);
            current = current.ParentNode;
        }

        ret.Reverse();
        return ret;
    }

    public List<GridNode> GetNeightbours(GridNode node, bool acceptEnemyNode = false)
    {
        List<GridNode> ret = new List<GridNode>();

        //left
        GridNode left = GetNode(node.X - 1, node.Z);
        if(left != null)
        {
            GridNode neightbour = GetNeightbour(left, acceptEnemyNode);

            if (neightbour != null)
            {
                ret.Add(neightbour);
            }
        }

        //right
        GridNode right = GetNode(node.X + 1, node.Z);
        if (right != null)
        {
            GridNode neightbour = GetNeightbour(right, acceptEnemyNode);

            if (neightbour != null)
            {
                ret.Add(neightbour);
            }
        }

        //left
        GridNode up = GetNode(node.X, node.Z + 1);
        if (up != null)
        {
            GridNode neightbour = GetNeightbour(up, acceptEnemyNode);

            if (neightbour != null)
            {
                ret.Add(neightbour);
            }
        }

        //left
        GridNode down = GetNode(node.X, node.Z - 1);
        if (down != null)
        {
            GridNode neightbour = GetNeightbour(down, acceptEnemyNode);

            if (neightbour != null)
            {
                ret.Add(neightbour);
            }
        }

        return ret;
    }

    private GridNode GetNeightbour(GridNode node, bool acceptEnemyNode = false)
    {
        GridNode ret = null;

        if (!acceptEnemyNode)
        {
            if(!node.HasEnemy && !node.IsBlocked)
                ret = node;
        }
        else
            ret = node;
        
        return ret;
    }

    private int GetDistance(GridNode A, GridNode B)
    {
        int x = Mathf.Abs(A.X - B.X);
        int z = Mathf.Abs(A.Z - B.Z);

        return x + z;
    }

    private GridNode GetNode(int x, int z)
    {
        return GridManager.Instance.GetNode(x, z);
    }

    private void ClearPathfinder()
    {
        if(Path != null)
            Path.Clear();
        startNode = null;
        targetNode = null;
    }

    public void JobFinished()
    {
        OnPathfindFinished?.Invoke(Path);
    }
}
