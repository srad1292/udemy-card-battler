using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public TMP_Text playerManaText;
    public TMP_Text playerHealthText;
    public TMP_Text enemyHealthText;


    public GameObject manaWarning;
    public float manaWarningTime;
    private float manaWarningCounter;

    public GameObject drawCardButton;
    public GameObject endTurnButton;


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
}
