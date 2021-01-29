using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanSensorySystem : SensorySystem {
    // todo add all the states and transitions to 

    public Human thisHuman;

    public HumanSensorySystem(Human human) : base(human) {
        this.thisHuman = human;
    }

    

}
