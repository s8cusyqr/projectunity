using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private List<GameObject> _inactiveObjects, _activeObjects;
    [SerializeField]
    private GameObject _cannon;
    private const int _maxCannonCount=80;

    // Start is called before the first frame update
    void Awake()
    {
        Init();
    }
    //Initialize objs and add them to list of inactive
    private void Init()
    {
        _inactiveObjects = new List<GameObject>();
        _activeObjects = new List<GameObject>();
        // create sleeping cannons
        for (int i = 0; i < _maxCannonCount; i++)
        {
            GameObject pooledObject = Instantiate(_cannon, transform);
            pooledObject.SetActive(false);
            _inactiveObjects.Add(pooledObject);
        }
    }
    //deactive obj and add to list of inactive
    public void Deactivate(GameObject obj)
    {
        if (!_activeObjects.Contains(obj)) return;
        obj.SetActive(false);
        _activeObjects.Remove(obj);
        _inactiveObjects.Add(obj);
        GameManager.cannonCount--;
    }
    //activate obj from list of inactive objs
    public GameObject Activate(GameObject obj, Vector3 pos, Quaternion rot)
    {
        GameManager.cannonCount++;
        // no objects left in queue
        if (_inactiveObjects.Count == 0)
        {
            obj = Instantiate(obj, transform);
        }
        else if (_inactiveObjects.Count > 0)
        {
            obj = _inactiveObjects[0];
            _inactiveObjects.RemoveAt(0);
        }

        obj.transform.position = pos;
        obj.transform.rotation = rot;
        _activeObjects.Add(obj);
        obj.SetActive(true);
        return obj;
    }
}
