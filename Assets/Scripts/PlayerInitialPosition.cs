using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInitialPosition : MonoBehaviour {
    public void initialPos(Vector3 position) {
        transform.position = position;
    }
}