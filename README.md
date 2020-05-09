# Flocking (Unity / C#)

![Flocking animation](/doc/flocking.gif)

## What is it?

An clone of famous "boids" flocking algorithm. There is also wall avoidance and goal seeking which are not part of the original concept AFAIK.

# How to use
Open the Unity project and scene and run the program. Use mouse left click to set goal position. Adjust Spawner Inspector settings to change how agents move.

# Features
Use Spawner object's inspector to change flocking parameters. System can tolerate roughly 100 agents before it gets sluggish on my computer, this is in no way an optimized way to do crowd movement for large amount of agents.


# Classes

##  Settings.cs
Class which holds shared agent parameters.

## Spawn.cs
Class which spawns agents and has an instance of Settings to be used runtime for agents.

## Movement.cs
Agent movement class. Each agent cloned from agent prefab has this attached to itself. Agents move using acceleration. I first tried constant velocity, which pretty much looked better, except for steering. That is why there is clamping to minimum velocity, otherwise agents will occasionally "crawl" instead of moving at steady pace.

# About
I created this flocking system for myself, as a learning experience. It is mostly based on reading the papers by Craig Renolds.

# Copyright
Code created by Sami S. use of any kind without a written permission from the author is not allowed. But feel free to take a look.
