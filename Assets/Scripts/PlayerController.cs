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

    private bool isGameOver = false;

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
                // PlaneController.CreatePlane(3, 0.50f);
                PlaneController.CreatePlane(3, 1);
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
            else
            {
                collectedCubeCount -= 1;
                int lastChildIndex = StackTransform.childCount - 1;

                Transform lastChildTransform = StackTransform.GetChild(lastChildIndex);

                lastChildTransform.SetParent(null);

                lastChildTransform.position = new Vector3(transform.position.x, 0, transform.position.z);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CollectableCube collectableCube = other.GetComponent<CollectableCube>();

        if (collectableCube != null)
        {
            other.gameObject.SetActive(false);

            Vector3 nextStackPosition = StackTransform.position + Vector3.up * collectedCubeCount;
            Instantiate(collectedCubePrefab, nextStackPosition, Quaternion.identity, StackTransform);

            collectedCubeCount += 1;
        }
    }
}
