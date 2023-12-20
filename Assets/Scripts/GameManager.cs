using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI currentLevelText;
    public TextMeshProUGUI pickedPowerupText;

    private int waveNumber;
    private int score;
    private string pickedPowerupDescr;

    void Start()
    {
        ResetGame();
    }

    void Update()
    {
        currentLevelText.text = "Level: " + GetCurrentWaveNumber();
        scoreText.text = "Score: " + score;
        pickedPowerupText.text = "Buff picked: " + pickedPowerupDescr;
    }

    public void ResetGame()
    {
        ResetWaveNumber();
        ResetScore();
        SetPickedPowerupDescription("-");
    }

    public void ResetWaveNumber()
    {
        waveNumber = 0;
    }

    public void IncrementWaveNumber()
    {
        waveNumber += 1;
    }

    public int GetCurrentWaveNumber()
    {
        return waveNumber;
    }

    public void IncreaseScore(int scoreToAdd)
    {
        score += scoreToAdd;
    }

    public void ResetScore()
    {
        score = 0;
    }

    public void SetPickedPowerupDescription(string description)
    {
        pickedPowerupDescr = description;
    }
}
