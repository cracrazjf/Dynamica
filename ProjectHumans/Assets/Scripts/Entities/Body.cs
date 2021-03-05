using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body {

    protected Entity thisEntity;
    protected float height;
    protected float heightScale;
    protected Rigidbody rigidbody;
    protected GameObject gameObject;

    protected bool[] states;
    protected List<string> stateLabelList;
    protected Dictionary<string, int> stateIndexDict;
    protected Dictionary<string, bool> stateDict;
    
    public bool[] GetStates() { return states; }
    public List<string> GetStateLabels() { return stateLabelList; }
    public Dictionary<string, int> GetStateIndices() { return stateIndexDict; }
    public Dictionary<string, bool> GetStateDict() { return stateDict; }
    public GameObject GetGameObject() { return gameObject; }

    public Transform globalPos;

    public Body(Entity passed, Vector3 position) {
        thisEntity = passed;

        InitHeight();
    }

    public virtual void InitGameObject(GameObject loadedPrefab) {
        gameObject = GameObject.Instantiate(loadedPrefab, globalPos.position, globalPos.rotation) as GameObject;
        gameObject.name = thisEntity.GetName();

        rigidbody = gameObject.GetComponent<Rigidbody>();
        globalPos = gameObject.transform;
        gameObject.SetActive(true);
    }
    
    public void InitHeight() {
        heightScale = thisEntity.GetPhenotype().GetTraitDict()["size"]; 
        height = thisEntity.GetPhenotype().GetTraitDict()["height"] * heightScale; 
    }

    public float GetHeight() { return height; }

    public Vector3 GetXZPosition() {
        return new Vector3(globalPos.position.x, 0f, globalPos.position.z);
    }

    public void VerticalBump(float height) {
        globalPos.position += new Vector3(0, height, 0);
    }
}