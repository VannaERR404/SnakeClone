using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int maxGameAreaX;
    public int maxGameAreaY;
    private void Awake() {
        if((maxGameAreaX + maxGameAreaY) % 2 != 0) {
            Debug.LogError("Game area cannot be even number!");
            return;
        }
    }
}
