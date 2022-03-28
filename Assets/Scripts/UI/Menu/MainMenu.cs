using Assets.Scripts.Controller.SaveSystem;
using Assets.Scripts.Entities.SaveSystem;
using Assets.Scripts.GameScene.Controller;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button ContinueButton;
    [SerializeField] private string BootSceneName;

    FileInfo[] saveFiles;

    private void Start()
    {
        if (SceneManager.GetSceneByName(BootSceneName).isLoaded)
        {

        }
        else
        {
            StartCoroutine(GetBootScene());
        }

        string path = Application.persistentDataPath + "/saves/";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        DirectoryInfo dir = new DirectoryInfo(path);
        saveFiles = dir.GetFiles("*.save");

        ContinueButton.enabled = saveFiles.Any();

    }

    private IEnumerator GetBootScene()
    {
        yield return SceneManager.LoadSceneAsync(BootSceneName, LoadSceneMode.Additive);
    }

    public void OnNewGame()
    {
        foreach (var _file in saveFiles)
        {
            File.Delete(_file.FullName);
        }

        var success = SerializationManager.Save(SaveData.Current.SaveName, SaveData.Current);

        Debug.Log($"Save succeeded? {success}");

        SceneManager.UnloadSceneAsync(GameSceneController.MainMenuScene);
        GameSceneController.Singleton.StartGame();
    }

    public void OnContinue()
    {
        Debug.Log("OnContinue");

        SceneManager.UnloadSceneAsync(GameSceneController.MainMenuScene);
        GameSceneController.Singleton.StartGame();
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
