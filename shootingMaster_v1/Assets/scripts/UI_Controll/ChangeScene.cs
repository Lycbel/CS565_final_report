using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    public void GoGameScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("level1");
    }

    public void GoMenuScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("ui——design");
    }
}
