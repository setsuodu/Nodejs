using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public bool isLocalPlayer = false; //TODO switch back when networking

    Vector3 oldPosition;
    Vector3 currentPosition;
    Quaternion oldRotation;
    Quaternion currentRotation;

    void Start()
    {
        oldPosition = transform.position;
        currentPosition = oldPosition;
        oldRotation = transform.rotation;
        currentRotation = oldRotation;
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        currentPosition = transform.position;
        currentRotation = transform.rotation;

        if (currentPosition != oldPosition)
        {
            //TODO Networking
            NetworkManager.instance.GetComponent<NetworkManager>().CommandMove(transform.position);
            oldPosition = currentPosition;
        }

        if (currentRotation != oldRotation)
        {
            //TODO Networking
            NetworkManager.instance.GetComponent<NetworkManager>().CommandTurn(transform.rotation);
            oldRotation = currentRotation;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //TODO Networking
            NetworkManager n = NetworkManager.instance.GetComponent<NetworkManager>();
            n.CommandShoot();
        }
    }

    public void CmdFire()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation) as GameObject;
        Bullet b = bullet.GetComponent<Bullet>();
        b.playerFrom = this.gameObject;
        b.GetComponent<Rigidbody>().velocity = bullet.transform.up * 6;
        Destroy(bullet, 2.0f);
    }
}
