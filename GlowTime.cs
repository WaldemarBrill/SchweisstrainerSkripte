using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowTime : MonoBehaviour {

    private float maxGlowFactor = 3.0f;
    private float minGlowFactor = 0.1f;

    private float currentGlowFactor;
    private float glowTime = 3.5f;

    private Color emissionColor = new Color(1.0f, 0.2f, 0, 1.0f);
    private const string emissionColorString = "_EmissionColor";

    private Material mat;

    // Use this for initialization
    void Start () {
        mat = gameObject.GetComponent<Renderer>().material;
        mat.EnableKeyword("_EMISSION");
        currentGlowFactor = maxGlowFactor;
    }


    private void FixedUpdate()
    {
        if (currentGlowFactor <= minGlowFactor)
            return;

        currentGlowFactor = currentGlowFactor - (Time.fixedDeltaTime / glowTime) * maxGlowFactor;

        mat.SetColor(emissionColorString, emissionColor * currentGlowFactor);

    }

    public void AddSomeGlow(float glowfactor)
    {
        float temp = glowfactor * (maxGlowFactor - minGlowFactor) + minGlowFactor;
        if (temp > currentGlowFactor)
            currentGlowFactor = temp;
    }
}
