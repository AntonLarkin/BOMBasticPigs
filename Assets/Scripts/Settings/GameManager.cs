using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [Header("Field")]
    [SerializeField] private Vector2 startPoint;
    [SerializeField] private float length;
    [SerializeField] private float width;
    [SerializeField] private Vector2 horizontalStep;
    [SerializeField] private Vector2 verticalStep;
    [SerializeField] private float closestPointSpace;
    private List<Vector2> fieldPositions;

    [SerializeField] private int maximumLifes;
    private int currentLifes;

    [SerializeField] private Seeds seed;
    private float totalSeedsQuantity;
    private float currentSeeds;

    [SerializeField] private Bomb bombPrefab;
    [SerializeField] private float numberOfBombs;
    private float currentNumberOfBombs;

    public List<Vector2> FieldPositions => fieldPositions;
    public float TotalSeedsQuantity => totalSeedsQuantity;
    public float CurrentSeeds => currentSeeds;
    public int CurrentLifes => currentLifes;
    public int MaximumLifes => maximumLifes;
    public float NumberOfBombs => numberOfBombs;
    public float CurrentNumberOfBombs => currentNumberOfBombs;


    public event Action OnBombDrop;

    private void Start()
    {
        currentNumberOfBombs = numberOfBombs;
        currentLifes = maximumLifes;
        currentSeeds = -1;
        fieldPositions = new List<Vector2>();
        FillField();
        CreateSeeds();
        StartCoroutine(OnStartCountSeeds());
    }

    public void OnBombButtonClicked()
    {
        OnBombDrop?.Invoke();
    }

    public Vector2 AttachItemToAGrid(Vector2 position)
    {
        foreach (Vector2 pos in fieldPositions)
        {
            if (Vector2.Distance(position, pos) < closestPointSpace)
            {
                return pos;
            }
        }
        return position;
    }

    public void AddSeeds(float value)
    {
        currentSeeds+=value;
    }

    public void ReduceLife()
    {
        currentLifes--;
    }

    public void CreateBomb(Vector2 position)
    {
        if (currentNumberOfBombs > 0)
        {
            Instantiate(bombPrefab, AttachItemToAGrid(position), Quaternion.identity);
            currentNumberOfBombs--;
        }
    }

    private void FillField()
    {
        Vector2 currentPosition = startPoint;

        for(int i = 0; i < length; i++)
        {
            for(int j = 0; j < width; j++)
            {
                fieldPositions.Add(currentPosition);
                currentPosition += verticalStep;
            }
            startPoint += horizontalStep;
            currentPosition = startPoint;
        }
    }

    private void CreateSeeds()
    {
        foreach (Vector2 pos in fieldPositions)
        {
            Instantiate(seed, pos, Quaternion.identity);
        }
    }

    private void CountAllSeeds()
    {
        var seeds = FindObjectsOfType<Seeds>();
        totalSeedsQuantity = seeds.Length;
    }

    private IEnumerator OnStartCountSeeds()
    {
        yield return new WaitForEndOfFrame();

        CountAllSeeds();
    }

}
