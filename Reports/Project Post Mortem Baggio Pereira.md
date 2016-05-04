# DSW Racing Post Mortem
## Baggio Pereira

### Contribution to the project
The areas in which I have contributed in this project were: 
* Suspension and steering
* Utility
* Camera
* AI

I was heavily involved with the suspension portion of this project, so that the car behaved properly. Initially we had different versions of a model with suspension but settled on a having a trailing arm suspension for our car. The suspension was rather tricky due to the numerous changes the model had gone through but ultimately the suspension was tweaked and an acceptable result was achieved. The suspension was created using fixed joints, hinge joints and spring joints and was easier to modify than to re-code the car physics. Alongside the suspension, I created the basic steering needed to control the car. The steering was easy to create and needed only a few changes so the behaviour of the car is not irregular, this included the visual effect of the wheel i.e. turning in the direction the user wants the car to go. An issue that occurred between the suspensions and steering, was that the visual of the wheels was not behaving correctly once the joints were applied. This was resolved by having additional joints so that the steering has priority to the general rotation to the wheels when in motion.

Another part of the project was the camera update, so that it is always following the car that is leading the race. This part was generally easy as it was a quick check every frame to see which car is ahead. The camera then sets the lead car as the target that it should follow, until another car overtakes it. The code used for the camera update is rather short due to the overall direction of travel in the game is in the Z-Axis and is the axis that is only checked.

For the AI, I worked on the Utility section which is needed for the planner of the car. The Utility gets the state of the car and sets a target which it needs to reach, usually the AI path that is created, it also takes into account an “aggressiveness” parameter which means it will target another car, i.e. the closest car. Upon getting a target, it will then get a direction of travel. It also checks for any obstacles along the way and also the other cars that are on the track. It will then check for the distance between the car and the range of obstacles and cars and diverts the car away from them if it is too close to them unless the car is aggressive. This direction is normalized and then passed back to the planner which creates a set of rules for the car to follow. We had numerous issues with this due to the car not behaving as envisioned. Whilst this is my first AI Utility system, it can be improved so that the car can essentially be a smart and completely avoid anything at any speed or in any direction it is travelling.

I have also helped with the general AI of the car, so the car can have better control but with any AI system, they cannot be perfect and will have flaws in them and will see them usually act oddly until the code is refined.

These contributions were initially created in a separate branch to the project, so the project does not get corrupt and before any modifications were merged to the main project, they were viewed by Jean and modified if needed to fit the current spec of the project. Upon completing one part, I would create a new branch from the latest version on the master branch so that the code I was creating works with the latest build of the project. This helped me keep up with an big changes that may have occurred. 

### What worked well
In this project, initially I believe we had made great progress in the first phase of game. We had different ideas and contributions to how the physics of the car e.g. the suspension, steering, motor, is setup until we came to the final conclusion of which setup we should use. 

The artwork from the artists were very detailed and complete so we can use it for building the game, any modifications that were made to the meshes did not affect the game heavily and were easy to fix. Whilst we had many part timers on the team, the communication between the members were very good and each member knew what was needed of them. Upon completing a task, I would message them to say what I have accomplished and asked if there was anything that is required to be change or added. 

Even though we had members get involved with search for a star and also with their jobs, we had managed to communicate with each other and made time for the car game so that other members were not waiting for a certain task to be accomplished, which would have caused a backlog of tasks that needed to be completed. 

Also Jean-Pascal would write a review for the members that cannot make the team reviews and detail the progress that has been made by everyone and also assign new tasks to each member. This was a great way to pass on the message to everyone, so we all knew that everyone is pulling their weight in the project.

### What didn’t work well
Whilst we did make great progress in the first phase, we also did have a delay, primarily due to the car not responding to the way we intended. The issue arose when the mesh of the car was not responding correctly. There were numerous variations and setups for the car that were tested and this had pushed back my tasks and I was tasked with finding a solution as quickly as possible. The issue I had was to do with the suspension on the mesh, upon getting a suitable solution, it did create an issue where the wheels did not turn in the direction the user wanted to go, even though the car did respond correctly. Whilst this was not a major issue, it was something that did trouble us since it would have been forgotten if we had left it. 

Another issue was the Utility, since I was unsure what was needed for the AI Plan, I had created a system that returned target positions for the car to reach so the AI Plan could create the plan but what was really required was a direction that the car needed to travel. This issue was delayed due to 2 people having created separate sections of the AI for the project but it did help not putting all the work load onto one person.

### Improvements
If doing this project again from the beginning, I would have had the team meetings in the evening of a weekday or on a weekend, rather than in the morning/afternoon, only due to people being at their jobs and not being able to attend the meeting or skype. I would plan to have a group skype meeting in which each member would show their progress on their part of the project in turn, and get some feedback on what they have achieved. 

Also I would have given a time limit in which a task needs to be completed, this may cause an issue with people that have other duties they need to do but it will help in completion and getting the project moving in the right direction without causing a backlog of tasks. Whilst we did use a Trello board to sort out our to-do list and tasks for each member, I believe that it should have been the main way to communicate between members, in which I personally did not use it as much as others. It would have been the best way to say whether a task has been completed in the main build of the game or in a separate build before being added to the game.

### Conclusion
In conclusion, I am very pleased with the current build of the game given the time constraints that people have had and I believe with the additional time, we can get the game to be what we had originally envisioned. 
