using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
public enum Directions : int {
    up = 0,
    left = 1,
    down = 2,
    right = 3
}
public static class DirectionsExtensions {
    public static bool CheckOpposite(this Directions direction1, Directions direction2) => Mathf.Abs(direction1-direction2) == 2;
}
public class PlayerController : MonoBehaviour
{
    private Directions currentDirection;
    private Directions queuedDirection;
    public static readonly Vector3[] directions = new Vector3[4] {
        Vector3.up,
        Vector3.left,
        Vector3.down,
        Vector3.right
    };
    private Vector3 lastSectionPos;
    private List<GameObject> snakeSections = new();
    public GameObject sectionPrefab;
    public FoodSpawner foodSpawner;
    private void Awake() {
        InvokeRepeating("Movement",1,0.06f);
        snakeSections.Add(this.gameObject);
        foodSpawner = FindObjectOfType<FoodSpawner>();
        lastSectionPos = this.transform.position;
        CreateNewSection();
    }
    public void MovementInput (CallbackContext context) {
        if(context.started) {
            queuedDirection = context.control.path switch {
                "/Keyboard/w" => Directions.up,
                "/Keyboard/a" => Directions.left,
                "/Keyboard/s" => Directions.down,
                "/Keyboard/d" => Directions.right,
                _ => throw new System.ArgumentException()
            };
        }
    }
    private void Movement() {
        getQueuedDirection();
        for (int i = snakeSections.Count-1; i >= 0; i--) {
            GameObject section = snakeSections[i];
            if(i == snakeSections.Count-1)
                lastSectionPos = snakeSections[i].transform.position;
            if(i != 0)
                section.transform.position = snakeSections[i-1].transform.position;
            else
                transform.position += directions[(int)currentDirection];
            if(transform.position == snakeSections[i].transform.position && i != 0 && i != 1)
                Debug.Log("Game Over! Ran into self!");
            foodSpawner.validCoords.Remove(new Vector2((int)snakeSections[i].transform.position.x,(int)snakeSections[i].transform.position.y));
            if(!foodSpawner.validCoords.Contains(lastSectionPos)) {
                foodSpawner.validCoords.Add(lastSectionPos);
            }
            
        }
        if(transform.position == foodSpawner.foodLocation) {
            Destroy(foodSpawner.food);
            foodSpawner.SpawnFood();
            CreateNewSection();
        }
        
    }
    private void getQueuedDirection() {
        if (currentDirection.CheckOpposite(queuedDirection))
            return;
        currentDirection = queuedDirection;
        
        
    }
    private void CreateNewSection() {
        GameObject newSection = Instantiate(sectionPrefab, lastSectionPos, Quaternion.identity);
        snakeSections.Add(newSection);
    }
    

}
