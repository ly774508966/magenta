using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class PurpleNetwork : MonoBehaviour
{
    public int    number_of_players = 32;
    public int    port_number       = 25000;
    public string password          = "a password";
    public NetworkViewID last_view_id;

    private Dictionary<string, PurpleNetCallback> event_listeners = new Dictionary<string, PurpleNetCallback>();
    private Dictionary<string, Type> callback_types = new Dictionary<string, Type>();


    void Start ()
    {
        instance = this;
    }



    // SINGLETON /////////////////////////
    //
    private static NetworkView   network_view;
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

    private void wakie() { /* just get our instance up*/ }



    // SERVER ////////////////////////////
    //
    public void launch_server()
    {
        Instance.wakie ();

        Network.incomingPassword = password;
        bool use_nat = !Network.HavePublicAddress();
        Network.InitializeServer (number_of_players, port_number, use_nat);
    }


    // SERVER EVENTS
    void OnServerInitialized()                      { OnConnectedToServer(); /* For local client */ }
    void OnPlayerDisconnected(NetworkPlayer player) {  }
    void OnPlayerConnected(NetworkPlayer player)    {  } // We dont really care, since the player will register themselves with the game



    // CLIENT ////////////////////////////
    //
    public void connect_to(string server_host)
    {
        Instance.wakie ();
        Network.Connect(server_host, port_number, password);
    }


    // CLIENT EVENTS
    void OnConnectedToServer()      { /* TODO trigger local I'm connected events */}
    void OnDisconnectedFromServer() { }



    // EVENT DISPATCH ////////////////////
    //
    private void add_listener <T> (string event_name, PurpleNetCallback listener)
    {
        if (!event_listeners.ContainsKey (event_name))
        {
          event_listeners.Add(event_name, null);
        }

        // delegates can be chained using addition
        event_listeners[event_name] += listener;
    }


    // SEND
    private void broadcast (string event_name, object message)
    {
        string json_message = JsonMapper.ToJson(message);
        network_view.RPC("receive_event_message", RPCMode.All, event_name, json_message);
    }

    private void to_server (string event_name, object message)
    {
        string json_message = JsonMapper.ToJson(message);

        if (Network.isServer)
        {
          receive_event_message(event_name, json_message, new NetworkMessageInfo());
        }
        else
        {
          network_view.RPC("receive_event_message", RPCMode.Server, event_name, json_message);
        }
    }



    // RECEIVE
    [RPC]
    void receive_event_message(string event_name, string json_message, NetworkMessageInfo info)
    {
        event_listeners[event_name](json_message);
    }



    // CLASS SINGLETON METHODS ///////////
    //
    public static void AddListener<T> (string event_name, PurpleNetCallback listener)
    {
        Instance.add_listener<T> (event_name, listener);
    }


    public static void Broadcast (string event_name, object message)
    {
        Instance.broadcast (event_name, message);
    }

    public static void ToServer (string event_name, object message)
    {
        Instance.to_server (event_name, message);
    }


    public static void LaunchServer ()
    {
      Instance.launch_server ();
    }


    public static void ConnectTo (string host)
    {
      Instance.connect_to (host);
    }

}



// DELEGATES FOR CALLBACK

public delegate void PurpleNetCallback(object converted_object); // With message



// MESSAGE CLASSES
public class PurpleMessage
{
  NetworkMessageInfo message_info;
}

public class EmptyMessage : PurpleMessage
{
}
