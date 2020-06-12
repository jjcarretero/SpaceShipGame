using UnityEngine;

public class PowerUpsControl : MonoBehaviour
{
	public float Speed = 5; /*Movement Speed, could be randomrange to make it more difficult to grab*/

	//Movement
	void Update()
	{
		transform.Translate(Vector2.left * Speed * Time.deltaTime);
	}

	//Destroy if player miss grab
	private void OnBecameInvisible()
	{
		Destroy(gameObject);
	}
}
