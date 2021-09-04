using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCannister : MonoBehaviour
{
    public int healAmount = 50;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().HealPlayer(healAmount);
            Destroy(gameObject);
        }
    }
}
