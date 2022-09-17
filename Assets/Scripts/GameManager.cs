using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // to have only 1 game manager script in all levels
    public static GameManager singleton;

    private GroundPiece[] allGroundPieces;

    // Start is called before the first frame update
    void Start()
    {
        SetupNewLevel();
    }

    private void SetupNewLevel()
    {
        allGroundPieces = FindObjectsOfType<GroundPiece>(); // this finds all the ground pieces in the whole scene.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
