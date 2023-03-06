using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    public GameObject planePrefab;

    [SerializeField] private int defaultPlaneCount;
    [SerializeField] private List<GameObject> pooledPlane;
    [SerializeField] private int amountToPool = 10;

    public int CurrentPlaneCount = 0;

    public CubeSpawnerController CubeSpawnerController;

    void Start()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            var plane = Instantiate(planePrefab);
            plane.SetActive(false);
            plane.transform.SetParent(this.transform);
            pooledPlane.Add(plane);
        }
        CreatePlane(defaultPlaneCount, 1f);
    }

    public void CreatePlane(int count, float probability)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 nextPlanePosition = transform.position + (Vector3.forward * 5) * CurrentPlaneCount;

            float randomNumber = Random.Range(0f, 1f);

            if (randomNumber < probability)
            {
                var plane = getPlane();
                plane.transform.position = nextPlanePosition;
                plane.name = "Plane " + CurrentPlaneCount;

                CubeSpawnerController.GenerateCubes(CurrentPlaneCount, 5);
            }


            CurrentPlaneCount += 1;
        }
    }
    private GameObject getPlane()
    {
        for (int i = 0; i < pooledPlane.Count; i++)
        {
            if (!pooledPlane[i].activeInHierarchy)
            {
                pooledPlane[i].SetActive(true);
                return pooledPlane[i];
            }
        }
        return null;
    }
}
