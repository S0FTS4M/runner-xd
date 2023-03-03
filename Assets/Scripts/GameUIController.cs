using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUIController : MonoBehaviour
{
    public PlayerController playerController;

    public TextMeshProUGUI scoreText;

    public TextMeshProUGUI distanceText;

    public TextMeshProUGUI boxCountText;

    private void Start()
    {
        playerController.BoxCountChanged += OnBoxCountChanged;

        distanceText.SetText("Distance: " + (int)playerController.transform.position.z);
        scoreText.SetText("Score: " + (int)playerController.score);
        boxCountText.SetText("Boxes: " + playerController.collectedCubeCount);
    }

    private void Update()
    {
        distanceText.SetText("Distance: " + (int)playerController.transform.position.z);

        scoreText.SetText("Score: " + (int)playerController.score);
    }

    private void OnDestroy()
    {
        playerController.BoxCountChanged -= OnBoxCountChanged;
    }

    private void OnBoxCountChanged()
    {
        boxCountText.SetText("Boxes: " + playerController.collectedCubeCount);
        Debug.Log("count updated");
    }
}
