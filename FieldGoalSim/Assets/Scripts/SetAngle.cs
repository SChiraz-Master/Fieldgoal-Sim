using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetAngle : MonoBehaviour {

    public Slider[] Angles;
    int CurrentObj = 0;

    bool Max = false;
    public Material Mat;

    // Use this for initialization
    void Start ()
    {
		//Mat = GetComponentInChildren<Material>();
	}

    void Update() {

        if (CurrentObj < Angles.Length) { Rotate(); }
        else { if (Input.GetMouseButtonUp(0)) { Mat.SetFloat("_OutlineWidth", 0.05f); GetComponent<Kick>().enabled = true; this.enabled = false; } }

        if(Input.GetMouseButtonDown(0)) { CurrentObj++; }

    }

    void Rotate()
    {
        if (!Max) { Angles[CurrentObj].value += Time.deltaTime; }
        else { Angles[CurrentObj].value -= Time.deltaTime; }

        if (Angles[CurrentObj].value == 1) { Max = true; }
        if (Angles[CurrentObj].value == 0) { Max = false; }
    }

    public void ResetAngles()
    {
        CurrentObj = 0;
        Max = false;
    }
}
