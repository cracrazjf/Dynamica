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
    public Dictionary<string, float> traitDict = new Dictionary<string, float>();

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
            float traitValue;

            if (currentGene.geneType == "binary"){
                traitValue = createBinaryTrait(currentGene.geneSequence);
                traitDict.Add(label, traitValue);
                this.numTraits++;
            }
            else if (currentGene.geneType == "int"){
                traitValue = createIntTrait(currentGene.geneSequence);
                traitDict.Add(label, traitValue);
                this.numTraits++;
            }
            else if (currentGene.geneType == "float"){
                traitValue = createFloatTrait(currentGene.geneSequence);
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
            float traitValue = thisLivingObject.genome.constantDict[label]; // the first value in the list of this string-list dict entry
            traitDict.Add(label, traitValue);
            this.numTraits++;
        }
    }

    private float createBinaryTrait(BitArray geneSequence){
        // convert the binary bit string to an integer, ie 001101 = 13
        if (geneSequence.Length > 32)
            throw new ArgumentException("Argument length shall be at most 32 bits.");

        int[] intValueArray = new int[1];
        geneSequence.CopyTo(intValueArray, 0);
        int intValue = intValueArray[0];
        float floatValue = (float)intValue;
        return floatValue;
    }

    private float createIntTrait(BitArray geneSequence){
        float value = 0.0f;
        for (int i = 0; i < geneSequence.Length; i++)
        {
            if (geneSequence[i] == true){
                value += 1.0f;
            }
        }
        return value;
    }

    private float createFloatTrait(BitArray geneSequence){
        float value = 0.0f;
        for (int i = 0; i < geneSequence.Length; i++)
        {
            if (geneSequence[i] == true){
                value += 1.0f;
            }
        }
        value /= Convert.ToSingle(geneSequence.Length);
        return value;
    }

    public string GetDisplayInfo() {
        string toReturn = "";

        for (int i=0; i<numTraits; i++){
            string outputString = i.ToString();
            string label = traitLabelList[i];
            outputString = outputString + " " + label;
            outputString = outputString + " " + traitDict[label];            
            toReturn += outputString + "\n";
        }

        return toReturn;
    }

    private void Start(){}

    private void Update(){}
}