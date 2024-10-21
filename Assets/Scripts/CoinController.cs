using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    private Animator anima;
    // Start is called before the first frame update
    void Awake()
    {
        anima = GetComponent<Animator>();
    }

    // Update is called once per frame
    void OnEnable()
    {
        anima.SetTrigger("Spawn");
    }

   private void OnTriggerEnter(Collider other)
    {
      if(other.tag ==  "Player")
      {
        Debug.Log("coin collected");
        GameManager.Instance.GetCoin();
        anima.SetTrigger("Collected");
        
      }
    }
}
