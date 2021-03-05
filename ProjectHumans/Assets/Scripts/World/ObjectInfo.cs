using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using System.Linq;


public class ObjectInfo {
    // this is the initial number that will be spawned
    public string name;
    public int startingN;
    public Genome genome;

    public ObjectInfo(string passedSpeciesName, int passedStartingN) {
        name = passedSpeciesName;
        startingN = passedStartingN;

        this.genome = new Genome();
        ImportObjectInfo();
    }

    public void ImportObjectInfo(){

        string line;
        System.IO.StreamReader file;
        
        string filename = @"Assets/Scripts/config/"+ name.ToLower() + ".config";
        file = new System.IO.StreamReader(filename);  

        while((line = file.ReadLine()) != null)  
        {  
            string[] lineInfo = line.Split(new[] { "=" }, StringSplitOptions.None);
            string[] leftArray = lineInfo[0].Split(new[] { "." }, StringSplitOptions.None);
            string[] rightArray = lineInfo[1].Split(new[] { "," }, StringSplitOptions.None);

            
            if (leftArray[0] == "gene") { genome.AddGeneToGenome(leftArray[1], rightArray); 
            } else if (leftArray[0] == "constant") {
                genome.AddConstantToGenome(leftArray[1], rightArray);
            } else if (leftArray[0] == "quality") {
                genome.AddQualToGenome(leftArray[1], rightArray);
            }
        }  
        file.Close();
    }
}