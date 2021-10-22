# 2D_Multi_RPG_Prod


https://aiarcost.github.io/2D_Multi_RPG_Prod/

The enemies will now speak to you when they spawn
Typing "/Drop value" with the value being how much gold you want to drop

BUG: when the non master client attempts to drop, it errors out. It is an error that I have not encountered before. The error occurs from PhotonNetwork.Destroy(gameobject); in Pickup script. This error does not occur when the master client does the command.
