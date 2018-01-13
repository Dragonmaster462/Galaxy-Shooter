using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool canTripleShot = false;
    public bool isSpeedboostActive = false;
    public bool isShieldActive = false;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _explosionAnimation;
    [SerializeField]
    private GameObject _shield;

    [SerializeField]
    private float _fireRate = 0.25f;

    private float _canFire = 0.0f;

    [SerializeField]
    private float _speed = 5.0f;

    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private GameObject[] _engines;

    private UIManager _uiManager;
    private GameManager _gameManager;
    private SpawnManager _spawnManager;
    private AudioSource _audioSource;

    private int hitCount = 0;

	// Use this for initialization
	void Start ()
    {
        transform.position = new Vector3(0, 0, 0);

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_uiManager != null)
        {
            _uiManager.UpdateLives(_lives);
        }

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager != null)
        {
            _spawnManager.StartSpawnRoutines();
        }

        _audioSource = GetComponent<AudioSource>();

        hitCount = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        Movement();

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (Time.time > _canFire)
        {
            _audioSource.Play();
            if (canTripleShot)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.7f, 0), Quaternion.identity);
            }

            _canFire = Time.time + _fireRate;
        }
    }

    private void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (isSpeedboostActive)
        {
            transform.Translate(Vector3.right * _speed * horizontalInput * 1.5f * Time.deltaTime);
            transform.Translate(Vector3.up * _speed * verticalInput * 1.5f * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.right * _speed * horizontalInput * Time.deltaTime);
            transform.Translate(Vector3.up * _speed * verticalInput * Time.deltaTime);
        }

        if (transform.position.y > 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y < -4.3f)
        {
            transform.position = new Vector3(transform.position.x, -4.3f, 0);
        }

        if (transform.position.x < -9.09f)
        {
            transform.position = new Vector3(9.09f, transform.position.y, 0);
        }
        else if (transform.position.x > 9.09f)
        {
            transform.position = new Vector3(-9.09f, transform.position.y, 0);
        }
    }

    public void TripleShotPowerupOn()
    {
        canTripleShot = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        canTripleShot = false;
    }

    public void SpeedboostPowerupOn()
    {
        isSpeedboostActive = true;
        StartCoroutine(SpeedboostPowerDownRoutine());
    }

    public IEnumerator SpeedboostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        isSpeedboostActive = false;
    }

    public void ShieldPowerupOn()
    {
        isShieldActive = true;
        _shield.SetActive(true);
    }

    public void Damage()
    {
 

        if (isShieldActive)
        {
            isShieldActive = false;
            _shield.SetActive(false);
            return;
        }
        hitCount++;
        if (hitCount == 1)
        {
            _engines[0].SetActive(true);
        }
        else if (hitCount == 2)
        {
            _engines[1].SetActive(true);
        }

        _lives--;
        _uiManager.UpdateLives(_lives);

        if (_lives < 1 )
        {
            Instantiate(_explosionAnimation, transform.position, Quaternion.identity);
            _gameManager.gameOver = true;
            _uiManager.ShowTitleScreen();
            Destroy(this.gameObject);
        }
    }
}
