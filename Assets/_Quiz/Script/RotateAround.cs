using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour {

    public float speed;
	// Update is called once per frame
	void Update () {
        this.transform.RotateAround(-Vector3.forward,Time.deltaTime*speed);
	}
}
