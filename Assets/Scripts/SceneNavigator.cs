using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator : MonoBehaviour
{
    [Header("Navigation Settings")]
    [Tooltip("Exact name of the main menu scene as listed in Build Profiles.")]
    [SerializeField] private string menuSceneName = "menu";

    public void LoadSceneByName(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogWarning("[SceneNavigator] Target scene name is empty.");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }

    public void ReturnToMenu()
    {
        LoadSceneByName(menuSceneName);
    }

    public void QuitApplication()
    {
#if UNITY_EDITOR
        Debug.Log("[SceneNavigator] QuitApplication called in Unity Editor.");
#endif
        Application.Quit();
    }
}
