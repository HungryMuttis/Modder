# Modder [WORK IN PROGRESS]

Modder is a program made to make it easier to mod other programs (made for Modder) and/or work with different people on the same project

## How does it work?

Modder works by loading mods from `Mods/` directory. It doesn't do much by itself, but can do anything with the mods.

All of the mods must implement the `IGameMod` interface which is defined in `Mod` project.
Also there are different categories of mods:

* Core
* Core content
* Library
* Core mod
* Mod

All of the mod type descriptions can be found in `Mod/Mod.cs`
