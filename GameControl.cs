using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Time Constants
const int BossCount = 60;
const int PowerUpCount = 5;

public class GameControl : MonoBehaviour
{
	//Game Variables
	public float Speed;
	public Camera Camera;
	public List<GameObject> Enemies, SavedEnemies, PowerUps;
	public bool b_FinalBoss = false, b_PowerUpSpeed = false, b_PowerUpTriple = false;
	public float PowerUpRateS, PowerUpRateT, PowerUpRespawn;
	private float startTime, elapsedTime, Seconds, Minutes, FireRate, RateEnemy01, RateEnemy02, BossCont;
	private bool b_DeadPlayer = false, b_GunReady = true;

	//Global/Player Variables
	public int Score, MaxScore;
	private int Lifes = 3, Armor = 30, RandomPowerUp;
	private Rigidbody2D Rigid;
	public GameObject Bullet;
	public Transform BulletPos01, BulletPos02;

	//UI Variables
	public GameObject Life01, Life02, Life03;
	public Image ArmorBar, ProgBar, PowerUpBar, PowerUpImg;
	public Slider ProgressSlider;
	public Text UI_Score, UI_Time;
	public Text FinalText;


	// Initial conditions
	void Start()
	{
		GetScore(0); /*Place initial score*/
		GetMaxScore(); /*If there is previous score saved*/
		startTime = Time.time;
		Time.timeScale = 1f;
		Rigid = GetComponent<Rigidbody2D>();
		ArmorBar.fillAmount = (float)Armor / 100;
		FinalText.enabled = false;
	}

	void Update()
	{
		//Time counters
		BossCont += Time.deltaTime;
		elapsedTime = Time.time - startTime;
		Minutes = (int)(elapsedTime / 60f);
		Seconds = (int)(elapsedTime % 60f);
		UI_Time.text = "Time: " + Minutes.ToString("00") + ":" + Seconds.ToString("00");

		//Explanations of these voids are on top of each one of them
		Movement();
		DirDetect();
		Attack();
		RespawnPowerUp();
		ProgressionBar();
		FireRating();
		PowerImage();

		//Only spawn enemies if Boss is not active
		if (!b_FinalBoss)
		{
			RespawnEnemy01();
			RespawnEnemy02();
		}

		//When time gets to 60 (variable on top) it spawns Boss
		if (BossCont >= BossCount)
		{
			if (!b_FinalBoss)
			{
				b_FinalBoss = true;
				CreateBoss();
			}
		}

		//When AttackSpeedIncreased Power Up is active
		if (b_PowerUpSpeed)
		{
			PowerUpRateS += Time.deltaTime;
			PowerUpBar.fillAmount = 1 - PowerUpRateS / PowerUpCount;
			{
				if (PowerUpRateS >= PowerUpCounter)
				{
					b_PowerUpSpeed = false;
					PowerUpRateS = 0;
				}
			}
		}

		//When TripleAttack Power Up is active
		if (b_PowerUpTriple)
		{
			PowerUpRateT += Time.deltaTime;
			PowerUpBar.fillAmount = 1 - PowerUpRateT / PowerUpCount;
			{
				if (PowerUpRateT >= PowerUpCount)
				{
					b_PowerUpTriple = false;
					PowerUpRateT = 0;
				}
			}
		}
	}

#region Score
	//Initial Score (int = 0) and each time Player kill an Enemy
	public void GetScore(int EnemyScore)
	{
		Score += EnemyScore;
		UI_Score.text = "Score: " + Score.ToString();
	}

	//Set previous Score saved in Player Prefs
	private void GetMaxScore()
	{
		if (PlayerPrefs.HasKey("SpaceShipGame_MaxScore"))
			MaxScore = PlayerPrefs.GetInt("SpaceShipGame_MaxScore");
		else
			MaxScore = 0;
	}
#endregion

	//When Player is Hit, UI Upload
	public void RemoveLife()
	{
		Lifes -= 1;
		if (Lifes == 2)
			Life03.GetComponent<Image>().color = Color.grey;
		else if (Lifes == 1)
			Life02.GetComponent<Image>().color = Color.grey;
		else if (Lifes <= 0)
		{
			Life01.GetComponent<Image>().color = Color.grey;
			b_DeadPlayer = true;
			if (Score > MaxScore)
				PlayerPrefs.SetInt("SpaceShipGame_MaxScore", Score);
			Invoke("ToMainMenu", 1);
		}
	}

    private void DirDetect()
    {

        if (Speed != 0)
        {
            if (Input.GetAxis("Vertical") > 0)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(2).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);
            }

            if (Input.GetAxis("Vertical") < 0)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(2).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(false);
            }

            if (Input.GetAxis("Vertical") == 0)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(2).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(false);
            }
        }


    }

    private void Movement()
    {
        Rigid.velocity = new Vector2(0, Speed * Input.GetAxis("Vertical"));

        if (transform.position.y >= 2.5f)
        {
            transform.position = new Vector3(transform.position.x, 2.5f, 0);
        }
        if (transform.position.y <= -4f)
        {
            transform.position = new Vector3(transform.position.x, -4f, 0);
        }

        if (b_FinalBoss == false)
        {
            transform.Translate(Vector2.right * 0.5f * Speed * Time.deltaTime);
        }

    }

    private void Attack()
    {

        if (Input.GetKey(KeyCode.Space) && b_GunReady == true && b_PowerUpTriple == true)
        {
            GameObject tmp01 = (GameObject)Instantiate(Bullet, BulletPos01.position, Quaternion.Euler(0, 0, 0));

            GameObject tmp02 = (GameObject)Instantiate(Bullet, BulletPos02.position, Quaternion.Euler(0, 0, 0));
            GameObject tmp03 = (GameObject)Instantiate(Bullet, BulletPos01.position, Quaternion.Euler(0, 0, 30));
            GameObject tmp04 = (GameObject)Instantiate(Bullet, BulletPos02.position, Quaternion.Euler(0, 0, -30));

            b_GunReady = false;
        }
        else if (Input.GetKey(KeyCode.Space) && b_GunReady == true && b_PowerUpTriple == false)
        {
            GameObject tmp01 = (GameObject)Instantiate(Bullet, BulletPos01.position, Quaternion.Euler(0, 0, 0));

            GameObject tmp02 = (GameObject)Instantiate(Bullet, BulletPos02.position, Quaternion.Euler(0, 0, 0));

            b_GunReady = false;
        }
    }

    public void CreateEnemy01()
    {
        GameObject NewEnemy = (GameObject)Instantiate(Enemies[0], new Vector3(transform.position.x + 16f, Random.Range(-3.5f, 3f) - 0.5f, 0), Quaternion.Euler(0, 0, 0));
        NewEnemy.GetComponent<EnemyControl>().Manager = transform.GetComponent<GameControl>();
        NewEnemy.GetComponent<EnemyControl>().Player = transform.gameObject;
        SavedEnemies.Add(NewEnemy);

    }

    public void CreateEnemy02()
    {
        GameObject NewEnemy = (GameObject)Instantiate(Enemies[1], new Vector3(transform.position.x + 16f, Random.Range(-3.5f, 3f) + 0.5f, 0), Quaternion.Euler(0, 0, 0));
        NewEnemy.GetComponent<EnemyControl>().Manager = transform.GetComponent<GameControl>();
        NewEnemy.GetComponent<EnemyControl>().Player = transform.gameObject;
        SavedEnemies.Add(NewEnemy);
    }

    public void CreateBoss()
    {
        GameObject NewEnemy = (GameObject)Instantiate(Enemies[2], new Vector3(transform.position.x + 16f, 0, 0), Quaternion.Euler(0, 0, 0));
        NewEnemy.GetComponent<EnemyControl>().Manager = transform.GetComponent<GameControl>();
        NewEnemy.GetComponent<EnemyControl>().Player = transform.gameObject;
        NewEnemy.GetComponent<EnemyControl>().BossBool = true;
        NewEnemy.GetComponent<EnemyControl>().Health = 100;
        NewEnemy.GetComponent<EnemyControl>().MaxHealth = 100;
        SavedEnemies.Add(NewEnemy);
    }

    private void RespawnEnemy01()
    {

        RateEnemy01 += Time.deltaTime;
        if (RateEnemy01 > Random.Range(2, 6))
        {
            CreateEnemy01();
            RateEnemy01 = 0;

        }


    }

    private void RespawnEnemy02()
    {

        RateEnemy02 += Time.deltaTime;
        if (RateEnemy02 > Random.Range(3, 4.5f))
        {
            CreateEnemy02();
            RateEnemy02 = 0;

        }


    }

    public void FireRating()
    {
        FireRate = FireRate + Time.deltaTime;
        if (b_PowerUpSpeed == true)
        {
            if (FireRate > 0.2f)
            {
                b_GunReady = true;
                FireRate = 0;
            }
        }
        else
        {
            if (FireRate > 0.8f)
            {
                b_GunReady = true;
                FireRate = 0;
            }
        }


    }

    private void OnTriggerEnter2D(Collider2D Col)
    {
        if (Col.tag == "Enemy")
        {
            GetDamage();
        }

        if (Col.tag == "Enemy02")
        {
            GetDamage();
        }

        if (Col.tag == "BossBullet")
        {
            GetDamage();
        }

        if (Col.tag == "Armor")
        {
            Armor = Armor + 10;
            ArmorBar.fillAmount = (float)Armor / 100;
            Destroy(Col.gameObject);
        }

        if (Col.tag == "PowerUpSpeed")
        {
            b_PowerUpSpeed = true;
            Destroy(Col.gameObject);
        }

        if (Col.tag == "PowerUpTriple")
        {
            b_PowerUpTriple = true;
            Destroy(Col.gameObject);
        }
    }

    public void GetDamage()
    {


        if (b_DeadPlayer == false && Armor == 0)
        {
            RemoveLife();
        }

        if (b_DeadPlayer == false && Armor != 0)
        {
            Armor = Armor - 10;
            ArmorBar.fillAmount = (float)Armor / 100;
        }


    }

    public void ProgressionBar()
    {
        ProgressSlider.value = ProgressSlider.value + (0.0165f * Time.deltaTime);
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void RespawnPowerUp()
    {
        PowerUpRespawn += Time.deltaTime;
        if (PowerUpRespawn > Random.Range(4, 10))
        {
            CreatePowerUp();
            PowerUpRespawn = 0;
        }
    }

    public void CreatePowerUp()
    {
        RandomPowerUp = Random.Range(1, 4);

        if (RandomPowerUp == 1)
        {
            GameObject NewPowerUp = (GameObject)Instantiate(PowerUps[0], new Vector3(transform.position.x + 16f, Random.Range(-3.5f, 3f) - 0.5f, 0), Quaternion.Euler(0, 0, 0));
        }
        if (RandomPowerUp == 2)
        {
            GameObject NewPowerUp = (GameObject)Instantiate(PowerUps[1], new Vector3(transform.position.x + 16f, Random.Range(-3.5f, 3f) - 0.5f, 0), Quaternion.Euler(0, 0, 0));
        }
        if (RandomPowerUp == 3)
        {
            GameObject NewPowerUp = (GameObject)Instantiate(PowerUps[2], new Vector3(transform.position.x + 16f, Random.Range(-3.5f, 3f) - 0.5f, 0), Quaternion.Euler(0, 0, 0));

        }
    }

    public void PowerImage()
    {
        if (b_PowerUpSpeed == true)
        {
            PowerUpImg.transform.GetChild(0).gameObject.SetActive(false);
            PowerUpImg.transform.GetChild(1).gameObject.SetActive(true);
            PowerUpImg.transform.GetChild(2).gameObject.SetActive(false);
        }
        else if (b_PowerUpTriple == true)
        {
            PowerUpImg.transform.GetChild(0).gameObject.SetActive(false);
            PowerUpImg.transform.GetChild(1).gameObject.SetActive(false);
            PowerUpImg.transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            PowerUpImg.transform.GetChild(0).gameObject.SetActive(true);
            PowerUpImg.transform.GetChild(1).gameObject.SetActive(false);
            PowerUpImg.transform.GetChild(2).gameObject.SetActive(false);
        }

    }

    public void WinGame()
    {
        if (Score > MaxScore)
        {
            PlayerPrefs.SetInt("SpaceShipGame_MaxScore", Score);
        }
        FinalText.enabled = true;
        Invoke("ToMainMenu", 4);
        Camera.GetComponent<CameraControl>().Speed = 0;
        transform.Translate(transform.position.x + 13, transform.position.y, transform.position.z);
    }
}