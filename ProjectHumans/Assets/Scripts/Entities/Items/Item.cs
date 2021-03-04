using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.IO;
using UnityEngine;


abstract public class Item : Entity {
    public Item(string objectType, int index, Genome motherGenome) : 
            base (objectType, index, motherGenome) {}
}