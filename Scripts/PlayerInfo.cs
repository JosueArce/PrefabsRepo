using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour {

	private int health;
	// Use this for initialization
	void Start () {
		health = 100;
	}
	
	public void Hurt(int damage){
		health -= damage;
	}
}
