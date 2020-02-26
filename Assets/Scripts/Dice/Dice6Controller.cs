using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice6Controller : DiceController {

    void Start() {
        Initialization(6);
        CheckValue();
    }  

}
