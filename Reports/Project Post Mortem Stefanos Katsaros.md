#Stefanos Katsaros

##Dodge this Racing

##Individual Assessment


**Personal work summary in the project:**

*Prototyping cars with unity physics:* The first phase was research and creating prototypes using solely unity physics. Although I was optimistic at start as I had begun working on two different models it slowly became apparent that there were occasional errors that made each model unrealistic and it became clear that in the future if we were to base the project solely on unity’s physics we would have serious problems. Therefore the group decision was made to give up working and relying only on Unity to create the cars and actually implement our own physics.

*RPM:* After dividing the tasks among the team, I began working on the RPM. The vehicle RPM is based off a Corvettes’ Torque/Power diagram since to find the RPM extra calculations are needed. Much math was involved and after it was completed it based on input, and therefore when RPM increases and exceeds 6000 as the pedal is down, the gear increases. Likewise when the pedal is not pushed down or we hit the breaks or we are in reverse rpm can be decreased smoothly transitioning through lower gears. Furthermore depending on the gear we are currently at, the RPM will increase and decrease as needed. Therefore it will increase faster when in first gear and slower when in sixth gear.

*AI using commands:* After reading the AI commands I was in charge of feeding the NPC the commands. It worked similarly to how the data is read from the user, but instead of taking keyboard or joystick input I coded a way so that they read continuously the commands that are given to me. Therefore whenever the AI controlled vehicle is given a different command I made sure that the car would instantly take the new data and use it accordingly to move to different locations. 

*Joystick and controls setup:* I was given the task to setup the control scheme for the game so that 2 player were on the keyboard and 2 more were on Xbox joysticks. Getting grasp of mostly the joystick left axis was a bit tricky but after some testing I got the results needed.

*Sound engineering:* Since I created the RPM measurements I was later in charge of the sound system implementation. This included all in game sounds such as background music and colliding with cars and objects, but also combining the car engine RPM with a realistic car engine sound. This was achieved by having a continuously looping engine sound and moving through its pitch frequency values. Therefore when in low RPM the pitch is lower, and as it increases so does the pitch. Of course this was done so by creating a math formula that converted the RPM to the desired pitch values.

*Game score system and game flow logic:* For the game to have a goal, I implemented the winning and losing system, where each time a vehicle falls behind the camera it is destroyed and the vehicle loses a life. When lives are 0 they will not respawn, and the last remaining vehicle is the winner. Once the game is finished after 7 seconds we are redirected to the main menu.
*UI score & commendation system implementation:* Obviously implementing the above knowledge would not be enough if the user didn’t know about it. Therefore I created a UI that denotes how many lives the user has left as long with adding comments each time a player loses or a player wins.

**Personal Challenges Faced:**

*Teamwork Elements:* My prime concern was working in a group of such a size. I found it difficult to add or base my code on my teammates’ code because some of it was complex and more efficient than my own and I found difficulty adapting to it.

**Successful elements in the game:**

*Realistic car engine behaviour:* I believe that the movement of the vehicle has been made in a very realistic manner. The way the vehicle moves, turns and the speed it gains over time has been done in a way that can be applied in a real life situation.

*Vehicle motion team-work combination:* Having each of us work on an separate part of the vehicle at first, I think that combining all the pieces was done in a very harmonious manner, were everyone had commented adequately so that everyone could understand each other’s code.

*Fun and unique gameplay flow:* The way the game is designed takes us away from how car games are usually developed. All players share the same screen and the interactions between other players provides much amusement.

*Competitive:* For me the core of car games is not only a realistic vehicle motion and game environment but also the level of competition. This game has been made in a way that you have to be competitive and want your opponent to lose, instead of just trying to get to a checkpoint faster than others. The options the player has and the difficult NPC declare this game to be highly competitive and unique.

**Unsuccessful elements in the game:**

*Lack of immersion:* I believe that although the game is extremely fun and competitive, we weren’t able to achieve a high level of immersion. Having all the vehicles and sounds in one screen took away a bit the feeling of being immerged in the game and actually ‘living’ the situation.

*Rear bugs concerning car physics:* Although most bugs have been dealt with, occasional physics bugs may occur. For example sometimes the when a vehicle is in mid-air some results may not go as planned.

**Future improvement**

*Additional game content:* The positive outcome of our game is that it has a great level of future improvements that can be made. Level designs, different tracks, more car vehicle attributes, dynamic obstacles and many more are such examples.

*Customer testing for minor improvements and bug fixes:* Since the game just got out of the development phase, customer testing could provide us with additional improvements to make as well as point out bugs that might have not become apparent so far.

*Extra vehicles:* Since the core logic of how vehicle motion works has been applied, more vehicles can be added, which may have different attributes, different detachable parts and add a greater variety for the user game experience.

*Motorcycle addition:* What could be better than extending the car game to a motorcycle game? As a last expansion to our game I would suggest adding different tracks exclusively for playing with motorcycles.

