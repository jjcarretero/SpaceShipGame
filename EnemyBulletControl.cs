using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletControl : MonoBehaviour
{

	//Variables
	public float Speed = 15;
	public int Damage;

	/* These Variables are only for the Boss */
	public bool Missile = false;
	public GameObject Target;
	private Vector2 TargDet;
	public enum BulletState
	{
		POINT, ATTACK,
	}
	public BulletState State;

	void Update()
	{
		MissileLauncher();
	}

	//Destroy Bullet when out of Screen
	private void OnBecameInvisible()
	{
		Destroy(gameObject);
	}

	//Destroy Bullet if hits the Player
	private void OnTriggerEnter2D(Collider2D Col)
	{
		if (Col.tag == "Player")
			Destroy(gameObject);
	}

	//If the Bullet is a Missile, use this to change State
	private void ChangeState(BulletState NewState)
	{
		State = NewState;
	}

	//Only for Boss. He has two types of bullets: Normal (moves left), Missiles (Aim for Player and moves left)
	private void MissileLauncher()
	{
		if (Missile == false)
			transform.Translate(Vector2.right * Speed * Time.deltaTime);

		else
		{
			TargDet = new Vector2(transform.position.x, Target.transform.position.y);
			switch (State)
			{
				case BulletState.POINT:
					transform.position = Vector2.MoveTowards(transform.position, TargDet, Speed * Time.deltaTime);

					if (transform.position.y == TargDet.y)
						ChangeState(BulletState.ATTACK);
					break;

				case BulletState.ATTACK:
					transform.Translate(Vector2.right * Speed * 1.5f * Time.deltaTime);
					break;
			}
		}
	}
}
