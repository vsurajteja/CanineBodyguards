using GTA;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using GTA.Native;

// A mod for spawning a character model
// The model name can be entered in the string variable, 'modelName'

// Later added the working to spawn different Ped models of dogs. 
// Using a random(), the different dog models are added to a List of strings and they
//  are created randomly. Other dogs can be added to the list for a more diverse spawning.

// The dogs will be following the player character and attack the Peds that hate the player.
// The dogs are also killed when the player is killed in the game.
public class ScriptSample : Script
{

    int maxSpawnedModels = 10;
    List<Ped> spawnedModels = new List<Ped>();


    public ScriptSample()
    {
        Tick += OnTick;
        KeyUp += OnKeyUp;
        KeyDown += OnKeyDown;

        Interval = 10;
    }

    //Executes on every frame change in the game
    void OnTick(object sender, EventArgs e)
    {

        Player player = Game.Player;

        if (player.IsDead && spawnedModels.Count > 0)
        {
            for (int i = 0; i < spawnedModels.Count; i++)
            {
                spawnedModels[i].Kill();
                spawnedModels.RemoveAt(i);
            }
        }

        for (int i = 0; i < spawnedModels.Count; i++)
        {
            if (spawnedModels[i].IsDead)
            {
                spawnedModels.RemoveAt(i);
            }
        }


    }

    //Executes on releasing the key
    void OnKeyUp(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.H && maxSpawnedModels < 5)
        {
            Ped player = Game.Player.Character;
            GTA.Math.Vector3 spawnLocation = player.Position + (player.ForwardVector * 5);

            //The list of dog models
            List<string> dogList = new List<string>();
            dogList.Add("a_c_chop");
            dogList.Add("a_c_retriever");
            dogList.Add("a_c_shepherd");
            dogList.Add("a_c_rottweiler");
            dogList.Add("a_c_husky");

            //string modelName = "a_c_rottweiler";
            Random rnd = new Random();
            Ped spawned = GTA.World.CreatePed(dogList[rnd.Next(0, dogList.Count)], spawnLocation);
            spawnedModels.Add(spawned);


            int playerGroup = GTA.Native.Function.Call<int>(GTA.Native.Hash.GET_PED_GROUP_INDEX, player.Handle);
            GTA.Native.Function.Call(GTA.Native.Hash.SET_PED_AS_GROUP_MEMBER, spawned.Handle, playerGroup);


            spawned.Task.ClearAllImmediately();
            GTA.Native.Function.Call(GTA.Native.Hash.TASK_COMBAT_HATED_TARGETS_IN_AREA, spawned, 50000, 0);
            GTA.Native.Function.Call(GTA.Native.Hash.SET_PED_KEEP_TASK, spawned, true);

        }

        //When the J key is pressed
        if (e.KeyCode == Keys.J)
        {
            spawnedModels[spawnedModels.Count - 1].Kill();
            spawnedModels.RemoveAt(spawnedModels.Count - 1);
        }

    }

    // Nothing to do when any key is pressed
    void OnKeyDown(object sender, KeyEventArgs e)
    {

    }

}

