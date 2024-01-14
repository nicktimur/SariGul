using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{

	private Transform player;
	private Player playerScript;

	public bool isFlipped = false;

    private void Start()
    {
		GameObject playerObject = GameObject.Find("PlayerKnight");
        player = playerObject.GetComponent<Transform>();
		playerScript = playerObject.GetComponent<Player>();
    }
    public void LookAtPlayer()
	{
		Vector3 flipped = transform.localScale;
		flipped.z *= -1f;

		if (transform.position.x > player.position.x && isFlipped)
		{
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = false;
		}
		else if (transform.position.x < player.position.x && !isFlipped)
		{
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = true;
		}
	}

	public void DisablePlayer()
	{
		playerScript.DisableControls();
	}

	public void EnablePlayer()
	{
		playerScript.EnableControls();
	}

}
