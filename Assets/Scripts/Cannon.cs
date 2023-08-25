using System.Collections;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    private IEnumerator _rotCor;
    [SerializeField]
    private GameObject _cannonBallObj;
    private const int _maxCount = 10;
    private int _currentCount = 0;
    private SpriteRenderer _spriteRend;
    public bool isNewCannon;
    private ObjectPoolBall _objectPool;

    void Start()
    {
        InvokeRotate();
    }
    //Invoke rotation of cannon 
    public void InvokeRotate()
    {
        _currentCount = 0;
        _objectPool = GameObject.FindWithTag("GameManager").GetComponent<ObjectPoolBall>();
        _spriteRend = GetComponent<SpriteRenderer>();
        _rotCor = Rotate();
        StartCoroutine(_rotCor);
    }

    //rotate cannon every second and spawn cannonball
    private IEnumerator Rotate()
    {
        if (isNewCannon)
        {
            _spriteRend.color = new Color(255f, 0f, 0f);
            yield return new WaitForSeconds(4f);
        }
        while (_currentCount < _maxCount)
        {
            _spriteRend.color = new Color(0f, 255f, 0f);
            float rand = Random.Range(10f, 35f);
            gameObject.transform.Rotate(0, 0, rand, Space.Self);
            GameObject obj = _objectPool.Activate(_cannonBallObj, transform.position, transform.rotation);
            obj.GetComponent<CannonBall>().originCannon = this.gameObject;
            _currentCount++;
            if (_currentCount == _maxCount)
            {
                _spriteRend.color = new Color(255f, 0f, 0f);
                break;
            }
            yield return new WaitForSeconds(1f);
        }
    }

}
