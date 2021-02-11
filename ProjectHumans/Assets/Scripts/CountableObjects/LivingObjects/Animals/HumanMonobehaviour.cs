using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HumanMonobehaviour : MonoBehaviour
{
    public Human thisHuman;

    public void SetHuman(Human passedHuman){
        this.thisHuman = passedHuman;
    }
}