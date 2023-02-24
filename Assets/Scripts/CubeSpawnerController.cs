using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawnerController : MonoBehaviour
{
    public GameObject cubePrefab;

    public void GenerateCubes(int currentPlaneIndex, int scale)
    {
        Vector3 planePos = transform.position + Vector3.forward * scale * currentPlaneIndex;
        
        for (int i = -1; i <= 1; i++)
        {
            float randomNumber = Random.Range(0f, 1f);

            if(randomNumber > 0.5f)
            {
                Vector3 newPosition = new Vector3(planePos.x + i, planePos.y + 1, planePos.z);
                Instantiate(cubePrefab, newPosition, Quaternion.identity);
            }
        }
    }
}
