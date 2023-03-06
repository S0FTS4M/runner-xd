using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawnerController : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private int amountToPool = 20;
    [SerializeField] private List<GameObject> pooledCube = new List<GameObject>();
    [SerializeField] private int limit = -1;


    private void Start()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject cube = Instantiate(cubePrefab);
            cube.SetActive(false);
            cube.transform.SetParent(this.transform);
            pooledCube.Add(cube);
        }
    }
    public void GenerateCubes(int currentPlaneIndex, int scale)
    {
        Vector3 planePos = transform.position + Vector3.forward * scale * currentPlaneIndex;

        for (int i = -1; i <= 1; i++)
        {
            float randomNumber = Random.Range(0f, 1f);

            if (randomNumber > 0.5f)
            {
                Vector3 newPosition = new Vector3(planePos.x + i, planePos.y + 1, planePos.z);
                // Instantiate(cubePrefab, newPosition, Quaternion.identity);
                var activeCube = getPooledObject();
                activeCube.transform.position = newPosition;

            }
        }
    }
    public GameObject getPooledObject()
    {

        for (int i = 0; i < pooledCube.Count; i++)
        {
            if (!pooledCube[i].activeInHierarchy)
            {
                pooledCube[i].SetActive(true);
                return pooledCube[i];
            }
        }

        limit++;
        limit %= pooledCube.Count;
        return pooledCube[limit % pooledCube.Count];


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
        return list;

    }
}
