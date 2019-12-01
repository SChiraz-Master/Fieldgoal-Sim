using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kick : MonoBehaviour {

    public float Bounce = 0.9f; // Determines how the ball will be bouncing after landing. The value is [0..1]
    public float BasePower = 10f;
    public float ForceCap = 5.0f; // Pressing time upper limit
    public Transform CameraT;

    Rigidbody RB;
    public Material Mat;
    //--
    //********************************************************************************************************
    //-- Setting Force of Trajectory
    public Scrollbar PowerDisplay;

    int RoundCount = 0;
    float PowerDispSize = 0;
    float TKickPress; // Keeps time, when you press button
    float TCharge; // Keeps time interval between button press and release 
    Vector3 WindForce = new Vector3(0, 0, 0);
    Vector3 BallVelocity; // Keeps rigidbody velocity, calculated in FixedUpdate()
    Vector3 PtOfImpact;
    //--
    //********************************************************************************************************
    //-- Setting Angle of Trajectory
    public Slider XZAngle;
    public Slider YZAngle;

    float YRot;
    float XRot;
    //--
    //********************************************************************************************************
    //-- Game Manager Related
    public bool IsKicked;
    //--

    private void Awake()
    {
        RB = GetComponent<Rigidbody>();
        //Mat = GetComponentInChildren<Material>();
    }

    void Update()
    {
        BuildPower();
        if(IsKicked && RB.velocity.magnitude <= 1) { this.enabled = false; /*IsKicked = false;*/ }
    }

    void FixedUpdate()
    {
        if (TCharge != 0)
        {
            SetYAngle();
            SetXAngle();

            RB.constraints = RigidbodyConstraints.None;
            RB.AddForceAtPosition(new Vector3(BasePower * Mathf.Clamp(TCharge, 0.0f, ForceCap) * Mathf.Sin(XRot),
                                            BasePower * Mathf.Clamp(TCharge, 0.0f, ForceCap) * Mathf.Sin(YRot),
                                            BasePower * Mathf.Clamp(TCharge, 0.0f, ForceCap) * Mathf.Cos(YRot)),
                                            PtOfImpact,
                                            ForceMode.VelocityChange);
            TCharge = 0;
            IsKicked = true;
        }
        if (IsKicked) { AddWind(); }
        BallVelocity = RB.velocity;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ground")
        {
            GetComponent<Rigidbody>().velocity = new Vector3(BallVelocity.x, -BallVelocity.y * Mathf.Clamp01(Bounce), BallVelocity.z);
        }
    }

    void BuildPower()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TKickPress = Time.time;
            StartCoroutine("MeterFill");
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopCoroutine("MeterFill");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Ball")
                {
                    Mat.SetFloat("_OutlineWidth", 0.0f);
                    TCharge = Time.time - TKickPress;
                    PtOfImpact = hit.point;
                }
            }
        }
    }
    private IEnumerator MeterFill()
    {
        PowerDispSize = 0;
        PowerDisplay.GetComponent<Scrollbar>().size = 0;

        while (PowerDisplay.GetComponent<Scrollbar>().size < 1)
        {
            PowerDispSize += Time.deltaTime;
            PowerDisplay.GetComponent<Scrollbar>().size = PowerDispSize / 5;
            yield return null;
        }
    }

    void SetYAngle()
    {
        float Coef = YZAngle.value - 0.5f;
        YRot = 45 + (Coef * 30 / 0.5f);
        YRot = Mathf.Deg2Rad * YRot;
    }

    void SetXAngle()
    {
        float Coef = XZAngle.value - 0.5f;
        XRot = Coef * 15 / 0.5f;
        XRot = Mathf.Deg2Rad * XRot;
    }

    public float ResetKick()
    {
        StopCoroutine("MeterFill");
        PowerDispSize = 0;
        PowerDisplay.GetComponent<Scrollbar>().size = 0;

        RB.velocity = new Vector3(0, 0, 0);
        RB.constraints = RigidbodyConstraints.FreezeAll;

        float RandomZ = Random.Range(1, 46);
        transform.rotation = new Quaternion(0, 0, 0, 0);
        transform.position = new Vector3(0, -0.505f, RandomZ * 0.724f);
        CameraT.transform.position = new Vector3(0, 2, RandomZ * 0.724f - 3);
        RoundCount++;

        if (RoundCount >= 3)
        {
            int Direction = Random.Range(0, 5);
            float RandomWind = Random.Range(5f, 15f);
            if (Direction >= 3) { RandomWind *= -1; }
            else if (Direction == 0) { RandomWind = 0; }
            WindForce = new Vector3(RandomWind, 0, 0);
        }

        return RandomZ;
    }

    void AddWind()
    {
        RB.AddForce(WindForce * Time.deltaTime, ForceMode.Impulse);
    }

    public float GetWind()
    {
        return WindForce.x;
    }
}
