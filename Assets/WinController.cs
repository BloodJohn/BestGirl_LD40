using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinController : MonoBehaviour
{
    [SerializeField] private Button screenBtn;

    public static AsyncOperation WinGame()
    {
        return SceneManager.LoadSceneAsync("win");
    }

    public static AsyncOperation LoseGame()
    {
        return SceneManager.LoadSceneAsync("lose");
    }

    private void Awake()
    {
        screenBtn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        SceneController.StartGame();
    }
}
