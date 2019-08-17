using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSoundScript : MonoBehaviour
{
    public AudioSource footstepAudioSource;

    private GameController gameController;
    private Vector3 lastPosition;
    private bool isMoving;
    private float lastTime;
    private float thisTime;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        lastPosition = transform.position;
        thisTime = Time.timeSinceLevelLoad;
        lastTime = thisTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        thisTime = Time.timeSinceLevelLoad;

        if ((thisTime - lastTime) / Time.fixedDeltaTime > 10)
        {
            lastTime = thisTime;

            if (!gameController.IsPaused())
            {
                

                if (
                    //Mathf.Abs(transform.position.x - lastPosition.x) > 0.05
                    //|| Mathf.Abs(transform.position.z - lastPosition.z) > 0.05
                    Vector3.Distance(transform.position, lastPosition) > 0.2
                    )
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
                if (isMoving && !footstepAudioSource.isPlaying && Time.timeSinceLevelLoad >= 1)
                {
                    footstepAudioSource.Play();
                    print("start play footsteps");
                }
                if (!isMoving && footstepAudioSource.isPlaying)
                {
                    footstepAudioSource.Stop();
                    print("stop play footsteps");
                }
            }
            else
            {
                if (footstepAudioSource.isPlaying)
                {
                    footstepAudioSource.Stop();
                }
            }
        }
    }
}
