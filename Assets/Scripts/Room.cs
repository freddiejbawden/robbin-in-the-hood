using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {
    private bool isAttached = false;
    private bool isCritical = false;
    public bool connectable = true;
    public bool left;
    public bool right;
    public bool up;
    public bool down;

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

    public bool getAttached() {
        return isAttached;
    }

    public bool getCritical() {
        return isCritical;
    }

    public void setAttached(bool attached) {
        isAttached = attached;
    }

    public void setCritical(bool critical) {
        isCritical = critical;
        isAttached = critical;
    }

    public void setColour(Color colour) {
        gameObject.GetComponent<MeshRenderer>().material.color = colour;
    }
}