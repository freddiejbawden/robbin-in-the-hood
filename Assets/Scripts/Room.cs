using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {
    private bool isCritical = false;
    public bool left;
    public bool right;
    public bool up;
    public bool down;

    public bool getCritical() {
        return isCritical;
    }

    public void setCritical(bool critical) {
        isCritical = critical;
    }

    public void setColour(Color colour) {
        gameObject.GetComponent<MeshRenderer>().material.color = colour;
    }
}