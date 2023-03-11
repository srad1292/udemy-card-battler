using System;
using UnityEngine;

public class BattleController : MonoBehaviour {

    public static BattleController Instance;

    public int startingMana = 4;
    public int maxMana = 12;
    public int playerMana;
    public int startCardAmount = 5;

    public enum TurnOrder {PlayerActive, PlayerCardAttacks, EnemyActive, EnemyCardAttacks };
    public TurnOrder currentPhase;

    private void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    private void Start() {
        playerMana = startingMana;
        UIController.Instance.SetPlayerManaText(playerMana);
        DeckController.Instance.DrawMultipleCards(startCardAmount);
        currentPhase = TurnOrder.PlayerActive;
    }

    public void SpendPlayerMana(int amount) {
        playerMana = Math.Max(playerMana-amount, 0);
        UIController.Instance.SetPlayerManaText(playerMana);
    }

    public void AdvanceTurn() {
        currentPhase = (int)currentPhase >= Enum.GetValues(typeof(TurnOrder)).Length ? 0 : currentPhase + 1;

        switch(currentPhase) {
            case TurnOrder.PlayerActive:
                break;
            case TurnOrder.PlayerCardAttacks:
                break;
            case TurnOrder.EnemyActive:
                break;
            case TurnOrder.EnemyCardAttacks:
                break;
            default:
                break;
        }

    }
}
