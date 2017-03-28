using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour {

    public IntVector2 size;
    public MazeCell cellPrefab;
    public MazePassage passagePrefab;
    public MazeWall wallPrefab;
	public Tank tankPrefab;
	public GamePlayer playerPrefab;
	public GameObject glodPrefab;
	public int moveSteps = 100;

	public IntVector2 targetCoordinates;
	public int numTanks;
	//tanksController.createTanks(tankNum);
	private MazeCell[,] cells;
	private Tank[] tanks;
	private GamePlayer player;
	private GameObject gold;

	private PathFinding pathFinding;

	public void initialize()
	{
		cells = new MazeCell[size.x, size.z];
		foreach (Transform child in transform) {
			MazeCell cell = child.gameObject.GetComponent<MazeCell> ();
			cells[cell.coordinates.x, cell.coordinates.z] = cell;
		}
		numTanks = 1;
		tanks = new Tank[numTanks];

		Tank newTank = this.createTank(0, new IntVector2(1,0));
		tanks[0] = newTank;

		this.pathFinding = new PathFinding ();
	}

    public IEnumerator Generate()
    {
        WaitForSeconds delay = new WaitForSeconds(0.01f);
        cells = new MazeCell[size.x, size.z];
        List<MazeCell> activeCells = new List<MazeCell>();
        DoFirstGenerationStep(activeCells);
        while (activeCells.Count > 0)
        {
            yield return delay;
            DoNextGenerationStep(activeCells);
        }
		targetCoordinates = RandomCoordinates;
		foreach (Transform child in cells [targetCoordinates.x, targetCoordinates.z].transform) {
			//Debug.Log(child.
			Renderer rendered = child.gameObject.GetComponent<Renderer>();
			if (rendered != null) {
				rendered.material.color = Color.red;
			}
		}
		tanks = new Tank[numTanks];

		for(int i=0;i<numTanks;i++)
		{
			IntVector2 coordinates = RandomCoordinates;
			Tank newTank = this.createTank(i, coordinates);
			tanks[i] = newTank;
		}
		this.pathFinding = new PathFinding ();
    }
	private Tank createTank(int id, IntVector2 coordinates)
	{
		Tank newTank = Instantiate(tankPrefab) as Tank;
		newTank.transform.position = cells[coordinates.x, coordinates.z].transform.position;
		newTank.name = "Tank " + id;
		newTank.coordinates = coordinates;
		Debug.Log ("createTank: " + id+", "+newTank.coordinates.x+", "+newTank.coordinates.z);
		return newTank;
	}
    private void DoFirstGenerationStep(List<MazeCell> activeCells)
    {
        MazeCell newCell = CreateCell(RandomCoordinates);
        //Debug.Log(newCell.coordinates.x+", "+ newCell.coordinates.z);
        activeCells.Add(newCell);
    }
	private MazeCell GetStartCell(Vector3 startPos)
	{
		int x = -1, z = -1;
		float minDist = 10000;
		for (int i = 0; i < size.x; i++) {
			for (int j = 0; j < size.z; j++) {
				
				Vector3 distVect = cells [i, j].transform.position - startPos;
				if(distVect.sqrMagnitude<minDist){
					minDist = distVect.sqrMagnitude;
					x = i;
					z = j;
				}

			}

		}
		return cells [x, z];
	}
    private void DoNextGenerationStep(List<MazeCell> activeCells)
    {
        int currentIndex = activeCells.Count - 1;
        MazeCell currentCell = activeCells[currentIndex];
        if (currentCell.IsFullyInitialized)
        {
            activeCells.RemoveAt(currentIndex);
            return;
        }
        MazeDirection direction = currentCell.RandomUninitializedDirection;
        IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();
        if (ContainsCoordinates(coordinates))
        {
            MazeCell neighbor = GetCell(coordinates);
            if (neighbor == null)
            {
                neighbor = CreateCell(coordinates);
                CreatePassage(currentCell, neighbor, direction);
                activeCells.Add(neighbor);
            }
            else
            {
                CreateWall(currentCell, neighbor, direction);
                // No longer remove the cell here.
            }
        }
        else
        {
            CreateWall(currentCell, null, direction);
            // No longer remove the cell here.
        }
    }
    private void CreatePassage(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        MazePassage passage = Instantiate(passagePrefab) as MazePassage;
        passage.Initialize(cell, otherCell, direction);
        passage = Instantiate(passagePrefab) as MazePassage;
        passage.Initialize(otherCell, cell, direction.GetOpposite());
    }

	private void CreateWall (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		MazeWall wall = Instantiate(wallPrefab) as MazeWall;
		wall.Initialize(cell, otherCell, direction);
		if (otherCell != null) {
			wall = Instantiate(wallPrefab) as MazeWall;
			wall.Initialize(otherCell, cell, direction.GetOpposite());
		}
	}
    public MazeCell GetCell(IntVector2 coordinates)
    {
        return cells[coordinates.x, coordinates.z];
    }

    private MazeCell CreateCell(IntVector2 coordinates)
    {
        MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
        cells[coordinates.x, coordinates.z] = newCell;
        newCell.coordinates = coordinates;
        newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
        newCell.transform.parent = transform;
        newCell.transform.localPosition = new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0f, coordinates.z - size.z * 0.5f + 0.5f);
        return newCell;
    }
	public IntVector2 RandomCoordinates {
		get {
			return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
		}
	}

	public bool ContainsCoordinates (IntVector2 coordinate) {
		return coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z;
	}

	public List<MazeCell> findPath2Targte(IntVector2 startCoordinates)
	{
		MazeCell fromCell = cells[startCoordinates.x, startCoordinates.z];
		MazeCell targetCell = cells [targetCoordinates.x, targetCoordinates.z];
		bool[,] isVisited = new bool[cells.GetLength (0), cells.GetLength (1)];

		List<MazeCell> pathCells = this.pathFinding.AStarFindPath(fromCell, targetCell, cells.GetLength (0), cells.GetLength (1));
		return pathCells;
	}

	public void Move2Target()
	{
		for(int i=0;i<numTanks;i++){
			List<MazeCell> pathCells = this.findPath2Targte (tanks[i].coordinates);
			Debug.Log (pathCells.Count);
			tanks[i].startMoving(pathCells);
		}
	}
}
