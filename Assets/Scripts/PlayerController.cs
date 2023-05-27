using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
public class PlayerController : MonoBehaviour
{
    private Vector2 currentDirection;
    private Vector2 queuedDirection;
    private Vector2 lastSectionPos;
    private List<GameObject> snakeSections = new();
    public GameObject sectionPrefab;
    public FoodSpawner foodSpawner;
    public GameManager gameManager;
    private void Awake() {
        InvokeRepeating("Movement",1,0.5f);
        snakeSections.Add(this.gameObject);
        foodSpawner = FindObjectOfType<FoodSpawner>();
        lastSectionPos = this.transform.position;
        queuedDirection = new(0,1);
        CreateNewSection();
    }
    public void HorizontalInput(CallbackContext context) {
        if(context.performed) 
            queuedDirection = new(context.ReadValue<float>(),0);
    }
    public void VerticalInput(CallbackContext context) {
        if(context.performed) 
            queuedDirection = new(0,context.ReadValue<float>());
    }
    private void Movement() {
        getQueuedDirection();
        for (int i = snakeSections.Count-1; i >= 0; i--) {
            GameObject section = snakeSections[i];
            if(i == snakeSections.Count-1)
                lastSectionPos = section.transform.position;
            if(i != 0)
                section.transform.position = snakeSections[i-1].transform.position;
            else
                transform.position += (Vector3)currentDirection;
            foodSpawner.validCoords.Remove(new Vector2((int)section.transform.position.x,(int)section.transform.position.y));
            if(!foodSpawner.validCoords.Contains(lastSectionPos)) {
                foodSpawner.validCoords.Add(lastSectionPos);
            }
        }
        CheckForFood();
        ScreenWrap();
        CheckCollision();
    }
    private void getQueuedDirection() {
        if ((transform.position + (Vector3)queuedDirection) != snakeSections[1].transform.position)
            currentDirection = queuedDirection;
    }
    private void CreateNewSection() {
        GameObject newSection = Instantiate(sectionPrefab, lastSectionPos, Quaternion.identity);
        snakeSections.Add(newSection);
    }
    private void CheckForFood() {
        if(transform.position == foodSpawner.foodLocation) {
            Destroy(foodSpawner.food);
            foodSpawner.SpawnFood();
            CreateNewSection();
        }
    }
    private void ScreenWrap() {
        if(Mathf.Abs(transform.position.x) >= (gameManager.maxGameAreaX/2+1))
            transform.position = new Vector3(Mathf.Clamp((transform.position.x*-1),((gameManager.maxGameAreaX-1)/2)*-1,(gameManager.maxGameAreaX-1)/2),transform.position.y);
        if(Mathf.Abs(transform.position.y) >= (gameManager.maxGameAreaY/2+1))
            transform.position = new Vector3(transform.position.x, Mathf.Clamp((transform.position.y*-1),((gameManager.maxGameAreaY-1)/2)*-1,(gameManager.maxGameAreaY-1)/2));
    }
    
    private void CheckCollision() {
        if(snakeSections.Skip(1).Any(section => section.transform.position == transform.position))
            Debug.Log("Game Over! Ran into self!");
    }
}
