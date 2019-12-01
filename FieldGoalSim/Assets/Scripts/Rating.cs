using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rating : MonoBehaviour {

    public Canvas GameCanvas;
    public Text CScore;
    //public Text FScore;

    float TotalPts;
    GameManager TheGame = new GameManager();
    Collider FGCollider;

    private void Start()
    {
        TheGame = GameCanvas.GetComponent<GameManager>();
        FGCollider = GetComponent<Collider>();
        TotalPts = 0;
    }

    private void OnTriggerEnter(Collider o)
    {
        if (o.gameObject.tag == "Ball")
        {
            //Physics.IgnoreCollision(FGCollider, o, true);

            float width = GetComponent<Collider>().bounds.size.x;
            float height = GetComponent<Collider>().bounds.size.y;

            Vector3 PtOfImpact = new Vector3(o.transform.position.x, o.transform.position.y, 0);
            Vector3 Center = new Vector3(transform.position.x, transform.position.y, 0);

            float Distance = Vector3.Distance(PtOfImpact, Center);

            float MaxDist = Mathf.Sqrt(Mathf.Pow(Center.x, 2) + Mathf.Pow(Center.y, 2));

            float pts = (1 - (Distance / MaxDist)) * 100;
            CalcPts(pts);
            TheGame.KickIsGood = true;
        }
    }

    void CalcPts(float PtsAmount)
    {
        TotalPts += Mathf.Floor(PtsAmount);
        CScore.text = "Points: " + TotalPts;
        //FScore.text = "Final Score: " + TotalPts;
    }
}
