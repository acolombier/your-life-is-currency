using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathCounter : MonoBehaviour
{
    private TextMeshProUGUI mText;
    // Start is called before the first frame update
    void Start()
    {
        mText = transform.Find("Value").GetComponent<TextMeshProUGUI>();

        EventManager.StartListening("update_sacrifice_count", UpdateValue);
    }
    
    void UpdateValue(object[] args)
    {
        mText.text = Parser.Unit((int)args[0]);
    }
}
