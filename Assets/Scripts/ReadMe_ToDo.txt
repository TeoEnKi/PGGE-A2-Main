PunRPC: so that the function runned will be runned in others' game

Refactoring
1. Object pooling of bullet
	a. Added an empty game object containing pre-initialised bullets
	b. Create ObjectPoolBullet script and add to "Pre-initialised bullets" empty game object
	c. Changing the FireBullet(): Instead of instantiating game object, set the game object to active
	d. Changing the destroy gameobject on created, to disabling gameobject

May do:
1. Find a big function to break down
2.abstract class 
