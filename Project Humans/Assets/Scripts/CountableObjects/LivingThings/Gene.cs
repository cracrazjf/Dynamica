using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gene : MonoBehaviour
{
    public string label;
    public string geneType;
    public int geneSize;
    public BitArray geneSequence;
    public bool mutable;
    public float specifiedMean;
    public float specifiedStdev;

    public float mutationRate = 0.01f;

    public Gene(string label, string geneType, int geneSize, BitArray geneSequence, bool mutable, float specifiedMean, float specifiedStdev)
    {
        // constructors for all necessary functionale
        this.label = label;
        this.geneType = geneType;
        this.geneSize = geneSize;
        this.geneSequence = geneSequence;
        this.mutable = mutable;
        this.specifiedMean = specifiedMean;
        this.specifiedStdev = specifiedStdev;

        generateSequence();
        // create a string called this.output_string which is a string concatenation of all the above variables
    }

    void start()
    {


    }

    void update()
    {


    }

    void sexualReproduction(Gene fatherGene)
    {
        // for each element in the gene sequence
        for (int i = 0; i < geneSize; i++)
        {

            // randomly either keep the mother value, or switch to the father value
            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                geneSequence[i] = fatherGene.geneSequence[i];
            }

            // draw a random float, if < mutation rate, flip the element to its opposite
            UnityEngine.Random random = new UnityEngine.Random();
            if (random < mutationRate)
            {
                geneSequence[i] = !geneSequence[i];
            }

        }
    }

    void generateSequence()
    {

    }
}
