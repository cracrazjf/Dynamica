using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
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

    void start(){}

    void update(){}

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

    public static void importSpeciesInfo(string species){
        // eventually replace this code with the code that reads from the file
        // note the config file now has constants and genome examples in it
        // each needs to get read into its own identical data structure
        // the genome items need to get inserted into genomeInfoDict
        // the constants need to get inserted into the constantDict

        // see https://docs.microsoft.com/en-us/dotnet/api/system.configuration.configurationmanager?redirectedfrom=MSDN&view=dotnet-plat-ext-3.1

        var genomeInfo = ConfigurationManager.GenomeInfo;
        if (genomeInfo.Count == 0)  {  
            Console.WriteLine("GenomeInfo is empty.");  
        } else {  
            int i = 0;
            foreach (var key in genomeInfo.AllKeys)  {  
                Console.WriteLine("Key: {0} Value: {1}", key, genomeInfo[key]); 
                genomeInfoDict.Add(key, genomeInfo[key]);
                geneLabelList.Add(key);
                geneIndexDict.Add(key,i);
                i++;
            }  
        } 
        var genomeConstInfo = ConfigurationManager.GenomeConstInfo;
        if (genomeContiInfo.Count == 0)  {  
            Console.WriteLine("GenomeInfo is empty.");  
        } else {  
            int j = 0;
            foreach (var key in genomeConstInfo.AllKeys)  {  
                Console.WriteLine("Key: {0} Value: {1}", key, genomeInfo[key]);
                constantLabelList.Add(key);
                constantIndexDict.Add(key,i);
                j++;
            }  
        } 
    }

    public void createGenome(string species)
    {
        importSpeciesInfo(species);
        
        for (int i = 0; i < numGenes; i++){
            Gene newGene = new Gene();
            string label = geneLabelList[i];
            string[] geneInfo = text.Split(genomeInfoDict[label]);

            newGene.generateGene(label, geneInfo[0], geneInfo[1], geneInfo[2], geneInfo[3], geneInfo[4], geneInfo[5]);
            geneLabelList.Add(label);
            geneDict.Add(label, newGene);
            geneIndexDict.Add(label, i.ToString());
        }
    }

}