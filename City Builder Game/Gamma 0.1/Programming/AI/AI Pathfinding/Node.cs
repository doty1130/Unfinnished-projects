﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public Vector3 worldPos;
    public bool walkable;

    public int gridX;
    public int gridY;
    public int movementPenalty;

    public int gCost;
    public int hCost;
    int heapIndex;


    public Node parent;
    public Node(bool _walkable, Vector3 _worldpos, int _gridX, int _gridY, int _penalty)
    {
        worldPos = _worldpos;
        walkable = _walkable;
        gridX = _gridX;
        gridY = _gridY;
        movementPenalty = _penalty;
    }

    public int fCost 
    {
        get { return gCost + hCost; }
    }

    public int HeapIndex 
    {
        get {
            return heapIndex;
        }
        set { heapIndex = value; }
    }
    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }

}
