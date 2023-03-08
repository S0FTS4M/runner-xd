using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class GameOverCanvasManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private PlayerController player;

    public static GameOverCanvasManager instance;

    public event EventHandler RestartEvent;
    private void Awake()
    {
        instance = this;
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        this.gameObject.SetActive(false);
    }
    void Update()
    {

    }
    private void OnEnable()
    {
        ScoreText.text = "" + player.score;
    }
    public void Restart()
    {
        player.isGameOver = false;
        this.gameObject.SetActive(false);
        RestartEvent?.Invoke(this, EventArgs.Empty);
    }
}
