using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpController : MonoBehaviour {
    [SerializeField]
    private ResultManager results;
    [SerializeField]
    private GameObject guidingLine;
    [SerializeField]
    private GameObject speedTacho;

    [SerializeField]
    private GameObject distanceLeft;
    [SerializeField]
    private GameObject distanceHelperLeft;
    [SerializeField]
    private GameObject distanceRight;
    [SerializeField]
    private GameObject distanceHelperRight;
    
    [SerializeField]
    private GameObject complementaryAngleLeft;
    [SerializeField]
	private GameObject complementaryAngleHelperUpLeft;
	[SerializeField]
	private GameObject complementaryAngleHelperDownLeft;
    [SerializeField]
    private GameObject complementaryAngleRight;
    [SerializeField]
    private GameObject complementaryAngleHelperUpRight;
    [SerializeField]
    private GameObject complementaryAngleHelperDownRight;

    [SerializeField]
    private GameObject guidingAngle;
    [SerializeField]
    private GameObject guidingAngleWorkpiece;
    [SerializeField]
    private GameObject guidingAngleHelperLeft;
    [SerializeField]
    private GameObject guidingAngleHelperRight;

    private Vector3 speedFirstPoint;
    private bool speedSet = false;
    private bool complementaryAngleSetToRight = false;

	public bool toHigh = false;
	public bool toLow = false;
    public bool toLeft = false;
    public bool toRight = false;

    public void ChildManager(bool Rechts, bool speed, bool distance, bool complementaryAngle, bool guidingAngle)
    {
        if (!distance)
        {
            distanceLeft.SetActive(false);
            distanceRight.SetActive(false);
        }
        if (!speed)
        {
            speedTacho.SetActive(false);
        } else
        {
            speedTacho.SetActive(true);
        }
        if (!complementaryAngle)
        {
            complementaryAngleLeft.SetActive(false);
            complementaryAngleRight.SetActive(false);
        }
        if (!guidingAngle)
        {
            this.guidingAngle.SetActive(false);
        } else
        {
            this.guidingAngle.SetActive(true);
        }

        if (Rechts)
        {
            if (distance)
            {
                distanceLeft.SetActive(false);
                distanceRight.SetActive(true);
            }
            if (complementaryAngle)
            {
                complementaryAngleLeft.SetActive(false);
                complementaryAngleRight.SetActive(true);
            }
            if (!complementaryAngleSetToRight)
            {
                guidingAngleWorkpiece.transform.Rotate(Vector3.forward * 40.0f, Space.Self);
                complementaryAngleSetToRight = true;
            }
        } else
        {
            if (distance)
            {
                distanceLeft.SetActive(true);
                distanceRight.SetActive(false);
            }
            if (complementaryAngle)
            {
                complementaryAngleLeft.SetActive(true);
                complementaryAngleRight.SetActive(false);
            }
            if (complementaryAngleSetToRight)
            {
                guidingAngleWorkpiece.transform.Rotate(Vector3.forward * -40.0f, Space.Self);
                complementaryAngleSetToRight = false;
            }
        }
    }

    public void DistanceController(float distance)
    {
        Vector3 movement = new Vector3(0.0f, 0.0f + (distance*5.0f), 0.0f);
        distanceHelperLeft.transform.localPosition = movement;
        distanceHelperRight.transform.localPosition = movement;
    }

    public void SpeedController()
    {
        int index = results.speed.Count;
        if (index == 0)
        {
            return;
        }
        Vector3 tmp = results.speed[index - 1];
        if (speedSet)
        {
            float velocity = Vector3.Distance(speedFirstPoint, tmp);
            results.velocity.Add(velocity);
            speedTacho.GetComponent<Tachometer>().InputValue = velocity * 10000.0f;
            speedSet = false;
        } else
        {
            speedFirstPoint = tmp;
            speedSet = true;
        }
    }

    public void SpeedControllerInvoker()
    {
        if (!IsInvoking("SpeedController"))
        {
            InvokeRepeating("SpeedController", 1f, 1f);
        }
        
    }
    public void SpeedControllerCancelInvoker()
    {
        if (IsInvoking("SpeedController"))
        {
            CancelInvoke("SpeedController");
            speedTacho.GetComponent<Tachometer>().SetToZero();
        }
    }

	public void ComplementaryAngleController()
	{
        if (toHigh)
        {
            complementaryAngleHelperDownLeft.SetActive(true);
            complementaryAngleHelperUpLeft.SetActive(false);
            complementaryAngleHelperDownRight.SetActive(true);
            complementaryAngleHelperUpRight.SetActive(false);
            results.toHigh += Time.deltaTime;
            return;
        }
        else if (toLow)
        {
            complementaryAngleHelperUpLeft.SetActive(true);
            complementaryAngleHelperDownLeft.SetActive(false);
            complementaryAngleHelperUpRight.SetActive(true);
            complementaryAngleHelperDownRight.SetActive(false);
            results.toLow += Time.deltaTime;
            return;
        }
        else
        {
            complementaryAngleHelperDownLeft.SetActive(false);
            complementaryAngleHelperUpLeft.SetActive(false);
            complementaryAngleHelperDownRight.SetActive(false);
            complementaryAngleHelperUpRight.SetActive(false);
            results.justRightComplementaryAngle += Time.deltaTime;
            return;
        }
    }

    public void GuidingAngleController()
    {
        if (toLeft)
        {
            guidingAngleHelperLeft.SetActive(false);
            guidingAngleHelperRight.SetActive(true);
            results.toLeft += Time.deltaTime;
            return;
        }
        else if (toRight)
        {
            guidingAngleHelperLeft.SetActive(true);
            guidingAngleHelperRight.SetActive(false);
            results.toRight += Time.deltaTime;
            return;
        }
        else
        {
            guidingAngleHelperLeft.SetActive(false);
            guidingAngleHelperRight.SetActive(false);
            results.justRightGuidingAngle += Time.deltaTime;
            return;
        }
    }

    public void GuidingLineControllerLeft(float z)
    {
        int firstIndex = 0;
        int lastIndex = guidingLine.GetComponent<LineRenderer>().positionCount - 1;
        Vector3 topLeft = guidingLine.GetComponent<LineRenderer>().GetPosition(firstIndex);
        Vector3 botLeft = guidingLine.GetComponent<LineRenderer>().GetPosition(lastIndex);
        guidingLine.GetComponent<LineRenderer>().SetPosition(firstIndex, new Vector3(topLeft.x, topLeft.y, guidingLine.GetComponentInParent<Transform>().position.z - z));
        guidingLine.GetComponent<LineRenderer>().SetPosition(lastIndex, new Vector3(botLeft.x, botLeft.y, guidingLine.GetComponentInParent<Transform>().position.z - z));
    }
    public void GuidingLineControllerRight(float z)
    {
        int firstIndex = 0+1;
        int lastIndex = guidingLine.GetComponent<LineRenderer>().positionCount - 2;
        Vector3 topLeft = guidingLine.GetComponent<LineRenderer>().GetPosition(firstIndex);
        Vector3 botLeft = guidingLine.GetComponent<LineRenderer>().GetPosition(lastIndex);
        guidingLine.GetComponent<LineRenderer>().SetPosition(firstIndex, new Vector3(topLeft.x, topLeft.y, guidingLine.GetComponentInParent<Transform>().position.z - z));
        guidingLine.GetComponent<LineRenderer>().SetPosition(lastIndex, new Vector3(botLeft.x, botLeft.y, guidingLine.GetComponentInParent<Transform>().position.z - z));
    }

    public void GuidingAngleControllerMovement(float z)
    {
        guidingAngleWorkpiece.transform.position = new Vector3(guidingAngleWorkpiece.transform.position.x, guidingAngleWorkpiece.transform.position.y, z);
    }
}
