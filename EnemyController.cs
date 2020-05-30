using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyControl : MonoBehaviour
{
	//Public Variables
	public bool b_BossBool = false;
	public float Speed, Health = 2, MaxHealth;
	public GameControl Manager;
	public GameObject Player, EnemyBullet, EnemyBulletPosition;
	public Slider slider;

	//Private Variables
	private bool b_Enemy02, b_Point01 = true;
	private float BossFireRate;
	private int EnemyScore, RandomBullet;
	private Vector3 Pos01, Pos02, BossPos;

	void Start()
	{
		//Only active if is Enemy02 (not lineal movement)
		if (gameObject.tag == "Enemy02")
			b_Enemy02 = true;
		else
			b_Enemy02 = false;

		//Random Speed and Score so each enemy is slightly different
		Speed = Random.Range(2, 8);
		EnemyScore = Random.Range(5, 11);
		//Random initial position
		Pos01 = new Vector3(transform.position.x, Random.Range(-4f, -1.5f), 0);
		Pos02 = new Vector3(transform.position.x, (Pos01.y + Random.Range(1.5f, 4f)), 0);
	}

	// Update is called once per frame
	void Update()
	{

		EnemyMovement();
		DirectionMovement();

		//Destroy Enemy when out of view
		if (transform.position.x <= Player.transform.position.x - 3)
		{
			Destroy(gameObject);
			Manager.SavedEnemies.Remove(gameObject);
		}

		//Just in case the Enemy is the Boss
		if (b_BossBool == true)
		{
			BossFireRate += Time.deltaTime;
			if (BossFireRate >= 1f)
			{
				Attack();
				BossFireRate = 0;
			}
			slider.value = Health / MaxHealth;
		}
	}

	//Bool change so the Enemy knows where it should be moving to
	public void DirectionMovement()
	{
		if (transform.position.y == Pos01.y)
			b_Point01 = false;
		if (transform.position.y == Pos02.y)
			b_Point01 = true;
	}

	public void EnemyMovement()
	{
		if (b_Enemy02 == false && b_BossBool == false)
			transform.Translate(Vector2.left * Speed * Time.deltaTime);
		else if (b_Enemy02 == false && b_BossBool == true)
		{
			if (transform.position.x == Player.transform.position.x + 11)
			{
				Pos01 = new Vector3(transform.position.x, -3, 0);
				Pos02 = new Vector3(transform.position.x, 1.58f, 0);

				if (b_Point01)
					transform.position = Vector2.MoveTowards(transform.position, Pos01, Speed * Time.deltaTime);
				else
					transform.position = Vector2.MoveTowards(transform.position, Pos02, Speed * Time.deltaTime);
			}
			else
			{
				BossPos = new Vector3(Player.transform.position.x + 11, transform.position.y, transform.position.z);
				transform.position = Vector3.MoveTowards(transform.position, BossPos,Speed*Time.deltaTime);
			}
		}
		else
		{
			if (b_Point01)
				transform.position = Vector2.MoveTowards(transform.position, Pos01, Speed * Time.deltaTime);
			else
				transform.position = Vector2.MoveTowards(transform.position, Pos02, Speed * Time.deltaTime);
		}
	}

	// Hitting enemies. Damage if crashed with player. Damaged if bullet.
	private void OnTriggerEnter2D(Collider2D Col)
	{
		if (Col.tag == "Player")
			Damage(2);

		if (Col.tag == "Bullet")
		{
			Destroy(Col.gameObject);
			Damage(Col.GetComponent<BulletController>().Damage);
		}
	}

	//Attack is available for Boss, not for Enemies 01 or 02. There is 3 different attacks. Linear bullet, Triple Bullet and Target Missile.
	public void Attack()
	{
		RandomBullet = Random.Range(1, 4);

		if (RandomBullet == 1)/*Linear Bullet*/
			GameObject tmpEnBullet = (GameObject)Instantiate(EnemyBullet, EnemyBulletPosition.transform.position, Quaternion.Euler(0, 180, 0));

		if(RandomBullet == 2)/*Triple Bullet*/
		{
			GameObject tmpEnBullet = (GameObject)Instantiate(EnemyBullet, EnemyBulletPosition.transform.position, Quaternion.Euler(0, 180, 0));
			GameObject tmpEnBullet01 = (GameObject)Instantiate(EnemyBullet, EnemyBulletPosition.transform.position, Quaternion.Euler(0, 180, 30));
			GameObject tmpEnBullet02 = (GameObject)Instantiate(EnemyBullet, EnemyBulletPosition.transform.position, Quaternion.Euler(0, 180, -30));
		}
		if(RandomBullet == 3)/*Missile. Points to Player position*/
		{
			GameObject tmpEnBullet = (GameObject)Instantiate(EnemyBullet, EnemyBulletPosition.transform.position, Quaternion.Euler(0, 180, 0));
			tmpEnBullet.GetComponent<EnemyBulletControl>().Missile = true;
			tmpEnBullet.GetComponent<EnemyBulletControl>().Target = Player;
		}
	}

	//Public void called from Bullet/Player scripts
	public void Damage(int Dmg)
	{
		Health -= Dmg;
		//Destroy Enemy
		if (Health <= 0)
		{
			Manager.GetScore(EnemyScore);
			Destroy(gameObject);
			Manager.SavedEnemies.Remove(gameObject);

			//In case the Enemy is a Boss, the level is over and it should call the WinGame Void
			if(b_BossBool == true)
				Player.GetComponent<GameControl>().WinGame();
		}
	}
}