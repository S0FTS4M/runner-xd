using UnityEngine;
using Fusion;
public class PlaneController : NetworkBehaviour,ISpawned
{
    public NetworkObject planePrefab;

    public int defaultPlaneCount;

    public int CurrentPlaneCount = 0;

    public CubeSpawnerController CubeSpawnerController;


    public void CreatePlane(int count, float probability)
    {
       
        for (int i = 0; i < count; i++)
        {
            Vector3 nextPlanePosition = transform.position + (Vector3.forward * 5) * CurrentPlaneCount;

            float randomNumber = Random.Range(0f, 1f);

            if(randomNumber < probability)
            {
                //GameObject plane = Instantiate(planePrefab, nextPlanePosition, Quaternion.identity);
                var plane = Runner.Spawn(planePrefab, nextPlanePosition, Quaternion.identity,Runner.LocalPlayer);

                plane.name = "Plane " + i;

                CubeSpawnerController.GenerateCubes(CurrentPlaneCount, 5);
            }


            CurrentPlaneCount += 1;
        }
    }
}
