using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSelectButton : MonoBehaviour
{

    public string levelToLoad;

    public void SelectBattle() {
        SceneManager.LoadScene(levelToLoad);
    }
}
