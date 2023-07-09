using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using Clock;
public class PlayerController : MonoBehaviour {
    private Vector2 currentDirection;
    private Vector2 queuedDirection;
    private Vector2 newSectionPos;
    private GameObject lastSection;
    public GameObject sectionPrefab;
    private List<GameObject> snakeSections = new();
    public FoodSpawner foodSpawner;
    public GameManager gameManager;
    private Clock.Clock clock = new(0.1f);

    private void Awake() {
        newSectionPos = transform.position - new Vector3(0,1);
        snakeSections.Add(this.gameObject);
        foodSpawner = FindObjectOfType<FoodSpawner>();
        queuedDirection = new(0,1);
        lastSection = snakeSections.Last();
        CreateNewSection();
        clock.onTrigger = Movement;
    }

    private void Update() => clock.Update(Time.deltaTime);

    public void HorizontalInput(CallbackContext context) {
        if(context.performed) 
            queuedDirection = new(context.ReadValue<float>(),0);
    }

    public void VerticalInput(CallbackContext context) {
        if(context.performed) 
            queuedDirection = new(0,context.ReadValue<float>());
    }

    private void Movement() {
        var dir = getQueuedDirection();
        if(gameManager.gameOver) return;
        lastSection = snakeSections.Last();
        if(CheckCollision(dir)) {
            gameManager.GameOver();
            return;
        }
        Move(dir);
        CheckForFood();
    }


    private Vector3 getQueuedDirection() => queuedDirection + currentDirection != Vector2.zero ? currentDirection = queuedDirection : currentDirection;

    private void CreateNewSection() {
        snakeSections.Add(Instantiate(sectionPrefab, newSectionPos, Quaternion.identity));
        UpdateValidCoords(newSectionPos);
    }

    private void CheckForFood() {
        if(transform.position != foodSpawner.foodLocation) return;
        Destroy(foodSpawner.food);
        CreateNewSection();
        foodSpawner.SpawnFood();
        clock.interval = Mathf.Clamp(clock.interval - 0.05f,0.1f,1f);
    }

    private void ScreenWrap() {
        if(Mathf.Abs(transform.position.x) >= ((gameManager.maxGameAreaX+1)/2))
            transform.position = new Vector3(Mathf.Clamp((transform.position.x*-1),((gameManager.maxGameAreaX-1)/2)*-1,(gameManager.maxGameAreaX-1)/2),transform.position.y);
        else if(Mathf.Abs(transform.position.y) >= ((gameManager.maxGameAreaY+1)/2))
            transform.position = new Vector3(transform.position.x, Mathf.Clamp((transform.position.y*-1),((gameManager.maxGameAreaY-1)/2)*-1,(gameManager.maxGameAreaY-1)/2));
    }

    private bool CheckCollision(Vector3 dir) => snakeSections.Skip(1).Any(section => section.transform.position == transform.position + dir);

    private void Move(Vector3 dir) {
        newSectionPos = lastSection.transform.position;
        lastSection.transform.position = transform.position;
        transform.position += dir;
        ScreenWrap();
        UpdateValidCoords(transform.position, newSectionPos);
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
