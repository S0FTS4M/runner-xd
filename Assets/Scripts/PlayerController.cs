using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject collectedCubePrefab;

    public float Speed;

    public Transform rayTransform;

    public PlaneController PlaneController;

    public Transform StackTransform;

    public int collectedCubeCount;

    public bool isGameOver = false;
    [SerializeField] private Vector3 lastPlacedCoor;
    [SerializeField] private List<GameObject> pooledCollectedCubeFree;
    [SerializeField] private List<GameObject> pooledCollectedCubeUsed;

    [SerializeField] private int PoolSizeInStart;

    private void Awake()
    {
        pooledCollectedCubeFree = new List<GameObject>();
        pooledCollectedCubeUsed = new List<GameObject>();
        for (int i = 0; i < PoolSizeInStart; i++)
        {
            generateCubes();
        }
    }

    void Update()
    {
        if (isGameOver == true)
            return;

        transform.position += Vector3.forward * Time.deltaTime * Speed;
        if (Input.GetKeyDown(KeyCode.LeftArrow) && transform.position.x > -1)
        {
            transform.position += Vector3.left;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && transform.position.x < 1)
        {
            transform.position += Vector3.right;
        }

        int currentPlaneIndex = (int)transform.position.z / 5;
        currentPlaneIndex += 1;

        if (currentPlaneIndex % 3 == 0)
        {
            if (PlaneController.CurrentPlaneCount - currentPlaneIndex == 2)
            {
                PlaneController.CreatePlane(3, 0.50f);
                //PlaneController.CreatePlane(3, 1);
            }
        }
    }

    private void FixedUpdate()
    {
        bool didHit = Physics.Raycast(rayTransform.position, Vector3.down, 10);

        // Debug.DrawRay(rayTransform.position, Vector3.down, Color.red, 10f);

        if (didHit == false)
        {
            if (collectedCubeCount == 0)
            {
                Debug.Log("Oyunu Kaybettin");
                isGameOver = true;
            }
            else if (lastPlacedCoor != new Vector3(transform.position.x, 0, Mathf.RoundToInt(transform.position.z)))
            {
                collectedCubeCount -= 1;
                int lastChildIndex = StackTransform.childCount - 1;
                Transform lastChildTransform = getObjectFromStack().transform;
                lastPlacedCoor = new Vector3(transform.position.x, 0, Mathf.RoundToInt(transform.position.z));
                lastChildTransform.position = lastPlacedCoor;

            }

        }
    }
    private GameObject getObject()
    {
        int totalFree = pooledCollectedCubeFree.Count;
        if (totalFree == 0)
        {
            if (pooledCollectedCubeUsed.Count < 3)
            {
                generateCubes();
                return getObject();
            }
            var lastusedcube = pooledCollectedCubeUsed[0];
            pooledCollectedCubeUsed.Remove(lastusedcube);
            pooledCollectedCubeFree.Add(lastusedcube);

            return getObject();
        }
        GameObject g = pooledCollectedCubeFree[totalFree - 1];
        pooledCollectedCubeFree.RemoveAt(totalFree - 1);
        g.SetActive(true);
        return g;
    }
    private GameObject getObjectFromStack()
    {
        int totalFree = pooledCollectedCubeFree.Count;
        if (StackTransform.childCount > 0)
        {
            var g = StackTransform.GetChild(StackTransform.childCount - 1).gameObject;
            pooledCollectedCubeUsed.Add(g);
            g.transform.SetParent(null);
            return g;

        }
        return getObject();
    }
    private void generateCubes()
    {
        GameObject g = Instantiate(collectedCubePrefab);
        g.transform.parent = transform;
        g.SetActive(false);
        pooledCollectedCubeFree.Add(g);

    }

    private void OnTriggerEnter(Collider other)
    {
        CollectableCube collectableCube = other.GetComponent<CollectableCube>();

        if (collectableCube != null)
        {
            other.gameObject.SetActive(false);

            Vector3 nextStackPosition = StackTransform.position + Vector3.up * collectedCubeCount;
            var collectedCube = getObject();
            collectedCube.transform.SetParent(StackTransform);
            collectedCube.transform.position = nextStackPosition;
            collectedCubeCount += 1;
        }
    }
}
