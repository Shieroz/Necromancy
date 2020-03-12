using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float rotationRate = 50f;
    public float moveSpeed = 1f;
    // Update is called once per frame
    void Update()
    {
        float vertical = Input.GetAxis("Horizontal");
        float horizontal = Input.GetAxis("Vertical");
        transform.position += transform.forward * horizontal * moveSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, vertical * rotationRate * Time.deltaTime);
    }
}
