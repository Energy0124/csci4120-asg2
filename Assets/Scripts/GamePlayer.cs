using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayer : MonoBehaviour {

	public GameObject bulletPrefab;
    public Transform bulletSpawn;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
		//Debug.Log (transform.position.x + ", " + transform.position.z);
    }
    void Fire()
    {

    }
}
