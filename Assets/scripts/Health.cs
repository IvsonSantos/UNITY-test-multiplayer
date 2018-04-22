using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

/**
 * Changes to the player’s current health should only be applied on the Server. 
 * These changes are then synchronized on the Clients. 
 * This is called Server Authority. 
 
	To make our current health and damage system network aware and working under Server authority, 
	we need to use State Synchronization and a special member variable on networked objects called SyncVars. 
	Network synchronized variables, or SyncVars, are indicated with the attribute [SyncVar]. 
*/
public class Health : NetworkBehaviour {

	public const int maxHealth = 100;

	[SyncVar(hook = "OnChangeHealth")]
	public int currentHealth = maxHealth;

	public RectTransform healthBar;

	public bool destroyOnDeath;

	// Add a new array of the type NetworkStartPosition to hold the spawn points.
	private NetworkStartPosition[] spawnPoints;

	public void TakeDamage(int amount)
	{

		if (!isServer)
		{
			return;
		}

		currentHealth -= amount;

		if (currentHealth <= 0)
		{

			if (destroyOnDeath) {
				Destroy (gameObject);
			} else {
				
				currentHealth = maxHealth;

				// called on the Server, but invoked on the Clients
				RpcRespawn ();
			}
		}
	
	}

	/**
	 * This brings us to another tool for State Synchronization: the SyncVar hook. SyncVar hooks will link a function to the SyncVar. These functions are invoked on the Server and all Clients when the value of the SyncVar changes. For more information on SyncVars and SyncVar hooks, please see the page on State Synchronization.
	 * */

	void OnChangeHealth (int health)
	{
		healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
	}

	/**
	 * This will also serve as a way to introduce the [ClientRpc] attribute, which is another tool for State Synchronization.

	ClientRpc calls can be sent from any spawned object on the Server with a NetworkIdentity. Even though this function is called on the Server, it will be executed on the Clients. ClientRpc's are the opposite of Commands. Commands are called on the Client, but executed on the Server. ClientRpc's are called on the Server, but executed on the Client.

	To make a function into a ClientRpc call, use the [ClientRpc] attribute and add “Rpc” as a prefix to the name of the function. This function will now be run on Clients when it is called on the Server. Any arguments will automatically be passed to the Clients as part of the ClientRpc call. For more information on the [ClientRpc] attribute, please see the page on Remote Actions.
	*/
	[ClientRpc]
	void RpcRespawn()
	{
		if (isLocalPlayer)
		{
			// move back to zero location
			//transform.position = Vector3.zero;

			// Set the spawn point to origin as a default value
			Vector3 spawnPoint = Vector3.zero;

			// If there is a spawn point array and the array is not empty, pick a spawn point at random
			if (spawnPoints != null && spawnPoints.Length > 0)
			{
				spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
			}

			// Set the player’s position to the chosen spawn point
			transform.position = spawnPoint;
		}
	}

	void Start ()
	{
		// add a check to test if this GameObject is associated with the Local Player
		if (isLocalPlayer)
		{
			spawnPoints = FindObjectsOfType<NetworkStartPosition>();
		}
	}
}
