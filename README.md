# Modder [WORK IN PROGRESS]

Modder is a program made to make it easier to mod other programs (made for Modder) and/or work with different people on the same project

## How does it work?

Modder works by loading mods from `Mods/` directory. It doesn't do much by itself, but can do anything with the right mods installed.

## How to make a mod?
I suggest you to make mods in `C#` (idk how to do it with other languages if it is possible possible at all).
All of the mods must implement the `IGameMod` interface which is defined in Mod project.
There are different categories of mods:

* `Core render` + only 1 can be mounted at a time (should not include any contend, only the rendering)
* `Core content` + a mod that adds the core features of a game (can be multiple mounted at once)
* `Library` - a library that others mods use
* `Core render mod` - a mod that expands the rendering
* `Mod` - a mod that adds content to the game

All of the descriptions can be found in `Mod/Mod.cs`

When the mod is started the `[NOT IMPLEMENTED]` event is fired.
Other events do other things (see their respective comment)
