using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabWorkpiece : MonoBehaviour {

    private GameObject workpiece;
    private GameObject spawnedObject;

    [SerializeField]
    private GameObject spawnPlace;

    private void FixedUpdate()
    {
        if (spawnedObject != null)
        {
            if (Input.GetAxis("HTC_VIU_RightTrigger") == 1.0f)
            {
                Destroy(spawnedObject);
                workpiece = null;
            }
        }
           
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WeldingMaterial"))
        {
            if (workpiece == null)
            {
                workpiece = other.transform.parent.gameObject;
                spawnedObject = Instantiate(workpiece, spawnPlace.transform.position, spawnPlace.transform.rotation);
                spawnedObject.transform.SetParent(spawnPlace.transform);
            }
        }
    }
}
