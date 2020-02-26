using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceController : MonoBehaviour {

    public enum DicesAvailable { d6 = 6, d20 = 20}

    protected int numberOfFaces;
    public int NumberOfFaces {
        get { return numberOfFaces; }
    }

    protected int result;
    public int Result {
        get { return result; }
    }
    

    protected GameObject[] faceReferenceList;
    
    protected void Initialization(int numberOfSides) {
        numberOfFaces = numberOfSides;

        faceReferenceList = new GameObject[numberOfFaces];
        for (int i = 0; i < numberOfFaces; i++) {
            faceReferenceList[i] = (this.transform.GetChild(0).GetChild(i).gameObject);
            //Debug.Log(faceReferenceList[i]);
        }
    }

    protected void CheckValue() {
        StartCoroutine(CheckValueCoroutine());
    }

    IEnumerator CheckValueCoroutine() {
        yield return new WaitForSeconds(0.1F);

        while (isMoving()) {
            yield return null;
            //print("moving");
        }

        yield return new WaitForSeconds(.5F);

        result = FaceIsSittingOn();

        //print("D"+ numberOfFaces + " stopped on " + result);
        yield return null;
    }

    public bool isMoving() {
        if (GetComponent<Rigidbody>().velocity.magnitude < 0.5) {
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            return false;
        }
        else {
            return true;
        }
    }

    public int FaceIsSittingOn() {
        return OppositeFace(whichFaceIsLower());
    }

    public int OppositeFace(int faceDown) {
        return (numberOfFaces + 1) - faceDown;
    }

    protected int whichFaceIsLower() {
        double min = 10000;
        int index = -1;

        for (int i = 0; i < numberOfFaces; i++) {
            //Debug.Log(i+1 +" : " + faceReferenceList[i].GetComponent<Transform>().position.y);
            if (faceReferenceList[i].transform.position.y < min) {
                min = faceReferenceList[i].transform.position.y;
                index = i;
            }
        }

        return index + 1;
    }

}