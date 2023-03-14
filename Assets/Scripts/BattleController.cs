using System;
using UnityEngine;

public class BattleController : MonoBehaviour {

    public static BattleController Instance;

    public int startingMana = 4;
    public int maxMana = 12;
    public int playerMana;
    public int startCardAmount = 5;
    public int cardsToDrawPerTurn = 1;

    public enum TurnOrder {PlayerActive, PlayerCardAttacks, EnemyActive, EnemyCardAttacks };
    public TurnOrder currentPhase;
    public Transform discardPoint;

    private int currentPlayerMaxMana;

    public int playerHealth;
    public int enemyHealth;


    private void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    private void Start() {
        currentPlayerMaxMana = startingMana;
        RefillPlayerMana();
        UIController.Instance.SetPlayerHealthText(playerHealth);
        UIController.Instance.SetEnemyHealthText(enemyHealth);

        DeckController.Instance.DrawMultipleCards(startCardAmount);
        currentPhase = TurnOrder.PlayerActive;
    }

    public void SpendPlayerMana(int amount) {
        playerMana = Math.Max(playerMana-amount, 0);
        UIController.Instance.SetPlayerManaText(playerMana);
    }

    public void RefillPlayerMana() {
        playerMana = currentPlayerMaxMana;
        UIController.Instance.SetPlayerManaText(playerMana);
    }

    public void AdvanceTurn() {
        currentPhase++;

        if((int)currentPhase >= Enum.GetValues(typeof(TurnOrder)).Length) {
            currentPhase = 0;
        }

        switch(currentPhase) {
            case TurnOrder.PlayerActive:
                TransitionToPlayerActive();
                break;
            case TurnOrder.PlayerCardAttacks:
                TransitionToPlayerAttack();
                break;
            case TurnOrder.EnemyActive:
                TransitionToEnemyActive();
                break;
            case TurnOrder.EnemyCardAttacks:
                TransitionToEnemyAttack();
                break;
            default:
                break;
        }

    }

    

    private void TransitionToPlayerActive() {
        UIController.Instance.endTurnButton.SetActive(true);
        UIController.Instance.drawCardButton.SetActive(true);
        if(currentPlayerMaxMana < maxMana) {
            currentPlayerMaxMana++;
        }
        RefillPlayerMana();
        DeckController.Instance.DrawMultipleCards(cardsToDrawPerTurn);
    }

    private void TransitionToPlayerAttack() {
        CardPointsController.Instance.PlayerAttack();
    }

    public void EndPlayerTurn() {
        UIController.Instance.endTurnButton.SetActive(false);
        UIController.Instance.drawCardButton.SetActive(false);
        AdvanceTurn();
    }

    private void TransitionToEnemyActive() {
        EnemyController.Instance.StartAction();
    }

    private void TransitionToEnemyAttack() {
        CardPointsController.Instance.EnemyAttack();
    }

    public void DamagePlayer(int damageAmount) {
        if(playerHealth > 0) {
            playerHealth = Math.Max(playerHealth - damageAmount, 0);
        
            if(playerHealth == 0) {
                // End battle
            }

            UIController.Instance.SetPlayerHealthText(playerHealth);
            UIDamageIndicator damageClone = Instantiate(UIController.Instance.playerDamage, UIController.Instance.playerDamage.transform.parent);
            damageClone.damageText.text = damageAmount.ToString();
            damageClone.gameObject.SetActive(true);
        }

    }

    public void DamageEnemy(int damageAmount) {
        if(enemyHealth > 0) {
            enemyHealth = Math.Max(enemyHealth - damageAmount, 0);
            if(enemyHealth == 0) {
                // end battle
            }

            UIController.Instance.SetEnemyHealthText(enemyHealth);
            UIDamageIndicator damageClone = Instantiate(UIController.Instance.enemyDamage, UIController.Instance.enemyDamage.transform.parent);
            damageClone.damageText.text = damageAmount.ToString();
            damageClone.gameObject.SetActive(true);
        }
    }
}
