
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private Transform player;
    [SerializeField] private float offset;
    [SerializeField] private float camSpeed;
    private float lookAhead;
    private float upward;

    // Update is called once per frame
    void Update()
    {
        lookAhead = Mathf.Lerp(lookAhead, (-offset * player.localScale.x), Time.deltaTime * camSpeed);
        upward = Mathf.Lerp(upward, (player.localScale.y * 2), Time.deltaTime * camSpeed * 2);
        transform.position = new Vector3(player.position.x + lookAhead, player.position.y + upward, transform.position.z);
    }
    
}
