using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowerController : MonoBehaviour {
    public GameObject d4PrefabReference;
    public GameObject d6PrefabReference;
    public GameObject d10PrefabReference;
    public GameObject d20PrefabReference;

    public enum AvailableDice { d4 = 4, d6 = 6, d10 = 10, d20 = 20}

    class ThrowProperties {
        public Vector3 position;
        public Vector3 directionOfThrow;
        public ThrowProperties(Vector3 position, Vector3 directionOfThrow) {
            this.position = position;
            this.directionOfThrow = directionOfThrow;
        }
        public Vector3 GetPositionFlutuated() {
            float positionFlutuation = ((Random.value) * (Random.value > 0.5 ? 1 : -1))/5; //return a value in [-0.2,0.2]
            return new Vector3((position.x + positionFlutuation), position.y, (position.z + positionFlutuation));
        }
    }

    static float throwHeight = 9.3F;

    ThrowProperties[] ThrowablePositions = {
        new ThrowProperties(new Vector3(+0     , throwHeight, -0.931F), new Vector3(0,0,1)),
        new ThrowProperties(new Vector3(+0.687F, throwHeight, -0.663F), new Vector3(-.7F,0,.7F)),
        new ThrowProperties(new Vector3(+0.93F , throwHeight, -0.077F), new Vector3(-1,0,0)),
        new ThrowProperties(new Vector3(+0.687F, throwHeight, +0.663F), new Vector3(-.7F,0,-.7F)),

        new ThrowProperties(new Vector3(+0     , throwHeight, +0.931F), new Vector3(0,0,-1)),
        new ThrowProperties(new Vector3(-0.687F, throwHeight, -0.663F), new Vector3(+.7F,0,.7F)),
        new ThrowProperties(new Vector3(-0.93F , throwHeight, -0.077F), new Vector3(+1,0,0)),
        new ThrowProperties(new Vector3(-0.687F, throwHeight, -0.663F), new Vector3(+.7F,0,+.7F))
    };


    // Use this for initialization
    void Start () {}
	
	// Update is called once per frame
	void Update () {
        //KeyboardTesterFunctionCall();
    }

    void KeyboardTesterFunctionCall() { //put on Update function to facilitate tests
        if (Input.GetKeyDown("space")) {
            ThrowD6();
        }
        if (Input.GetKeyDown("return")) {
            ThrowD20();
        }
    }

    public void ThrowD4() {
        ThrowDice(d4PrefabReference);
    }

    public void ThrowD6() {
        ThrowDice(d6PrefabReference);
    }

    public void ThrowD10() {
        ThrowDice(d10PrefabReference);
    }

    public void ThrowD20() {
        ThrowDice(d20PrefabReference);
    }

    void ThrowDice(GameObject prefab) {
        int positionToThrow = (int)(Random.value * 8);

        GameObject go = (GameObject)Instantiate(prefab, ThrowablePositions[positionToThrow].GetPositionFlutuated(), new Quaternion(0, 0, 0, 0));
        go.transform.eulerAngles += new Vector3(Random.value*360, Random.value * 360, Random.value * 360);

        float throwFoce = (Random.value * 2) + 2;
        ApplyImpulseForce(go, (ThrowablePositions[positionToThrow].directionOfThrow * throwFoce));
        
    }

    void ApplyImpulseForce(GameObject go, Vector3 direction) {
        go.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
    }
    
    public void ClearDiceOnTray() {
        GameObject[] dice = GameObject.FindGameObjectsWithTag("Dice");
        
        if (dice != null) {
            foreach (GameObject oneDice in dice) {
                Destroy(oneDice);
            }
        }
    }

    public void CalculateDiceOnTray() {
        GameObject[] dice = GameObject.FindGameObjectsWithTag("Dice");

        List<int> resultsOfD4 = new List<int>();
        List<int> resultsOfD6 = new List<int>();
        List<int> resultsOfD10 = new List<int>();
        List<int> resultsOfD20 = new List<int>();

        string strD4 =  string.Empty;
        string strD6 =  string.Empty;
        string strD10 = string.Empty;
        string strD20 = string.Empty;

        foreach (GameObject oneDice in dice) {
            DiceController diceController = oneDice.GetComponent<DiceController>();
            int numberOfFaces = diceController.NumberOfFaces;

            switch (numberOfFaces) {
                case ((int)AvailableDice.d4):
                    resultsOfD4.Add(diceController.Result);
                    if (strD4 == string.Empty) strD4 = "D4: " + diceController.Result;
                    else strD4 += " + " + diceController.Result ;
                    break;
                case ((int)AvailableDice.d6):
                    resultsOfD6.Add(diceController.Result);
                    if (strD6 == string.Empty) strD6 = "D6: " + diceController.Result;
                    else strD6 += " + " + diceController.Result;
                    break;
                case ((int)AvailableDice.d10):
                    resultsOfD10.Add(diceController.Result);
                    if (strD10 == string.Empty) strD10 = "D10: " + diceController.Result;
                    else strD10 += " + " + diceController.Result;
                    break;
                case ((int)AvailableDice.d20):
                    resultsOfD20.Add(diceController.Result);
                    if (strD20 == string.Empty) strD20 = "D20: " + diceController.Result;
                    else strD20 += " + " + diceController.Result;
                    break;
            }


            Destroy(oneDice);
        }

        print("Result: ");
        PrintIfNotEmpty(strD4);
        PrintIfNotEmpty(strD6);
        PrintIfNotEmpty(strD10);
        PrintIfNotEmpty(strD20);
    }

    void PrintIfNotEmpty(string str) {
        if (str != string.Empty) print(str);
    }

    public static string[] AvailableCommands = { "/r" };

    public void ParseCommand(string text) {

        string commandToDo = "";

        string[] words = text.Split(' ');
        foreach (string command in AvailableCommands) {
            if (words[0] == command) {
                commandToDo = command;
                break;
            }
        }


        if (commandToDo == AvailableCommands[0]) {
            ParseThrowCommand(words);
        }// TODO - NEW COMMANDS - ELSE IFS TO INFINITE

        GameObject.Find("Command Input Field").GetComponent<InputField>().text = string.Empty;

    }

    private void ParseThrowCommand(string[] commands) {
        foreach(string command in commands) {
            string[] words = command.Split('d');
            if (words.Length == 2) {
                int numberOfDice = int.Parse(words[0]);
                switch (int.Parse(words[1])) {
                    case ((int)AvailableDice.d4):
                        for (int i = 0; i < numberOfDice; i++) ThrowD4();
                        break;
                    case ((int)AvailableDice.d6):
                        for (int i = 0; i < numberOfDice; i++) ThrowD6();
                        break;
                    case((int)AvailableDice.d10):
                        for (int i = 0; i < numberOfDice; i++) ThrowD10();
                        break;
                    case ((int)AvailableDice.d20):
                        for (int i = 0; i < numberOfDice; i++) ThrowD20();
                        break;
                }
            }
        }
    }

}
