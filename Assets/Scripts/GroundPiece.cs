using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GroundPiece : MonoBehaviour
{
    public bool isColored = false;
    
    public void ChangeColor(Color color)
    {
        // get the color of the object
        GetComponent<MeshRenderer>().material.color = color;
        isColored = true;

        GameManager.singleton.CheckComplete(); // important code to make scene move to the next
    }
    
}
