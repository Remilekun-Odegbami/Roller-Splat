using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

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

    private void Awake() // method called before the start method
    {
        if(singleton == null)
        {
            singleton = this; // the current game manager becomes the singleton
        } else if(singleton != this)
        {
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject); // destroy only if the current game manager is not the singleton
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        SetupNewLevel();
    }

    public void CheckComplete()
    {
        bool isDone = true;

        for (int i = 0; i < allGroundPieces.Length; i++)
        {
            if (allGroundPieces[i].isColored == false) // if all ground pieces are not colored
            {
                isDone = false; // do not move to the next scene or level
                break;
            }
        }

        if (isDone) // if it is
        {
            // show next level
            NextLevel();
        }
    }
       private void NextLevel()
        {
        if(SceneManager.GetActiveScene().buildIndex == 3) // if game has 20 scene, it'll be == 20
        {
            SceneManager.LoadScene(0); // load the first scene
        } else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // load the next scene
        }
           
        }
   
}
