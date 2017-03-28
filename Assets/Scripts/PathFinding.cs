using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinding : MonoBehaviour
{

    long minDist = 100000;

    //*********************************
    //  AStar Search
    //  from: start cell
    //  to: target cell
    //  height: the height of the maze
    //  width: the width of the maze
    //
    //  Hint: the MazeCell is the to represent each cell in the gameworld. The most important variables are coordinates and neighbors
    //  The first one is the coordiantes of the cell. And the second one is to indicate the neighbors of the cell. It is a graph already build for implementation.
    //*********************************
    // 
    public List<MazeCell> AStarFindPath(MazeCell from, MazeCell to, int height, int width)
    {
        minDist = 100000;

        HashSet<MazeCell> closedSet = new HashSet<MazeCell>(); // The set of cells already evaluated.
        HashSet<MazeCell> openSet = new HashSet<MazeCell>();  // The set of currently discovered cells that are not evaluated yet.

        float[,] gScore = new float[height, width];            // For each cell, the cost of getting from the start cell to that cell.
        float[,] fScore = new float[height, width];            // For each cell, the total cost of getting from the start cell to the target

        Dictionary<MazeCell, MazeCell> comeFrom = new Dictionary<MazeCell, MazeCell>();
        List<MazeCell> path = new List<MazeCell>();           // This best path

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                gScore[i, j] = float.MaxValue;
                fScore[i, j] = float.MaxValue;

            }
        }

        openSet.Add(from);

        gScore[from.coordinates.x, from.coordinates.z] = 0;
        fScore[from.coordinates.x, from.coordinates.z] = heuristicCostEstimate(from, to);

        MazeCell current = from;

        while (openSet.Count != 0)
        {
            bool first = true;

            foreach (MazeCell cell in openSet)
            {
                if (first)
                {
                    current = cell;
                    first = false;
                }

                if (GetFScore(cell, fScore) < GetFScore(current, fScore))
                {
                    current = cell;
                }
            }
            if (current.coordinates.Equals(to.coordinates))
            {
                break;
            }
            openSet.Remove(current);
            closedSet.Add(current);
            foreach (MazeCell neighbor in current.neighbors)
            {
                if (closedSet.Contains(neighbor))
                {
                    continue;
                }
                float tentative_gScore = GetGScore(current, gScore) + GetDistance(current, neighbor);
                if (!openSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);

                }
                else if (tentative_gScore >= GetGScore(neighbor, gScore))
                {
                    continue;
                }
                comeFrom[neighbor] = current;
                gScore[neighbor.coordinates.x, neighbor.coordinates.z] = tentative_gScore;
                fScore[neighbor.coordinates.x, neighbor.coordinates.z] =
                    GetGScore(neighbor, gScore) + heuristicCostEstimate(neighbor, to);


            }
        }

        path.Add(current);
        while (comeFrom.ContainsKey(current))
        {
            current = comeFrom[current];
            path.Add(current);

        }
        path.Reverse();

        return path;
    }
    /////////////////////////////////////////////
    /// 
    ///  The heuristic function used to evaulute the cost between two points
    /// 
    /// 
    /// /////////////////////////////////////////	
    private float heuristicCostEstimate(MazeCell from, MazeCell to)
    {
        int dx = Math.Abs(from.coordinates.x - to.coordinates.x);
        int dz = Math.Abs(from.coordinates.z - to.coordinates.z);
        return dx + dz;
    }


    /////////////////////////////////////////////
    /// 
    ///  GetFScore
    /// 
    /// 
    /// /////////////////////////////////////////	
    private float GetFScore(MazeCell cell, float[,] fScore)
    {
        return fScore[cell.coordinates.x, cell.coordinates.z];
    }

    /////////////////////////////////////////////
    /// 
    ///  GetGScore
    /// 
    /// 
    /// /////////////////////////////////////////	
    private float GetGScore(MazeCell cell, float[,] gScore)
    {
        return gScore[cell.coordinates.x, cell.coordinates.z];
    }

    private static float GetDistance(MazeCell cell1, MazeCell cell2)
    {
        return (float)Math.Sqrt(Math.Pow((cell2.coordinates.x - cell1.coordinates.x), 2) + Math.Pow((cell2.coordinates.z - cell1.coordinates.z), 2));
    }
}
