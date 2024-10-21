using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { set; get; }
    private bool isGameStarted = false;
    public bool IsDead { set; get; }
    private  PlayerMotor playerMotor;
    private  float coinScore, score, modifierScore;
    public  TextMeshProUGUI scoreText, coinText, modifierText, hiScoreText;
    private int lastScore;
     public  TextMeshProUGUI deadscoreText, deadcoinText,totalCoinScoreText;
     public Animator deadAnim;
     public Animator gameCanvas, menuAnim , diamondAnim,settingsAnim, characterAnim;
     private AudioSource audioSource;
     public AudioClip gameOverClip, musicClip;
     private float totalCoins = 0;
     public GameObject Player;



    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        modifierScore = 1;
        playerMotor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
        modifierText.text = "x" + modifierScore.ToString("0.0");
        scoreText.text =  score.ToString("0");
        coinText.text = coinScore.ToString("0");
        hiScoreText.text = PlayerPrefs.GetInt("Hiscore").ToString();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = musicClip;
       totalCoins = PlayerPrefs.GetInt("Coins");
       totalCoinScoreText.text = PlayerPrefs.GetInt("Coins").ToString();
       Debug.Log(totalCoins);
             string colorString = PlayerPrefs.GetString("Color");

      if(colorString != "")
      {

      Color color;
        // Try to parse the string into a Unity Color
        if (ColorUtility.TryParseHtmlString(colorString, out color))
        {
            Debug.Log("Successfully converted string to Color: " + color);
            // You can now use the color in your code, e.g., apply it to a material
    Player.GetComponent<Renderer>().material.SetColor("_Color", color);


        }
        else
        {
            Debug.LogError("Failed to convert string to Color.");
        }
      }


    }

    // Update is called once per frame
    void Update()
    {
      if (isGameStarted && !IsDead)
      {
        score += (Time.deltaTime * modifierScore);
        if(lastScore != (int)score)
        {
            lastScore = (int)score;
        scoreText.text =  score.ToString("0");
        }

      }   
    }


    public void UpdateModifier(float modifierAmount)
    {
       modifierScore = 1.0f + modifierAmount;
       modifierText.text = "x" + modifierScore.ToString("0.0");

       
    }
    public  void GetCoin()
    {
      diamondAnim.SetTrigger("Pop");
        coinScore++;
        coinText.text =  coinScore.ToString("0");
        score += 5.0f;
        scoreText.text =  score.ToString("0");


       
    }

    public void OnPlayButton()
    {
      totalCoins += coinScore;
        PlayerPrefs.SetInt("Coins", (int)totalCoins);       
        totalCoinScoreText.text = PlayerPrefs.GetInt("Coins").ToString();
        coinScore = 0;
      UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
      audioSource.clip = musicClip;
      audioSource.loop = true;

      totalCoinScoreText.text = PlayerPrefs.GetInt("Coins").ToString();



    }

    public void OnDead()
    {
        

       IsDead = true;
       audioSource.clip = gameOverClip;
       audioSource.Play();
       audioSource.loop = false;
       deadscoreText.text = score.ToString("0");
       deadcoinText.text =  coinScore.ToString("0");
       deadAnim.SetTrigger("Dead");
       FindObjectOfType<GlacierSpawner>().IsScrolling = false;
       gameCanvas.SetTrigger("Hide");

       if (score > PlayerPrefs.GetInt("Hiscore"))
       {
         float s =  score;
          if(s % 1 == 0)
              s+=1;
              PlayerPrefs.SetInt("Hiscore", (int) s);
       }
    }

    public void ShowSettings()
    {
      settingsAnim.SetTrigger("SettingShow");
    }

        public void ShowCharacter()
    {
      characterAnim.SetTrigger("CharacterShow");
    }
   
    public void HideCharacter()
    {
      characterAnim.SetTrigger("CharacterHide");
    }
    public void HideSettings()
    {
      settingsAnim.SetTrigger("SettingHide");
    }

public void StartGame()
{
        if (!isGameStarted)
      {isGameStarted = true;
      playerMotor.StartedRunning();
      FindObjectOfType<GlacierSpawner>().IsScrolling = true;
      FindObjectOfType<CameraMotor>().IsMoving = true;
      gameCanvas.SetTrigger("Show");
      menuAnim.SetTrigger("Hide");
      }

}

  public void ChangeColorOfficially(int amount)
  {
    totalCoins = totalCoins - amount;
    PlayerPrefs.SetInt("Coins", (int)totalCoins);
    totalCoinScoreText.text = PlayerPrefs.GetInt("Coins").ToString();    
  }
   public void RetainColor(string colorString)
   {
    PlayerPrefs.SetString("Color" , colorString);
   }
}
