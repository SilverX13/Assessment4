using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoad : MonoBehaviour
{
 public void loadLevel1(string SampleScene)
    {
        SceneManager.LoadScene(SampleScene);
    }
}
