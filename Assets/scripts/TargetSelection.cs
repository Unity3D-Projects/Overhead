﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelection : MonoBehaviour {

	[Header("To assign")]
	public float maximumHealth = 100.0f;
	public GameObject enemyHealthBar;
	public GameObject deathEffect;
	public Material overMaterial;

	[Header("Auto assign (No need to assign)")]
	public Material originalMaterial;
	public Material tempMaterial;
	public float HP;

	void Start () {
		// Setting current HP to maximumHP
		HP = maximumHealth;

		// Getting reference for materials to be used later
		originalMaterial = GetComponent<MeshRenderer> ().material;
		tempMaterial = GetComponent<MeshRenderer> ().material;

		// Rotating enemy UI (health bar) at beginning and desactivating it
		enemyHealthBar.transform.rotation = GameObject.FindGameObjectWithTag ("MainCamera").gameObject.transform.rotation;
		enemyHealthBar.SetActive (false);
	}

	void OnMouseDown ()  // If this.gameObject had been clicked
	{
		// Target it! (logic for untargetting happens on GameController script)
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ().SetTarget (this.gameObject);
	}

	void OnMouseEnter ()  // If mouse is over this.gameObject
	{
		// Gets current material
		tempMaterial = gameObject.GetComponent<MeshRenderer> ().material;

		// Sets actual material color to overMaterial material
		overMaterial.color = originalMaterial.color;

		// Sets actual material to overMaterial (with emission, but with originalMaterial color)
		gameObject.GetComponent<MeshRenderer> ().material = overMaterial;
	}

	void OnMouseExit ()  // If mouse is not over this.gameObject
	{
		gameObject.GetComponent<MeshRenderer> ().material = tempMaterial;
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Skill") {
			HP -= other.gameObject.GetComponent<SkillsProperties>().damage;
		}
	}

	void Update () {  // Put update here cause it's the last consequence
		if (HP <= 0) {
			Instantiate (deathEffect, transform.position, transform.rotation);
			Destroy (gameObject);
		}
	}
}