using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{

    public static BattleController Instance;

    public int startingMana = 4;
    public int maxMana = 12;
    public int playerMana;
    public int startCardAmount = 5;

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
    }

    public void SpendPlayerMana(int amount) {
        playerMana = Math.Max(playerMana-amount, 0);
        UIController.Instance.SetPlayerManaText(playerMana);
    }
}
