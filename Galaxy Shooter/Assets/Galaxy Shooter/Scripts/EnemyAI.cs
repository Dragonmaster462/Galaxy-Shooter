using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;

    [SerializeField]
    private GameObject _enemyExplosionAnimation;

    private UIManager _uiManager;

	// Use this for initialization
	void Start ()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6.3)
        {
            float randomX = Random.Range(-7.97f, 7.97f);
            transform.position= new Vector3(randomX, 6.07f, 0);
        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            Instantiate(_enemyExplosionAnimation, transform.position, Quaternion.identity);
            Destroy(this.gameObject);

        }
        else if(other.tag =="Laser")
        {      

            if (other.transform.parent != null)
            {
                Destroy(other.transform.parent.gameObject);
            }
            Destroy(other.gameObject);

            Instantiate(_enemyExplosionAnimation, transform.position, Quaternion.identity);
            _uiManager.UpdateScore();
            Destroy(this.gameObject);
        }
    }
}
