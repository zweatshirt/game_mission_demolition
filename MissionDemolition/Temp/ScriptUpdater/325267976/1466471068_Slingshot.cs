using UnityEngine;
using System.Collections;

public class Slingshot : MonoBehaviour {
    [Header("Set in Inspector")] // a
    public GameObject prefabProjectile;

    [Header("Set Dynamically")] // a
    public GameObject launchPoint; 
    public Vector3 launchPos; // b 
    public GameObject projectile; // b 
    public bool aimingMode; // b
    



    void Awake() {
        Transform launchPointTrans = transform.Find("LaunchPoint"); 
        launchPoint = launchPointTrans.gameObject; 
        launchPoint.SetActive( false );
        launchPos = launchPointTrans.position; // c
}

    void OnMouseEnter() { //print("Slingshot:OnMouseEnter()");
        launchPoint.SetActive( true ); 
    }

    void OnMouseExit() {
        //print("Slingshot:OnMouseExit()"); 
        launchPoint.SetActive( false ); 
    }

}