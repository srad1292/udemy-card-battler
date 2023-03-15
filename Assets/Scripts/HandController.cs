using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{

    public List<Card> cardsInHand;
    public Transform minPos;
    public Transform maxPos;
    public List<Vector3> cardPositions = new List<Vector3>();


    public static HandController Instance;

    private void Awake() {
        if (Instance != null && Instance == this) {
            Destroy(this);
        }
        else {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetCardPositionsInHand();
    }



    public void SetCardPositionsInHand() {
        cardPositions.Clear();

        Vector3 distanceBetweenPoints = Vector3.zero;
        if(cardsInHand.Count > 1) {
            distanceBetweenPoints = (maxPos.position - minPos.position) / (cardsInHand.Count - 1);
        }

        for(int idx = 0; idx < cardsInHand.Count; idx++) {
            cardPositions.Add(minPos.position + (idx*distanceBetweenPoints));
            cardsInHand[idx].MoveToPoint(cardPositions[idx], minPos.rotation);
            cardsInHand[idx].inHand = true;
            cardsInHand[idx].handPosition = idx;
        }
    }

    public void RemoveCardFromHand(Card cardToRemove) {
        cardsInHand.Remove(cardToRemove);
        SetCardPositionsInHand();
    }

    public void AddCardToHand(Card cardToAdd) {
        cardsInHand.Add(cardToAdd);
        SetCardPositionsInHand();
    }

    public void EmptyHand() {
        foreach(Card heldCard in cardsInHand) {
            heldCard.inHand = false;
            heldCard.MoveToPoint(BattleController.Instance.discardPoint.position, heldCard.transform.rotation);
        }
        cardsInHand.Clear();

    }
}
