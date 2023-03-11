using System.Collections;
using UnityEngine;

public class CardPointsController : MonoBehaviour
{

    public static CardPointsController Instance;

    public CardPlacePoint[] playerCardPoints;
    public CardPlacePoint[] enemyCardPoints;

    public float timeBetweenAttacks = 0.3f;

    private void Awake() {
        if (Instance != null && Instance == this) {
            Destroy(this);
        }
        else {
            Instance = this;
        }
    }

    public void PlayerAttack() {
        StartCoroutine(PlayerAttackCo());
    }


    IEnumerator PlayerAttackCo() {
        yield return new WaitForSeconds(timeBetweenAttacks);

        for(int idx = 0; idx < playerCardPoints.Length; idx++) {
            if(playerCardPoints[idx].activeCard != null) {
                if (enemyCardPoints[idx].activeCard != null) {
                    // Attack enemy card
                    enemyCardPoints[idx].activeCard.DamageCard(playerCardPoints[idx].activeCard.attackPower);
                } else {
                    // Attack opponent
                }

                yield return new WaitForSeconds(timeBetweenAttacks);

            }
        }

        BattleController.Instance.AdvanceTurn();
    }
    
}
