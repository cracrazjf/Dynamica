using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.IO;
using UnityEngine;


abstract public class NonlivingObject : CountableObject
{

    public Dictionary<string,List<string>> propertyDict = new Dictionary<string, List<string>>();

    public NonlivingObject(string objectType, int index, Nullable<Vector3> position, Dictionary<string, List<string>> passedPropertyDict) : 
            base (objectType, index, position) 
    {
        propertyDict = passedPropertyDict;
    }
}
