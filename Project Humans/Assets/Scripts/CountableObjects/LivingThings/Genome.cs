using System;
using System.Collections;
using System.Collections.Generic;
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
        //FOR DEBUG
        // Debug.Log("Inheret Genome");
        // var arrayOfAllKeys = geneDict.Keys.ToArray();
        // for (int i=0; i<geneDict.Count; i++){
        //     string outputString = i.ToString();
        //     string label = arrayOfAllKeys[i];
            
        //     Gene currentGene = geneDict[label];
        //     outputString = outputString + " " + label;
        //     outputString = outputString + " " + currentGene.geneType;
        //     outputString = outputString + " " + currentGene.geneSize.ToString();
        //     outputString += " ";
        //     for (int j=0; j<currentGene.geneSize; j++){
        //         int value = Convert.ToInt32(currentGene.geneSequence[j]);
        //         outputString += value.ToString();
        //     }
        //     Debug.Log(outputString);
        // }
        return success;
    }

    public void importSpeciesInfo(string species){
        int counter = 0;  
        string line;  
        
        // Read the file and display it line by line.  
        System.IO.StreamReader file = new System.IO.StreamReader(@"Assets/config/human.config");  
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
        // FOR DEBUGGING
        // var arrayOfAllKeys = genomeInfoDict.Keys.ToArray();
        // for (int i=0; i<genomeInfoDict.Count; i++){
        //     string key = arrayOfAllKeys[i];
        //     Debug.Log(i);
        //     Debug.Log(key);
        //     Debug.Log(genomeInfoDict[key][0]);
        // }
    }
    
    public void createGenome(string species)
    {
        importSpeciesInfo(species);
        
        for (int i = 0; i < numGenes; i++){
            Gene newGene = new Gene();
            string label = geneLabelList[i];
            List<string> geneInfo = genomeInfoDict[label];
            newGene.generateGene(label, geneInfo[0], geneInfo[1], geneInfo[2], geneInfo[3], geneInfo[4], geneInfo[5]);
            geneDict.Add(label, newGene);
        }
        // FOR DEBUG
        // Debug.Log("Create Genome");
        // var arrayOfAllKeys = geneDict.Keys.ToArray();
        // for (int i=0; i<geneDict.Count; i++){
        //     string outputString = i.ToString();
        //     string label = arrayOfAllKeys[i];
            
        //     Gene currentGene = geneDict[label];
        //     outputString = outputString + " " + label;
        //     outputString = outputString + " " + currentGene.geneType;
        //     outputString = outputString + " " + currentGene.geneSize.ToString();
        //     outputString += " ";
        //     for (int j=0; j<currentGene.geneSize; j++){
        //         int value = Convert.ToInt32(currentGene.geneSequence[j]);
        //         outputString += value.ToString();
        //     }
        //     Debug.Log(outputString);

        

    }

}
