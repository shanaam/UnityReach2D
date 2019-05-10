using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;

namespace UXFExamples
{

    public class ExperimentGenerator : MonoBehaviour
    {
        public void Generate(Session session)
		{
			List<int> numTrialArray = session.settings.GetIntList("trials_per_block");
            List<string> arrayValueArray = session.settings.GetStringList("array_value");
            // create two blocks
            //Block block1 = session.CreateBlock(numTrialArray[0]);
            //Block block2 = session.CreateBlock(numTrialArray[1]);
            for (int i = 0; i < numTrialArray.Count; i++)
            {
                session.CreateBlock(numTrialArray[i]);
                session.blocks[i].settings.SetValue("arrayValue", arrayValueArray[i]);
                // this is to ignore some mechanic in this example
                session.blocks[i].settings.SetValue("inverted", false);
            }


            // add catch trials
            MakeCatchTrials(session.blocks[0]);
            MakeCatchTrials(session.blocks[1]);

			// for each trial in the session, 50/50 chance of correct target being on left or right
            foreach (Trial trial in session.Trials)
			{
                TargetPosition pos = Random.value > 0.5 ? TargetPosition.Left : TargetPosition.Right;
				trial.settings.SetValue("correct_target_position", pos);
			}

			//// set the block to be inverted ("go to the opposite target") or not, depending on the participant
   //         bool invertedBlockFirst;

			//try
			//{
   //             invertedBlockFirst = (bool) session.participantDetails["inverted_block_first"];
			//}
			//catch (System.NullReferenceException)
			//{
   //             // during quick start mode, there are no participant details, so we get null reference exception
   //             invertedBlockFirst = Random.value > 0.5;
			//	Debug.LogFormat("Inverted block first: {0}", invertedBlockFirst);
			//}			

			
			//if (invertedBlockFirst)
			//{
   //             session.blocks[0].settings.SetValue("inverted", true);
   //             session.blocks[1].settings.SetValue("inverted", false);
			//}
			//else
			//{
   //             session.blocks[0].settings.SetValue("inverted", false);
   //             session.blocks[1].settings.SetValue("inverted", true);
			//}

		}

		/// <summary>
		/// Modify a block by adding several catch trials and then shuffling the trial list.
		/// </summary>
		/// <param name="block"></param>
		void MakeCatchTrials(Block block)
		{
			int numCatchTrials = block.settings.GetInt("catch_trials_per_block");
			
			if (numCatchTrials > block.trials.Count)
			{
				throw new System.Exception("There shouldn't be more catch trials than total trials per block!");
			}

			for (int i = 0; i < numCatchTrials; i++)
			{
				// double the existing delay time during catch trials
				Trial trial = block.trials[i];
				float delayTime = 2 * trial.settings.GetFloat("delay_time");
                trial.settings.SetValue("delay_time", delayTime); 
			}

			// shuffle the trial order, so the catch trials are in random positions
			block.trials.Shuffle();
		}

    }
}