using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHab : MonoBehaviour
{
    public float rotSpeed = 10f;
    void Update()
    {
        transform.Rotate(transform.up * rotSpeed * Time.deltaTime, Space.World);
    }
}
