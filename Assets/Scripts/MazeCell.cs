using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour {

    public IntVector2 coordinates;
    private MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];

    private int initializedEdgeCount;
	public List<MazeCell> neighbors = new List<MazeCell>();
	public bool isOccupied;
	public void addNeighbor(MazeCell neighbor)
	{

	}
    public MazeCellEdge GetEdge(MazeDirection direction)
    {
        return edges[(int)direction];
    }

    public bool IsFullyInitialized
    {
        get
        {
            //Debug.Log("initializedEdgeCount:MazeDirections.Count: "+ initializedEdgeCount+", "+ MazeDirections.Count);
            return initializedEdgeCount == MazeDirections.Count;
        }
    }

    public void SetEdge(MazeDirection direction, MazeCellEdge edge)
    {
        edges[(int)direction] = edge;
        initializedEdgeCount += 1;
		if (initializedEdgeCount == 5) {
			Debug.Log (initializedEdgeCount);
		}
    }
    public MazeDirection RandomUninitializedDirection
    {
        get
        {
            int skips = Random.Range(0, MazeDirections.Count - initializedEdgeCount);
            for (int i = 0; i < MazeDirections.Count; i++)
            {
                if (edges[i] == null)
                {
                    if (skips == 0)
                    {
                        return (MazeDirection)i;
                    }
                    skips -= 1;
                }
            }
            throw new System.InvalidOperationException("MazeCell has no uninitialized directions left.");
        }
    }
}
