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

    public float moveSpeed = 5f;
    public float rotateSpeed = 540f;

    public TMP_Text nameText;
    public TMP_Text actionText;
    public TMP_Text loreText;
    public TMP_Text attackPowerText;
    public TMP_Text currentHealthText;
    public TMP_Text manaCostText;

    public Image characterArt;
    public Image backgroundArt;

    private Vector3 targetPoint;
    private Quaternion targetRotation;

    public bool inHand;

    public int handPosition;

    private HandController handController;

    private void Start() {
        SetupCard();

        handController = FindObjectOfType<HandController>();
    }

    private void Update() {
        transform.position = Vector3.Lerp(transform.position, targetPoint, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    private void OnMouseOver() {
        if(inHand) {
            MoveToPoint(handController.cardPositions[handPosition] + new Vector3(0f, 1f, 0.5f), Quaternion.identity);
        }
    }

    private void OnMouseExit() {
        if (inHand) {
            MoveToPoint(handController.cardPositions[handPosition], handController.minPos.rotation);
        }
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

    public void MoveToPoint(Vector3 destination, Quaternion rotation) {
        targetPoint = destination;
        targetRotation = rotation;
    }

    

}
