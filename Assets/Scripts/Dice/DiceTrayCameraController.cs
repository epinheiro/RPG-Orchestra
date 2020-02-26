using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceTrayCameraController : MonoBehaviour {
    /*
        POSITION 0 1.019 -1.7
        ROTATION 35.39 0 0

        POSITION 0 1.698 -0.054
        ROTATION 87.82 0 0

        ---> 
        POSITION +0 +0.679 +1.646
        ROTATION +52,43
    */
    Vector3 positionIncrement = new Vector3(+0, +0.679F, +1.646F);
    Vector3 rotationIncrement = new Vector3(+52.43F, +0, +0);

    // CHECK if this is the best way of doing so
    Rect copyOfRect;
    static float viewportRectX = 0.83F;
    static float viewportRectY = 0.74F;
    GameObject ZoomOutButton;



    bool zoomedIn = false;

    void Start() {
        copyOfRect = this.GetComponent<Camera>().rect;
        ZoomOutButton = GameObject.Find("ZoomOutButton Placeholder");
        ZoomOutButton.SetActive(false);
    }

    public void ZoomIn() {
        if (!zoomedIn) {
            this.transform.position += positionIncrement;
            this.transform.eulerAngles += rotationIncrement;
            ZoomInViewport();
            ZoomReturnButtonOn();
            zoomedIn = true;
        } else {
            this.transform.position -= positionIncrement;
            this.transform.eulerAngles -= rotationIncrement;
            ZoomOutViewport();
            ZoomReturnButtonOff();
            zoomedIn = false;
        }
    }

    void ZoomReturnButtonOn() {
        ZoomOutButton.SetActive(true);
    }

    void ZoomReturnButtonOff() {
        ZoomOutButton.SetActive(false);
    }

    void ZoomInViewport() {
        this.GetComponent<Camera>().rect = new Rect(0,0,1,1);
    }
    void ZoomOutViewport() {
        this.GetComponent<Camera>().rect = copyOfRect;
    }

}
