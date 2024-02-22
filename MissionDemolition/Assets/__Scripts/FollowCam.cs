using UnityEngine;
using System.Collections;


public class FollowCam : MonoBehaviour {
    public static GameObject POI; // The static point of interest // a
    
    [Header("Set in Inspector")] 
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;

    [Header("Set Dynamically")]
    public float camZ; // The desired Z pos of the camera
    public AudioSource wind;
    public bool windPlayed = false;
    Rigidbody rb; // projectile rigidbody
    public float volume;
    public float minVolume;
    public float maxVolume;
    public float minVelocityMag;
    public float maxVelocityMag;
    public float normalizedMagnitude;


    void Awake() {
        camZ = this.transform.position.z;
    }

    void Start() {
        wind.playOnAwake = false;
        minVolume = 0f;
        maxVolume = 1f;
        minVelocityMag = 1f;
        maxVelocityMag = 1f;
    }
    
    void FixedUpdate () {
    // if there's only one line following an if, it doesn't need braces 
        // if (POI == null) return; // return if there is no poi // b
        // // Get the position of the poi
        // Vector3 destination = POI.transform.position;


        Vector3 destination;
        // If there is no poi, return to P:[ 0, 0, 0 ]
        if (POI == null ) {
            destination = Vector3.zero;
            windPlayed = false;
        }  
        else {
            // Get the position of the poi
            destination = POI.transform.position;
            // If poi is a Projectile, check to see if it's at rest 
            if (POI.tag == "Projectile" ) {
                rb = POI.GetComponent<Rigidbody>();

                if (destination.x > -10 && windPlayed == false)  {
                    wind.Play();
                    windPlayed = true;
                }
                
                changeWindVolume();

                // if it is sleeping (that is, not moving)
                if ( POI.GetComponent<Rigidbody>().IsSleeping() ) {
                    // return to default view
                    POI = null ;
                    wind.Stop();
                    // in the next update
                    return ;
                }
            }
        }
        
        // Limit the X & Y to minimum values
        destination.x = Mathf.Max( minXY.x, destination.x );
        destination.y = Mathf.Max( minXY.y, destination.y );
        // Interpolate from the current Camera position toward destination 
        destination = Vector3.Lerp(transform.position, destination, easing);


        // Force destination.z to be camZ to keep the camera far enough away 
        destination.z = camZ;
        // Set the camera to the destination
        transform.position = destination;
        // Set the orthographicSize of the Camera to keep Ground in view
        Camera.main.orthographicSize = destination.y + 10;
    }


    // updates wind volume based on projectile velocity
    void changeWindVolume()  {

        float magnitude = rb.velocity.magnitude; 
        if (magnitude < minVelocityMag) {
            minVelocityMag = magnitude;
        }

        if (rb.velocity.magnitude > maxVelocityMag) {
            maxVelocityMag = magnitude;
        }

        normalizedMagnitude = 
            (magnitude - minVelocityMag) / (maxVelocityMag - minVelocityMag); 

        Debug.Log("Velocity magnitude normalized: " + normalizedMagnitude);

        if (normalizedMagnitude <= 1) {
            this.volume = Mathf.Lerp(minVolume, maxVolume, normalizedMagnitude);
            wind.volume = this.volume;
            Debug.Log("Volume: " + this.volume);
        }

    }

}
