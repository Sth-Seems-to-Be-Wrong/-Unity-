using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowCoins : MonoBehaviour
{
    private Text coinText;
    // Start is called before the first frame update
    void Start()
    {
        coinText = transform.GetChild(1).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        coinText.text = CoinControl.GetnowCoins().ToString();
    }
}
