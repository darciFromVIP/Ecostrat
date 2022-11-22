using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    private TextMeshProUGUI textField;
    [SerializeField] private Color positiveColor;
    [SerializeField] private Color negativeColor;
    private Animator animator;
    private void Awake()
    {
        textField = GetComponent<TextMeshProUGUI>();
        animator = GetComponent<Animator>();
    }
    public void UpdateText(string text, bool positive, bool up)
    {
        textField.text = text;
        if (positive)
            textField.color = positiveColor;
        else
            textField.color = negativeColor;
        if (up)
            animator.SetTrigger("FloatUp");
        else
            animator.SetTrigger("FloatDown");
    }
    public void DestroyText()
    {
        Destroy(gameObject);
    }
}
