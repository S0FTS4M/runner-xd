using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameUIController : MonoBehaviour
{
    public PlayerController playerController;

    public TextMeshProUGUI scoreText;

    public TextMeshProUGUI distanceText;

    public TextMeshProUGUI boxCountText;

    private void Start()
    {
        playerController.BoxCountChanged += OnBoxCountChanged;
        playerController.DistanceChanged += OnDistanceChanged;
        playerController.ScoreChanged    += OnScoreChanged;

        distanceText.SetText("Distance: " + (int)playerController.transform.position.z);
        scoreText.SetText("Score: " + (int)playerController.score);
        boxCountText.SetText("Boxes: " + playerController.collectedCubeCount);
    }

    private void OnDestroy()
    {
        playerController.BoxCountChanged -= OnBoxCountChanged;
        playerController.DistanceChanged -= OnDistanceChanged;
        playerController.ScoreChanged -= OnScoreChanged;
    }

    private void OnBoxCountChanged()
    {
        boxCountText.SetText("Boxes: " + playerController.collectedCubeCount);
    }

    private void OnDistanceChanged()
    {
        distanceText.SetText("Distance: " + (int)playerController.transform.position.z);
    }

    private void OnScoreChanged()
    {
        scoreText.SetText("Score: " + (int)playerController.score);
    }
}
