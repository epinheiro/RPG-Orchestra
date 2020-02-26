using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice20Controller : DiceController {

    void Start() {
        Initialization(20);
        CheckValue();
    }
}
