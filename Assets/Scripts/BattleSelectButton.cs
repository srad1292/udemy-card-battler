using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSelectButton : MonoBehaviour
{

    public string levelToLoad;

    private void Start() {
        AudioManager.Instance.PlayBattleSelectMusic();
    }

    public void SelectBattle() {
        SceneManager.LoadScene(levelToLoad);
    }
}
