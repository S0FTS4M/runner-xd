using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverUIController : MonoBehaviour
{
    public PlayerController playerController;

    public TextMeshProUGUI maxScoreText;

    public TextMeshProUGUI maxDistanceText;

    public GameObject GameOverPanel;

    // Start is called before the first frame update
    void Start()
    {
        playerController.GameOver += OnGameOver;
    }

    private void OnDestroy()
    {
        playerController.GameOver -= OnGameOver;
    }

    private void OnGameOver()
    {
        GameOverPanel.SetActive(true);

        int maxScore = PlayerPrefs.GetInt("MaxScore", 0);

        maxScoreText.SetText("Max Score: " + maxScore);
    }
}
