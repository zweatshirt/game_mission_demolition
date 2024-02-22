
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

    static private MissionDemo S; // a private Singleton

    [Header( "Set in Inspector" )]
    public TextMeshProUGUI uitLevel; // The UIText_Level Text
    public TextMeshProUGUI uitShots; // The UIText_Shots Text
    public TextMeshProUGUI uitButton; // The Text on UIButton_View 
    private Vector3 castlePos1 = new Vector3(35f, -9.5f, 0f); // The place to put castles 
    private Vector3 castlePos2 = new Vector3(40f, -9.5f, 0f);
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

        castle.transform.position = (level <= 4) ? castlePos1 : castlePos2;
        
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
        uitLevel.text = "Level: " +(level + 1)+ " of " +levelMax; 
        uitShots.text = "Shots Taken: " + shotsTaken + " of " + maxShots;
    }


    void Update() { 
        UpdateGUI();
        // Check for level end
        if ( (mode == GameMode.playing) && Goal.goalMet ) { // Change mode to stop checking for level end
            mode = GameMode.levelEnd;

            // Zoom out
            SwitchView("Show Both" );

            // Start the next level in 2 seconds 
            Invoke("NextLevel", 2f);
        }
        else if (mode == GameMode.playing && !Goal.goalMet) {

            // End game if the max shots have been taken
            if (shotsTaken == maxShots && !FollowCam.POI)
                 SceneManager.LoadScene(1);

        }
    }
    

    void NextLevel() {
        level++;

        if (level == levelMax) {
            level = 0;
        }
        StartLevel();
    }


    public void SwitchView( string eView ="" ) {

        if (eView =="") {
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