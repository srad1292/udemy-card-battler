using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public TMP_Text playerManaText;
    public TMP_Text playerHealthText;
    public TMP_Text enemyHealthText;
    public TMP_Text enemyManaText;
    public TMP_Text battleResultText;


    public GameObject manaWarning;
    public float manaWarningTime;
    private float manaWarningCounter;

    public GameObject drawCardButton;
    public GameObject endTurnButton;

    public UIDamageIndicator playerDamage;
    public UIDamageIndicator enemyDamage;

    public GameObject battleEndScreen;

    public GameObject pauseMenu;

    public string mainMenuScene;
    public string battleSelectScene;


    private void Awake() {
        if(Instance != null && Instance==this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    private void Update() {
        if(manaWarningCounter > 0) {
            manaWarningCounter -= Time.deltaTime;

            if(manaWarningCounter <= 0) {
                manaWarning.SetActive(false);
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape)) {
            PauseUnpause();
        }
    }

    public void SetPlayerManaText(int manaAmount) {
        playerManaText.text = "Mana: " + manaAmount;
    }

    public void SetPlayerHealthText(int hpAmount) {
        playerHealthText.text = "P Health: " + hpAmount;
    }

    public void SetEnemyHealthText(int hpAmount) {
        enemyHealthText.text = "E Health: " + hpAmount;
    }

    public void SetEnemyManaText(int manaAmount) {
        enemyManaText.text = "Mana: " + manaAmount;
    }

    public void ShowManaWarning() {
        manaWarning.SetActive(true);
        manaWarningCounter = manaWarningTime;
    }

    public void HideDrawButton() {
        drawCardButton.SetActive(false);
    }

    public void DrawCard() {
        DeckController.Instance.DrawCardWithMana();
    }

    public void EndPlayerTurn() {
        BattleController.Instance.EndPlayerTurn();
    }

    public void GoToMainMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }

    public void RestartLevel() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ChooseNewBattle() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(battleSelectScene);
    }

    public void Resume() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void PauseUnpause() {
        bool isPaused = pauseMenu.activeSelf;
        if(isPaused) {
            Resume();
        }
        else {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
