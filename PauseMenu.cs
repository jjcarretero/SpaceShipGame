using UnityEngine;
using UnityEngine.SceneManagement;

//This is an active script since the game starts until it ends. It loads the manu and pauses the game
public class PauseMenu : MonoBehaviour
{
	public static bool GameisPause = false; /*Starts in False until button is pressed*/
	public GameControl Manager;
	public GameObject pauseMenuUI; /*In game Menu Gameobject. Starts with active = false*/
	private int Score, MaxScore;

	private void Start()
	{
		MaxScore = Manager.MaxScore;
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
			PauseButton();
	}

	// Pause Button Pressed
	public void PauseButton()
	{
		pauseMenuUI.SetActive(true);
		Time.timeScale = 0f; /*Time = 0 pauses game*/
		GameisPause = true;
	}

	//Continues the game
	public void Resume()
	{
		pauseMenuUI.SetActive(false);
		Time.timeScale = 1f;
		GameisPause = false;
	}

	//To come back to main menu. Ends the current game.
	public void LoadMenu()
	{
		Debug.Log("Loading Menu...");
		SetScore();
		SceneManager.LoadScene("00_Intro");
	}

	//Set all the game to 0 and restats the current game.
	public void RestartLevel()
	{
		Debug.Log("Starting game...");
		SetScore();
		Time.timeScale = 1f; /*Remember time= 0 so the game is paused, so we need to reset time*/
		SceneManager.LoadScene("01_Game");
	}

	public void QuitGame()
	{
		Debug.Log("Quitting game...");
		SetScore();
		Application.Quit();
	}

	public void SetScore()
	{
		Score = Manager.Score;
		if (Score > MaxScore) /*Only in the case the actual score is better than saved in Playerpref*/
			PlayerPrefs.SetInt("SpaceShipGame_MaxScore", Score);
	}
}