using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    public static DeckController Instance;

    public List<CardSO> deckToUse = new List<CardSO>();
    private List<CardSO> activeCards = new List<CardSO>();

    public Card cardToSpawn;

    public int drawCardCost = 2;

    public float waitBetweenDrawingCards = 0.25f;


    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        }
        else {
            Instance = this;
        }
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
        while(tempDeck.Count > 0) {
            int selected = Random.Range(0, tempDeck.Count);
            activeCards.Add(tempDeck[selected]);
            tempDeck.RemoveAt(selected);
        }
    }

    public void DrawCardToHand() {
        if(activeCards.Count <= 0) {
            SetupDeck();
        }

        Card newCard = Instantiate(cardToSpawn, transform.position, transform.rotation);
        newCard.cardSO = activeCards[0];
        newCard.SetupCard();

        activeCards.RemoveAt(0);

        HandController.Instance.AddCardToHand(newCard);
        AudioManager.Instance.PlaySFX(AudioManager.SfxTrack.CardDraw);

    }


    public void DrawCardWithMana() {
        if (BattleController.Instance.playerMana < drawCardCost) {
            UIController.Instance.ShowManaWarning();
            UIController.Instance.HideDrawButton();
        }
        else {
            DrawCardToHand();
            BattleController.Instance.SpendPlayerMana(drawCardCost);
        }
    }

    public void DrawMultipleCards(int amountToDraw) {
        StartCoroutine(DrawMultipleCo(amountToDraw));
    }

    IEnumerator DrawMultipleCo(int amountToDraw) {
        for (int idx = 0; idx < amountToDraw; idx++) {
            DrawCardToHand();
            yield return new WaitForSeconds(waitBetweenDrawingCards);

        }
    }
}
