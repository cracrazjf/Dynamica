using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.IO;
//using System.Text.Json;
//using System.Text.Json.Serialization;
using UnityEngine;


abstract public class NonlivingObject : CountableObject
{
    public NonlivingObject(string objectType, int index) : base (objectType, index) {}
}
