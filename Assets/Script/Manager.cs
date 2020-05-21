using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Manager : MonoBehaviour
{
    public GameObject[] twoCharacter=new GameObject[2];
    public Transform[] startpos = new Transform[2];

    public bool CollectDataFromPlayer = false;
    List<string> collectedData = new List<string>();
    StreamWriter tdr;

    float Elaps;
    float timeTrial = 20;

    void Start()
    {

    }
    private void OnApplicationQuit()
    {
        if(CollectDataFromPlayer == true)
        {
            foreach (string td in collectedData)
            {
                tdr.WriteLine(td);
            }
            tdr.Close();
        }
    }
    public void Collect()
    {
        string path = Application.dataPath + "/traningData.txt";
        tdr = File.CreateText(path);
        twoCharacter[0].transform.position = startpos[0].position;
        twoCharacter[1].transform.position = startpos[1].position;
        twoCharacter[0].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        twoCharacter[1].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        CollectDataFromPlayer = true;
    }


    private void FixedUpdate()
    {
        if (CollectDataFromPlayer)
        {
            Elaps += Time.deltaTime;
            string sdata = "";
            float distance = 1 - (Mathf.Abs(twoCharacter[0].transform.position.x - twoCharacter[1].transform.position.x) / 19);
            float myDistance = Round(myDistanceMap(twoCharacter[0].transform.position.x));
            float Xinput = Round(myinputMap(twoCharacter[0].GetComponent<CharacterScript>().Xtrans));
            float negah; // 0 rosh be samt enemy nist 1 hasat
            bool[] myAllAction = twoCharacter[0].GetComponent<CharacterScript>().normalMove;
            float myMove = myActionsMoveRound(myAllAction);
            float myAction = myActionRound(myAllAction);
            if (twoCharacter[0].transform.position.x - twoCharacter[1].transform.position.x > 0) //>
            {
                if (twoCharacter[0].transform.localScale.x > 0)
                {
                    negah = 0;
                }
                else negah = 1;
            }
            else
            {
                if (twoCharacter[0].transform.localScale.x > 0)
                {
                    negah = 1;
                }
                else negah = 0;
            }
            sdata = Round(distance) + "," + myDistance + "," + Xinput + "," + negah + "," + myMove + "," + myAction;
            if (!collectedData.Contains(sdata))
            {
                collectedData.Add(sdata);
            }
            if (Elaps > timeTrial)
            {
                CollectDataFromPlayer = false;
                Elaps = 0;
                twoCharacter[0].transform.position = startpos[0].position;
                twoCharacter[1].transform.position = startpos[1].position;
                twoCharacter[0].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                twoCharacter[1].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                foreach (string td in collectedData)
                {
                    tdr.WriteLine(td);
                }
                tdr.Close();
            }
        }
        
    }


    float myActionsMoveRound(bool[] ac)
    {
        if (ac[2])
        {
            return 1;
        }
        else return 0;
    }
    float myActionRound(bool[] ac)
    {
        if (ac[3])
        {
            return .25f;
        }
        else if (ac[4])
        {
            return .5f;
        }
        else if (ac[5])
        {
            return .75f;
        }
        else if(ac[6])
        {
            return 1;
        }
        else return 0;
    }
    float Round(float needRound)
    {
        return (float)System.Math.Round(needRound, 1);
    }
    //maps
    float myDistanceMap(float d)
    {
        if (d > 0) return (.5f + (d/2) / 9.5f);
        else if (d < 0) return (.5f+(d/2) / 9.5f);
        else return .5f;
    }
    float myinputMap(float d)
    {
        if (d > 0) return (.5f + (d / 2) / 1);
        else if (d < 0) return (.5f + (d / 2) / 1);
        else return .5f;
    }


}
