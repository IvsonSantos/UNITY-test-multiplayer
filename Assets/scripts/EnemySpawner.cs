using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour  {

	public GameObject enemyPrefab;
	public int numberOfEnemies;

	// The class uses an implementation of the virtual function OnStartServer. 
	// OnStartServer is called on the Server when the Server starts listening to the Network.
	public override void OnStartServer()
	{
		// When the server starts, a number of enemies are generated at a random position and rotation and then these are spawned using NetworkServer.Spawn(enemy)
		for (int i=0; i < numberOfEnemies; i++)
		{
			var spawnPosition = new Vector3(
				Random.Range(-8.0f, 8.0f),
				0.0f,
				Random.Range(-8.0f, 8.0f));

			var spawnRotation = Quaternion.Euler( 
				0.0f, 
				Random.Range(0,180), 
				0.0f);

			var enemy = (GameObject)Instantiate(enemyPrefab, spawnPosition, spawnRotation);
			NetworkServer.Spawn(enemy);
		}
	}
}
