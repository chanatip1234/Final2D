using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAutoStarter
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void LoadStartScene()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            Debug.Log("Forcing Game to start from Start Menu...");
            SceneManager.LoadScene(0);
        }
    }
}
