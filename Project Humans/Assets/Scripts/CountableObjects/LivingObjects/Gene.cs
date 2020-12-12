
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gene 
{
    public string label;
    public string geneType;
    public int geneSize;
    public BitArray geneSequence;
    public bool mutable;
    public float specifiedMean;
    public float specifiedStdev;

    public string visible;
    public float mutationRate = 0.01f;

    void start() 
    {


    }

    void update()
    {


    }
    //I am not sure if this is working, need test later;
    public Gene ShallowCopy() {
        return (Gene) this.MemberwiseClone();
    }

    public void sexualReproduction(Gene fatherGene){
        // for each element in the gene sequence
        for (int i = 0; i < geneSize; i++){
            // randomly either keep the mother value, or switch to the father value
            if(Random.Range(0, 2) == 0 ){
                geneSequence[i] = fatherGene.geneSequence[i];
            }
            // draw a random float, if < mutation rate, flip the element to its opposite
            // Random random = new Random();
            float random = Random.value; // Returns a random number between 0.0 [inclusive] and 1.0 [inclusive]
            if (this.mutable){
                if (random < mutationRate){
                    geneSequence[i] = !geneSequence[i];
                }
            }
        }
    }

    public void generateGene(string label, string geneType, string geneSize, 
                                 string mutable, string specifiedMean, string specifiedStdev){
        this.label = label;
        this.geneType = geneType;
        this.geneSize = int.Parse(geneSize);
        if (mutable == "immutable"){ // should be "mutable"?
            this.mutable = true;
        }
        else{
            this.mutable = false;
        }
        this.specifiedMean = float.Parse(specifiedMean);
        this.specifiedStdev = float.Parse(specifiedStdev);

        // TODO we need to impliment this to create these bit-arrays to use specifiedMean and specifiedStdev
        // use those two vars to generate a value from that distribution
        // then convert that to the appropriate bit array
        geneSequence = new BitArray(this.geneSize);
        for (int i = 0; i < this.geneSize; i++){
            int newValue = Random.Range(0, 2); // changed (0,2) to (0,1) becasue range is inclusive
            if (newValue == 0) {
                geneSequence[i] = false;
            }
            else{
                geneSequence[i] = true;
            }
        }
    }
}
