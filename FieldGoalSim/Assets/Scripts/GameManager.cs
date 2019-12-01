using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject Ball;
    public GameObject GoalZone;
    //public Image MsgBG;
    //public Image MsgBG2;

    public List<Button> UIBtn = new List<Button>();
    public List<Text> UIText = new List<Text>();
    public List<GameObject> UIGO = new List<GameObject>();

    public bool KickIsGood;
    public List<AudioClip> SoundFXS = new List<AudioClip>();

    Kick KickInfo = new Kick();
    AudioSource GameAudio = new AudioSource();
    Rigidbody BallRB;
    float Delay = 3.0f;
    double Dist = 0;


    void Start () {

        NewRound();
        KickInfo = Ball.GetComponent<Kick>();
        BallRB = Ball.GetComponent<Rigidbody>();
        GameAudio = GetComponent<AudioSource>();
    }
	
	void Update () {

        if (KickInfo.IsKicked && BallRB.velocity.magnitude <= 1) { CheckKick(); }

        if (KickInfo.GetWind() != 0)
        {
            if (KickInfo.GetWind() > 0) { UIText[2].text = "Wind: " + System.Math.Round(KickInfo.GetWind() * 3.6f, 2) + " km/h"; UIText[4].text = "->"; }
            else { UIText[2].text = "Wind: " + System.Math.Round( KickInfo.GetWind() * 3.6f * -1, 2) + " km/h"; UIText[4].text = "<-"; }
        }
        else
        {
            UIText[2].text = "No Wind."; UIText[4].text = "-";
        }

	}

    void CheckKick()
    {
        if (KickIsGood)
        {
            GameAudio.PlayOneShot(SoundFXS[0]);
            UIBtn[0].gameObject.SetActive(true);
            UIBtn[2].gameObject.SetActive(true);
            //UIText[0].gameObject.SetActive(true);
            UIGO[1].gameObject.SetActive(true);
            //MsgBG2.gameObject.SetActive(true);
        }
        else
        {
            GameAudio.PlayOneShot(SoundFXS[1]);
            StartCoroutine("DistMissed");

            if(Dist == 0)
            {
                Dist = System.Math.Round((55 - Ball.transform.position.z) * 0.724f, 2);
                UIText[5].text = "You missed by " + Dist + " Yards";
                UIGO[2].gameObject.SetActive(true);
            }

            if (Delay <= 0)
            {
                StopCoroutine("DistMissed");
                KickInfo.IsKicked = false;
                UIBtn[1].gameObject.SetActive(true);
                UIBtn[2].gameObject.SetActive(true);
                UIGO[0].gameObject.SetActive(true);
                UIGO[2].gameObject.SetActive(false);
                Dist = 0;
                Delay = 3.0f;
            }

            //UIText[1].gameObject.SetActive(true);
            //UIText[5].gameObject.SetActive(true);
            //UIText[7].text = "You missed by " + System.Math.Round((55 - Ball.transform.position.z) * 0.724f, 2) + " Yards";
            //UIText[7].gameObject.SetActive(true);
            //MsgBG.gameObject.SetActive(true);
        }
    }

    public void NewRound()
    {
        //5 yards = 3.62
        //1 yard = 0.724

        //Physics.IgnoreCollision(Ball.GetComponent<Collider>(), GoalZone.GetComponent<Collider>(), false);

        if (!GameAudio) { GameAudio = GetComponent<AudioSource>(); } // Because I am calling this elsewhere (from the Ball I believe)
        if (GameAudio.isPlaying) { GameAudio.Stop(); }

        Ball.GetComponent<SetAngle>().enabled = true;
        Ball.GetComponent<SetAngle>().ResetAngles();

        float RandomZ = Ball.GetComponent<Kick>().ResetKick();
        UIText[1].text = (55 - RandomZ) + " Yard Line";

        Ball.GetComponent<Kick>().IsKicked = false;
        Ball.GetComponent<Kick>().enabled = false;

        KickIsGood = false;

        for(int cpt = 0; cpt < UIBtn.Count; cpt++)
        {
            UIBtn[cpt].gameObject.SetActive(false);
        }

        for (int cpt = 0; cpt < UIGO.Count; cpt++)
        {
            UIGO[cpt].gameObject.SetActive(false);
        }

        //for (int cpt = 0; cpt < UIText.Count; cpt++)
        //{
        //    UIText[cpt].gameObject.SetActive(false);
        //    //MsgBG.gameObject.SetActive(false);
        //    //MsgBG2.gameObject.SetActive(false);
        //}
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    private IEnumerator DistMissed()
    {
        Delay -= Time.deltaTime;
        yield return null;
    }
}
