using UnityEngine;
using UnityEngine.SceneManagement;

public class WinController : MonoBehaviour {
    public static AsyncOperation WinGame()
    {
        return SceneManager.LoadSceneAsync("win");
    }
}
