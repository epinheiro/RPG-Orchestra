using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice10Controller : DiceController {

    void Start() {
        Initialization(10);
        CheckValue();
    }
}
