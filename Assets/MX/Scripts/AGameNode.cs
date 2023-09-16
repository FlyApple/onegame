using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MXAI;

public class AGameNode : MonoBehaviour, IAPathNode
{
    [SerializeField]
    public int index = -1;
    [SerializeField]
    private Vector3Int _grid = new Vector3Int(0, 0, 0);
    public void SetGrid(Vector3Int grid)
    {
        this._grid = grid;
    }

    //Grid X
    public int X { get { return this._grid.x; } }
    //Grid Y
    public int Y { get { return this._grid.y; } }
    public int Z { get { return this._grid.z; } }
    public int A { get { return 0; } }


    /// <summary>
    /// AStar PathFinder walkable check.
    /// </summary>
    ///
    public bool walkable
    {
        get
        {
            // 开启，或禁用都不可以寻路
            return true;
        }
    }

    /// <summary>
    /// AStar PathFinder Kinds check.
    /// </summary>
    /// <param name="node"></param>
    /// <returns>default: false. Not kinds</returns>
    public bool iskind(IAPathNode node)
    {
        if (node == null) { return false; }
        return iskind((AGameNode)node);

    }

    public bool iskind(AGameNode node)
    {
        if (node == null) { return false; }

        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
