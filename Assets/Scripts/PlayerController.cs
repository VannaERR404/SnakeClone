using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
public class PlayerController : MonoBehaviour
{
    private Vector2 currentDirection;
    private Vector2 queuedDirection;
    private Vector2 newSectionPos;
    private GameObject lastSection;
    public GameObject sectionPrefab;
    private List<GameObject> snakeSections = new();
    public FoodSpawner foodSpawner;
    public GameManager gameManager;

    private void Awake() {
        InvokeRepeating("Movement",1,0.5f);
        newSectionPos = transform.position;
        snakeSections.Add(this.gameObject);
        foodSpawner = FindObjectOfType<FoodSpawner>();
        queuedDirection = new(0,1);
        lastSection = snakeSections.Last();
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
        lastSection = snakeSections.Last();
        Move();
        CheckForFood();
        if(CheckCollision())
            gameManager.GameOver();
    }


    private Vector3 getQueuedDirection() => transform.position + (Vector3)queuedDirection != snakeSections[1].transform.position ? currentDirection = queuedDirection : currentDirection;

    private void CreateNewSection() {
        snakeSections.Add(Instantiate(sectionPrefab, newSectionPos, Quaternion.identity));
        UpdateValidCoords(null, newSectionPos);
    }

    private void CheckForFood() {
        if(transform.position == foodSpawner.foodLocation) {
            Destroy(foodSpawner.food);
            foodSpawner.SpawnFood();
            CreateNewSection();
        }
    }

    private void ScreenWrap() {
        if(Mathf.Abs(transform.position.x) >= ((gameManager.maxGameAreaX+1)/2))
            transform.position = new Vector3(Mathf.Clamp((transform.position.x*-1),((gameManager.maxGameAreaX-1)/2)*-1,(gameManager.maxGameAreaX-1)/2),transform.position.y);
        if(Mathf.Abs(transform.position.y) >= ((gameManager.maxGameAreaY+1)/2))
            transform.position = new Vector3(transform.position.x, Mathf.Clamp((transform.position.y*-1),((gameManager.maxGameAreaY-1)/2)*-1,(gameManager.maxGameAreaY-1)/2));
    }

    private bool CheckCollision() => snakeSections.Skip(1).Any(section => section.transform.position == transform.position);

    private void Move() {
        newSectionPos = lastSection.transform.position;
        var dir = getQueuedDirection();
        lastSection.transform.position = transform.position;
        transform.position += dir;
        ScreenWrap();
        UpdateValidCoords(transform.position, lastSection.transform.position);
        snakeSections.RemoveAt(snakeSections.Count()-1);
        snakeSections.Insert(1, lastSection);
        lastSection = snakeSections.Last();
    }

    private void UpdateValidCoords(Vector3? coordToRemove = null, Vector3? coordToAdd = null) {
        if(coordToAdd != null)
            foodSpawner.validCoords.Add((Vector2)coordToAdd);
        if(coordToRemove != null)
            foodSpawner.validCoords.Remove((Vector2)coordToRemove);
    }
}
