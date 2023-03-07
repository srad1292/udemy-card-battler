using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card", order = 1)]
public class CardSO : ScriptableObject
{
    public int currentHealth;
    public int attackPower;
    public int manaCost;

    public string cardName;
    [TextArea] public string cardAction;
    [TextArea] public string cardLore;

    public Sprite characterSprite;
    public Sprite backgroundSprite;

}
