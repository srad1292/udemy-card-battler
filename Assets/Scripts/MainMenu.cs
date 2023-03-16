using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public string battleSelectScene = "BattleSelect";

    public void StartGame() {
        SceneManager.LoadScene(battleSelectScene);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
