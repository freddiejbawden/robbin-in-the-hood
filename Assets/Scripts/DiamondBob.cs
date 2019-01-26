using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondBob : MonoBehaviour {
    private Vector3 originalPosition;
    [SerializeField] private float amplitude;
    private Vector3 speed = new Vector3(0.0f, 2.0f, 0.0f);
    private bool up = true;

    private void Start() {
        originalPosition = transform.position;
    }

    private void Update() {
        move();
    }

    private void move() {
        if(Mathf.Abs(Vector3.Distance(originalPosition, transform.position)) >= amplitude) {
            up = !up;
        }

        if(up) {
            transform.position += speed * Time.deltaTime;
        } else {
            transform.position -= speed * Time.deltaTime;
        }
    }
}
