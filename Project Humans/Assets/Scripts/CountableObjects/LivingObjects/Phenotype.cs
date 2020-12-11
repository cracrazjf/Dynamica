using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phenotype {
    //holding the reference of Animal
    public LivingObject thisLivingObject;

    // the 5 Change values
    public int numTraits; 
    public List<string> traitLabelList = new List<string>();
    public Dictionary<string, int> traitIndexDict = new Dictionary<string, int>();
    public Dictionary<string, string> traitDict = new Dictionary<string, string>();
    public Dictionary<string, bool> traitDisplayDict = new Dictionary<string, bool>();

    public Phenotype(LivingObject livingObject) {
        this.thisLivingObject = livingObject;
        this.numTraits = 0;
        this.createGeneTraits();
        this.createConstantTraits();
    }

    private void createGeneTraits(){
        
        // go through each item in genome's gene dict, create their phenotypic values, and add them to the trait dict
        for (int i = 0; i < thisLivingObject.genome.numGenes; i++){

            string label = thisLivingObject.genome.geneLabelList[i];
            Gene currentGene = thisLivingObject.genome.geneDict[label];
            traitLabelList.Add(label);
            traitIndexDict.Add(label, this.numTraits);
            traitDisplayDict.Add(label, currentGene.display);

            if (currentGene.geneType == "binary"){
                string traitValue = createBinaryTrait(currentGene.geneSequence);
                traitDict.Add(label, traitValue);
                this.numTraits++;
            }
            else if (currentGene.geneType == "int"){
                string traitValue = createIntTrait(currentGene.geneSequence);
                traitDict.Add(label, traitValue);
                this.numTraits++;
            }
            else if (currentGene.geneType == "float"){
                string traitValue = createFloatTrait(currentGene.geneSequence);
                traitDict.Add(label, traitValue);
                this.numTraits++;
            }
            else{
                // if we got here, it was because someone had an incorrect gene type in a config file
                // we need a contingency plan here, and should have one for each of the  values in that file
            }
        }
    }

    private void createConstantTraits(){
        
            // go through each item in genome's constant dict, and add them to the trait data structures
        for (int i = 0; i < thisLivingObject.genome.numConstants; i++){
            string label = thisLivingObject.genome.constantLabelList[i];
            //Debug.Log(label);
            traitLabelList.Add(label);
            traitIndexDict.Add(label, i);
            // not sure if the following two lines are ok C# syntax
            string traitValue = thisLivingObject.genome.constantDict[label][0]; // the first value in the list of this string-list dict entry
            string traitDisplayValue = thisLivingObject.genome.constantDict[label][1]; // the second value in the list of this string-list dict entry
            traitDict.Add(label, traitValue);
            if (traitDisplayValue == "1"){
                traitDisplayDict.Add(label, true);
            }
            else{
                traitDisplayDict.Add(label, false);
            }
            
            this.numTraits++;
        }
    }

    private string createBinaryTrait(BitArray geneSequence){
        // convert the binary bit string to an integer, ie 001101 = 13
        if (geneSequence.Length > 32)
            throw new ArgumentException("Argument length shall be at most 32 bits.");

        int[] intValueArray = new int[1];
        geneSequence.CopyTo(intValueArray, 0);
        int intValue = intValueArray[0];
        string stringValue = intValue.ToString();
        return stringValue;
    }

    private string createIntTrait(BitArray geneSequence){
        int intValue = 0;
        for (int i = 0; i < geneSequence.Length; i++)
        {
            if (geneSequence[i] == true){
                intValue += 1;
            }
        }
        string stringValue = intValue.ToString();
        return stringValue;
    }

    private string createFloatTrait(BitArray geneSequence){
        float floatValue = 0.0f;
        for (int i = 0; i < geneSequence.Length; i++)
        {
            if (geneSequence[i] == true){
                floatValue+= 1.0f;
            }
        }
        floatValue /= Convert.ToSingle(geneSequence.Length);
        string stringValue = floatValue.ToString();
        return stringValue;
    }

    public string GetDisplayInfo() {
        string toReturn = "";

        for (int i=0; i<numTraits; i++){
            string outputString = i.ToString();
            string label = traitLabelList[i];
            outputString = outputString + " " + label;
            outputString = outputString + " " + traitDict[label];
            outputString = outputString + " Display=" + traitDisplayDict[label].ToString();
            
            toReturn += outputString + "\n";
        }

        return toReturn;
    }

    private void Start(){}

    private void Update(){}
}