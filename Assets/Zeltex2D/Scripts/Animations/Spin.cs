using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public bool IsLeft;
    private float RotateSpeed = 40f;
	
	// Update is called once per frame
	void Update ()
    {
		if (!IsLeft)
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * RotateSpeed, Space.Self);
        }
        else
        {
            transform.Rotate(-Vector3.forward * Time.deltaTime * RotateSpeed * 2f, Space.Self);
        }
	}
}
