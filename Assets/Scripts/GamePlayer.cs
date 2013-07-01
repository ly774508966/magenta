using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// An example game player tied to a network client player object
// The network player is created for every connection and is passed
// with the RPC commands to identify the sender
//
public class GamePlayer
{
  public NetworkPlayer player_info;
  public string player_name;

  public List<GameObject> owned_objects;
}
