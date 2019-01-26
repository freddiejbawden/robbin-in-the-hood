using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {
    [SerializeField] private List<GameObject> bedroomObjects;
    [SerializeField] private List<GameObject> kitchenObjects;
    [SerializeField] private List<GameObject> placeableObjects;
    [SerializeField] private GameObject diamond;
    private bool canPopulate = false;
    private bool isAttached = false;
    public bool isCritical = false;
    public bool connectable = true;
    public bool left;
    public bool right;
    public bool up;
    public bool down;
    public bool goal;

    public void placeDiamond() {
        GameObject diamondObj = Instantiate(diamond, this.transform);
        diamondObj.transform.position += new Vector3(0.0f, 1.0f, 0.0f);
    }

    public void populateRoom() {
        //20% chance of being a bedroom

        float roomType = Random.Range(0.0f, 1.0f);

        if(roomType < 0.2f) {
            List<GameObject> toPlace = bedroomObjects;
            createFurniture(toPlace);
        } else if(roomType < 0.4f) {
            List<GameObject> toPlace = kitchenObjects;
            createFurniture(toPlace);
        }        

        if(Random.Range(0.0f, 1.0f) < 0.2f) {
            float maxOff = 3.0f;
            float xOffset = Random.Range(-maxOff, maxOff);
            float yOffset = Random.Range(-maxOff, maxOff);
            int item = Random.Range(0, placeableObjects.Count);

            Instantiate(placeableObjects[item], transform.position + new Vector3(xOffset, 10.0f, yOffset), Quaternion.identity);
        }
            
    }

    public int getAdjoining() {
        int result = 0;

        if(left)
            result++;

        if(right)
            result++;

        if(up)
            result++;

        if(down)
            result++;

        return result;
    }

    public void createFurniture(List<GameObject> toPlace) {
        bool[] corners = new bool[] { false, false, false, false };

        foreach(GameObject obj in toPlace) {
            bool placed = false;

            while(!placed) {
                int rand = Random.Range(0, 4);
                placed = !corners[rand];

                if(!corners[rand]) {
                    corners[rand] = true;

                    GameObject placedObj = Instantiate(obj, transform);
                    placedObj.transform.eulerAngles = new Vector3(0.0f, 90.0f * rand, 0.0f);
                }
            }
        }
    }

    public bool getAttached() {
        return isAttached;
    }

    public bool getCritical() {
        return isCritical;
    }

    public bool getPopulate() {
        return canPopulate;
    }

    public void setAttached(bool attached) {
        isAttached = attached;
    }

    public void setCritical(bool critical) {
        isCritical = critical;
        isAttached = critical;
    }

    public void setPopulate(bool populate) {
        canPopulate = populate;
    }

    public void setColour(Color colour) {
        gameObject.GetComponent<MeshRenderer>().material.color = colour;
    }
}