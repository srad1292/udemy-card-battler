using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public string battleSelectScene = "BattleSelect";

    private void Start() {
        AudioManager.Instance.PlayMenuMusic();
    }

    public void StartGame() {
        SceneManager.LoadScene(battleSelectScene);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
