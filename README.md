# Flocking animation (Unity / C#)

![Stat system image](/doc/flocking.gif)

## What is it?

An clone of famous "boids" flocking algorithm. There's also wall avoidance and goal seeking which are not part of original concept I think.

# How to use
Open the Unity project and scene and run the program. Use mouse left click to set goal position.

# Features
Use spawner object's inspector to change flocking parameters. System can tolerate roughly 100 agents before it gets sluggish on my computer, this is in no way optimized way to do crowd movement for large amount of agents.


# Classes

##  Settings.cs
Test class holds shared agent parameters.

## Spawn.cs
Class which spawns agents and has an instance of Settings.

## Movement.cs
Agent movement class. Each agent cloned from prefab has this attached to itself


# About
I created this flocking system for myself, as a learning experience. It is mostly based on reading the papers by Craig Renolds.

# Copyright 
Code created by Sami S. use of any kind without a written permission from the author is not allowed. But feel free to take a look.
