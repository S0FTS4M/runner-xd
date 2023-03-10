using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class CubeSpawnerController : NetworkBehaviour
{
    public NetworkObject goodCubePrefab;
    public NetworkObject badCubePrefab;

    public void GenerateCubes(int currentPlaneIndex, int scale)
    {
        Vector3 planePos = transform.position + Vector3.forward * scale * currentPlaneIndex;
        
        for (int i = -1; i <= 1; i++)
        {
            float randomNumber = Random.Range(0f, 1f);

            if(randomNumber > 0.5f)
            {
                Vector3 newPosition = new Vector3(planePos.x + i, planePos.y + 1, planePos.z);
                Runner.Spawn(goodCubePrefab, newPosition, Quaternion.identity);
                
            }
            else if(randomNumber>0.4f)
            {
                Vector3 newPosition = new Vector3(planePos.x + i, planePos.y + 1, planePos.z);
                Runner.Spawn(badCubePrefab, newPosition, Quaternion.identity);                
            }
        }
    }
}
