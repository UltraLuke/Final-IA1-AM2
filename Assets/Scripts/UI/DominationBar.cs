using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DominationBar : MonoBehaviour
{
    public FlagGoal flagGoal;

    Slider _slider;
    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }
    private void Start()
    {
        if(flagGoal != null && _slider != null)
        {
            _slider.minValue = flagGoal.leftValue;
            _slider.maxValue= flagGoal.rightValue;
        }
    }

    private void Update()
    {
        _slider.value = flagGoal.dominanceValue;
    }
}
