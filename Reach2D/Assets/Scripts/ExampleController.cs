using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UXF;
using System;

public class ExampleController : MonoBehaviour {

    Session session;
    //List<int> startingPositions = new List<int>();

    public GameObject target;
    public GameObject homePosition;
    public bool isCursorVisible = true;

    List<int> targetList = new List<int>();
    List<int> shuffledTargetList = new List<int>();
    //int[] shuffledTargetList;

    //GENERATE TRIALS AND BLOCK!!!
    private void Start()
    {
        // disable the whole task initially to give time for the experimenter to use the UI
        gameObject.SetActive(false);
    }
    public void GenerateExperiment(Session experimentSession)
    {
        //get a reference to session
        //whatever Session class thing I give this (expSession) will be the session this thing uses as the private reference I made earlier
        session = experimentSession;

        //after I do _________ ExperimentSession.settings will have all the settings in the JSON file

        //makes the blocks and trials!
        //first grab the settings to figure out trial numbers
        int numAlignedTrials1 = Convert.ToInt32(session.settings["num_trials_aligned_reach_1"]);

        //make the first block
        Block alignedReachBlock1 = session.CreateBlock(numAlignedTrials1);
        alignedReachBlock1.settings["visible_cursor"] = true;

        //make the no_cursor blocks (open JSON file to check the correct names)
        int numNoCursorTrials1 = Convert.ToInt32(session.settings["num_trials_noCursor_reach_1"]);
        Block noCursorBlock1 = session.CreateBlock(numNoCursorTrials1);
        noCursorBlock1.settings["visible_cursor"] = false;

        int minTarget = Convert.ToInt32(session.settings["min_target"]);
        int maxTarget = Convert.ToInt32(session.settings["max_target"]);
        int numTargets = Convert.ToInt32(session.settings["num_targets"]);


        // BELOW: Pseudo random targets 1
        if (Math.Abs(maxTarget - minTarget) % numTargets != 0)
        {
            print("ERROR: Check targets and number of targets!");
            
            //QUIT Program
        }

        int targetStep = Math.Abs(maxTarget - minTarget) / (numTargets - 1);

        for(int i = numTargets; i > 0; i --)
        {
            //add min target to the list
            targetList.Add(minTarget);

            // change minTarget to the next target
            minTarget += targetStep;
        }

        
        //Adam's code
        //var objs = session.settings["test_list"];
        // var list = (List<object>)objs;
        // foreach(object ob in list)
        // {
        //     string integervalue = (string)ob;
        //     print(integervalue);
        // }
        // print(typeof));
    }

    //START A TRIAL!
    //call this next one on the "On Trial Begin" event
    public void StartReachTrial(Trial trial)
    {
        Debug.Log("starting reach trial!");

        //turn off home position at start of a trial
        homePosition.SetActive(false);

        //randomize location of target
        Vector3 newTargetPosition = new Vector3(UnityEngine.Random.Range(-4.5f, 4.5f), UnityEngine.Random.Range(-4.5f, 4.5f), -1);
        Quaternion targetRotation = new Quaternion(0, 0, 0, 0);

        Instantiate(target, newTargetPosition, targetRotation);

        //Pseudorandom targets 2
        if(shuffledTargetList.Count < 1)
        {
            shuffledTargetList = new List<int>(targetList);
            shuffledTargetList.Shuffle();

            //print("SHUFFLED!");
            //foreach (int target in shuffledTargetList)
            //{
            //    print(target);
            //}
        }

        //take the first (or last) number from shuffledTargetList
        int targetLocation = shuffledTargetList[0];

        //instantiate target (NOT REQUIRED if instantiation is done elsewhere using the target Location)
        Debug.Log(targetLocation);

        //remove the used number from list
        shuffledTargetList.RemoveAt(0);
        

        // explicitly convert settings["visible_cursor"] to a boolean for if statement
        string cursorStatus;
        bool visibleCursor = Convert.ToBoolean(trial.settings["visible_cursor"]);

        if (visibleCursor)
        {
            cursorStatus = "visible!";
        }
        else
        {
            cursorStatus = "not visible!";
            isCursorVisible = false;
        }

        //Debug.LogFormat("the cursor is {0}", cursorStatus);
        //trial.result["cursor_visibility"] = trial.settings["visible_cursor"];
    }

    private void Update()
    {
        //m ake the Hand Cursor invisible if
        if (isCursorVisible)
        {
            //CHANGE THIS IF YOU NEED TO(CURSOR IS VISIBLE AS DEFAULT FOR NOW)
        }
        else
        {
            GameObject.Find("Hand Cursor").transform.localScale = new Vector3(0, 0, 0);
        }
    }

    // end session or begin next trial (used for an example, find this in Hand Cursor's OnTriggerEnter method
    public void EndAndPrepare()
    {
        Debug.Log("ending reach trial...");
        session.currentTrial.End();
        if (session.currentTrial == session.lastTrial)
        {
            session.End();
        }
        else
        {
            session.BeginNextTrial();
        }
    }
}
