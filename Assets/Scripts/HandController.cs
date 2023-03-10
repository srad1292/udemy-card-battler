using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{

    public List<Card> cardsInHand;
    public Transform minPos;
    public Transform maxPos;
    public List<Vector3> cardPositions = new List<Vector3>();

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
}
