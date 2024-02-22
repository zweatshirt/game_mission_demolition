
using UnityEngine;
using System.Collections;
using UnityEngine.UI; // a
using TMPro;
using UnityEngine.SceneManagement;

public enum GameMode { // b 
    idle,
    playing,
    levelEnd
}

public class MissionDemo : MonoBehaviour {

    private static MissionDemo S; // a private Singleton


    [Header( "Set in Inspector" )]
    // The UIText_Level Text
    public TextMeshProUGUI uitLevel; 

     // The UIText_Shots Text
    public TextMeshProUGUI uitShots; 

    // The Text on UIButton_View 
    public TextMeshProUGUI uitButton; 

    // The place to put castles 
    private Vector3 castlePos = new Vector3(28f, -9.5f, 0f);
    
    // shift castle to the right by this amt each new level
    private float shiftCastleVal = 2f;


    public GameObject[] castles; // An array of the castles
    public int maxShots = 7;

    [Header( "Set Dynamically" )]
    public int level; // The current level
    public int levelMax; // The number of levels
    public int shotsTaken;
    public GameObject castle; // The current castle
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot" ; // FollowCam mode

    


    void Start() {
        S = this; // Define the Singleton
        level = 0;
        levelMax = castles.Length; 

        StartLevel();
    }

    void StartLevel() {
        // Get rid of the old castle if one exists 
        if (castle != null ) {
            Destroy( castle );
        }
        // Destroy old projectiles if they exist
        GameObject[] gos = GameObject.FindGameObjectsWithTag( "Projectile" ); 
        foreach (GameObject pTemp in gos) {
            Destroy( pTemp );
        }

        // Instantiate the new castle
        castle = Instantiate< GameObject>( castles[level] );

        if (level > 1) {
            castlePos.x += shiftCastleVal;
        }

        castle.transform.position = castlePos;

        shotsTaken = 0;

        // Reset the camera
        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        // Reset the goal
        Goal.goalMet = false;

        UpdateGUI();
        mode = GameMode.playing;
    }


    void UpdateGUI() {
        // Show the data in the GUITexts
        uitLevel.text = "Level: " + (level + 1)+ " of " + levelMax; 
        uitShots.text = "Shots Taken: " + shotsTaken + " of " + maxShots;
    }


    void Update() { 
        UpdateGUI();

        // Check for level end
        // Change mode to stop checking for level end
        if ( (mode == GameMode.playing) && Goal.goalMet ) {
            mode = GameMode.levelEnd;

            // Zoom out
            SwitchView("Show Both" );

            // Start the next level in 2 seconds 
            Invoke("NextLevel", 2f);
        }
        else if (mode == GameMode.playing && !Goal.goalMet) {

            // End game if the max shots have been taken
            // This is... an iffy way to do this
            if (shotsTaken == maxShots && !FollowCam.POI)
                 SceneManager.LoadScene(1);
            else if (shotsTaken > maxShots) {
                SceneManager.LoadScene(1);
            }

        }

    }
    

    // Update level
    void NextLevel() {
        level++;

        if (level == levelMax) {
            level = 0;
        }
        StartLevel();

    }


    // switch view on button press
    // option 1: close up of slingshot
    // option 2: close up of castle
    // option 3: zoomed out view of both slingshot and castle
    public void SwitchView(string eView ="") {

        if (eView == "") {
            eView = uitButton.text;
        }

        showing = eView;

        switch (showing) {

            case "Show Slingshot":
                FollowCam.POI = null;
                uitButton.text = "Show Castle";
                break;

            case "Show Castle":
                FollowCam.POI = S.castle;
                uitButton.text = "Show Both" ;
                break;

            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth" );
                uitButton.text = "Show Slingshot";
                break;
        } 

    }
    // Static method that allows code anywhere to increment shotsTaken 
    public static void ShotFired() { // d
        S.shotsTaken++;
    }
    
}