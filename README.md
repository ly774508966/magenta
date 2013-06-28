Magenta
=======

Unity 3D network api for Games Learning Society.

Attempting to make it easy to send data over the network with a singleton instance.

Things learned:
---------------

* AllocateViewID jumps between clients, but will always return a unique number.
  ex: client1 last id = 50 then client2 ids will go 48, 49, 100
