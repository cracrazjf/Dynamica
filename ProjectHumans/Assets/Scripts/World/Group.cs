using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using System.Linq;
using Random=UnityEngine.Random;

public class Group {
    protected string name;
    public string GetName() { return name; }

    protected static List<Entity> memberList = new List<Entity>();
    protected static Dictionary<string, Entity> memberDict = new Dictionary<string, Entity>();
    public static List<Entity> GetEntityList() { return memberList; }

    public Group(Population population, int numEntities, float density, Vector3 origin) {
        name = population.NameGroup();
        SpawnMembers(population, numEntities, density, origin);
    }

    public Group(List<Entity> passedList) {

        foreach (Entity ent in passedList) {
            memberList.Add(ent);
            memberDict[ent.GetName()] = ent;
        }
    }

    public static void SpawnMembers(Population population, int numEntities, float density, Vector3 origin) {
        // Density in a 1x1 unit area
        float range = numEntities / density;

        for (int i = 0; i < numEntities; i++) {
            float xRan = Random.Range(origin.x - range, origin.x + range);
            float zRan = Random.Range(origin.z - range, origin.z + range);
            Vector3 memberPos = new Vector3 (0, 0, 0);

            Entity newMember = World.AddEntity(population, memberPos);
            World.SaveEntity(newMember, population);
            SaveMember(newMember);
        }
    }

    public static void SaveMember(Entity passed) {
        memberList.Add(passed);
        memberDict.Add(passed.GetName(), passed);
    }

    
}