using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/**
In a networked project, the Server and all of the Clients are executing the same code from the same scripts on the same GameObjects at the same time. 
This means that in a situation with a Server and two Clients, there will be six potential player GameObjects being processed. 
This is because there will be two players and therefore two player GameObjects on the Server. There will also be two players on each of the two Clients. 
This makes a total of six player GameObjects
*/
public class PlayerController : NetworkBehaviour {

	public GameObject bulletPrefab;
	public Transform bulletSpawn;

	void Update()
    {
		
		/**
		All of these player GameObjects were spawned from the same player prefab asset and all share a copy of the same PlayerController script. With a script that derives from NetworkBehaviour, which player “owns” what GameObject is understood automatically and is managed as part of the spawning process. The LocalPlayer is the player GameObject “owned” by the local Client. This ownership is set by the NetworkManager when a Client connects to the Server and new player GameObjects are created from the player prefab. When a Client connects to the Server, the instance created on the local client is marked as the LocalPlayer. All of the other player GameObjects that represent this player - whether this is on the Server or another Client - are not marked as a LocalPlayer.

		By using the boolean check isLocalPlayer, a script can acknowledge or ignore code depending upon whether or not it is owned by the LocalPlayer. In our case, in Update, we have a check that says:
		*/
		// Add a check for isLocalPlayer in the Update function, so that only the local player processes input
		if (!isLocalPlayer) {
			return;
		}

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);
		
		if (Input.GetKeyDown(KeyCode.Space)) {
			Fire();
		}
    }
	
	/**
	This function is only called by the LocalPlayer on their Client. This will make each player see their local player GameObject as blue. The OnStartLocalPlayer function is a good place to do initialization that is only for the local player, such as configuring cameras and input.
	*/
	public override void OnStartLocalPlayer() {
		GetComponent<MeshRenderer>().material.color = Color.blue;
	}
	
	void Fire() {
		
		// Create the Bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate (
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);

		// Add velocity to the bullet
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

		// Destroy the bullet after 2 seconds
		Destroy(bullet, 2.0f);
	}
}
