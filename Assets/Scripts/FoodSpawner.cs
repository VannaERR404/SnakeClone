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
    public PlayerController playerController;
    private List<List<Vector2>> allCoords = new();
    public List<Vector2> validCoords = new();
    private void Awake() {
        if(maxGameAreaX % 2 == 0|| maxGameAreaY % 2 == 0) {
            Debug.LogError("Game area cannot be even number!");
            return;
        }
        addXCoords();
        InitializeValidCoords();
        SpawnFood();
    }
    public void SpawnFood() {
        foodLocation = validCoords[Random.Range(0,validCoords.Count)];
        food = Instantiate(foodPrefab, foodLocation, Quaternion.identity);
    }


    private void InitializeValidCoords() {
        for (int i = 0; i < allCoords.Count; i++) {
            List<Vector2> list = allCoords[i];
            for (int i1 = 0; i1 < list.Count; i1++) {
                validCoords.Add(allCoords[i][i1]);
            }
        }
    }
    private void addXCoords() {
        for (int i = 0; i < maxGameAreaX; i++)
            allCoords.Add(addYCoords(i));
    }

    private List<Vector2> addYCoords(int xIndex) {
        List<Vector2> listToReturn = new();
        for (int i = 0; i < maxGameAreaY; i++)
            listToReturn.Add(new Vector2(((-maxGameAreaX-1)/2)+xIndex,((maxGameAreaX+1)/2)-i));
        return listToReturn;
    }
}
