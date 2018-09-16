using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Script : MonoBehaviour {

	public float damage = 10f;
	public float impactForce = 30f;
	public float fireRate = 15f;
	public float range = 100f;

	public int maxAmmo = 10;
	public static int currentAmmo = -1;
	public float reloadTime = 2.5f;
	private bool isReloading = false;

	public Camera fpsCam;
	public ParticleSystem muzzelFlash;
	public GameObject bloodFX;
	public GameObject woodHoleFX;
    public GameObject floorHoleFX;
    public GameObject stoneHoleFX;
    public GameObject waterImpactFX;

    private float nextTimeToFire = 0f;
	public Animator animator;

    public AudioSource reloadSound;
    public AudioSource woodImpact;
    public AudioSource floorImpact;
    public AudioSource stoneImpact;
    public AudioSource enemyImpact;
    public AudioSource waterImpact;
    

    void Start(){
		if(currentAmmo == -1){
			currentAmmo = maxAmmo;
		}
	}

	void OnEnable(){
		isReloading = false;
		animator.SetBool("reloading",false);
	}

    // Update is called once per frame
    void Update () {
		if(isReloading){
			return;
		}
		animator.SetBool("idle",true);
		if(currentAmmo <= 0){
			animator.SetBool("idle",false);
			StartCoroutine(Reload());
			return;
		}

		if(Input.GetButton("Fire1") && Time.time >= nextTimeToFire){
			animator.SetBool("idle",false);
			nextTimeToFire = Time.time + 1f/fireRate;
			StartCoroutine(Shoot());
		}
		
	}

	IEnumerator Shoot(){
		muzzelFlash.Play();
		currentAmmo --;
		RaycastHit hit;
		animator.SetBool("shooting", true);
        AudioSource shootSound = GetComponent<AudioSource>();
        shootSound.Play();
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)){
            
            //Debug.Log(hit.transform.name);
            
			Enemy enemy = hit.transform.GetComponent<Enemy>();

			//if(enemy != null){
			//	enemy.TakeDamage(damage);
			//}
			if(hit.rigidbody != null){
				hit.rigidbody.AddForce(-hit.normal * impactForce);
			}
            if (hit.transform.tag == "enemy")
            {
                enemyImpact.Play();
                Instantiate(bloodFX, hit.point, Quaternion.LookRotation(hit.normal));
                enemy.TakeDamage(this.damage);
            }
            if (hit.transform.tag == "wood")
            {
                woodImpact.Play();
                Instantiate(woodHoleFX, hit.point, Quaternion.LookRotation(hit.normal));
            }
            if (hit.transform.tag == "floor")
            {
                floorImpact.Play();
                Instantiate(floorHoleFX, hit.point, Quaternion.LookRotation(hit.normal));
            }
            if (hit.transform.tag == "water")
            {
                waterImpact.Play();
                Instantiate(waterImpactFX, hit.point, Quaternion.LookRotation(hit.normal));
            }
            if (hit.transform.tag == "Stone")
            {
                stoneImpact.Play();
                Instantiate(stoneHoleFX, hit.point, Quaternion.LookRotation(hit.normal));
            }
            yield return new WaitForSeconds(0f);	
			animator.SetBool("shooting",false);
		}
		animator.SetBool("shooting",false);
	}

	IEnumerator Reload(){
        reloadSound.Play();
		isReloading = true;
		animator.SetBool("reloading",true);
		yield return new WaitForSeconds(reloadTime -0.25f);
		animator.SetBool("reloading",false);
		yield return new WaitForSeconds(0.25f);
		currentAmmo = maxAmmo;
		isReloading = false;
	}

    
}
