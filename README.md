# Funat Server
[![Build Status](https://travis-ci.com/Adanlink/Funat_Server.svg?branch=master "Build Status")](https://travis-ci.com/Adanlink/Funat_Server "Build Status")
## Description
This repository saves the server made for a Project of Investigation for my highschool.
It is based on [SaltyEmu](https://github.com/BlowaXD/SaltyEmu "SaltyEmu").
Also the client for which it is thought is for [Funat Client](https://github.com/Adanlink/Funat_Client "Funat Client").
## How to make it work
### What I used
I used Visual Studio Community 2019 and remember that this project uses .NET Core 3.1.

Also for the database part it only works with Microsoft SQL Server (only tested in the Developer Edition of 2017).
### Setting up the database
Once you got the editor or the terminal to build the Login Server or the World Server, if you run one of them, they will produce an error.

This happens because it has generated the configuration file with default values, which does not have the correct credentials to connect to the database.

This configuration file is different for each server, and can be found in the "Config" folder in the same folder as the executable (it is in YAML format).

Once the server gets the correct credentials it should set up the database automatically and not show any error.
### Important notes
If you think that the Login Server is using too much resources define the ProcessorsToUse to the number of logical processors you want it to use for Hashing the passwords.
World Server has a memory leak relationated with the Session of players, causing on the long run to "explode", so watchout with making a lot of connections without restarting the World Server.
## Talking about the architecture
It is really simple and I am aware that things like this are horrible: 
- Hardcoded IP and Port for World Server.
- No direct connection between Login Server and World Server, making the redirection of the connection between them difficult.
- Not saving the Session IDs in a cached server like Redis.

But I did not have time to do it all, I had a limited time (studies, date limit, etc.).

Also here is a preview of the architecture between the client and the servers (sorry it has some gibberish in Spanish, it is recycled from the writen part for the highschool).

![Architecture](https://gyazo.com/cab8cc6116e8f5f242cf11f6a8b1e0c3.png "Architecture")

## Known errors
There are problems for which I also did not have time to fix (probably I can not list all of them):
- The possible incorrect implementation of LazyLoadingProxies in the database, that caused some unexpected responses or conflicts with the Database.
- Not detecting inactivity because lack of time to implement it.
- To fix MovableComponent.cs phyisics.
- Auto regulation of the parameters of Argon2 hashes on the long run.
- World Server memory leak.