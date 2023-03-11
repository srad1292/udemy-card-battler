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

    private HandController handController;
    public bool inHand;
    public int handPosition;

    private bool isSelected;
    private Collider myCollider;
    public LayerMask desktopLayer;
    public LayerMask placementLayer;

    private bool justPressed = false;

    private CardPlacePoint assignedPoint;

    private void Start() {
        SetupCard();

        handController = FindObjectOfType<HandController>();
        myCollider = GetComponent<Collider>();
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

    private void Update() {
        transform.position = Vector3.Lerp(transform.position, targetPoint, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        if(isSelected) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, desktopLayer)) {
                MoveToPoint(hit.point + new Vector3(0,2f,0), Quaternion.identity);
            }

            if(Input.GetMouseButtonDown(1)) {
                ReturnToHand();
            }

            // OnMouseDown picks up card
            // Input in Update fires after OnMouseDown
            // Need to toggle justPressed 
            // Which will allow the first click to pick it up without returning to hand
            if(Input.GetMouseButtonDown(0) && !justPressed) {
                if(Physics.Raycast(ray, out hit, 100f, placementLayer)) {
                    CardPlacePoint selectedPoint = hit.collider.GetComponent<CardPlacePoint>();
                    if(selectedPoint.activeCard == null && selectedPoint.isPlayerPoint) {
                        if(BattleController.Instance.playerMana >= manaCost) {
                            PlaceCard(selectedPoint);
                            isSelected = false;
                            RemoveFromHand();
                            BattleController.Instance.SpendPlayerMana(manaCost);
                        } else {
                            UIController.Instance.ShowManaWarning();
                            ReturnToHand();
                        }
                        
                    } else {
                        ReturnToHand();
                    }
                } else {
                    ReturnToHand();
                }
            }
        }
        justPressed = false;
    }

    private void PlaceCard(CardPlacePoint selectedPoint) {
        selectedPoint.activeCard = this;
        assignedPoint = selectedPoint;
        MoveToPoint(selectedPoint.transform.position, Quaternion.identity);
    }

    private void RemoveFromHand() {
        inHand = false;
        handController.RemoveCardFromHand(this);
    }

    private void ReturnToHand() {
        MoveToPoint(handController.cardPositions[handPosition], handController.minPos.rotation);
        isSelected = false;
        myCollider.enabled = true;
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

    private void OnMouseDown() {
        if (inHand) {
            justPressed = true;
            isSelected = true;
            myCollider.enabled = false;
        }
    }

    public void ChangeCard(CardSO cardSO) {
        this.cardSO = cardSO;
        SetupCard();
    }

    public void MoveToPoint(Vector3 destination, Quaternion rotation) {
        targetPoint = destination;
        targetRotation = rotation;
    }

    

}
