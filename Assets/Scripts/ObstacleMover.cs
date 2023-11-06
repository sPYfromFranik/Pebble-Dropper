using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    int movementVerticalDirection = 1;
    int movementHorizontalDirection = 1;
    [SerializeField] int movementSpeed;
    [SerializeField] float horizontalConstraint;
    [SerializeField] float verticalConstraint;
    float startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (horizontalConstraint > 0) //if there is a value set in Unity - move this obstacle in the specified range
        {
            transform.Translate(Vector3.right * movementHorizontalDirection * movementSpeed * Time.deltaTime);
            if (transform.position.x < -horizontalConstraint)
                movementHorizontalDirection = 1;
            if (transform.position.x > horizontalConstraint)
                movementHorizontalDirection = -1;
        }
        if (verticalConstraint > 0) //if there is a value set in Unity - move this obstacle in the specified range
        {
            transform.Translate(Vector3.up * movementVerticalDirection * movementSpeed * Time.deltaTime);
            if (transform.position.y < -verticalConstraint+startPos)
                movementVerticalDirection = 1;
            if (transform.position.y > verticalConstraint+startPos)
                movementVerticalDirection = -1;
        }
    }
}
