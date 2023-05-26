using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FoodSpawner : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject foodPrefab;
    public Vector3 foodLocation;
    public GameObject food;
    public List<Vector2> validCoords = new();
    private void Awake() {
        InitializeValidCoords();
        SpawnFood();
    }
    public void SpawnFood() {
        foodLocation = validCoords[Random.Range(0,validCoords.Count)];
        food = Instantiate(foodPrefab, foodLocation, Quaternion.identity);
    }

    private void InitializeValidCoords() {
        for(int i  = 0; i < gameManager.maxGameAreaX*gameManager.maxGameAreaY; i++) {
            int x = i/gameManager.maxGameAreaY;
            int y = i % gameManager.maxGameAreaY;
            validCoords.Add(new Vector2(((-gameManager.maxGameAreaX+1)/2)+x,((gameManager.maxGameAreaX-1)/2)-y));
        }
    }
}

