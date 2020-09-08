using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AsyncSceneLoader.LoadAsyncAdditive("TestHeavyScene", this);
        }
    }
}
