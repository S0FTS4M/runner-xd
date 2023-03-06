using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Material NonTransparentCubeMat;

    enum HorizantalPos
    {
        Left=-1,
        Mid=0,
        Right=1
    }
    private HorizantalPos horizantalPos;

    private string MaxScoreString = "MaxScore";

    public GameObject collectedCubePrefab;

    public float Speed;

    public float Difficulty;

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


    private void Start()
    {
        ScoreChanged += IncreaseDifficulty;
    }

    private void OnDestroy()
    {
        ScoreChanged -= IncreaseDifficulty;
    }


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
            horizantalPos--;           
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            horizantalPos++;           
        }        
        if (Input.anyKeyDown)
        {
            CalculatePosition();
        }
        
        int currentPlaneIndex = (int)transform.position.z / 5;
        currentPlaneIndex += 1;

        if (currentPlaneIndex % 3 == 0)
        {
            if (PlaneController.CurrentPlaneCount - currentPlaneIndex == 2)
            {
                PlaneController.CreatePlane(3, 1-Difficulty);
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

                lastChildTransform.position = new Vector3(transform.position.x, 0, (int)transform.position.z+1);
                lastChildTransform.GetChild(0).GetComponent<MeshRenderer>().material = NonTransparentCubeMat;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CollectableCube collectableCube = other.GetComponent<CollectableCube>();

        if (collectableCube != null)
        {
            Destroy(other.gameObject);
            

            if (collectableCube.IsBad)
            {
                collectedCubeCount -= 1;
                int lastChildIndex = StackTransform.childCount - 1;
                Transform lastChildTransform = StackTransform.GetChild(lastChildIndex);
                Destroy(lastChildTransform.gameObject);
                
            }
            else
            {
                Vector3 nextStackPosition = StackTransform.position + Vector3.up * collectedCubeCount;
                Instantiate(collectedCubePrefab, nextStackPosition, Quaternion.identity, StackTransform);

                collectedCubeCount += 1;

                score += 10;
                ScoreChanged?.Invoke();

                
            }
            BoxCountChanged?.Invoke();


        }
    }

    private void CalculateDistance()
    {
        var difference = (int)transform.position.z - prevDist;
        if (difference == 1)
        {
            prevDist = (int)transform.position.z;
            DistanceChanged?.Invoke();
            ScoreChanged?.Invoke();
        }
    }

    private void CalculatePosition()
    {
        horizantalPos = (HorizantalPos) System.Math.Clamp((int) horizantalPos, -1, 1);
        var tempPosValue = transform.position;
        tempPosValue.x = (float) horizantalPos;
        transform.position = tempPosValue;
    }

    private void IncreaseDifficulty()
    {
        if(Difficulty<0.8f)  Difficulty += 0.005f;
    }
}
