using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ÜbungEinstellungen : MonoBehaviour {

    [SerializeField]
    private GameObject weld;
    private Weld welder;

    [SerializeField]
    private GameObject helpController;
    private HelpController helper;

    private bool speed = true;
    private bool guidingAngle = true;
    private bool distance = true;
    private bool rightHanded = true;
    private bool complementaryAngle = true;

    void OnEnable()
    {
        welder = weld.GetComponent<Weld>();
        helper = helpController.GetComponent<HelpController>();
        SetHelperActive();
    }

    public void DeactivateHelper()
    {
        helper.ChildManager(rightHanded, false, false, false, false);
    }

    public void SetHelperActive()
    {
        helper.ChildManager(rightHanded, speed, distance, complementaryAngle, guidingAngle);
    }

    public void SetLeftHanded(bool wert)
    {
        welder.LeftHanded = wert;
    }

    public void SetRightHanded(bool wert)
    {
        welder.RightHanded = wert;
        rightHanded = wert;
        SetHelperActive();
    }

    public void SetElectricArc(bool wert)
    {
        welder.ElectricArc = wert;
    }

    public void SetMask(bool wert)
    {
        welder.Mask = wert;
    }

    public void SetGuidingAngle(bool wert)
    {
        guidingAngle = wert;
        SetHelperActive();
    }

    public void SetComplementaryAngle(bool wert)
    {
        complementaryAngle = wert;
        SetHelperActive();
    }

    public void SetSpeed(bool wert)
    {
        speed = wert;
        SetHelperActive();
    }

    public void SetGhosting(bool wert)
    {
    }

    public void SetDistance(bool wert)
    {
        distance = wert;
        SetHelperActive();
    }
}
