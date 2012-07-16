WindowThing
===========

Window thing is a really simple tiling window manager for Windows.

You have two columns of windows, the main one on the left (70%) of the screen, and a thinner one on the right taking the rest. Use hotkeys to move windows to the left or right, all the windows on that side get tiled in a vertical stack.

Apps are ignored until you grab them, so you can just have, say, one app on the left and one on the right and ignore all other apps.

I haven't hacked on this for a while, but from memory it handles multiple monitors okay. It treats each monitor separately.

Currently the (few) hotkeys are fixed:

* ctrl-win J: move the app to the left stack.
* ctrl-win L: move the app to the right stack.
* ctl-win I: move the app to the left and move all other apps to the right.
* ctrl-win K: stop controlling the current app.