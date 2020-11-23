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
            //SongPlayer.Instance.Replay();
            SelectionWindow.Instance.Show("Test", "Testing window popup", new List<SelectionWindow.ButtonInfo>
            {
                //new SelectionWindow.ButtonInfo("test1 button", Test1),
                new SelectionWindow.ButtonInfo("test2 button", Test2),
                new SelectionWindow.ButtonInfo("test3 button", delegate(){ Test3(12); })
            });
        }
	}

    private void Test1()
    {
        Debug.Log("Test1");
    }
    private void Test2()
    {
        Debug.Log("Test2");
    }
    private void Test3(int a)
    {
        Debug.Log(a.ToString());
    }
}
