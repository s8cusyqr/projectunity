using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static bool isGameOver;
    public static int cannonCount;
    [SerializeField]
    private Text _counter;

    // Start is called before the first frame update
    void Start()
    {
        cannonCount = 1;
    }

    //change count text number to count of cannons
    public void SetCounter()
    {
        _counter.text = "Cannons: " + cannonCount.ToString();
    }
}
