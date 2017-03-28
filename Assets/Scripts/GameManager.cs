using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Maze mazePrefab;
	public Maze mazeInstance;
	public bool isInstanciated;
	//public TanksController tanksController;

    private void Start()
    {
		if (!isInstanciated) {
			StartCoroutine (BeginGame ());
		} else {
			mazeInstance.initialize();
		}
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }*/
		/*if (Input.GetKeyDown ("f")) {
			mazeInstance.findPath();
		}*/

		if (Input.GetKeyDown("f")) {
			mazeInstance.Move2Target();
		}
    }

    private IEnumerator BeginGame()
    {
        mazeInstance = Instantiate(mazePrefab) as Maze;
		mazeInstance.transform.position = new Vector3 (0, 0, 0);
        yield return StartCoroutine(mazeInstance.Generate());
    }

    /*private void RestartGame()
    {
        Destroy(mazeInstance.gameObject);
        StartCoroutine(BeginGame());
    }*/
}
