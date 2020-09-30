using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phenotype : MonoBehaviour
{
    // Velocity and accerlerationRate
    // these now all exist in genome.constantDict
    public float initialVelocity = 0.0f;
    public float finalVelocity = 50.0f;
    public float currentVelocity = 0.0f;
    public float accelerationRate = 0.01f;

    //rotate angle and rotate speed;
    public float rotation;
    public float rotationleft = 360;
    public float rotationspeed = 10;

    // the 5 Change values
    public int numTraits; 
    public List<string> traitLabelList = new List<string>();
    public Dictionary<string, int> traitIndexDict = new Dictionary<string, int>();
    public Dictionary<string, string> traitDict = new Dictionary<string, string>();
    public Dictionary<string, bool> traitDisplayDict = new Dictionary<string, bool>();

    public Phenotype(Animal thisAnimal) {
        this.animal = thisAnimal;
        this.numTraits = 0;

        // go through each item in genome's constant dict, and add them to the trait data structures
        for (int i = 0; i < thisAnimal.genome.numConstants; i++){
            string label = thisAnimal.genome.constantLabelList[i];
            traitLabelList.Add(label)
            traitIndexDict.Add(label, i.ToString());
            // not sure if the following two lines are ok C# syntax
            string traitValue = thisAnimal.genome.constantDict[label][0]; // the first value in the list of this string-list dict entry
            string traitDisplayValue = thisAnimal.genome.constantDict[label][1]; // the second value in the list of this string-list dict entry
            traitDict.Add(label, traitValue);
            if (traitDisplayValue == "1"){
                traitDisplayDict.Add(label, True);
            }
            else{
                traitDisplayDict.Add(label, False);
            }
            
            this.numTraits++;
        }

        // go through each item in genome's gene dict, create their phenotypic values, and add them to the trait dict
        for (int i = 0; i < thisAnimal.genome.numGenes; i++){
            string label = thisAnimal.genome.geneLabelList[i];
            Gene currentGene = thisAnimal.genome.geneDict[label];
            traitLabelList.Add(label)
            traitIndexDict.Add(label, this.numTraits);
            traitDisplayDict.Add(label, currentGene.display);
            if (currentGene.geneType == "binary"){
                int traitValue = createBinaryTrait(currentGene.geneSequence);
            }
            else if (currentGene.geneType == "int"){
                int traitValue = createIntTrait(currentGene.geneSequence);
            }
            else if (currentGene.geneType == "float"){
                float traitValue = createFloatTrait(currentGene.geneSequence);
            }
            else{
                // if we got here, it was because someone had an incorrect gene type in a config file
                // we need a contingency plan here, and should have one for each of the  values in that file
            }
            traitDict.Add(label, traitValue);
            this.numTraits++;
        }

    }

    private int createBinaryTrait(BitArray geneSequence){
        // convert the binary bit string to an integer, ie 001101 = 13
        return 1;
    }

    private int createIntTrait(BitArray geneSequence){
        // sums the bitstring
        return 1;
    }

    private float createFloatTrait(BitArray geneSequence){
        // sums the bitstring and divides by its length
        return 0.5;
    }

    private void Start()
    {

    }

    private void Update()
    {
        
    }
}

