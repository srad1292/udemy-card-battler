using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    public int attackPower;
    public int currentHealth;
    public int manaCost;

    public TMP_Text attackPowerText;
    public TMP_Text currentHealthText;
    public TMP_Text manaCostText;

    private void Start() {
        attackPowerText.SetText(attackPower.ToString());
        currentHealthText.SetText(currentHealth.ToString());
        manaCostText.SetText(manaCost.ToString());
    }

}
