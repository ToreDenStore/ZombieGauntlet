using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSoundScript : MonoBehaviour
{
    public AudioSource footstepAudioSource;

    private GameController gameController;
    private Vector3 lastPosition;
    private bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gameController.IsPaused())
        {
            if (transform.position.x != lastPosition.x
                || transform.position.z != lastPosition.z)
            {
                print(transform.position);
                print(lastPosition);
                isMoving = true;
                lastPosition = transform.position;
            }
            else
            {
                isMoving = false;
            }

            //Play footstep sound
            if (isMoving && !footstepAudioSource.isPlaying)
            {
                print(isMoving);
                print("play footstep audio");
                footstepAudioSource.Play();
            }
            if (!isMoving && footstepAudioSource.isPlaying)
            {
                footstepAudioSource.Stop();
            }
        } else
        {
            if (footstepAudioSource.isPlaying)
            {
                footstepAudioSource.Stop();
            }
        }
    }
}
