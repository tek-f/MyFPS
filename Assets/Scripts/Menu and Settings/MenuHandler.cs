using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    /// <summary>
    /// Loads scene according to _scene.
    /// </summary>
    /// <param name="_scene">Scene ID in the Build Settings to be loaded in.</param>
    public void LoadScene(int _scene)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(_scene);
    }
    /// <summary>
    /// Exits game in both build and editor play window.
    /// </summary>
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
