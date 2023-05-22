using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FoodSpawner : MonoBehaviour
{
    public int maxGameAreaX;
    public int maxGameAreaY;
    public GameObject foodPrefab;
    public Vector3 foodLocation;
    public GameObject food;
    public List<Vector2> validCoords = new();
    private void Awake() {
        if(maxGameAreaX % 2 == 0|| maxGameAreaY % 2 == 0) {
            Debug.LogError("Game area cannot be even number!");
            return;
        }
        InitializeValidCoords();
        SpawnFood();
    }
    public void SpawnFood() {
        foodLocation = validCoords[Random.Range(0,validCoords.Count)];
        food = Instantiate(foodPrefab, foodLocation, Quaternion.identity);
    }

    private void InitializeValidCoords() {
        for(int i  = 0; i < maxGameAreaX*maxGameAreaY; i++) {
            int x = i/maxGameAreaY;
            int y = i % maxGameAreaY;
            validCoords.Add(new Vector2(((-maxGameAreaX+1)/2)+x,((maxGameAreaX-1)/2)-y));
        }
    }
}

