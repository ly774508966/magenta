using UnityEngine;
using System.Collections;
using LitJson;

public class PurpleNetwork : MonoBehaviour
{
    public int    number_of_players = 32;
    public int    port_number       = 25000;
    public string password          = "a password";
    public NetworkViewID last_view_id;

    private ArrayList event_listeners; // TODO change to dictionary of arrays per thing

    // TODO Server Listeners? (or just if(server))
    // TODO Direct Responders
    // TODO broadcast player connects and disconnects to others so they may request info.
    // TODO send directly to server


    void Start ()
    {
        instance = this;
    }



    // DEBUG GUI /////////////////////////
    //
    void OnGUI ()
    {
        if (GUILayout.Button ("Host"   )) { launch_server ();         }
        if (GUILayout.Button ("Connect")) { connect_to ("127.0.0.1"); }
        if (Network.isClient) { GUILayout.Label ("Connected as Client."); };
        if (Network.isServer) { GUILayout.Label ("Hosting as Server.");   };

        if (GUILayout.Button ("New ID")) { last_view_id = Network.AllocateViewID (); }

        GUILayout.Label ("View: " + last_view_id.ToString() );
    }



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


    // SERVER EVENTS
    void OnServerInitialized()  { Debug.Log ("Server Initialized."); }
    void OnPlayerDisconnected() { Debug.Log ("Player Disconnected"); }
    void OnPlayerConnected()    { Debug.Log ("Player Connected");    }



    // CLIENT ////////////////////////////
    //
    public void connect_to(string server_host)
    {
        Network.Connect(server_host, port_number, password);
        Debug.Log ("Connecting to Server");
    }


    // CLIENT EVENTS
    void OnConnectedToServer()      { Debug.Log ("Connected to server");      }
    void OnDisconnectedFromServer() { Debug.Log ("Disconnected from server"); }



    // EVENT DISPATCH ////////////////////
    //
    private void add_listener (string event_name, PurpleNetCallback listener)
    {
      Debug.Log ("ADD LISTENER " + event_name);

      // TODO add to array here
    }


    private void broadcast (string event_name, object message)
    {
      Debug.Log ("BROADCAST " + event_name);

      network_view.RPC("receive_broadcast", RPCMode.All, event_name, JsonMapper.ToJson(message));
    }

    private void server_event (string event_name, object message)
    {
      Debug.Log ("SERVER EVENT " + event_name);

      network_view.RPC("receive_event", RPCMode.Server, event_name, JsonMapper.ToJson(message));
    }

    [RPC]
    void receive_broadcast(string event_name, string json_message, NetworkMessageInfo info)
    {
      Debug.Log ("RECEIVED BROADCAST: " + event_name + " -- " + json_message);
      // TODO fire to all listeners for message
    }

    [RPC]
    void receive_event(string event_name, string json_message, NetworkMessageInfo info)
    {
      Debug.Log ("RECEIVED EVENT: " + event_name + " -- " + json_message);
      // TODO rebroadcast after checking some logic? or.. fire server listeners
    }



    // SINGLETON STATIC METHODS ///////////
    //
    public static void AddListener (string event_name, PurpleNetCallback listener)
    {
        //Instance.listeners.Add (listener);
        Instance.add_listener (event_name, listener);
    }


    public static void Broadcast (string event_name, object message)
    {
        Instance.broadcast (event_name, message);
    }

}



// DELEGATES FOR CALLBACK

public delegate void PurpleNetCallback();          // Without message
public delegate void PurpleNetCallback<T>(T arg1); // With message
