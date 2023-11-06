using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRotator : MonoBehaviour
{
    [SerializeField] GameObject rotationCenter;
    float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rotationSpeed = Random.Range(-1f, 1f) * 10;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.RotateAround(rotationCenter.transform.position, Vector3.forward, rotationSpeed); //rotates an obstacle around the empty child gameobject
    }
}
