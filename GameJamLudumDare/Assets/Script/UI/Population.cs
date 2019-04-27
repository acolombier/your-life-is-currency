using UnityEngine;
using System.Collections;

public class Population : MonoBehaviour
{

    [Header("Icons")]
    public UnityEngine.UI.Image MaleImage;
    public UnityEngine.UI.Image FemaleImage;
    public UnityEngine.UI.Image ChildrenImage;

    [Header("Textures")]
    public Texture2D MaleTexture;
    public Texture2D FemaleTexture;
    public Texture2D ChildrenTexture;
    public Texture2D EmptyTexture;

    [Header("Appearence")]
    public GUIStyle Style = new GUIStyle();

    public Vector2 Position = new Vector2(20, 40);
    private Vector2 mSize;

    private Vector2 mContentSize;

    private float mMaleRatio;
    private float mFemaleRatio;
    private float mChildrenRatio;
    private float mFullfilmentRatio;
    private float mTotal;

    void OnGUI()
    {
        //draw the background:
        GUI.BeginGroup(new Rect(Position.x, Position.y, mSize.x, mSize.y));
        GUI.Box(new Rect(0, 0, mSize.x, mSize.y), "Max", Style);

        Debug.Log(GetComponent<RectTransform>().localPosition);

        if (mSize.magnitude == 0)
        {
            mSize = GetComponent<RectTransform>().rect.size;
            mContentSize = new Vector2(mSize.x - Style.padding.left - Style.padding.right, mSize.y - Style.padding.top - Style.padding.bottom);
        }
        Debug.Log(mSize);

        float fullRatio = 0.6f * (1.0f - mFullfilmentRatio);

        if (fullRatio == 0f)
        {
            // Full design
        }

        float offset = Style.padding.top;
        GUI.DrawTexture(new Rect(Style.padding.left, offset, mContentSize.x, mContentSize.y * fullRatio), EmptyTexture, ScaleMode.StretchToFill);
        offset += mContentSize.y * fullRatio;
        GUI.DrawTexture(new Rect(Style.padding.left, offset, mContentSize.x, mContentSize.y * mFemaleRatio * (1.0f - fullRatio)), FemaleTexture, ScaleMode.StretchToFill);
        offset += mContentSize.y * mFemaleRatio * (1.0f - fullRatio);
        GUI.DrawTexture(new Rect(Style.padding.left, offset, mContentSize.x, mContentSize.y * mMaleRatio * (1.0f - fullRatio)), MaleTexture, ScaleMode.StretchToFill);
        offset += mContentSize.y * mMaleRatio * (1.0f - fullRatio);
        GUI.DrawTexture(new Rect(Style.padding.left, offset, mContentSize.x, mContentSize.y * mChildrenRatio * (1.0f - fullRatio)), ChildrenTexture, ScaleMode.StretchToFill);

        GUI.EndGroup();
    }

    void Start()
    {
        EventManager.StartListening("population_update", UpdatePopulation);
    }
    
    void UpdatePopulation(object[] args)
    {
        int male = (int)args[0], female = (int)args[1], children = (int)args[2], max = (int)args[3];

        int total = male + female + children;
        mMaleRatio = (float)male / (float)total;
        mFemaleRatio = (float)female / (float)total;
        mChildrenRatio = (float)children / (float)total;

        mFullfilmentRatio = (float)total / (float)max;

        Debug.Log(mMaleRatio + ", " + mFemaleRatio + ", " + mChildrenRatio + ", " + total + ", " + mFullfilmentRatio);
    }
}