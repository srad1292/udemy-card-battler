using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour
{
    public CardSO cardSO;

    public int attackPower;
    public int currentHealth;
    public int manaCost;

    public TMP_Text nameText;
    public TMP_Text actionText;
    public TMP_Text loreText;
    public TMP_Text attackPowerText;
    public TMP_Text currentHealthText;
    public TMP_Text manaCostText;

    public Image characterArt;
    public Image backgroundArt;

    private void Start() {
        SetupCard();
    }

    public void ChangeCard(CardSO cardSO) {
        this.cardSO = cardSO;
        SetupCard();
    }

    public void SetupCard() {
        attackPower = cardSO.attackPower;
        currentHealth = cardSO.currentHealth;
        manaCost = cardSO.manaCost;

        attackPowerText.SetText(attackPower.ToString());
        currentHealthText.SetText(currentHealth.ToString());
        manaCostText.SetText(manaCost.ToString());

        nameText.SetText(cardSO.cardName);
        actionText.SetText(cardSO.cardAction);
        loreText.SetText(cardSO.cardLore);

        characterArt.sprite = cardSO.characterSprite;
        backgroundArt.sprite = cardSO.backgroundSprite;

    }

}
