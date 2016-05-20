#Elio de Berardinis

##Dodge this Racing

##Individual Assessment


**What you did on the project and what responsibilities you took on board on the team?**

The main responsibility that I took on in this project, as a first year part-time student and therefore not involved in the Physics programming, was the implementation of the Graphic User Interface. I formed a team with Federico, another first year part-time student, and we worked collaboratively with the rest of the team programmers, in particular Stefanos and Jean-Pascal, who also did work on the player input system. As a part-time, I started contributing to the project one month into development.
I have implemented the functionalities that regulate the menu scene, which allows players to select up to four cars and enter the game in play mode. The GUI was directly inspired by Nintendo’s Smash Brothers. The system detects automatically when a player is active, based on the controller that is being used. It creates a new player and assigns it the relative control scheme. Players can also be created, assigned a control scheme, set as AI players or deleted by apposite panels. The system supports two joystick controllers and two keyboard based (WASD and Arrows). 
I added methods and optimized some elements in the LevelManager.cs and PlayerSelector.cs scripts that Federico created. I also created a script called MoveSelector.cs that deals with the specific player’s control scheme.
Moreover, the script IconCollider.cs is attached to each car icon in the GUI, to detect if the car is being used by the player. Inside the script the function checkControlType() checks the control type used by the player and makes it possible to select or deselect it.
The functions SelectCar() and DeselectCar() make it possible to drop the token on a given car sprite and select, or deselect it. 

**Details of some of the challenges you faced and how you dealt with them (i.e. your approach to resolving any technical or artistic challenges you faced)**

The most challenging aspects I faced were of technical nature. Being in a team where each individual had his own task it was often difficult to merge the contributions without running into problems. I had to do this on a double layer. First with Federico that was directly working on the same aspects of the game with me (GUI) and then with the rest of the team (mostly Jean-Pascale and Stefanos) to link our interface with the main game. The problems with Federico were mostly organizational that then reflected into technical issues as we were both trying to resolve the same problems but in different ways. This lead to a few discussion on which one was the best solution. Eventually we always agreed on a final implementation. To avoid incurring into this problems again at some point we started having more meetings where we gave each other specific tasks. This led to a seamless team work between Federico and me until the end of the project.
The problems with the rest of the team were mostly on interfacing our GUI with the main game. Because of the lack of Xbox controllers I used my Nintendo GameCube ones. This obviously created some incompatibilities with the main game that had to be corrected and optimized each time a merge was needed. Stefanos was in charge of the control scheme for the main game. Sometimes it was difficult to meet with him because of his busy schedule with other assignments and a part-time job. As a result of this lack of communication my implementation of control schemes and his were radically different at some point. Nonetheless, Stefanos is a skilled programmer and we resolved this very efficiently after a few meetings in person.
I would have liked to add interesting graphics to the GUI at this point but the artists were already overworked with the main assets of the game. Therefore the graphical makeover of the GUI has been postponed and will hopefully be implemented before the second review on May 25th.
Overall I think this project was very ambitious and probably suffered from all the other duties our teammates had. I’m glad we can improve on it for another month.

**Areas of your project which you felt went to plan - things you were particularly pleased about. These could be technical accomplishments, team achievements, great communication, good self-critique & team reviews, efficient task planning etc.**

I am quite happy with the implementation of the GUI scripts I created. They are not at a professional level and they might be slightly unpolished but I feel like getting it work represented a satisfying technical feat. I’m planning to polish them with the extra given time. There have been bugs such as detecting wrong control inputs, selecting the right car and providing the main game with the correct data selected by the players that I’ve been able to clean up and now the GUI works smoothly. There was good communication with the team, although mine consisted mostly with Federico, who worked at the GUI with me. I’m particularly pleased with Jean-Pascal who unofficially took the role of team leader and coordinated everybody’s efforts, complaints and deadlines very well. He also helped me and Federico solve some internal problems on top of those of interfacing the GUI with the main game.
We set up a work environment in Trello and a Facebook group, which were effective in keeping the rhythm and the goals in focus. Perhaps we could have used more efficient tools (Slack) but so far this set up worked quite well. 

**Areas which didn't go quite as well as anticipated - problem areas, tasks which took longer than planned, roadblocks, pipeline issues etc.**

Although GUI programming is relatively straightforward, it took longer than anticipated, given the elaborate template that we chose. The code base could have been cleaner and more efficient. 
I feel like there could have been perhaps a more organized schedule, although it has to be considered how this project had to be developed in the midst of other assignments and deadlines. It is conceivable that we could have used the time resources more efficiently. Probably it could have been a good idea to have a clearer project management approach and a specific person in charge of its implementation. It would have helped and sped up the process. 
I know there were time constraints and people had to prioritize but I would have liked a bit more input from the artists on the GUI that could have been in a more polished state at this point.

**What you'd do different next time - i.e. what did you learn and how would you go about processes or collaborative efforts differently next time? e.g. Task tracking was an issue, so next time we'd use a backlog.**

Federico and I were in a particular position this time as we worked on a very separate task. I would have liked a bit more involvement in the main decisions about the game. Perhaps, with more time and an official leader role in the team this could have been accomplished. Often time I was oblivious of what other teammates were doing and was difficult to keep track. Probably keeping a diary for each person would have helped the others understand better what was going on. Maybe having small individual presentation at each meeting would have helped keeping the team more on track, focused and aligned with the main vision . I would definitely implement this things in a future project. Specifically, electing a project leader is something I would definitely do in the future. Not necessarily to give special powers to a single person but to better convey different ideas and avoid useless discussions. 
I definitely learned how to compromise with people and I think this really helped me and the others in the team. When you work in solo you often think your ideas are the best. You only realize this is not the case when you are challenged by another person who makes you look at the problem from another angle. I wish I had more chances for healthy discussions with more experienced programmers such as Jean-Pascal and also with the artists with whom I had very little contact unfortunately.
Ultimately, this was a very good simulation of a game development team that gave me insights on the many aspects this job entails. Especially, teamwork, people skills, organization play a vital role and are not less important that the core technical skills some co-workers might or might not have. Amazing skills can do nothing against poor time management, disagreement and lack of respect among the team members. I’m glad we were able to pull through our difficulties efficiently and finally produce a working game at this point. I look forward to the polishing session until the end of May.

**EXTRA MONTH UPDATE**

Thanks to the Extra month we were given to polish the game I was able to improve the GUI's functionalities that now include: 

-Player 1 now controls the CPU players tokens in the selection screen, can change other player's control schemes as well as activating/deactivating them.
-Automatic car selection for CPU players if no cars are selected.
-Booking system for the car selection with visual indications.
-Overall graphical improvements with new assets from the Artists.
-Added sounds.
-Other small performance and cosmetics changes.

##Images

**Intro Screen**
![Alt text](https://github.com/JeanPascalEvette/DodgeThisRacing/blob/master/Reports/Pictures/Elio/New_Title.JPG?raw=true"Screen 1")

**Car Select Screen 1**
![Alt text](https://github.com/JeanPascalEvette/DodgeThisRacing/blob/master/Reports/Pictures/Elio/NewGui1.JPG?raw=true"Screen 2")

**Car Select Screen 2**
![Alt text](https://github.com/JeanPascalEvette/DodgeThisRacing/blob/master/Reports/Pictures/Elio/NewGui3.JPG?raw=true"Screen 3")