using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using System.Linq;


public class Genome {

    // this is where the genes go
    public int numGenes;
    public List<string> geneLabelList;
    public Dictionary<string, Gene> geneDict;
    public Dictionary<string, int> geneIndexDict;

    // this is where the constants get stored from each species's config file
    public int numConstants;
    public List<string> constantLabelList;
    public Dictionary<string, float> constantDict;
    public Dictionary<string, int> constantIndexDict;

    // some random qualitative traits
    public int numQuals;
    public Dictionary<string, string> qualDict;
    public Dictionary<string, string> GetQualDict() { return qualDict; }

    public Genome(Genome motherGenome, Genome fatherGenome) {
        numGenes = 0;
        geneLabelList = new List<string>();
        geneDict = new Dictionary<string, Gene>();
        geneIndexDict = new Dictionary<string, int>();

        numConstants = 0;
        constantLabelList = new List<string>();
        constantDict = new Dictionary<string, float>();
        constantIndexDict= new Dictionary<string, int>();

        numQuals = 0;
        qualDict = new Dictionary<string, string>();

        InheretGenome(motherGenome, fatherGenome);
    }

    public Genome(Genome motherGenome) {
        numGenes = 0;
        geneLabelList = new List<string>();
        geneDict = new Dictionary<string, Gene>();
        geneIndexDict = new Dictionary<string, int>();

        numConstants = 0;
        constantLabelList = new List<string>();
        constantDict = new Dictionary<string, float>();
        constantIndexDict= new Dictionary<string, int>();

        numQuals = 0;
        qualDict = new Dictionary<string, string>();

        InheretGenome(motherGenome);
    }

    public Genome() {
        numGenes = 0;
        geneLabelList = new List<string>();
        geneDict = new Dictionary<string, Gene>();
        geneIndexDict = new Dictionary<string, int>();

        numConstants = 0;
        constantLabelList = new List<string>();
        constantDict = new Dictionary<string, float>();
        constantIndexDict= new Dictionary<string, int>();

        numQuals = 0;
        qualDict = new Dictionary<string, string>();
    }


    public bool InheretGenome(Genome motherGenome, Genome fatherGenome) {
        bool success = true;

        if (motherGenome.numGenes == fatherGenome.numGenes && motherGenome.numConstants == fatherGenome.numConstants) {
            numConstants = motherGenome.numConstants;
            numGenes = motherGenome.numGenes;
            numQuals = motherGenome.numQuals;
            qualDict = motherGenome.qualDict;

            for (int i = 0; i < numGenes; i++) {
                if (motherGenome.geneLabelList[i] == fatherGenome.geneLabelList[i]) {
                    string geneLabel = motherGenome.geneLabelList[i];
                    Gene motherGene = motherGenome.geneDict[geneLabel];
                    Gene childGene = motherGene.ShallowCopy();//This might not be what we want.
                    childGene.SexualReproduction(fatherGenome.geneDict[geneLabel]);
                    geneLabelList.Add(geneLabel);
                    geneDict.Add(geneLabel, childGene);
                    geneIndexDict.Add(geneLabel, i);
   
                } else {
                    success = false;
                    string outputString = "Inheret Genome failed because parents' gene " + i.ToString() + " did not match";
                    Debug.Log(outputString);
                }
            }

            for (int i = 0; i < numConstants; i++) {
                if (motherGenome.constantLabelList[i] == fatherGenome.constantLabelList[i]) {
                    string constantLabel = motherGenome.constantLabelList[i];
                    float constantValue = motherGenome.constantDict[constantLabel];

                    constantLabelList.Add(constantLabel);
                    constantDict.Add(constantLabel,constantValue);
                    constantIndexDict.Add(constantLabel,numConstants);
                        
                } else {
                    success = false;
                    string outputString = "Inheret Genome failed because parents' constant " + i.ToString() + " did not match";
                    Debug.Log(outputString);
                }
            }

        } else {
            success = false;
            string outputString = "Inheret Genome failed because parents' numGenes or numConstants were not same size";
            Debug.Log(outputString);
        }
        return success;
    }

    // This method is for fission and does not call SexualReproduction... no mutation occurs for this reason
    public bool InheretGenome(Genome motherGenome) {
        bool success = true;

        numConstants = motherGenome.numConstants;
        numGenes = motherGenome.numGenes;
        numQuals = motherGenome.numQuals;
        qualDict = motherGenome.qualDict;

        for (int i = 0; i < numGenes; i++) {
            string geneLabel = motherGenome.geneLabelList[i];
            Gene motherGene = motherGenome.geneDict[geneLabel];
            Gene childGene = motherGene.ShallowCopy();//This might not be what we want.
            
            geneLabelList.Add(geneLabel);
            geneDict.Add(geneLabel, childGene);
            geneIndexDict.Add(geneLabel, i);
        }

        for (int i = 0; i < numConstants; i++) {
            string constantLabel = motherGenome.constantLabelList[i];
            float constantValue = motherGenome.constantDict[constantLabel];

            constantLabelList.Add(constantLabel);
            constantDict.Add(constantLabel,constantValue);
            constantIndexDict.Add(constantLabel,numConstants); 
        }   
        return success;
    }

    public void InitGenomeFromSpeciesTemplate(ObjectInfo passedInfo){
        numGenes = 0;
        numConstants = 0;

        for (int i = 0; i<passedInfo.genome.numGenes; i++){
            string geneLabel = passedInfo.genome.geneLabelList[i];
            geneLabelList.Add(geneLabel);
            geneIndexDict.Add(geneLabel, numGenes);
            numGenes++;
            Gene newGene = passedInfo.genome.geneDict[geneLabel].ShallowCopy();
            newGene.GenerateGeneSequence();
            geneDict.Add(geneLabel, newGene);
            
        }
        // there are probably data structure copy operations that would do this more efficiently
        for (int i = 0; i<passedInfo.genome.numConstants; i++){
            constantLabelList.Add(passedInfo.genome.constantLabelList[i]);
            constantDict.Add(passedInfo.genome.constantLabelList[i], passedInfo.genome.constantDict[passedInfo.genome.constantLabelList[i]]);
            constantIndexDict.Add(passedInfo.genome.constantLabelList[i], numConstants);
            numConstants++;
        }

        numQuals = passedInfo.genome.numQuals;
        qualDict = passedInfo.genome.qualDict;
    }

    public void AddGeneToGenome(string label, string[] geneInfo) {
        Gene newGene = new Gene();
        newGene.SetImportedGeneInfo(label, geneInfo);
        geneLabelList.Add(label);
        geneIndexDict.Add(label, numGenes);
        geneDict.Add(label, newGene);
        numGenes += 1;
    }

    public void AddConstantToGenome(string label, string[] constantInfo) {
        constantLabelList.Add(label);
        constantDict.Add(label, float.Parse(constantInfo[0]));
        constantIndexDict.Add(label, numConstants);
        numConstants++;
    }
    
    public void AddQualToGenome(string label, string[] qualInfo) {
        qualDict.Add(label, qualInfo[0]);
        numQuals++;
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
