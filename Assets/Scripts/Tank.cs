using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour {

	public IntVector2 coordinates;
	public bool isMoving = false; 

	private int moveSteps = 100;
	[HideInInspector]
	private List<MazeCell> pathCells;
	// Update is called once per frame

	void Update () {
		if (isMoving) {
			StartCoroutine(move());
		}
	}
		
	public void startMoving(List<MazeCell> pathCells){
		this.isMoving = true;
		this.pathCells = pathCells;
	}
					
	public IEnumerator move(){

		if(pathCells.Count>0){
			for (int i = 0; i < pathCells.Count; i++) {
				float startx = transform.position.x;
				float startz = transform.position.z;
				float endx = pathCells[i].transform.position.x;
				float endz = pathCells[i].transform.position.z;
				transform.LookAt(pathCells[i].transform.position);
				for(int j=0;j<=this.moveSteps;j++)				{
					transform.position = new Vector3(startx + (endx - startx) / moveSteps * j,transform.position.y,startz + (endz - startz) / moveSteps * j);
					yield return null;
				}
			}
		}
	}
}
