using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraScript : MonoBehaviour {

    float camXSensibility = 5;
    float camZSensibility = 5;

    bool cameraCanMove = true;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (cameraCanMove) {
            MoveCameraByKey();
            MoveCameraByMouse();
        }
    }

    /// <summary>
    /// UI Design funcion called to disable CAMERA MOVEMENT clicks when the mouse is on some UI element
    /// (Pointer ENTER and Pointer EXIT with event trigger) 
    /// </summary>
    public void ToggleCamera() {
        if (cameraCanMove) cameraCanMove = false;
        else cameraCanMove = true;
    }

    void MoveCameraByKey() {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
            var x = Input.GetAxis("Horizontal") * Time.deltaTime * camXSensibility;
            var z = Input.GetAxis("Vertical") * Time.deltaTime * camZSensibility;

            this.transform.localPosition = new Vector3(this.transform.localPosition.x + x, this.transform.localPosition.y, this.transform.localPosition.z + z);
        }
    }

    void MoveCameraByMouse() {
        double percentageOfScreen = 0.05F;
        Vector3 mouse = Input.mousePosition;

        double width = Screen.width;
        double height = Screen.height;

        // TOP
        if ( mouse.y > (Screen.height*(1-percentageOfScreen)) ) {
            float movement = Time.deltaTime * camXSensibility;
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.z + movement);
        }
        // BOTTOM
        else if (mouse.y < (Screen.height * percentageOfScreen)) {
            float movement = Time.deltaTime * camXSensibility;
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.z - movement);
        }

        // LEFT
        if (mouse.x > (Screen.width * (1 - percentageOfScreen))) {
            float movement = Time.deltaTime * camXSensibility;
            this.transform.localPosition = new Vector3(this.transform.localPosition.x + movement, this.transform.localPosition.y, this.transform.localPosition.z);
        }
        // RIGHT
        else if (mouse.x < (Screen.width * percentageOfScreen)) {
            float movement = Time.deltaTime * camXSensibility;
            this.transform.localPosition = new Vector3(this.transform.localPosition.x - movement, this.transform.localPosition.y, this.transform.localPosition.z);
        }
    }

}
