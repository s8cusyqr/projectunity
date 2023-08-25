using System.Collections;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    private const int _maxCannonCount = 80;
    private int _rand;
    private IEnumerator _moveCor;
    private int _currentMoveCount = 0;
    [SerializeField]
    private GameObject _cannonObj;
    private GameManager _gameManager;
    private ObjectPool _objectPool;
    private ObjectPoolBall _objectPoolBall;
    private float _unitPerFrame;
    public GameObject originCannon;


    void OnEnable()
    {
        _objectPoolBall = GameObject.FindWithTag("GameManager").GetComponent<ObjectPoolBall>();
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        _objectPool = GameObject.FindWithTag("GameManager").GetComponent<ObjectPool>();
        _rand = Random.Range(1, 6);
        //fixed update called 50 times/s 3 units equals 3/50=0.06
        _unitPerFrame = 3f / 50f;
        _moveCor = Move();
        StartCoroutine(_moveCor);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ChangePos();
        SpawnNewCannon();
    }

    private void ChangePos()
    {
        //fixed update called 50 times/s 3 units equals 3/50=0.06
        if (_currentMoveCount < _rand)
        {
            transform.position += transform.right * _unitPerFrame;
        }
    }

    //at end of trajectory spawn new cannon
    private void SpawnNewCannon()
    {
        if (_currentMoveCount == _rand && !GameManager.isGameOver)
        {
            _gameManager.SetCounter();
            GameObject newCannon = _objectPool.Activate(_cannonObj, transform.position, Quaternion.identity);
            newCannon.GetComponent<Cannon>().isNewCannon = true;
            if (GameManager.cannonCount >= _maxCannonCount && !GameManager.isGameOver)
            {
                GameManager.isGameOver = true;
                GameObject[] cannons = GameObject.FindGameObjectsWithTag("Cannon");
                foreach (GameObject cannnon in cannons)
                {
                    Cannon can = cannnon.GetComponent<Cannon>();
                    can.InvokeRotate();
                }
            }
            _objectPoolBall.Deactivate(gameObject);
        }
        else if (_currentMoveCount >= _rand)
        {
            _objectPoolBall.Deactivate(gameObject);
        }
    }

    //move counter
    private IEnumerator Move()
    {
        while (_currentMoveCount < _rand)
        {
            yield return new WaitForSeconds(1f);
            _currentMoveCount++;
        }
    }

    //destroy cannon and cannonball on trigger 
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Cannon" && col.gameObject != originCannon)
        {
            if (col != null && _objectPool != null)
            {
                _objectPool.Deactivate(col.gameObject);
            }
            _objectPoolBall.Deactivate(gameObject);
        }
    }
}
