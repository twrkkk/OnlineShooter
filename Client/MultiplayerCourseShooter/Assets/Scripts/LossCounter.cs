using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LossCounter : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    private int _playerLoss;
    private int _enemyLoss;

    public void SetPlayerLoss(int value)
    {
        _playerLoss = value;
        UpdateText();
    }

    public void SetEnemyLoss(int value)
    {
        _enemyLoss = value;
        UpdateText();
    }

    private void UpdateText()
    {
        _scoreText.text = $"{_enemyLoss} : {_playerLoss}";
    }
}
