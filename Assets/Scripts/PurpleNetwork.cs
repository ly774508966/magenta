using UnityEngine;
using System.Collections;

public class PurpleNetwork : MonoBehaviour
{
    public int    number_of_players = 32;
    public int    port_number       = 25000;
    public string password          = "a password";

    // SERVER

    public void launch_server()
    {
        Debug.Log ("Launching Server");
        Network.incomingPassword = password;
        bool use_nat = !Network.HavePublicAddress();
        Network.InitializeServer (number_of_players, port_number, use_nat);
    }


    void Start () {
      launch_server();
    }


    // TODO broadcast player connects and disconnects to others so they may request info.


    // TODO ID dispatcher


    // SINGLETON
    //
    public static void AddListener (string event_name, PurpleNetCallback listener)
    {
        //Instance.listeners.Add (listener);
    }


    public static void Broadcast (string event_name, object message)
    {
    }

}


// DELEGATES FOR CALLBACK

public delegate void PurpleNetCallback();
public delegate void PurpleNetCallback<T>(T arg1);

