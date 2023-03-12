using UnityEngine;
using TMPro;

public class UIDamageIndicator : MonoBehaviour
{
    public TMP_Text damageText;
    public float moveSpeed;
    public float lifetime = 2f;

    private RectTransform myRect;

    private void Start() {
        myRect = GetComponent<RectTransform>();
        Destroy(gameObject, lifetime);
    }

    private void Update() {
        myRect.anchoredPosition += new Vector2(0, -moveSpeed * Time.deltaTime);
    }


}
