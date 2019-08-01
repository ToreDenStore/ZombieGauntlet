using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject player;
    public float cameraXOffsetMax;
    public float cameraZOffsetMax;
    public float cameraMoveSpeedX;
    public float cameraMoveSpeedZ;

    private Vector3 offset;
    private Vector3 lastPlayerPosition;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position;
        lastPlayerPosition = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //After physics calculation and stuff
    void LateUpdate()
    {
        //Base
        //transform.position = player.transform.position + offset;

        if (player.transform.position != lastPlayerPosition)
        {
            moveCamera();
            lastPlayerPosition = player.transform.position;
        }
    }

    private void moveCamera()
    {
        bool playerIsMovingRight = (player.transform.position.x > lastPlayerPosition.x);
        bool playerIsMovingLeft = (player.transform.position.x < lastPlayerPosition.x);

        float playerPositionX = player.transform.position.x;
        float cameraPositionX = transform.position.x;

        if ((playerIsMovingRight && (cameraPositionX - playerPositionX) <= cameraXOffsetMax)
            || (playerIsMovingLeft && (cameraPositionX - playerPositionX) >= -cameraXOffsetMax))
        {
            //print("move camera to the right or left");
            transform.position = new Vector3(
                transform.position.x + (player.transform.position.x - lastPlayerPosition.x) * cameraMoveSpeedX,
                transform.position.y,
                transform.position.z
            );
        }
        else
        {
            transform.position = new Vector3(
                transform.position.x + player.transform.position.x - lastPlayerPosition.x,
                transform.position.y,
                transform.position.z
            );
        }

        bool playerIsMovingUp = (player.transform.position.z > lastPlayerPosition.z);
        bool playerIsMovingDown = (player.transform.position.z < lastPlayerPosition.z);

        float playerPositionZ = player.transform.position.z;
        float cameraPositionZ = transform.position.z;

        if ((playerIsMovingUp && (cameraPositionZ - playerPositionZ) <= cameraZOffsetMax)
            || (playerIsMovingDown && (cameraPositionZ - playerPositionZ) >= -cameraZOffsetMax))
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                transform.position.z + (player.transform.position.z - lastPlayerPosition.z) * cameraMoveSpeedZ
            );
        }
        else
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                transform.position.z + player.transform.position.z - lastPlayerPosition.z
            );
        }
    }
}
