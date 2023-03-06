using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawnerController : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private int amountToPool = 10;
    [SerializeField] private List<GameObject> pooledObject = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject cube = Instantiate(cubePrefab);
            cube.SetActive(false);
            pooledObject.Add(cube);
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
                activeCube.SetActive(true);
            }
        }
    }
    public GameObject getPooledObject()
    {

        for (int i = 0; i < pooledObject.Count; i++)
        {
            if (!pooledObject[i].activeInHierarchy)
            {
                return pooledObject[i];
            }
        }
        return null;
    }
}
