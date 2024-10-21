using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChangeRed : MonoBehaviour
{
  public Color newAlbedoColor;
  public GameObject Player;
  public int diamondamount;
  private int totalCoins;
  public TextMeshProUGUI errorText, redText, blackText;
  public string colorString;

  public bool redpurchase;
    public bool blackpurchase;

  private void Start()
  {
   totalCoins = PlayerPrefs.GetInt("Coins");

  }
  public void changeColor()
  {
    totalCoins = PlayerPrefs.GetInt("Coins");
   if(totalCoins >= diamondamount && !redpurchase || totalCoins >= diamondamount && !blackpurchase)
   {
    Player.GetComponent<Renderer>().material.SetColor("_Color", newAlbedoColor);
    GameManager.Instance.ChangeColorOfficially(diamondamount);
    GameManager.Instance.RetainColor(colorString);

    string purchase = "Purcahsed";
   PlayerPrefs.SetString("Purchase", purchase);
   if(diamondamount == 50)
   {
    redText.text = PlayerPrefs.GetString("Purchase");
    redpurchase = true;
   }else if(diamondamount == 100)
   {
    blackText.text = PlayerPrefs.GetString("Purchase");
    blackpurchase = true;
   
   }

   }
   else {
   Debug.Log("not enough diamonds");
   errorText.gameObject.SetActive(true);
   Invoke("DissapearText", .5f);
   }

  }

  private void DissapearText()
  {
   errorText.gameObject.SetActive(false);

  }
}
