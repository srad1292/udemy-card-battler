using System.Collections;
using UnityEngine;

public class CardPointsController : MonoBehaviour
{

    public static CardPointsController Instance;

    public CardPlacePoint[] playerCardPoints;
    public CardPlacePoint[] enemyCardPoints;

    public float timeBetweenAttacks = 0.3f;

    private void Awake() {
        if (Instance != null && Instance != this) {
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
                }
                else {
                    // Attack opponent
                    BattleController.Instance.DamageEnemy(playerCardPoints[idx].activeCard.attackPower);
                }
                playerCardPoints[idx].activeCard.animator.SetTrigger("Attack");
                yield return new WaitForSeconds(timeBetweenAttacks);

            }

            if (BattleController.Instance.battleEnded) {
                idx = playerCardPoints.Length;
            }
        }

        CheckAssignedCards();
        BattleController.Instance.AdvanceTurn();
    }

    public void EnemyAttack() {
        StartCoroutine(EnemyAttackCo());
    }


    IEnumerator EnemyAttackCo() {
        yield return new WaitForSeconds(timeBetweenAttacks);

        for (int idx = 0; idx < enemyCardPoints.Length; idx++) {
            if (enemyCardPoints[idx].activeCard != null) {
                if (playerCardPoints[idx].activeCard != null) {
                    // Attack Player card
                    playerCardPoints[idx].activeCard.DamageCard(enemyCardPoints[idx].activeCard.attackPower);

                }
                else {
                    // Attack Player
                    BattleController.Instance.DamagePlayer(enemyCardPoints[idx].activeCard.attackPower);
                }
                enemyCardPoints[idx].activeCard.animator.SetTrigger("Attack");
                yield return new WaitForSeconds(timeBetweenAttacks);

            }

            if (BattleController.Instance.battleEnded) {
                idx = enemyCardPoints.Length;
            }
        }

        CheckAssignedCards();
        BattleController.Instance.AdvanceTurn();
    }

    public void CheckAssignedCards() {
        foreach(CardPlacePoint point in playerCardPoints) {
            if (point.activeCard != null) {
                if (point.activeCard.currentHealth <= 0) {
                    point.activeCard = null;
                }
            }
        }

        foreach (CardPlacePoint point in enemyCardPoints) {
            if(point.activeCard != null) {
                if(point.activeCard.currentHealth <= 0) {
                    point.activeCard = null;
                }
            }
        }
    }
    
}
