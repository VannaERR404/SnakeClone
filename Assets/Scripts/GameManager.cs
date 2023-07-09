using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;
public class GameManager : MonoBehaviour {
	public bool gameOver;
	public int maxGameAreaX;
	public int maxGameAreaY;
	[SerializeField] private GameObject gameOverUI;
	[SerializeField] private GameObject escapeMenuUI;
	[SerializeField] private GameObject settingsUI;
    private void Awake() => Time.timeScale = 1;
	public void GameOver() {
		gameOver = true;
		gameOverUI.SetActive(true);
	}

	public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

	public void ReturnToMenu() => SceneManager.LoadScene(0);

	public void QuitGame() => Application.Quit();

	public void Settings() {
		settingsUI.SetActive(true);
		escapeMenuUI.SetActive(false);
	}
	public void EscapeMenu(CallbackContext context) {
		if(context.started && !gameOver) {
			if(settingsUI.activeSelf)
				settingsUI.SetActive(false);

			bool isActive = escapeMenuUI.activeSelf;
			escapeMenuUI.SetActive(!isActive);
			Time.timeScale = isActive ? 1 : 0;
		}
	}
}
