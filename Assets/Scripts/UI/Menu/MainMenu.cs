using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void OnNewGame()
    {
        Debug.Log("OnNewGame");
    }

    public void OnContinue()
    {
        Debug.Log("OnContinue");
    }

    public void OnOptions()
    {
        Debug.Log("OnOptions");
    }

    public void OnQuit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
