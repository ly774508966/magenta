using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameManagerStuff : MonoBehaviour
{
    // TODO example creating object.. getting id, and it going out to everyone
    // since this will have to happen for all clients on join to keep ids in sync
    // and send all objects to a new player. and track players on server
    // a player can modify own objects only.

    void Start ()
    {
        // Lets register with the network to handle game events.
        //

        // Event from basic server.
        PurpleNetwork.AddListener ("AddPlayer",    add_player_callback);

        // User created event with link transfer message
        PurpleNetwork.AddListener<string> ("LinkTransfer", link_transfer_callback);

        // User created event with no arguments and a second listener
        PurpleNetwork.AddListener ("Ping", ping_callback);
        PurpleNetwork.AddListener ("Ping", ping_two_callback);

        // TODO listen only on server for 'request game state' which will be in charge of sending commands directly  to one player
    }



    void Update ()
    {
        // Lets send a game event.
        //
        if (Input.GetButtonDown("Fire2"))
        {
            LinkTransferMessage link_transfer_message = new LinkTransferMessage();

            link_transfer_message.first_id  = 25;
            link_transfer_message.second_id = 32;
            link_transfer_message.link_type = "superduper";

            PurpleNetwork.Broadcast ("LinkTransfer", link_transfer_message);
        }

        if (Input.GetButtonDown("Fire3"))
        {
            PurpleNetwork.Broadcast ("Ping");
        }
    }


    // Game specific methods
    //

    void add_player_callback() // FIXME player message argument next
    {
        Debug.Log ("player callback!");
    }


    void link_transfer_callback(string json_message) // TODO GET JSON
    {
        Debug.Log ("link transfer callback! ::: "+ json_message);
    }


    void ping_callback()
    {
        Debug.Log ("ping callback!");
    }

    void ping_two_callback()
    {
        Debug.Log ("the other ping callback!");
    }
}


// GAME NETWORK MESSAGES
//

[System.Serializable]
public class LinkTransferMessage
{
    public int    first_id;
    public int    second_id;
    public string link_type;
}
