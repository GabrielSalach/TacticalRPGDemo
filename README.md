# Tactical RPG Movement Demo

This is a quick demo I made to create a movement system for a tactical RPG. The codebase is kinda messy especially the CombatManager class.

![Capture d’écran 2023-01-17 132237](https://user-images.githubusercontent.com/60842220/212935488-50bcb083-96fa-4f21-969e-34f5dedd574e.png)

### Default State
When you click on an ally unit, the tiles around it change their colors. Blue means the character can walk on that tile, red means the character can attack.
![Capture d’écran 2023-01-17 132256](https://user-images.githubusercontent.com/60842220/212935771-f5fc9520-5812-464a-b7ff-70f0e2c88f76.png)

### Path highlight
When hovering a tile, an arrow appears, representing the path the character is going to take.
![Capture d’écran 2023-01-17 132309](https://user-images.githubusercontent.com/60842220/212935907-afd6e0c6-0814-4e0e-9e2b-ffbcc7500acb.png)

When hovering an ennemy or an attackable tile, if the selected location is within reach, the game calculates itself a new path to that tile. 
![Capture d’écran 2023-01-17 132330](https://user-images.githubusercontent.com/60842220/212936054-2d54613b-7ea1-4abe-8e61-aed564f442e2.png)
![Capture d’écran 2023-01-17 132409](https://user-images.githubusercontent.com/60842220/212936096-db4c58b9-3bde-4f9d-b6a5-14da4f4dbaf4.png)

However, if the player already has a different path to get to the same cell, the path stays the same.
![Capture d’écran 2023-01-17 132358](https://user-images.githubusercontent.com/60842220/212936177-8e2411a1-b312-4305-be87-a0fa0e48c542.png)
