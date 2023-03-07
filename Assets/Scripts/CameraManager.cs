using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    private float distance;
    private void Awake()
    {
        distance = player.transform.position.z - transform.position.z;
    }
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z - distance);
    }
}
