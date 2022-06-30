using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    private Rigidbody rb;

    public GameObject explosion_prefab;
    public GameObject steam_prefab;
    public GameObject flame_prefab;
    public GameObject kindle_flame_prefab;
    public GameObject kindle_flame_prefab_big;
    public GameObject dissolveSound_prefab;
    public GameObject candleLight_prefab;
    private GameObject explosion;
    private GameObject steam;
    private GameObject kindleFlame;
    private GameObject kindleFlameBig;
    private GameObject woodBurn;
    private GameObject dissolveBurnSound;
    private GameObject candleFlame;
    public float steamDuration;
    public float explosionDuration;
    public float burnDuration;
    public float dissolveSoundDuration;
    private Vector3 spawnPos;

    public float explosionRadius;
    public float explosionPower;
    public LayerMask chosenLayer;
    private bool firstHit;
    private Vector3 startVelocity;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    public void OnCollisionEnter(Collision other) {

        if (this.firstHit || other.gameObject.tag == "Reflect") return;
        this.firstHit = true;             

        //burn something
        if(other.gameObject.tag == "Burnable"){
                other.gameObject.AddComponent<BurnController>();
                dissolveBurnSound = Instantiate(dissolveSound_prefab, other.GetContact(0).point, Quaternion.identity);
                Destroy(dissolveBurnSound, dissolveSoundDuration);
                
        }

        //kindle something
        if(other.gameObject.tag == "Kindle" || other.gameObject.tag == "KindleBig") {
            
            Vector3 offset = other.transform.GetComponent<KindleOffset>().getOffset();
            Vector3 offsetforward = new Vector3(0,0,0);
            offsetforward.z = offset.z;
            spawnPos = other.gameObject.GetComponent<Transform>().position;
            spawnPos.y += offset.y;
            if(other.gameObject.tag == "kindle"){
                kindleFlame = Instantiate(kindle_flame_prefab, spawnPos, Quaternion.identity);
            }
            else{
                kindleFlame = Instantiate(kindle_flame_prefab_big, spawnPos, Quaternion.identity);
            }
            kindleFlame.transform.Rotate(Vector3.up, other.gameObject.GetComponent<Transform>().rotation.eulerAngles.y);
            kindleFlame.transform.Translate(offsetforward, Space.Self);
            other.transform.GetChild(other.transform.GetChildCount()-1).gameObject.active = true;
            candleFlame = Instantiate(candleLight_prefab, kindleFlame.transform.position, Quaternion.identity);

            Debug.Log("kindle stuff");
        }

        //burn on wood other.gameObject.tag == "WoodBurn"
        if (other.gameObject.tag == "WoodBurn") {
            woodBurn = Instantiate(flame_prefab, other.GetContact(0).point, Quaternion.identity);
            Destroy(woodBurn, burnDuration);
        }

        //melting wall
        if(other.gameObject.tag == "Melting") {
            steam = Instantiate(steam_prefab, other.GetContact(0).point, Quaternion.identity);
            Destroy(steam, steamDuration);
            other.gameObject.AddComponent<MeltingController>(); //add Melting Script
        }

        //transform.GetComponent<Renderer>().enabled = false;
        this.explosionPyhsic(other.GetContact(0).point);
        Destroy(transform.gameObject);
    }
    private void explosionPyhsic(Vector3 position) 
    {
        Collider[] colliders = Physics.OverlapSphere(position, this.explosionRadius, chosenLayer);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if(rb == null)
                rb = hit.GetComponentInParent<Rigidbody>();
            ActiveByExplosion ae = hit.GetComponent<ActiveByExplosion>();
            if (ae != null) 
            {
                ae.activateRB();
                rb = hit.GetComponent<Rigidbody>();
            }


            if (rb != null)
                rb.AddExplosionForce(this.explosionPower, position, this.explosionRadius, 3.0F);
        }
    }

    private void rotateFlame(GameObject flame, Quaternion degree, Vector3 offsetForward){
        flame.transform.Rotate(Vector3.up, degree.eulerAngles.y);
        flame.transform.Translate(offsetForward, Space.Self);
       
    }
}
