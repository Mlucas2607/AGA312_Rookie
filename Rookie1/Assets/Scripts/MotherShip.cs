using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherShip : MonoBehaviour
{
    public UIManager ui;
    public int motherShipHealth = 10000;
    public GameObject deathEffect;
    public float healthBarRange = 1000f;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        ui.motherShipSlider.value = motherShipHealth;

        float playerDis = Vector3.Distance(player.position, transform.position);
        if (playerDis < healthBarRange)
            ui.motherShipHealthBar.SetActive(true);
        else
            ui.motherShipHealthBar.SetActive(false);

    }


    public void TakeDamageType2(int dmgAmt)
    {
        if (motherShipHealth <= 0)
            MotherShipDead();

        motherShipHealth -= dmgAmt;
    }

    void MotherShipDead()
    {
        Vector3 spwnPos = transform.position;
        Quaternion spwnRot = transform.rotation;
        Instantiate(deathEffect, spwnPos, spwnRot);
        Destroy(this.gameObject);
    }
}
