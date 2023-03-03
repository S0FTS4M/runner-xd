using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private string MaxScoreString = "MaxScore";

    public GameObject collectedCubePrefab;

    public float Speed;

    public Transform rayTransform;

    public PlaneController PlaneController;

    public Transform StackTransform;

    public int collectedCubeCount;

    public bool isGameOver = false;

    public float score;

    public int prevDist;



    public event System.Action BoxCountChanged;

    public event System.Action GameOver;

    public event System.Action DistanceChanged;

    public event System.Action ScoreChanged;


    // Update is called once per frame
    void Update()
    {
        if (isGameOver == true)
            return;

        Vector3 nextStep = Vector3.forward * Time.deltaTime * Speed;

        transform.position += nextStep;

        score += nextStep.z;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
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
            }
        }

        CalculateDistance();
    }

    private void FixedUpdate()
    {
        if (isGameOver == true)
            return;

        bool didHit = Physics.Raycast(rayTransform.position, Vector3.down, 10);

        // Debug.DrawRay(rayTransform.position, Vector3.down, Color.red, 10f);

        if (didHit == false)
        {
            if (collectedCubeCount == 0)
            {
                //TODO: Kaydedilmiş veri var mı
                if (PlayerPrefs.HasKey(MaxScoreString) == true)
                {
                    //TODO: kaydedilen veri varsa şu anki score daha büyük mü
                    int savedMaxScore = PlayerPrefs.GetInt(MaxScoreString);

                    if (savedMaxScore < score)
                    {
                        PlayerPrefs.SetInt(MaxScoreString, (int)score);
                    }
                }
                else
                {
                    PlayerPrefs.SetInt(MaxScoreString, (int)score);
                }


                //TODO: eğer büyükse yeni score kaydedilir

                isGameOver = true;

                GameOver?.Invoke();
            }
            else
            {
                collectedCubeCount -= 1;
                BoxCountChanged?.Invoke();

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
            Destroy(other.gameObject);

            Vector3 nextStackPosition = StackTransform.position + Vector3.up * collectedCubeCount;
            Instantiate(collectedCubePrefab, nextStackPosition, Quaternion.identity, StackTransform);

            collectedCubeCount += 1;

            score += 10;
            ScoreChanged?.Invoke();

            BoxCountChanged?.Invoke();
        }
    }

    public void CalculateDistance()
    {
        var difference = (int)transform.position.z - prevDist;
        if (difference == 1)
        {
            prevDist = (int)transform.position.z;
            DistanceChanged?.Invoke();
            ScoreChanged?.Invoke();
        }
    }
}
