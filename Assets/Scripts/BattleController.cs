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

    private void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    private void Start() {
        playerMana = startingMana;
    }

    public void SpendPlayerMana(int amount) {
        playerMana = Math.Max(playerMana-amount, 0);
    }
}
