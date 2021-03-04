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

    public Transform globalPos;

    public Body(Entity passed, Transform position) {
        thisEntity = passed;
        globalPos = position;

        InitHeight();
    }

    public virtual void InitGameObject(GameObject loadedPrefab) {
        gameObject = GameObject.Instantiate(loadedPrefab, globalPos.position, globalPos.rotation) as GameObject;
        gameObject.name = GetName();

        rigidbody = gameObject.GetComponent<Rigidbody>();
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
        head.transform.position += new Vector3(0, height, 0);
    }
}