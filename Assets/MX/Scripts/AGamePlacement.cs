using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MXAI;


public class AGamePlacement : MonoBehaviour, IAPathGridManager
{
    [SerializeField]
    private Vector3Int _size = new Vector3Int(1, 1, 0);
    public int CX { get { return this._size.x; } }
    public int CY { get { return this._size.y; } }
    public int CZ { get { return this._size.z; } }

    public void SetSize(Vector3Int size)
    {
        this._size = size;
    }

    //
    protected APathFinder<AGameNode> _pathFinder = null;

    [SerializeField]
    private List<AGameNode> _gameNodeSet = null;

    void Awake()
    {
        _gameNodeSet = new List<AGameNode>();
        _pathFinder = new APathFinder<AGameNode>(this);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Override:
    public List<APathNode<T>> NewPathNodeList<T>() where T : IAPathNode
    {
        return new List<APathNode<T>>();
    }

    public List<APathNode<T>> GetPathNodes<T>() where T : IAPathNode
    {
        List<APathNode<T>> nodeSet = this.NewPathNodeList<T>();
        foreach (AGameNode v in this._gameNodeSet)
        {
            // 将坐标转换为网格坐标，以左上角为原点，CX横向数量，CY竖向数量
            APathNode<T> node = new APathNode<T>();
            node.index = v.index;
            node.X = v.X;
            node.Y = v.Y;
            node.Z = v.Z;
            node.current = (T)(IAPathNode)v;
            nodeSet.Add(node);
        }
        return nodeSet;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
