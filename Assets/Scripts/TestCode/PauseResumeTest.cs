using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseResumeTest : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SongPlayer.Instance.Pause();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            SongPlayer.Instance.UnPause();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SongPlayer.Instance.Replay();
        }
	}
}
