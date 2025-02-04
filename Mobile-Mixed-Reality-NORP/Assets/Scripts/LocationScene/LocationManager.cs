using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

#if UNITY_ANDROID
using UnityEngine.Android;
#endif




public class LocationManager : MonoBehaviour
{

    [SerializeField] GameObject LocationTextGO;
    private TextMeshPro LocationText;

    [SerializeField] GameObject Explainer;
    [SerializeField] GameObject audioSource;

    [SerializeField] float locationThreshold;

    // location based --> cafeteria
    [SerializeField] float cafeteriaLatitude = 40.8913894f;
    [SerializeField] float cafeteriaLongitude = 29.3798876f;
    [SerializeField] List<GameObject> cafeteriaObjects;
    [SerializeField] AudioClip CafeteriaClip;

    // location based --> IC
    [SerializeField] float ICLatitude = 40.89021f;
    [SerializeField] float ICLongitude = 29.37744f;
    [SerializeField] List<GameObject> ICObjects;
    [SerializeField] AudioClip ICClip;


    // location based --> grass
    [SerializeField] float grassLatitude = 40.8913442f;
    [SerializeField] float grassLongitude = 29.3791577f;
    [SerializeField] List<GameObject> grassObjects;
    [SerializeField] AudioClip GrassClip;

    // location based --> FENS
    [SerializeField] float FENSLatitude = 40.8906637f;
    [SerializeField] float FENSLongitude = 29.3797448f;
    [SerializeField] List<GameObject> FENSObjects;
    [SerializeField] AudioClip FENSClip;
    

    // location based --> FMAN
    [SerializeField] float FMANLatitude = 40.8920220f;
    [SerializeField] float FMANLongitude = 29.3790876f;
    [SerializeField] List<GameObject> FMANObjects;
    [SerializeField] AudioClip FMANClip;


    // location based --> Dorms
    [SerializeField] float DormsLatitude = 40.892652f;
    [SerializeField] float DormsLongitude = 29.382913f;
    [SerializeField] List<GameObject> DormObjects;
    [SerializeField] AudioClip DormsClip;



    private LogicManager logicManager;
    private ObjectInitializer objectInitializer;

    private float _Distance;
    // private float timer = 0f;
    // private float checkInterval = 1f;
    private bool alreadyInBuilding = false;

    private float minDistance = float.MaxValue;
    private string minDistancedPlace;

    private float oldLocationLongitude = 0;
    private float oldLocationLatitude = 0;

    // Start is called before the first frame update
    void Start()
    {
        LocationText = LocationTextGO.GetComponent<TextMeshPro>();
        logicManager = FindObjectOfType<LogicManager>();
        objectInitializer = FindObjectOfType<ObjectInitializer>();
        // objectInitializer.InitializeObjectsInCircle(ICObjects);
        #if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }
        #endif

        if (!Input.location.isEnabledByUser)
        {
            LocationText.text = "Location is not enabled...";
        }
        else
        {
            Input.location.Start();
        }

        // logicManager.SpawnHoca(Explainer);

    }

    // Update is called once per frame
    void Update()
    {
        // timer += Time.deltaTime;

        // if (timer >= checkInterval)
        // {

            // timer = 0f;

        minDistance = float.MaxValue;

        if (Input.location.status == LocationServiceStatus.Running)
        {
            LocationInfo currentLocation = Input.location.lastData;

            if (currentLocation.latitude == oldLocationLatitude && currentLocation.longitude == oldLocationLongitude)
            {
                return;
            }
            oldLocationLatitude = currentLocation.latitude;
            oldLocationLongitude = currentLocation.longitude;

            // cafeteria
            if (IsCloseToLocation(currentLocation.latitude, currentLocation.longitude, cafeteriaLatitude, cafeteriaLongitude, "Cafeteria"))
            {
                LocationText.text = "Cafeteria: " + _Distance.ToString() + " meters";
                if (!alreadyInBuilding)
                {
                    alreadyInBuilding = true;
                    AudioSource _audioSource = audioSource.GetComponent<AudioSource>();
                    _audioSource.clip = CafeteriaClip;
                    _audioSource.Stop();
                    _audioSource.Play();

                    objectInitializer.InitializeObjectsInCircle(cafeteriaObjects);
                    logicManager.SpawnHoca(Explainer);
                }
            }
            // FENS
            else if (IsCloseToLocation(currentLocation.latitude, currentLocation.longitude, FENSLatitude, FENSLongitude, "FENS"))
            {
                LocationText.text = "FENS: " + _Distance.ToString() + " meters";
                if (!alreadyInBuilding)
                {
                    alreadyInBuilding = true;
                    AudioSource _audioSource = audioSource.GetComponent<AudioSource>();
                    _audioSource.clip = FENSClip;
                    _audioSource.Stop();
                    _audioSource.Play();

                    objectInitializer.InitializeObjectsInCircle(FENSObjects);
                    logicManager.SpawnHoca(Explainer);

                }

            }

            //grass
            else if (IsCloseToLocation(currentLocation.latitude, currentLocation.longitude, grassLatitude, grassLongitude, "Grass"))
            {
                LocationText.text = "Grass: " + _Distance.ToString() + " meters";
                if (!alreadyInBuilding)
                {
                    alreadyInBuilding = true;
                    AudioSource _audioSource = audioSource.GetComponent<AudioSource>();
                    _audioSource.clip = GrassClip;
                    _audioSource.Stop();
                    _audioSource.Play();

                    objectInitializer.InitializeObjectsInCircle(grassObjects);
                    logicManager.SpawnHoca(Explainer);

                }
            }
            // FMAN
            else if (IsCloseToLocation(currentLocation.latitude, currentLocation.longitude, FMANLatitude, FMANLongitude, "FMAN"))
            {
                LocationText.text = "FMAN: " + _Distance.ToString() + " meters";
                if (!alreadyInBuilding)
                {
                    alreadyInBuilding = true;
                    AudioSource _audioSource = audioSource.GetComponent<AudioSource>();
                    _audioSource.clip = FMANClip;
                    _audioSource.Stop();
                    _audioSource.Play();

                    objectInitializer.InitializeObjectsInCircle(FMANObjects);
                    logicManager.SpawnHoca(Explainer);

                }
            }
            // IC
            else if (IsCloseToLocation(currentLocation.latitude, currentLocation.longitude, ICLatitude, ICLongitude, "Library"))
            {
                LocationText.text = "Library: " + _Distance.ToString() + " meters";
                if (!alreadyInBuilding)
                {
                    alreadyInBuilding = true;
                    AudioSource _audioSource = audioSource.GetComponent<AudioSource>();
                    _audioSource.clip = ICClip;
                    _audioSource.Stop();
                    _audioSource.Play();

                    objectInitializer.InitializeObjectsInCircle(ICObjects);
                    logicManager.SpawnHoca(Explainer);

                }
            }
            // dorms (A4)
            else if (IsCloseToLocation(currentLocation.latitude, currentLocation.longitude, DormsLatitude, DormsLongitude, "Dorms"))
            {
                LocationText.text = "Dorms: " + _Distance.ToString() + " meters";
                if (!alreadyInBuilding)
                {
                    alreadyInBuilding = true;
                    AudioSource _audioSource = audioSource.GetComponent<AudioSource>();
                    _audioSource.clip = DormsClip;
                    _audioSource.Stop();
                    _audioSource.Play();

                    objectInitializer.InitializeObjectsInCircle(DormObjects);
                    logicManager.SpawnHoca(Explainer);

                }
            }
            else
            {
                if (alreadyInBuilding)
                {
                    objectInitializer.DestroyObjectsWithTag("TempLocationObject");
                }
                alreadyInBuilding = false;
                LocationText.text = "Unknown Location. Closest =>" + minDistancedPlace + ": " + minDistance.ToString() + " m";
                //healthText.text = "90";

            }
        }
        else
        {
            if (alreadyInBuilding)
            {
                objectInitializer.DestroyObjectsWithTag("TempLocationObject");
            }
            alreadyInBuilding = false;
            LocationText.text = "Location not verified...";

            if (!Input.location.isEnabledByUser)
            {
                LocationText.text = "Location is not enabled...";
            }
            else
            {
                Input.location.Start();
            }

        }
        // }
    }


    bool IsCloseToLocation(float currentLatitude, float currentLongitude, float targetLatitude, float targetLongitude, string place)
    {
        float distance = (float)GetDistance(currentLongitude, currentLatitude, targetLongitude, targetLatitude);

        _Distance = distance;
        if(distance < minDistance)
        {
            minDistance = distance;
            minDistancedPlace = place;
        }
        return distance < locationThreshold;
    }

    public double GetDistance(double longitude, double latitude, double otherLongitude, double otherLatitude)
    {
        var d1 = latitude * (Math.PI / 180.0);
        var num1 = longitude * (Math.PI / 180.0);
        var d2 = otherLatitude * (Math.PI / 180.0);
        var num2 = otherLongitude * (Math.PI / 180.0) - num1;
        var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

        return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
    }
}
