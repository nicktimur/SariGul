
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private Transform player;
    [SerializeField] private float offset;
    [SerializeField] private float camSpeed;
    private float lookAhead;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.position.x + lookAhead, transform.position.y, transform.position.z);
        lookAhead = Mathf.Lerp(lookAhead, (offset * player.localScale.x), Time.deltaTime * camSpeed);  }
    
}
