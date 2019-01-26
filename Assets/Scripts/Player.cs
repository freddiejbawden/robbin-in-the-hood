using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float pickupRadius=3;
    public Camera cm;
    private RaycastHit hitInfo;
    private Vector3 thing = new Vector3(1,1,1);
    public LayerMask objectLayer;
    private bool holding;
    private Transform itemHolding;
    private float offset = 2;
    private Collider[] itemColliders;

    private float minDist, dist;
    private Transform item;
    private int minIdx;


    private void OnTriggerEnter(Collider other)
    {
         Debug.Log(other.transform.name);

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {



        if (Input.GetKeyDown(KeyCode.F))
        {
            if (holding)
            {
                itemHolding.parent = null;
                itemHolding.GetComponent<Rigidbody>().useGravity = true;
                itemHolding.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

                holding = false;
            }
            else
            {
                itemColliders = Physics.OverlapSphere(transform.position, pickupRadius, objectLayer);
                if (itemColliders.Length == 0)
                {
                    return;
                }
                FindNearestItem();

                itemHolding = itemColliders[minIdx].transform;
                itemHolding.position = transform.position + transform.forward * offset;
                itemHolding.GetComponent<Rigidbody>().useGravity = false;
                itemHolding.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                itemHolding.parent = cm.transform;
                holding = true;
            }

        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (holding)
            {
                itemHolding.parent = null;
                itemHolding.GetComponent<Rigidbody>().useGravity = true;
                itemHolding.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                itemHolding.GetComponent<Rigidbody>().AddForce(transform.forward*300);
                holding = false;
            }
        }

        
    }

    void FindNearestItem()
    {
        minDist = Mathf.Infinity;

        for (int i = 0; i < itemColliders.Length; i++)
        {
            item = itemColliders[i].transform;

            dist = Vector3.Distance(transform.position, item.position);

            if (dist < minDist)
            {
                minDist = dist;
                minIdx = i;
            }
        }
    }
}
