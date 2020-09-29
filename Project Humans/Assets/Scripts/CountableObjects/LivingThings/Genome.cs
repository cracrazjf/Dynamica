using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Genome : MonoBehaviour
{
    int numGenes;
    public List<string> geneLabelList;
    public Dictionary<string, Gene> geneDict;
    public Dictionary<string, string> geneIndexDict;

    void start()
    {


    }

    void update()
    {


    }

    public bool inheretGenome(Genome motherGenome, Genome fatherGenome)
    {
        bool success = true;

        if (motherGenome.numGenes == fatherGenome.numGenes)
        {
            numGenes = motherGenome.numGenes;

            for (int i = 0; i < numGenes; i++)
            {
                if (motherGenome.geneLabelList[i] == fatherGenome.geneLabelList[i])
                {
                    string geneLabel = motherGenome.geneLabelList[i];
                    Gene motherGene = motherGenome.geneDict[geneLabel];
                    Gene childGene = motherGene.MemberwiseClone();
                    childGene.sexualReproduction(fatherGenome.geneDict[geneLabel]);
                    geneLabelList.Add(geneLabel);
                    geneDict.Add(geneLabel, childGene);
                    geneIndexDict.Add(geneLabel, i.ToString());
                }
                else
                {
                    success = false;
                }
            }
        }
        else
        {
            success = false;
        }
        return success;
    }

    public Dictionary<string, List<string>> genomeInfoDict = new Dictionary<string, List<string>>();
    public List<string> g1 = new List<string> { "float", "6", "mutable", ".3", ".25"};
    public List<string> g2 = new List<string> { "float", "6", "mutable", ".4", ".25"};
    public List<string> g3 = new List<string> { "float", "6", "mutable", ".5", ".25"};
    public List<string> g4 = new List<string> { "float", "6", "mutable", ".6", ".25"};
    public List<string> g5 = new List<string> { "float", "6", "mutable", ".7", ".25"};

    public void importGeneInfo(string species){
        // eventually replace this code with the code that reads from the file
        genomeInfoDict.Add("hunger_threshold", g1);
        genomeInfoDict.Add("thirst_threshold", g2);
        genomeInfoDict.Add("fatigue_threshold", g3);
        genomeInfoDict.Add("sleepiness_threshold", g4);
        genomeInfoDict.Add("health_threshold", g5);
        geneLabelList.Add("hunger_threshold");
        geneLabelList.Add("thirst_threshold");
        geneLabelList.Add("fatigue_threshold");
        geneLabelList.Add("sleepiness_threshold");
        geneLabelList.Add("health_threshold");
        numGenes = geneLabelList.Count;
        for (int i = 0; i < numGenes; i++){
            geneIndexDict.Add(geneLabelList[i], i.ToString());
        }
    }

    public void createGenome(string species)
    {
        importGeneInfo(species);
        for (int i = 0; i < numGenes; i++){
            Gene newGene = new Gene();
            string label = geneLabelList[i];
            List<string> geneInfo = new List<string>(genomeInfoDict[label]);
            newGene.generateSequence(label, geneInfo[0], geneInfo[1], geneInfo[2], geneInfo[3], geneInfo[4]);
            geneLabelList.Add(label);
            geneDict.Add(label, newGene);
            geneIndexDict.Add(label, i.ToString());
        }
    }

}