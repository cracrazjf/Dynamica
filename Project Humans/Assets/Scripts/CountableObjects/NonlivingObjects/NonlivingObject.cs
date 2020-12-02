using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.IO;
using UnityEngine;


abstract public class NonlivingObject : CountableObject
{

    public Dictionary<string,string> propertyDict = new Dictionary<string, string>();

    public NonlivingObject(string objectType, int index) : base (objectType, index) {}
}
