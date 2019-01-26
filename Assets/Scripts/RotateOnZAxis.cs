using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOnZAxis : MonoBehaviour {

    //Target rotation speed
    public float rotateSpeed = 10.0f;

    //Max angle rotation
    public float maxAngle = 30.0f;

    //Rotationtarget
    public Transform target;

    //Origin Angle
    public float originAngle = 0.0f;

    //Reset position?
    public static bool resetPosition;

    //The Current Angle
    public static float currentAngle;

    //Actual rotation speed
    private float rotSpeed;



    void Update(){

        //Calculate target rotation speed
        rotSpeed = rotateSpeed * Time.deltaTime;

        //Get the current angle
        currentAngle = target.localEulerAngles.z;

        //If current value is between 180 and 360 degrees, get it's negative equivalent
        //This just makes the math easier
        currentAngle = (currentAngle > 180) ? currentAngle - 360 : currentAngle;

        //If the current angle is greater than the max angle, set it equal to the max angle
        if (currentAngle > maxAngle) {
            currentAngle = maxAngle;
        }
        //Or If the current angle is smaller than the negative max angle equivalent,
        //set it equal to the negative max angle equivalen
        else if(currentAngle < -maxAngle){
            currentAngle = -maxAngle;
        }


        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.S)  ) {
          //If press Q, rotate the angle to the left, at the specified speed
          if (Input.GetKey (KeyCode.Q)){
              if (currentAngle < maxAngle) {
                  target.Rotate(0, 0, rotSpeed);

              }
          }



          //If press E, rotate the angle to the right, at the specified rotation speed
          if (Input.GetKey (KeyCode.E)) {
            if (currentAngle > -maxAngle) {
                    target.Rotate (0, 0, -rotSpeed);

                }
            }
        }




        //If either A or D is released, then set reset position to true
        if (Input.GetKeyUp (KeyCode.Q) || Input.GetKeyUp (KeyCode.E)) {
            resetPosition = true;
        }

        //If reset position is set to true
        if (resetPosition == true) {

            //And as long as Q or E are not pressed
            if ((!Input.GetKey (KeyCode.Q) && !Input.GetKey (KeyCode.E))) {
              if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.S)  ) {

                //If the current angle is greater than the origin angle then decrease it's value
                if (currentAngle > originAngle) {
                    target.Rotate (0, 0, -rotSpeed);
                }
                //Or if the current angle is less than origin angle then increase it's value
                else if (currentAngle < originAngle) {
                    target.Rotate (0, 0, rotSpeed);
                }

                //If the current angle is between 0.1 and -0.1 degrees, then set reset to false
                if (currentAngle < 1.0f && currentAngle > -1.0f) {
                    currentAngle = 0;
                    resetPosition = false;
                }
            }
          }
        }
    }
}
