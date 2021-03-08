using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phenotype {
    //holding the reference of Animal
    protected Entity thisEntity;
    protected Genome thisGenome;

    protected float[] traits;
    protected List<string> traitLabelList = new List<string>();
    protected Dictionary<string, int> traitIndexDict = new Dictionary<string, int>();
    protected Dictionary<string, float> traitDict = new Dictionary<string, float>();
    protected Dictionary<string, string> qualDict = new Dictionary<string, string>();

    public float[] GetTraits() { return traits; }
    public List<string> GetTraitLabels() { return traitLabelList; }
    public Dictionary<string, int> GetTraitIndices() { return traitIndexDict; }
    public Dictionary<string, float> GetTraitDict() { return traitDict; }
    
    public Phenotype(Entity passedObject) {
        thisEntity = passedObject;
        thisGenome = passedObject.GetGenome();
        
        CreateGeneTraits();
        CreateConstantTraits();
    }

    // Add all genes in the genome gene dictionary to traits
    private void CreateGeneTraits(){
        for (int i = 0; i < thisGenome.numGenes; i++){
            string label = thisGenome.geneLabelList[i];
            Gene currentGene = thisGenome.geneDict[label];
            float traitValue;

            traitLabelList.Add(label);
            traitIndexDict.Add(label, traitLabelList.Count);

            if (currentGene.geneType == "binary"){
                traitValue = CreateBinaryTrait(currentGene.geneSequence);
                traitDict.Add(label, traitValue);
            } else if (currentGene.geneType == "int"){
                traitValue = CreateIntTrait(currentGene.geneSequence);
                traitDict.Add(label, traitValue);
            } else if (currentGene.geneType == "float"){
                traitValue = CreateFloatTrait(currentGene.geneSequence);
                traitDict.Add(label, traitValue);
            } else {
                // if we got here, it was because someone had an incorrect gene type in a config file
                // we need a contingency plan here, and should have one for each of the  values in that file
            }
        }
    }

    // Add all genes in the genome constant gene dictionary to traits
    private void CreateConstantTraits() {
        for (int i = 0; i < thisGenome.numConstants; i++) {
            string label = thisGenome.constantLabelList[i];
            traitLabelList.Add(label);
            traitIndexDict.Add(label, i);

            float traitValue = thisGenome.constantDict[label]; // the first value in the list of this string-list dict entry
            traitDict.Add(label, traitValue);
        }
    }

    private float CreateBinaryTrait(BitArray geneSequence) {
        // convert the binary bit string to an integer, ie 001101 = 13
        if (geneSequence.Length > 32)
            throw new ArgumentException("Argument length shall be at most 32 bits.");

        int[] intValueArray = new int[1];
        geneSequence.CopyTo(intValueArray, 0);
        int intValue = intValueArray[0];
        float floatValue = (float)intValue;

        return floatValue;
    }

    private float CreateIntTrait(BitArray geneSequence) {
        float value = 0.0f;
        for (int i = 0; i < geneSequence.Length; i++) {
            if (geneSequence[i] == true) {
                value += 1.0f;
            }
        }
        return value;
    }

    private float CreateFloatTrait(BitArray geneSequence) {
        float value = 0.0f;
        for (int i = 0; i < geneSequence.Length; i++) {
            if (geneSequence[i] == true){
                value += 1.0f;
            }
        }
        value /= Convert.ToSingle(geneSequence.Length);
        return value;
    }

    public string GetDisplayInfo() {
        string toReturn = "";

        for (int i = 0; i < traitLabelList.Count; i++) {
            string outputString = i.ToString();
            string label = traitLabelList[i];

            outputString = outputString + " " + label;
            outputString = outputString + " " + traitDict[label];            
            toReturn += outputString + "\n";
        }
        return toReturn;
    }

    public void SetTrait(string label, float passed) {
        traitDict[label] = passed;
        int currentIndex = traitIndexDict[label];
        traits[currentIndex] = passed;
    }

    public float GetTrait(string label) {
        return traitDict[label];
    }

    private void Start(){}

    private void Update(){}
}