﻿using UnityEngine;

public class Enemy : MonoBehaviour {

	public float speed =10f;
	public float minDist = 0.2f;

	private WayPointsScript wayPoints;
	private Transform target;
	private int wavepointIndex = 0;
	private GameObject masterTower;
	private MasterTowerScript masterTowerScript;
	private float originalHeight;

	//WaveSpawner will use this to set the waypoint
	public void SetWayPoints(WayPointsScript wayP){
		wayPoints = wayP;
	}

	private void Start(){
		masterTower = GameObject.Find("MasterTower");
		masterTowerScript = masterTower.GetComponent<MasterTowerScript> ();
		target = wayPoints.GetPoints (0);
		originalHeight = transform.position.y;
	}

	private void Update (){
		Vector3 dir = target.position - transform.position;
		transform.Translate (dir.normalized * speed * Time.deltaTime, Space.World);
		transform.LookAt (target.position);
		if (Vector3.Distance (transform.position, target.position) <= minDist) {
			GetNextWayPoint ();
		}
	}

	//In the WayPoints array, get the next or attack the Main Tower
	private void GetNextWayPoint(){
		if (wavepointIndex >= wayPoints.GetPointsLength() - 1) {
			EnemyAttack ();
		} else {
			wavepointIndex++;
			target = wayPoints.GetPoints (wavepointIndex);
		}
	}

	//Attack the main tower
	private void EnemyAttack(){
		masterTowerScript.EnemyAttack ();
		Destroy (gameObject);
	}
}