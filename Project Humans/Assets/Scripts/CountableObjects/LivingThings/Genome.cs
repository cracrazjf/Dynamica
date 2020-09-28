using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genome : MonoBehaviour
{
    int numGenes;
    public List<string> geneLabelList;
    public Dictionary<string, int> geneDict;
    public Dictionary<string, string> geneIndexDict;

    // genome.hunger_threshold=''
    // genome.thirst_threshold=''
    // genome.sleepiness_threshold=''
    // genome.fatigue_threshold=''
    // genome.health_threshold=''

    void start()
    {


    }

    void update()
    {


    }

    bool inheretGenome(Genome motherGenome, Genome fatherGenome)
    {
        bool success = true;

        if (motherGenome.numGenes == fatherGenome.numGenes)
        {
            numGenes = motherGenome.numGenes;

            for (int i = 0; i < numGenes; i++)
            {
                if (motherGenome.geneLabelList[i] == fatherGenome.geneLabelList[i])
                {
                    label = motherGenome.geneLabelList[i];
                    geneLabelList.Add(label);
                    geneIndexDict[label] = i;
                    childGene = motherGenome.geneDict[label].MemberwiseClone();
                    childGene.sexualReproduction(fatherGenome.geneDict[label]);
                    geneDict[label] = childGene;
                }
                else
                {
                    return false;
                }
            }
        }
        else
        {
            return false;
        }
        return success;
    }

    void createGenome()
    {

        // eventually this needs to be put into a specifies specific config file
        // and we need a function that is passed the species name so it knows which file to read
        // and then it reads it in and parses it into this dict
        Dictionary<string, List<String>> genomeInfoDict = new Dictionary<string, List<String>>();

        public List<string> list1 = new List<string>(new string[] { "int", "5", "mutable", "3", "1" });
    genomeInfoDict.add("max_velocity", list1);
    }

}