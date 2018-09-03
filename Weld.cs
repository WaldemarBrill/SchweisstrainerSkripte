using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weld : MonoBehaviour {
    [SerializeField]
    private ResultManager results;

    [SerializeField]
    private int averageCounter = 5;

    [SerializeField]
    private float wireLengthConst = 0.015f;

    [SerializeField]
    private float wireFeed = 0.001f;

    [SerializeField]
    private float wireLoss = 0.004f;

    [SerializeField]
    private GameObject wire;

    [SerializeField]
    private GameObject objectToSpawn;

    [SerializeField]
    private GameObject parentObject;

    [SerializeField]
    private float distanceBetweenSpawns = 5e-07f;

    [SerializeField]
    private float growFactor = 0.1f;

    [SerializeField]
    private float glowDistance = 0.00002f;

    [SerializeField]
    private ParticleSystem sparks;

    [SerializeField]
    private ParticleSystem lightning;

    [SerializeField]
    private ParticleSystem smoke;

    [SerializeField]
    private GameObject weldingMaskBrightness;

    [SerializeField]
    private GameObject weldingMask;

    [SerializeField]
    private GameObject helper;

    [SerializeField]
    private AudioSource audioSource;

    private bool leftHanded = false;
    private bool rightHanded = true;
    private bool electricArc = true;
    private bool mask = true;

    private HelpController helpController;
    private RaycastHit hitinfo;
    private Vector3 lastSpawnPosition;
    private GameObject lastSpawnedObject;
    private bool weldingBrightnessModus = false;
    private int counter = 0;
    private float wireLength;
    private int soundCounter = 0;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private List<Vector3> averageSpawnTransform = new List<Vector3>();

    private const string WELDING_TAG = "WeldingMaterial";

    public bool LeftHanded
    {
        get
        {
            return leftHanded;
        }
        set
        {
            leftHanded = value;
        }
    }

    public bool RightHanded
    {
        get
        {
            return rightHanded;
        }
        set
        {
            rightHanded = value;
        }
    }

    public bool ElectricArc
    {
        get
        {
            return electricArc;
        }
        set
        {
            electricArc = value;
        }
    }

    public bool Mask
    {
        get
        {
            return mask;
        }
        set
        {
            mask = value;
        }
    }

    private void Start()
    {
        helpController = helper.GetComponent<HelpController>();
        sparks.Stop();
        smoke.Stop();
        lightning.gameObject.SetActive(false);
        weldingMaskBrightness.SetActive(false);
        lastSpawnPosition = Vector3.zero;
		results.Clear ();
        wireLength = wireLengthConst;
    }

    public void ResetWorkpiece()
    {
        foreach (Transform child in parentObject.transform)
        {
            if (child.CompareTag("Weld"))
            {
                Destroy(child.gameObject);
            }
        }
        results.Clear();
        wireLength = wireLengthConst;
        spawnedObjects.Clear();
        averageSpawnTransform.Clear();
        lastSpawnPosition = Vector3.zero;
        lastSpawnedObject = null;
        counter = 0;
    }

    private IEnumerator ChangeWeldingMaskBrightness(float waitTime, bool activate)
    {
        weldingBrightnessModus = activate;

        yield return new WaitForSeconds(waitTime);

        weldingMaskBrightness.SetActive(activate);
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (!Input.GetKey(KeyCode.Space))
        {
            if (soundCounter >= 10)
            {
                helpController.SpeedControllerCancelInvoker();
                sparks.Stop();
                smoke.Stop();
                lightning.gameObject.SetActive(false);
                if (weldingMaskBrightness.activeSelf)
                    StartCoroutine(ChangeWeldingMaskBrightness(.6f, false));
                audioSource.mute = true;
                return;
            } else
            {
                soundCounter++;
                return;
            }
            
        }

        if (Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out hitinfo, wireLength))
        {            
            if (objectToSpawn == null)
                return;

            if (!hitinfo.collider.gameObject.CompareTag(WELDING_TAG))
            {
                return;
            }

            if (!weldingBrightnessModus)
            {
                StopAllCoroutines();
                if (mask)
                {
                    StartCoroutine(ChangeWeldingMaskBrightness(.1f, true));
                }
               
            }
            helpController.GuidingAngleControllerMovement(hitinfo.point.z);
            
            if ((lastSpawnPosition - hitinfo.point).sqrMagnitude > distanceBetweenSpawns)
            {
                results.distance.Add(wireLength);
                results.speed.Add(hitinfo.point);
                helpController.DistanceController(wireLengthConst - wireLength);
                helpController.ComplementaryAngleController();
                helpController.GuidingAngleController();

                if (wireLength > 0.0f)
                {
                    wireLength -= wireLoss;
                    soundCounter = 0;
                }

                if (counter >= averageCounter)
                {
                    counter = 0;
                    float x = 0.0f;
                    float y = 0.0f;
                    float z = 0.0f;
                    foreach (Vector3 v in averageSpawnTransform)
                    {
                        x += v.x;
                        y += v.y;
                        z += v.z;
                    }
                    x = x / averageSpawnTransform.Count;
                    y = y / averageSpawnTransform.Count;
                    z = z / averageSpawnTransform.Count;

                    Vector3 average = new Vector3(x, y, z);
					
					results.positions.Add (transform.position);
					results.rotations.Add (transform.rotation);
                    
                    lastSpawnedObject = Instantiate(objectToSpawn, average, Quaternion.Euler(0, 0, 0));
                    if (leftHanded)
                    {
                        helpController.GuidingLineControllerLeft(lastSpawnedObject.transform.localPosition.z);
                    }
                    else if (rightHanded)
                    {
                        helpController.GuidingLineControllerRight(lastSpawnedObject.transform.localPosition.z);
                    }
                    spawnedObjects.Add(lastSpawnedObject);
                    lastSpawnedObject.transform.parent = parentObject.transform;
                    lastSpawnPosition = lastSpawnedObject.transform.position;

                    averageSpawnTransform.Clear();
                } else
                {
                    counter++;
                    averageSpawnTransform.Add(hitinfo.point);
                }

                foreach (GameObject spawned in spawnedObjects)
                {
                    float distance = (lastSpawnedObject.transform.position - spawned.transform.position).magnitude;

                    if (distance < glowDistance)
                    {                                       
                        spawned.GetComponent<GlowTime>().AddSomeGlow((distance / glowDistance) * 0.8f);
                        spawned.gameObject.transform.localScale += new Vector3(growFactor, growFactor, growFactor);
                    }
                }
            } else
            {
                if (lastSpawnedObject != null)
                    lastSpawnedObject.transform.localScale += (new Vector3(growFactor, growFactor, growFactor) * Time.deltaTime);
            }

            sparks.transform.position = hitinfo.point - gameObject.transform.forward.normalized * 0.1f;
            smoke.transform.position = hitinfo.point;
            sparks.Play();
            smoke.Play();
            if (electricArc)
            {
                lightning.gameObject.SetActive(true);
            }
            audioSource.mute = false;
            helpController.SpeedControllerInvoker();
            
        } else
        {
            if (wireLength <= (wireLengthConst * 3.0f))
            {
                wireLength += wireFeed;
            }

            if (soundCounter >= 10 || wireLength > (wireLengthConst + 0.004f)) {

                sparks.Stop();
                smoke.Stop();
                lightning.gameObject.SetActive(false);
                StartCoroutine(ChangeWeldingMaskBrightness(.6f, false));
                audioSource.mute = true;
            } else
            {
                soundCounter++;
            }
        }
        wire.transform.localScale = new Vector3(wire.transform.localScale.x, wireLength*10, wire.transform.localScale.z);
	}
}
