using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{

    public float speed = 4.0f;

    //Rotationtarget
    public Transform target;


    private CharacterController _charCount;

    // Start is called before the first frame update
    void Start()
    {
      _charCount = GetComponent<CharacterController> ();

    }

    // Update is called once per frame
    void Update()
    {
      if (!Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E) && !RotateOnZAxis.resetPosition ) {

        float deltaX = Input.GetAxis ("Horizontal") * speed * -1;
        float deltaZ = Input.GetAxis ("Vertical") * speed * -1;
        Vector3 movement = new Vector3 (deltaX, 0, deltaZ);
        movement = Vector3.ClampMagnitude (movement, speed);

        movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement);
        _charCount.Move (movement);
      }
      if (Input.GetKey(KeyCode.LeftShift)) {
        speed = 2;
      } else {
        speed = 4;
      }

    }
}
