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
        sortList(pooledPlane);

        pooledPlane[0].SetActive(false);

        return getPlane();

    }
    private List<GameObject> sortList(List<GameObject> list)
    {
        for (int i = 0; i < list.Count - 1; i++)
        {
            for (int j = 0; j < list.Count - 1 - i; j++)
            {
                int first = int.Parse(list[j].name.Split(" ")[1]);
                int second = int.Parse(list[j + 1].name.Split(" ")[1]);
                if (first > second)
                {
                    var temp = list[j];
                    list[j] = list[j + 1];
                    list[j + 1] = temp;
                }
            }
        }
        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log(int.Parse(list[i].name.Split(" ")[1]));
        }
        return list;

    }
}

