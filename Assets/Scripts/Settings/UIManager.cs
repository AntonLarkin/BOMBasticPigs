using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [Header("Score")]
    [SerializeField] private Text scoreText;

    [Header("Lifes")]
    [SerializeField] private List<GameObject> lifes;

    [Header("Bombs")]
    [SerializeField] private Text bombsText;

    private void Update()
    {
        UpdateScore();
        UpdateLifes();
        UpdateBombs();
        Debug.Log(gameManager.CurrentLifes);
    }

    private void UpdateScore()
    {
        string score = ($"{ gameManager.CurrentSeeds}/{ gameManager.TotalSeedsQuantity}");
        scoreText.text = score;
    }

    private void UpdateLifes()
    {
        if (gameManager.CurrentLifes != gameManager.MaximumLifes)
        {
            lifes[gameManager.CurrentLifes].SetActive(false);
        }
    }

    private void UpdateBombs()
    {
        string bombs = ($"{ gameManager.CurrentNumberOfBombs}/{ gameManager.NumberOfBombs}");
        bombsText.text = bombs;
    }

}
