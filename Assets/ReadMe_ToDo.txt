PunRPC: so that the function runned will be runned in others' game

Refactoring
1. Object pooling of bullet
	a. Added an empty game object containing pre-initialised bullets
	b. Create ObjectPoolBullet script and add to "Pre-initialised bullets" empty game object
	c. Changing the FireBullet(): Instead of instantiating game object, set the game object to active
	d. Changing the destroy gameobject on created, to disabling gameobject

	may addon. when the bullet is set to active, put it at the back of the list

2. Make the code (in the Player scipt) that handles the fire bullet input cleaner

3. Making the Move function in the Player Moement script cleaner
	a. Put the block of code related to moving player left, right, forward and backwards into a PlayerNavigation function
	b. Put the block of code related to rotating player into a PlayerRotation function
	c. use the 2 functions to replace the 2 block of codes from the Move function.

May do:
1. Find a big function to break down
2.abstract class 
