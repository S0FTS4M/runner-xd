using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private Vector3 deadCamera;
    [SerializeField] private float turnSpeed = 3f;

    private float distance;
    private bool turn;
    private void Awake()
    {
        distance = player.transform.position.z - transform.position.z;
    }
    void Update()
    {
        if (player.isGameOver)
        {
            if (turn == false)
            {
                transform.Rotate(Vector3.left, turnSpeed * Time.deltaTime);
                if (transform.rotation.x < -0.5) turn = true;
            }

            if (turn == true)
            {
                transform.Rotate(Vector3.right, turnSpeed * Time.deltaTime);
                if (transform.rotation.x > 0.5) turn = false;
            }
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z - distance);
        }
    }
}
