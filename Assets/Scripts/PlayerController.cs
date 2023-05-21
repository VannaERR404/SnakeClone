using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
public class PlayerController : MonoBehaviour
{
    private int currentDirection = 0;
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
    }
    public void MovementInput (CallbackContext context) {
        if(context.started) {
            switch (context.control.path)
                {
                    case "/Keyboard/w":
                        if(currentDirection == 2 && snakeSections.Count != 1)
                            break;
                        currentDirection = 0;
                        break;
                    case "/Keyboard/a":
                        if(currentDirection == 3 && snakeSections.Count != 1)
                            break;
                        currentDirection = 1;
                        break;
                    case "/Keyboard/s":
                        if(currentDirection == 0 && snakeSections.Count != 1)
                            break;
                        currentDirection = 2;
                        break;
                    case "/Keyboard/d":
                        if(currentDirection == 1 && snakeSections.Count != 1)
                            break;
                        currentDirection = 3;
                        break;
                    default:
                        Debug.LogError("Unexpected input case");
                        break;
                }
        }
    }
    private void Movement() {
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
    private void CreateNewSection() {
        GameObject newSection = Instantiate(sectionPrefab, lastSectionPos, Quaternion.identity);
        snakeSections.Add(newSection);
    }
    

}
