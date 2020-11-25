using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using System.Linq;


public class Genome 
{
    public LivingObject thisLivingObject;

    // this is the temp data structure that is used to read in data from the file
    public Dictionary<string, List<string>> genomeInfoDict = new Dictionary<string, List<string>>();

    // this is where the genes go
    public int numGenes;
    public List<string> geneLabelList = new List<string>();
    public Dictionary<string, Gene> geneDict = new Dictionary<string, Gene>();
    public Dictionary<string, string> geneIndexDict = new Dictionary<string, string>();

    // this is where the constants get stored from each species's config file
    public int numConstants;
    public List<string> constantLabelList = new List<string>();
    public Dictionary<string, List<string>> constantDict = new Dictionary<string, List<string>>();
    public Dictionary<string, int> constantIndexDict= new Dictionary<string, int>();

    public bool inheretGenome(Genome motherGenome, Genome fatherGenome)
    {
        bool success = true;

        if (motherGenome.numGenes == fatherGenome.numGenes && motherGenome.numConstants == fatherGenome.numConstants)
        {
            numConstants = motherGenome.numConstants;
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
            for (int i = 0; i < numConstants; i++) {
                if (motherGenome.constantLabelList[i] == fatherGenome.constantLabelList[i]) {
                    string constantLable = motherGenome.constantLabelList[i];
                    var constantValue = motherGenome.constantDict[constantLable];

                    constantLabelList.Add(constantLable);
                    constantDict.Add(constantLable,constantValue);
                    constantIndexDict.Add(constantLable,numConstants);
                        
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

    public void importSpeciesInfo(string species){
        int counter = 0;  
        string line;
        System.IO.StreamReader file;
        
        // Read the file and display it line by line. 
        if (species == "human") {
            file = new System.IO.StreamReader(@"Assets/config/human.config");  
        } else {
            file = new System.IO.StreamReader(@"Assets/config/penguin.config");  
        }
        
        while((line = file.ReadLine()) != null)  
        {  
            string[] lineInfo = line.Split(new[] { "=" }, StringSplitOptions.None);
            string[] leftArray = lineInfo[0].Split(new[] { "." }, StringSplitOptions.None);
            string[] rightArray = lineInfo[1].Split(new[] { "," }, StringSplitOptions.None);
            if (leftArray[0] == "genome"){
                geneLabelList.Add(leftArray[1]);
                geneIndexDict.Add(leftArray[1], numGenes.ToString());
                numGenes++;

                genomeInfoDict.Add(leftArray[1], rightArray.ToList());
            }
            else if (leftArray[0] == "constant"){
                constantLabelList.Add(leftArray[1]);
                constantDict.Add(leftArray[1], rightArray.ToList());
                constantIndexDict.Add(leftArray[1], numConstants);
                numConstants++;
                
            }
            counter++;  
        }  
        
        file.Close();
    }
    
    public void CreateGenome(string species)
    {
        importSpeciesInfo(species);
        
        for (int i = 0; i < numGenes; i++){
            Gene newGene = new Gene();
            string label = geneLabelList[i];
            List<string> geneInfo = genomeInfoDict[label];
            newGene.generateGene(label, geneInfo[0], geneInfo[1], geneInfo[2], geneInfo[3], geneInfo[4], geneInfo[5]);
            geneDict.Add(label, newGene);
        }

    }

    public string GetConstantInfo() {
        string toReturn = "";

        for (int i=0; i<numConstants; i++){
            string outputString = i.ToString();
            string label = constantLabelList[i];
            outputString = outputString + " " + label;
            outputString = outputString + " " + constantDict[label];
            
            toReturn += outputString + "\n";
        }

        return toReturn;
    }

}
