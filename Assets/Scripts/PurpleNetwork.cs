using UnityEngine;
using System.Collections;

public class PurpleNetwork : MonoBehaviour
{
    public int    number_of_players = 32;
    public int    port_number       = 25000;
    public string password          = "a password";

    private ArrayList event_listeners;


    // SINGLETON /////////////////////////
    //
    private static NetworkView network_view;
    private static PurpleNetwork instance;
    public  static PurpleNetwork Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject game_object = new GameObject ("PurpleNetworkManager"); // Name is unimportant
                instance     = game_object.AddComponent<PurpleNetwork> ();
                network_view = game_object.AddComponent<NetworkView>   ();
            }
            return instance;
        }
    }



    // SERVER ////////////////////////////
    //
    public void launch_server()
    {
        Debug.Log ("Launching Server");
        Network.incomingPassword = password;
        bool use_nat = !Network.HavePublicAddress();
        Network.InitializeServer (number_of_players, port_number, use_nat);
    }


    void Start () {
      instance = this;

      launch_server();
    }


    // TODO broadcast player connects and disconnects to others so they may request info.
    // TODO ID dispatcher


    // INSTANCE METHODS
    //
    private void add_listener (string event_name, PurpleNetCallback listener)
    {
      Debug.Log ("ADD LISTENER " + event_name);
    }



    // SINGLETON STATIC //////////////////
    //
    public static void AddListener (string event_name, PurpleNetCallback listener)
    {
        //Instance.listeners.Add (listener);
        Instance.add_listener (event_name, listener);
    }


    public static void Broadcast (string event_name, object message)
    {
    }

    // TODO Server Listeners
    // TODO Direct Responders

}



// DELEGATES FOR CALLBACK

public delegate void PurpleNetCallback();          // Without message
public delegate void PurpleNetCallback<T>(T arg1); // With message
