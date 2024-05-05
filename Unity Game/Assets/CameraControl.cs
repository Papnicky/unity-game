using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Vector3 playerPosition;
    public float offset; // determines the distance of the camera from the playes when facing direction changes
    public float offsetSmoothness; // determines how fast the camera can catchup to the player
    public GameObject player;
    
    void Start()
    {
        
    }
    void Update()
    {
        playerPosition = new Vector3(player.transform.position.x, player.transform.position.y , transform.position.z);
        if (player.transform.localScale.x > 0f)
        {
            playerPosition = new Vector3(playerPosition.x + offset, playerPosition.y, playerPosition.z);
        }
        else
        {
            playerPosition = new Vector3(playerPosition.x - offset, playerPosition.y, playerPosition.z);
        }
        transform.position =  Vector3.Lerp(transform.position, playerPosition, offsetSmoothness * Time.deltaTime);
    }
}
