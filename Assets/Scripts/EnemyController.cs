using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public List<CardSO> deckToUse = new List<CardSO>();
    private List<CardSO> activeCards = new List<CardSO>();

    public Card cardToSpawn;
    public Transform cardSpawnPoint;

    public enum AIType { placeFromDeck, handRandomPlace, handDefensive, handAttacking };
    public AIType enemyAIType;

    public static EnemyController Instance;

    private void Awake() {
        if(Instance != null && Instance == this) {
            Destroy(this);
        }
        Instance = this;
    }

    private void Start() {
        SetupDeck();
    }

    public void SetupDeck() {
        activeCards.Clear();
        // Get a copy of deck to use
        List<CardSO> tempDeck = new List<CardSO>();
        tempDeck.AddRange(deckToUse);
        // Shuffle deck by randomly moving cards from temp deck to active deck
        while (tempDeck.Count > 0) {
            int selected = Random.Range(0, tempDeck.Count);
            activeCards.Add(tempDeck[selected]);
            tempDeck.RemoveAt(selected);
        }
    }

    public void StartAction() {
        StartCoroutine(EnemyActionCO());
    }

    IEnumerator EnemyActionCO() {
        if(activeCards.Count <= 0) {
            SetupDeck();
        }

        yield return new WaitForSeconds(0.5f);

        switch(enemyAIType) {
            case AIType.placeFromDeck:
                PlaceFromDeck();
                break;
            case AIType.handRandomPlace:
                break;
            case AIType.handDefensive:
                break;
            case AIType.handAttacking:
                break;
            default:
                break;
        }

        yield return new WaitForSeconds(0.6f);

        BattleController.Instance.AdvanceTurn();
    }

    void PlaceFromDeck() {

        CardPlacePoint selectedPoint = SelectRandomPlacePoint();
        if (selectedPoint.activeCard == null) {
            Card newCard = Instantiate(cardToSpawn, cardSpawnPoint.transform.position, cardSpawnPoint.transform.rotation);
            newCard.cardSO = activeCards[0];
            activeCards.RemoveAt(0);
            newCard.SetupCard();
            newCard.MoveToPoint(selectedPoint.transform.position, selectedPoint.transform.rotation);
            selectedPoint.activeCard = newCard;
            newCard.assignedPoint = selectedPoint;
        }
    }

    private CardPlacePoint SelectRandomPlacePoint() {
        List<CardPlacePoint> cardPoints = new List<CardPlacePoint>();
        cardPoints.AddRange(CardPointsController.Instance.enemyCardPoints);

        int randomPoint = Random.Range(0, cardPoints.Count);
        CardPlacePoint selectedPoint = cardPoints[randomPoint];

        while (selectedPoint.activeCard != null && cardPoints.Count > 0) {
            randomPoint = Random.Range(0, cardPoints.Count);
            selectedPoint = cardPoints[randomPoint];
            cardPoints.RemoveAt(randomPoint);
        }

        return selectedPoint;
    }



}
