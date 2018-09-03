using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class valueTakeover : MonoBehaviour {

    private Text textfield;

    void OnEnable()
    {
        textfield = GetComponent<Text>();
    }

    public void onSliderValueChanged(float value)
    {
        textfield.text = "" + value.ToString("F0") + "%";
    }
}
