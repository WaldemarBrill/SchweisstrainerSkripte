using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelPointer : MonoBehaviour {

    [SerializeField]
    private Text screenOutput;

    [SerializeField]
    private GameObject tachometer;

    private string lv1ScreenText = "18V 150A";
    private string lv2ScreenText = "21V 190A";
    private string lv3ScreenText = "23V 200A";
    private string lv4ScreenText = "25V 230A";
    private string lv5ScreenText = "30V 260A";
    private float speedValue = 0.004f;

    void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case "Lv1":
                screenOutput.text = lv1ScreenText;
                speedValue = 0.004f;
                break;
            case "Lv2":
                screenOutput.text = lv2ScreenText;
                speedValue = 0.0045f;
                break;
            case "Lv3":
                screenOutput.text = lv3ScreenText;
                speedValue = 0.0054f;
                break;
            case "Lv4":
                screenOutput.text = lv4ScreenText;
                speedValue = 0.006f;
                break;
            case "Lv5":
                screenOutput.text = lv5ScreenText;
                speedValue = 0.007f;
                break;
            default:
                break;
        }
        tachometer.GetComponent<Tachometer>().SetBounds(speedValue);
        tachometer.GetComponent<Tachometer>().SetToZero();
    }

    public float GetSpeedValue()
    {
        return speedValue;
    }
}
