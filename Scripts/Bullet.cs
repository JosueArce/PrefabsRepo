using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public float speed = 10.0f;
	public int damage = 10;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(0, 0, speed * Time.deltaTime);
	}

	void onTriggerEnter(Collider other){
		PlayerInfo player = other.GetComponent<PlayerInfo>();
		if(player != null){
			player.Hurt(damage);
		}
		Destroy(this.gameObject);
	}
}
