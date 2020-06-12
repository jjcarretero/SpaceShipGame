using UnityEngine;

//Slider is the background movement script, so Player has the impress he´s moving
public class SliderControl : MonoBehaviour
{
	public Material material; /*This is the Background instance material*/
	public float ScreenSpeed = 2; /*Camera Speed*/
	public float Speed = 5; /*Background Speed*/
	public GameObject Target; /*Centered on the Target -> Player.*/
	private Vector3 FinalPos;

	void Start()
	{
		material = GetComponent<Renderer>().material;
	}

	void Update()
	{
		//As the Background is moving, we need to set an offset so the instance image moves with speed
		Vector2 offset = new Vector2(Time.time * ScreenSpeed, 0);
		material.mainTextureOffset = offset;

		if (Target.transform.parent != null)
		{
			FinalPos = new Vector3(Target.transform.position.x + 6, transform.position.y, transform.position.z);
			transform.position = Vector3.Lerp(transform.position, FinalPos, Speed * Time.deltaTime);
			if (transform.position.x < 0)
				transform.position = new Vector3(0, transform.position.y, transform.position.z);
			if (transform.position.y < 0)
				transform.position = new Vector3(transform.position.x, 0, transform.position.z);
		}
	}

	//Fixes the Double background bug so both instances works together
	private void FixedUpdate()
	{
		if (Target.transform.parent == null)
		{
			FinalPos = new Vector3(Target.transform.position.x + 6, transform.position.y, transform.position.z);
			transform.position = Vector3.Lerp(transform.position, FinalPos, Speed * Time.deltaTime);
			if (transform.position.x < 0)
				transform.position = new Vector3(0, transform.position.y, transform.position.z);
			if (transform.position.y < 0)
				transform.position = new Vector3(transform.position.x, 0, transform.position.z);
		}
	}
}
