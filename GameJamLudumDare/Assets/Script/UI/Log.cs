using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable] public class MessagesMapping : SerializableDictionary<string, string> { }

public class Log : MonoBehaviour, IPointerClickHandler
{
    public class Parser
    {
        internal static string Sad(string msg)
        {
            return "<color=red>" + msg + "</color>\n";
        }
        internal static string Neutral(string msg)
        {
            return "<color=blue>" + msg + "</color>\n";
        }
        internal static string Happy(string msg)
        {
            return "<color=green>" + msg + "</color>\n";
        }
    }

    public float OpenPosition;
    public float ClosePosition;
    public float TransitTime;
    
    public MessagesMapping SadMessages = new MessagesMapping
    {
        { "children_death", "{0} children died prematurly... Maybe you shold consider buying a Hospital" }
    };

    public MessagesMapping HappyMessages = new MessagesMapping
    {
        { "children_birth", "{0} babies have born this year!" }
    };

    public MessagesMapping NeutralMessages = new MessagesMapping
    {
        { "gender_commit", "Children just commit their gender! You've got {0} new women and {1} men" }
    };

    private float mUiProcessTime;
    private float mSelectedPosition;
    private TextMeshProUGUI mText;

    public bool movingUI { get { return mUiProcessTime > 0; } }

    public void OnPointerClick(PointerEventData eventData)
    {

        mUiProcessTime = Time.fixedTime;

        RectTransform canvasRect = GetComponent<RectTransform>();

        if (canvasRect.localPosition.y == ClosePosition)
        {
            mSelectedPosition = OpenPosition;
        } else
        {
            mSelectedPosition = ClosePosition;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        mText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        Debug.Log(mText);

        // Subscribe to every event
        foreach (KeyValuePair<String, String> entry in HappyMessages)
        {
            EventManager.StartListening(entry.Key, (object[] args) => EventHandler(entry, args));
            Debug.Log(entry);
        }
    }

    void EventHandler(KeyValuePair<String, String> message, object[] args)
    {
        string parsedMessage;

        if (HappyMessages.ContainsKey(message.Key))
            parsedMessage = Parser.Happy(String.Format(message.Value, args));
        else if (NeutralMessages.ContainsKey(message.Key))
            parsedMessage = Parser.Neutral(String.Format(message.Value, args));
        else if (SadMessages.ContainsKey(message.Key))
            parsedMessage = Parser.Sad(String.Format(message.Value, args));
        else
            parsedMessage = "Unknown event\n";

        mText.text = parsedMessage + mText.text;
    }

    private void proceedUI()
    {
        float percentage = (Time.fixedTime - mUiProcessTime) / TransitTime;

        RectTransform canvasRect = GetComponent<RectTransform>();

        // Set our position as a fraction of the distance between the markers.
        canvasRect.localPosition = new Vector3(canvasRect.localPosition.x, Mathf.Lerp(canvasRect.localPosition.y, mSelectedPosition, percentage), 0);

        if (percentage >= 1)
        {
            mUiProcessTime = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // UI
        if (movingUI)
            proceedUI();
    }
}
