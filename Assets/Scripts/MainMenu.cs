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
        AudioManager.Instance.PlaySFX(AudioManager.SfxTrack.ButtonPress);
    }

    public void QuitGame() {
        AudioManager.Instance.PlaySFX(AudioManager.SfxTrack.ButtonPress);
        Application.Quit();
    }
}
