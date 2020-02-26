using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice4Controller : DiceController {

    void Start() {
        Initialization(4);
        CheckValue();
    }
}
