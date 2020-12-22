using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UINumericControl : MonoBehaviour
{
    public Text textToControl;
    public int value;
    public int minLimit;
    public int maxLimit;
    public int amountInterval;
    public Button addButton;
    public Button substractButton;
    public IntEvent numericEvent;

    private void Start()
    {
        addButton.onClick.AddListener(Add);
        substractButton.onClick.AddListener(Substract);
        UpdateText();
    }
    public void Add()
    {
        value += amountInterval;
        if (value > maxLimit) value = maxLimit;
        numericEvent?.Invoke(value);
        UpdateText();
    }

    public void Substract()
    {
        value -= amountInterval;
        if (value < minLimit) value = minLimit;
        numericEvent?.Invoke(value);
        UpdateText();
    }

    void UpdateText()
    {
        textToControl.text = value.ToString();
    }
}

[System.Serializable]
public class IntEvent : UnityEvent<int> { }