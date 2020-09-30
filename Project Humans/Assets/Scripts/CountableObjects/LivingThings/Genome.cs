using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Genome : MonoBehaviour
{
    // this is the temp data structure that is used to read in data from the file
    public Dictionary<string, List<string>> genomeInfoDict = new Dictionary<string, List<string>>();

    // this is where the genes go
    int numGenes;
    public List<string> geneLabelList;
    public Dictionary<string, Gene> geneDict;
    public Dictionary<string, string> geneIndexDict;

    // this is where the constants get stored from each species's config file
    int numConstants;
    public List<string> constantLabelList;
    public Dictionary<string, string> constantDict;
    public Dictionary<string, int> constantIndexDict;

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
                    Gene childGene = motherGene.ShallowCopy();//This might not be what we want.
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

    
    public List<string> g1 = new List<string> { "float", "6", "mutable", ".3", ".25", "1"};
    public List<string> g2 = new List<string> { "float", "6", "mutable", ".4", ".25", "1"};
    public List<string> g3 = new List<string> { "float", "6", "mutable", ".5", ".25", "1"};
    public List<string> g4 = new List<string> { "float", "6", "mutable", ".6", ".25", "1"};
    public List<string> g5 = new List<string> { "float", "6", "mutable", ".7", ".25", "1"};
    public List<string> g6 = new List<string> { "float", "100", "mutable", ".01", ".0", "1"};
    public List<string> g7 = new List<string> { "float", "100", "mutable", ".01", ".0", "1"};
    public List<string> g8 = new List<string> { "float", "100", "mutable", ".01", ".0", "1"};
    public List<string> g9 = new List<string> { "float", "100", "mutable", ".01", ".0", "1"};
    public List<string> g10 = new List<string> { "float", "100", "mutable", ".01", ".0", "1"};
    public List<string> g11 = new List<string> { "binary", "1", "mutable", "-1", "-1", "1"};

    public List<string> c1 = new List<string> { "0.0", "1"};
    public List<string> c2 = new List<string> { "50.0", "1"};
    public List<string> c3 = new List<string> { "0.0", "1"};
    public List<string> c4 = new List<string> { "0.01", "1"};

    public void importSpeciesInfo(string species){
        // eventually replace this code with the code that reads from the file
        // note the config file now has constants and genome examples in it
        // each needs to get read into its own identical data structure
        // the genome items need to get inserted into genomeInfoDict
        // the constants need to get inserted into the constantDict
        genomeInfoDict.Add("hunger_threshold", g1);
        genomeInfoDict.Add("thirst_threshold", g2);
        genomeInfoDict.Add("fatigue_threshold", g3);
        genomeInfoDict.Add("sleepiness_threshold", g4);
        genomeInfoDict.Add("health_threshold", g5);
        genomeInfoDict.Add("hunger_change", g6);
        genomeInfoDict.Add("thirst_change", g7);
        genomeInfoDict.Add("fatigue_change", g8);
        genomeInfoDict.Add("sleepiness_change", g9);
        genomeInfoDict.Add("health_change", g10);
        genomeInfoDict.Add("sex", g11);

        constantDict.Add("initial_velocity", c1)
        constantDict.Add("initial_velocity", c2)
        constantDict.Add("initial_velocity", c3)
        constantDict.Add("initial_velocity", c4)


        // then, using the data in genomeInfoDict and constantInfoDict
        //      create the data structures below algorithmically rather than hard coded
        numGenes = 11;
        geneLabelList.Add("hunger_threshold");
        geneLabelList.Add("thirst_threshold");
        geneLabelList.Add("fatigue_threshold");
        geneLabelList.Add("sleepiness_threshold");
        geneLabelList.Add("health_threshold");
        geneLabelList.Add("hunger_change");
        geneLabelList.Add("thirst_change");
        geneLabelList.Add("fatigue_change");
        geneLabelList.Add("sleepiness_change");
        geneLabelList.Add("health_change");
        geneLabelList.Add("sex");
        for (int i = 0; i < numGenes; i++){
            geneIndexDict.Add(geneLabelList[i], i.ToString());
        }

        numConstants = 4;
        constantLabelList.add(""); // the label of the constant in a list
        constantLabelList.add("final_velocity");
        constantLabelList.add("current_velocity");
        constantLabelList.add("acceleration_rate");
        for (int i = 0; i < numConstants; i++){
            constantIndexDict.Add(constantLabelList[i], i.ToString());
        }

    }

    public void createGenome(string species)
    {
        importSpeciesInfo(species);
        
        for (int i = 0; i < numGenes; i++){
            Gene newGene = new Gene();
            string label = geneLabelList[i];
            List<string> geneInfo = new List<string>(genomeInfoDict[label]);
            newGene.generateGene(label, geneInfo[0], geneInfo[1], geneInfo[2], geneInfo[3], geneInfo[4], geneInfo[5]);
            geneLabelList.Add(label);
            geneDict.Add(label, newGene);
            geneIndexDict.Add(label, i.ToString());
        }
    }

}