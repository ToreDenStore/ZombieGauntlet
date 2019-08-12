using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSoundScript : MonoBehaviour
{
    public AudioSource footstepAudioSource;

    private GameController gameController;
    private Vector3 lastPosition;
    private bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameController.IsPaused())
        {
            if (Mathf.Abs(transform.position.x - lastPosition.x) > 0.02
                || Mathf.Abs(transform.position.z - lastPosition.z) > 0.02)
            {
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
