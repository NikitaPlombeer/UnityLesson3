using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Button TryAgainButton;
    public Text CoinsCountText;
    public Light SceneLight;
    public AudioSource loseSound;
    public AudioSource coinSound;
    public AudioSource winSound;

    private PlaneController _plane;
    public bool IsWasted { get; private set; }

    private CoinController[] coins;
    private int _coinsCollected;

    private void Awake()
    {
        instance = this;
        IsWasted = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        _plane = FindObjectOfType<PlaneController>();
        coins = FindObjectsOfType<CoinController>();
        HideCursor();
        ShowCoinsCount();
    }

    private void ShowCoinsCount()
    {
        CoinsCountText.text = _coinsCollected + "/" + coins.Length;
    }

    public void CollectCoin(GameObject collidedObj)
    {
        _coinsCollected++;
        coinSound.Play();
        collidedObj.transform.parent.gameObject.SetActive(false);
        ShowCoinsCount();
        if (_coinsCollected == coins.Length)
        {
            OnWin();
        }
    }

    public void OnWin()
    {
        ShowCursor(); 
        TryAgainButton.gameObject.SetActive(true);
        SceneLight.color = Color.green;
        winSound.Play();
        _plane.WinningPose();
    }
    
    public void OnLose()
    {
        Debug.Log("WASTED");
        ShowCursor();

        IsWasted = true;
        TryAgainButton.gameObject.SetActive(true);
        SceneLight.color = Color.red;
        loseSound.Play();
    }

    public void OnTryAgain()
    {
        HideCursor();
        ShowCoins();
        _coinsCollected = 0;

        IsWasted = false;
        TryAgainButton.gameObject.SetActive(false);
        SceneLight.color = Color.white;
        winSound.Stop();
        loseSound.Stop();

        _plane.RestoreInitialState();
        ShowCoinsCount();
    }

    private void ShowCoins()
    {
        foreach (var coin in coins)
        {
            coin.gameObject.SetActive(true);
        }
    }

    private void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}