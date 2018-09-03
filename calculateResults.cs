using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class calculateResults : MonoBehaviour {

    [SerializeField]
    private ResultManager results;

    [SerializeField]
    private GameObject levelPointer;

    [SerializeField]
    private Slider distanceSlider;

    [SerializeField]
    private Slider speedSlider;

    [SerializeField]
    private Slider guidingAngleSlider;

    [SerializeField]
    private Slider complementaryAngleSlider;

    [SerializeField]
    private Slider totalSlider;

    private float distanceMax = 0.019f;
    private float distanceMin = 0.007f;
    private float speedMax = 0.0f;
    private float speedMin = 0.0f;
    private float speedTolerance = 0.002f;
    private int difficultyScaling = 1;
    
    public void SetDifficultyScaling(int i)
    {
        difficultyScaling = i;
    }

    public void Calculate()
    {
        SetSpeedMaxMin();
        float resultDistance = CalculateDistance();
        float resultSpeed = CalculateSpeed();
        float resultGuidingAngle = CalculationGuidingAngle();
        float resultComplementaryAngle = CalculationComplementaryAngle();
        FillSlider(resultDistance, resultSpeed, resultGuidingAngle, resultComplementaryAngle);
    }

    private void SetSpeedMaxMin()
    {
        float temp = levelPointer.GetComponent<LevelPointer>().GetSpeedValue();
        speedMin = temp - speedTolerance;
        speedMax = temp + speedTolerance;
    }

    private float CalculateDistance()
    {
        int toHigh = 0;
        int toLow = 0;
        int justRight = 0;

        foreach (float f in results.distance)
        {
            if (f < distanceMin)
            {
                toLow += difficultyScaling;
            }
            else if (f > distanceMax)
            {
                toHigh += difficultyScaling;
            }
            else
            {
                justRight += 1;
            }
        }
        if (toHigh == 0 && toLow == 0 && justRight == 0)
        {
            return 0.0f;
        }
        float distanceResult = (100.0f / (toHigh + toLow + justRight)) * justRight;
        return distanceResult;
    }

    private float CalculateSpeed()
    {
        int toFast = 0;
        int toSlow = 0;
        int justRight = 0;

        foreach (float f in results.velocity)
        {
            if (f < speedMin)
            {
                toSlow += difficultyScaling;
            }
            else if (f > speedMax)
            {
                toFast += difficultyScaling;
            }
            else
            {
                justRight += 1;
            }
        }
        if (toFast == 0 && toSlow == 0 && justRight == 0)
        {
            return 0.0f;
        }
        float speedResult = (100.0f / (toSlow + toFast + justRight)) * justRight;
        return speedResult;
    }

    private float CalculationGuidingAngle()
    {
        float toRight = results.toRight;
        float toLeft = results.toLeft;
        float justRight = results.justRightGuidingAngle;
        if (toRight == 0 && toLeft == 0 && justRight == 0)
        {
            return 0.0f;
        }
        float guidingAngleResult = (100.0f / ((toRight * difficultyScaling) + (toLeft * difficultyScaling) + justRight)) * justRight;
        return guidingAngleResult;
    }

    private float CalculationComplementaryAngle()
    {
        float toHigh = results.toHigh;
        float toLow = results.toLow;
        float justRight = results.justRightComplementaryAngle;
        if (toHigh == 0 && toLow == 0 && justRight == 0)
        {
            return 0.0f;
        }
        float complementaryAngleResult = (100.0f / ((toHigh * difficultyScaling) + (toLow * difficultyScaling) + justRight)) * justRight;
        return complementaryAngleResult;
    }

    private void FillSlider(float distance, float speed, float guidingAngle, float complementaryAngle)
    {
        float totalResult = (distance + speed + guidingAngle + complementaryAngle) / 4.0f;

        distanceSlider.value = distance;
        speedSlider.value = speed;
        guidingAngleSlider.value = guidingAngle;
        complementaryAngleSlider.value = complementaryAngle;
        totalSlider.value = totalResult;
        results.Clear();
    }
}
