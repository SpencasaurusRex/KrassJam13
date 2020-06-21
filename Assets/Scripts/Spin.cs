using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float Rate;

    void Update()
    {
        transform.rotation *= Quaternion.AngleAxis(Rate * Time.deltaTime, Vector3.forward);
    }
}
