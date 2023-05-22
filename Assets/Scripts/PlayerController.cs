using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
public class PlayerController : MonoBehaviour
{
    private int currentDirection = 0;
    private int queuedDirection = 0;
    private Vector3[] directions = new Vector3[4];
    private Vector3 lastSectionPos;
    public List<GameObject> snakeSections = new();
    public GameObject sectionPrefab;
    public FoodSpawner foodSpawner;
    private void Awake() {
        InvokeRepeating("Movement",1,0.25f);
        directions[0] = Vector3.up;
        directions[1] = Vector3.left;
        directions[2] = Vector3.down;
        directions[3] = Vector3.right;
        snakeSections.Add(this.gameObject);
        foodSpawner = FindObjectOfType<FoodSpawner>();
        lastSectionPos = this.transform.position;
        CreateNewSection();
    }
    public void MovementInput (CallbackContext context) {
        if(context.started) {
            switch (context.control.path)
                {
                    case "/Keyboard/w":
                        queuedDirection = 0;
                        break;
                    case "/Keyboard/a":
                        queuedDirection = 1;
                        break;
                    case "/Keyboard/s":
                        queuedDirection = 2;
                        break;
                    case "/Keyboard/d":
                        queuedDirection = 3;
                        break;
                    default:
                        Debug.LogError("Unexpected input case");
                        break;
                }
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
                transform.position += directions[currentDirection];
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
        if (currentDirection == 0 && queuedDirection == 2 || currentDirection == 1 && queuedDirection == 3 || currentDirection == 2 && queuedDirection == 0 || currentDirection == 3 && queuedDirection == 1)
            return;
        currentDirection = queuedDirection;
        
        
    }
    private void CreateNewSection() {
        GameObject newSection = Instantiate(sectionPrefab, lastSectionPos, Quaternion.identity);
        snakeSections.Add(newSection);
    }
    

}
