using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
	public float MaxScore;
	public Text ScoreText;
	//We need maxScore to show it on the HighScore Scene. So we load this info at the start of the app
	void Start()
	{
		GetMaxScore();
	}

	void Update()
	{
		if (!ScoreText)
			return;
		ScoreText.text = MaxScore.ToString();
	}

	//Starts the game, there is no Level selection screen
	public void ToGame()
	{
		Debug.Log("Loading Game...");
		SceneManager.LoadScene("01_Game");
	}

	//Show the Max score saved on PlayerPref
	public void ToHighScores()
	{
		Debug.Log("Loading High Scores...");
		SceneManager.LoadScene("02_HighScore");
	}

	//Show instructions and game controls
	public void ToControls()
	{
		Debug.Log("Loading Controls...");
		SceneManager.LoadScene("03_Controls");
	}

	public void ExitGame()
	{
		Debug.Log("Quitting game...");
		Application.Quit();
	}

	//Used to come back to menu from the game scene
	public void IntroMenu()
	{
		Debug.Log("Loading Menu...");
		SceneManager.LoadScene("00_Intro");
	}

	//Load saved Score at the start of the app.
	private void GetMaxScore()
	{
		if (PlayerPrefs.HasKey("SpaceShipGame_MaxScore") == true)
			MaxScore = PlayerPrefs.GetInt("SpaceShipGame_MaxScore");
		else
			MaxScore = 0;
	}
}
