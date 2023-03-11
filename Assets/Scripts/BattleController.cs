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


    private int currentPlayerMaxMana;


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
                AdvanceTurn();
                break;
            case TurnOrder.EnemyCardAttacks:
                AdvanceTurn();
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
}
