using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class angleManager : MonoBehaviour
{

    [SerializeField]
    private GameObject helper;
    private HelpController helpController;

    private void Start()
    {
        helpController = helper.GetComponent<HelpController>();
    }

    void OnTriggerStay(Collider other)
    {
        string tag = other.gameObject.tag;
        ComplementaryAngleChecker(tag);
        GuidingAngleChecker(tag);
    }

    private void ComplementaryAngleChecker(string tag)
    {
        if (tag == "AngleTop")
        {
            helpController.toLow = false;
            helpController.toHigh = true;
            return;
        }
        else if (tag == "AngleBot")
        {
            helpController.toLow = true;
            helpController.toHigh = false;
            return;
        }
        else if (tag == "AngleMidComplementary")
        {
            helpController.toLow = false;
            helpController.toHigh = false;
            return;
        }
    }

    private void GuidingAngleChecker(string tag)
    {
        if (tag == "AngleLeft")
        {
            helpController.toRight = false;
            helpController.toLeft = true;
            return;
        }
        else if (tag == "AngleRight")
        {
            helpController.toRight = true;
            helpController.toLeft = false;
            return;
        }
        else if (tag == "AngleMid")
        {
            helpController.toRight = false;
            helpController.toLeft = false;
            return;
        }
    }
}