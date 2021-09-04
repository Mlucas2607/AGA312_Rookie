using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    public float torque = 500f;
    public float thrust = 1000f;
    private Rigidbody rb;
    public Transform player;
    public GameObject healthBarrel;

    public int gunDamage = 10;

    public float sightRange = 100f;
    public float gunSpread = 0.1f;
    public Transform barrelPos;
    public GameObject hitEffectDefault;
    public GameObject hitEffectPlayer;
    public GameObject deathEffect;
    public GameObject laserEffect;
    public float fireRate = 0.1f, nextShootTime;

    public int health = 50;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);

        Vector3 targetLocation = player.position - transform.position;
        float distnace = targetLocation.magnitude;
        rb.AddRelativeForce(Vector3.forward * Mathf.Clamp((distnace - 10)/50,0f,1f) * thrust);

        if (Time.time > nextShootTime)
            CheckForPlayer();
    }

    void CheckForPlayer()
    {
        float playerDis = Vector3.Distance(player.position,transform.position);
        if (playerDis < sightRange)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        Ray shootRay = new Ray(transform.position, transform.forward);

        shootRay.direction = transform.forward + new Vector3(Random.Range(-gunSpread, gunSpread), Random.Range(-gunSpread, gunSpread), Random.Range(-gunSpread, gunSpread));

        if (Physics.Raycast(shootRay, out hit,sightRange))
        {
            if (hit.collider.tag == "Player")
            {
                Instantiate(hitEffectPlayer, hit.point, transform.rotation);
                hit.collider.GetComponent<PlayerController>().TakeDamage(gunDamage);
            }
            else
                Instantiate(hitEffectDefault, hit.point, transform.rotation);
        }

        Instantiate(laserEffect, barrelPos.position, transform.rotation, transform);
        nextShootTime = Time.time + fireRate;
    }

    public void TakeDamage(int dmgAmount)
    {
        if (health <= 0)
            DestoryEnemy();

        health -= dmgAmount;
    }

    void DestoryEnemy()
    {
        Vector3 spwnPos = transform.position;
        Quaternion spwnRot = transform.rotation;
        Instantiate(deathEffect, spwnPos,spwnRot);
        Instantiate(healthBarrel, spwnPos, spwnRot);
        Destroy(this.gameObject);
    }
}
