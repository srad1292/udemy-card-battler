using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleController : MonoBehaviour {

    public static BattleController Instance;

    public int startingMana = 4;
    public int maxMana = 12;
    public int playerMana;
    public int enemyMana;
    public int startCardAmount = 5;
    public int cardsToDrawPerTurn = 1;

    public enum TurnOrder {PlayerActive, PlayerCardAttacks, EnemyActive, EnemyCardAttacks };
    public TurnOrder currentPhase;
    public Transform discardPoint;

    private int currentPlayerMaxMana;
    private int currentEnemyMaxMana;

    public int playerHealth;
    public int enemyHealth;

    public bool battleEnded;

    public float resultScreenDelay = 1.2f;
    
    [Range(0f,1f)]
    public float playerFirstChance = 0.5f;

    private void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    private void Start() {
        currentPlayerMaxMana = startingMana;
        currentEnemyMaxMana = startingMana;
        RefillPlayerMana();
        RefillEnemyMana();
        UIController.Instance.SetPlayerHealthText(playerHealth);
        UIController.Instance.SetEnemyHealthText(enemyHealth);

        DeckController.Instance.DrawMultipleCards(startCardAmount);

        if(Random.value > playerFirstChance) {
            currentPhase = TurnOrder.PlayerActive;
        } else {
            currentPhase = TurnOrder.PlayerCardAttacks;
            AdvanceTurn();
        }

        AudioManager.Instance.PlaySoundtrackMusic();
    }

    public void SpendPlayerMana(int amount) {
        playerMana = Math.Max(playerMana-amount, 0);
        UIController.Instance.SetPlayerManaText(playerMana);
    }

    public void SpendEnemyMana(int amount) {
        enemyMana = Math.Max(enemyMana - amount, 0);
        UIController.Instance.SetEnemyManaText(enemyMana);
    }

    public void RefillPlayerMana() {
        playerMana = currentPlayerMaxMana;
        UIController.Instance.SetPlayerManaText(playerMana);
    }

    public void RefillEnemyMana() {
        enemyMana = currentEnemyMaxMana;
        UIController.Instance.SetEnemyManaText(enemyMana);
    }

    public void AdvanceTurn() {
        if(battleEnded) { return; }

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
        if (currentEnemyMaxMana < maxMana) {
            currentEnemyMaxMana++;
        }
        RefillEnemyMana();
        EnemyController.Instance.StartAction();
    }

    private void TransitionToEnemyAttack() {
        CardPointsController.Instance.EnemyAttack();
    }

    public void DamagePlayer(int damageAmount) {
        if(playerHealth > 0 || battleEnded == false) {
            playerHealth = Math.Max(playerHealth - damageAmount, 0);
            AudioManager.Instance.PlaySFX(AudioManager.SfxTrack.HurtPlayer);


            if (playerHealth == 0) {
                EndBattle();
            }

            UIController.Instance.SetPlayerHealthText(playerHealth);
            UIDamageIndicator damageClone = Instantiate(UIController.Instance.playerDamage, UIController.Instance.playerDamage.transform.parent);
            damageClone.damageText.text = damageAmount.ToString();
            damageClone.gameObject.SetActive(true);
        }

    }

    public void DamageEnemy(int damageAmount) {
        if (enemyHealth > 0 || battleEnded == false) {
            enemyHealth = Math.Max(enemyHealth - damageAmount, 0);
            AudioManager.Instance.PlaySFX(AudioManager.SfxTrack.HurtEnemy);

            if (enemyHealth == 0) {
                EndBattle();
            }

            UIController.Instance.SetEnemyHealthText(enemyHealth);
            UIDamageIndicator damageClone = Instantiate(UIController.Instance.enemyDamage, UIController.Instance.enemyDamage.transform.parent);
            damageClone.damageText.text = damageAmount.ToString();
            damageClone.gameObject.SetActive(true);
        }
    }

    void EndBattle() {
        battleEnded = true;
        HandController.Instance.EmptyHand();
        if(enemyHealth == 0) {
            UIController.Instance.battleResultText.text = "You Won!";
            foreach(CardPlacePoint point in CardPointsController.Instance.enemyCardPoints) {
                if(point.activeCard != null) {
                    point.activeCard.MoveToPoint(discardPoint.position, point.activeCard.transform.rotation);
                }
            }
        }
        else {
            UIController.Instance.battleResultText.text = "You Lose.";
            foreach (CardPlacePoint point in CardPointsController.Instance.playerCardPoints) {
                if (point.activeCard != null) {
                    point.activeCard.MoveToPoint(discardPoint.position, point.activeCard.transform.rotation);
                }
            }
        }
        StartCoroutine(ShowResultCO());
    }

    IEnumerator ShowResultCO() {
        yield return new WaitForSeconds(resultScreenDelay);
        UIController.Instance.battleEndScreen.SetActive(true);

    }
}
