﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideEffect : MonoBehaviour {
	
	private float duration;

	[Header("Freeze Effect")]
	[SerializeField] private bool freeze = false;
	private float slowFactor;

	[Header("Burn Effect")]
	[SerializeField] private bool burn = false;
	private float burnRate;

	[Header("Range Effect")]
	[SerializeField] private bool range = false;
	[SerializeField] private GameObject normalBullet = null;
	private float rangeRadius = 5f;

	private GameObject target;

	public void SetTarget (GameObject _target) {
		target = _target;
	}

	public void SetBurnRate (float _burnRate) {
		burnRate = _burnRate;
	}

	public void SetSlowFactor (float _slowFactor) {
		slowFactor = _slowFactor;
	}

	public void SetRangeRadius (float _rangeRadius) {
		rangeRadius = _rangeRadius;
	}

	public void SetEffectDuration (float _duration) {
		duration = _duration;
	}

	public void StartEffect () {
		
		StartCoroutine (AutoDestroy ());

		// If it's already side affected
		if (!target.GetComponent<TargetSelection> ().IsSideAffected ()) {

			target.GetComponent<TargetSelection> ().SetSideEffect (true);

			if (freeze)
				Freeze ();
			else if (burn)
				StartCoroutine (Burn ());
			else if (range)
				Range ();
		} else // Don't affect again
			Destroy (this.gameObject);
	}

	void Update () {
		
		if (target != null) {
			// Follow target
			transform.position = target.transform.position;
		}
	}

	void Freeze () {
		target.GetComponent<Enemy> ().SetSpeed( target.GetComponent<Enemy> ().GetSpeed() * slowFactor );
	}

	IEnumerator Burn () {
		while (true) {
			
			if (target != null) {
				target.GetComponent<TargetSelection> ().TakeDamageBy (this.gameObject);
			}

			yield return new WaitForSeconds (0.1f / burnRate);
		}
	}

	void Range () {
		SphereCollider col = GetComponent<SphereCollider> ();

		if (col == null)
			col = gameObject.AddComponent<SphereCollider> ();

		if (!col.isTrigger) {
			col.isTrigger = true;
		}

		col.radius = rangeRadius;

		transform.localScale = Vector3.zero;

		StartCoroutine (GrowUpInTime ());
	}

	IEnumerator GrowUpInTime () {
		float t = Time.time;
		while (transform.localScale.magnitude < Vector3.Magnitude (Vector3.one * rangeRadius * 2)) {
			transform.localScale = Vector3.Slerp (Vector3.zero, Vector3.one * rangeRadius * 2, Time.time - t);
			yield return null;
		}

		Destroy (gameObject);
	}

	void OnTriggerEnter (Collider other) {
		Vector3 colDir = other.transform.position - transform.position;

		Quaternion colQua = Quaternion.FromToRotation (transform.forward, colDir);

		GameObject spellGO = (GameObject)Instantiate (normalBullet, transform.position, colQua);
		TowerSpell towerSpell = spellGO.GetComponent<TowerSpell>();
		SkillsProperties skillPro = spellGO.GetComponent<SkillsProperties> ();

		// Speel need to know who instantiated him
		skillPro.SetInvoker (gameObject);

		if (towerSpell != null)
			towerSpell.Seek (target.transform);
	}

	IEnumerator AutoDestroy () {
		yield return new WaitForSeconds (duration);

		if (target != null) {  // If target have not died yet
			target.GetComponent<Enemy> ().ReturnToOriginalSpeed ();
			target.GetComponent<TargetSelection> ().SetSideEffect(false);
		}

		Destroy (gameObject);
	}
}
