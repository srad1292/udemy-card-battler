using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour
{
    public CardSO cardSO;

    public bool isPlayer;

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

    public CardPlacePoint assignedPoint;

    public Animator animator;

    private void Start() {
        if(targetPoint == Vector3.zero) { 
            targetPoint = transform.position;
            targetRotation = transform.rotation;
        }


        SetupCard();

        handController = FindObjectOfType<HandController>();
        myCollider = GetComponent<Collider>();
    }

    public void SetupCard() {
        attackPower = cardSO.attackPower;
        currentHealth = cardSO.currentHealth;
        manaCost = cardSO.manaCost;

        UpdateCardDisplay();

        nameText.SetText(cardSO.cardName);
        actionText.SetText(cardSO.cardAction);
        loreText.SetText(cardSO.cardLore);

        characterArt.sprite = cardSO.characterSprite;
        backgroundArt.sprite = cardSO.backgroundSprite;

    }

    private void Update() {
        transform.position = Vector3.Lerp(transform.position, targetPoint, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        if(isSelected && BattleController.Instance.battleEnded == false && Time.timeScale != 0f) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, desktopLayer) && isPlayer) {
                MoveToPoint(hit.point + new Vector3(0,2f,0), Quaternion.identity);
            }

            if(Input.GetMouseButtonDown(1) && isPlayer && BattleController.Instance.battleEnded == false) {
                ReturnToHand();
            }

            // OnMouseDown picks up card
            // Input in Update fires after OnMouseDown
            // Need to toggle justPressed 
            // Which will allow the first click to pick it up without returning to hand
            if(Input.GetMouseButtonDown(0) && !justPressed && isPlayer && BattleController.Instance.battleEnded == false) {
                if(Physics.Raycast(ray, out hit, 100f, placementLayer) && BattleController.Instance.currentPhase == BattleController.TurnOrder.PlayerActive) {
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
        AudioManager.Instance.PlaySFX(AudioManager.SfxTrack.CardPlace);

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
        if(inHand && isPlayer && BattleController.Instance.battleEnded == false && Time.timeScale != 0f) {
            MoveToPoint(handController.cardPositions[handPosition] + new Vector3(0f, 1f, 0.5f), Quaternion.identity);
        }
    }

    private void OnMouseExit() {
        if (inHand && isPlayer && BattleController.Instance.battleEnded == false) {
            MoveToPoint(handController.cardPositions[handPosition], handController.minPos.rotation);
        }
    }

    private void OnMouseDown() {
        if (inHand && isPlayer && BattleController.Instance.currentPhase == BattleController.TurnOrder.PlayerActive && BattleController.Instance.battleEnded == false && Time.timeScale != 0f) {
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

    public void DamageCard(int damageAmount) {
        currentHealth -= damageAmount;
        UpdateCardDisplay();
        if (currentHealth <= 0 ) {
            AudioManager.Instance.PlaySFX(AudioManager.SfxTrack.CardDefeat);
            currentHealth = 0;
            assignedPoint.activeCard = null;
            MoveToPoint(BattleController.Instance.discardPoint.position, BattleController.Instance.discardPoint.rotation);
            animator.SetTrigger("Jump");
            Destroy(gameObject, 5f);
        } else {
            AudioManager.Instance.PlaySFX(AudioManager.SfxTrack.CardAttack);
        }

        animator.SetTrigger("Hurt");
    }

    public void UpdateCardDisplay() {
        attackPowerText.SetText(attackPower.ToString());
        currentHealthText.SetText(currentHealth.ToString());
        manaCostText.SetText(manaCost.ToString());
    }
    

}
