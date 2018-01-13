using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private int powerupID;  // 0 = tripleshot, 1 = speedboost, 2 = shields

    // Update is called once per frame
    void Update ()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6.3f)
        {
            Destroy(this.gameObject);
        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                
                if (powerupID == 0)
                {
                    //enable tripleshot
                    player.TripleShotPowerupOn();
                }
                else if (powerupID == 1)
                {
                    //enable speedboost
                    player.SpeedboostPowerupOn();
                }
                else if (powerupID == 2)
                {
                    //enable shields
                    player.ShieldPowerupOn();
                }
            }
            
            Destroy(this.gameObject);

        }
    }
}
