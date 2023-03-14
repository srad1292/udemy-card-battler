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

    private List<CardSO> cardsInHand = new List<CardSO>();
    public int startHandSize;

    public static EnemyController Instance;

    public int drawCardCost = 2;

    private void Awake() {
        if(Instance != null && Instance == this) {
            Destroy(this);
        }
        Instance = this;
    }

    private void Start() {
        SetupDeck();
        if (enemyAIType != AIType.placeFromDeck) {
            SetupHand();
        }
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

    private void DrawCard() {
        if (activeCards.Count <= 0) {
            SetupDeck();
        }
        cardsInHand.Add(activeCards[0]);
        activeCards.RemoveAt(0);
    }

    private void DrawCardWithManaCost() {
        DrawCard();
        BattleController.Instance.SpendEnemyMana(drawCardCost);
    }

    public void StartAction() {
        StartCoroutine(EnemyActionCO());
    }

    IEnumerator EnemyActionCO() {
        DrawCard();

        yield return new WaitForSeconds(0.5f);

        switch(enemyAIType) {
            case AIType.placeFromDeck:
                PlaceFromDeck();
                break;
            case AIType.handRandomPlace:
                PlaceFromHandRandom();
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
            PlaceCard(activeCards[0], selectedPoint);
            activeCards.RemoveAt(0);

        }
    }

    private CardPlacePoint SelectRandomPlacePoint() {
        List<CardPlacePoint> cardPoints = new List<CardPlacePoint>();
        cardPoints.AddRange(CardPointsController.Instance.enemyCardPoints);

        int randomPoint = Random.Range(0, cardPoints.Count);
        CardPlacePoint selectedPoint = cardPoints[randomPoint];
        cardPoints.RemoveAt(randomPoint);
        while (selectedPoint.activeCard != null && cardPoints.Count > 0) {
            randomPoint = Random.Range(0, cardPoints.Count);
            selectedPoint = cardPoints[randomPoint];
            cardPoints.RemoveAt(randomPoint);
        }

        return selectedPoint;
    }

    void PlaceCard(CardSO cardSO, CardPlacePoint selectedPoint) {
        Card newCard = Instantiate(cardToSpawn, cardSpawnPoint.transform.position, cardSpawnPoint.transform.rotation);
        newCard.cardSO = cardSO;
        newCard.SetupCard();
        newCard.MoveToPoint(selectedPoint.transform.position, selectedPoint.transform.rotation);
        selectedPoint.activeCard = newCard;
        newCard.assignedPoint = selectedPoint;
        BattleController.Instance.SpendEnemyMana(cardSO.manaCost);
    }

    void PlaceFromHandRandom() {
        CardPlacePoint selectedPoint = null;
        CardSO selectedCard = null;
        bool canPlaceACard = false;
        do {
            selectedPoint = SelectRandomPlacePoint();
            selectedCard = SelectRandomCardInHand();
            canPlaceACard = selectedPoint.activeCard == null && selectedCard != null;
            if (canPlaceACard) {
                PlaceCard(selectedCard, selectedPoint);
                cardsInHand.Remove(selectedCard);
            }
        } while (canPlaceACard);

        while(BattleController.Instance.enemyMana >= drawCardCost) {
            DrawCardWithManaCost();
        }
    }

    CardSO SelectRandomCardInHand() {
        if (cardsInHand.Count > 0) {
            List<CardSO> cardsToChooseFrom = new List<CardSO>();
            cardsToChooseFrom.AddRange(cardsInHand);
            int randomCard = Random.Range(0, cardsToChooseFrom.Count);
            CardSO selectedCard = cardsToChooseFrom[randomCard];
            cardsToChooseFrom.RemoveAt(randomCard);
            while (selectedCard.manaCost > BattleController.Instance.enemyMana && cardsToChooseFrom.Count > 0) {
                randomCard = Random.Range(0, cardsToChooseFrom.Count);
                selectedCard = cardsToChooseFrom[randomCard];
                cardsToChooseFrom.RemoveAt(randomCard);
            }
            if(selectedCard.manaCost < BattleController.Instance.enemyMana) {
                return selectedCard;
            }
        }

        return null;
    }

    private void SetupHand() {
        for(int idx = 0; idx < startHandSize; idx++) {
            if (activeCards.Count <= 0) {
                SetupDeck();
            }
            cardsInHand.Add(activeCards[0]);
            activeCards.RemoveAt(0);
        }
    }



}
