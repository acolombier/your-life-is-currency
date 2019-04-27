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
    public Texture2D BackgroundTexture;

    public Vector2 pos = new Vector2(20, 40);
    public Vector2 size = new Vector2(60, 20);

    private float mMaleRatio;
    private float mFemaleRatio;
    private float mChildrenRatio;
    private float mFullfilmentRatio;
    private float mTotal;

    void OnGUI()
    {
        //draw the background:
        GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
        GUI.Box(new Rect(0, 0, size.x, size.y), BackgroundTexture);

        float fullRatio = 0.6f * (1.0f - mFullfilmentRatio);

        if (fullRatio == 0f)
        {
            // Full design
        }

        float offset = fullRatio * size.y;
        GUI.DrawTexture(new Rect(0, offset, size.x, size.y * mFemaleRatio * (1.0f - fullRatio)), FemaleTexture, ScaleMode.StretchToFill);
        offset += size.y * mFemaleRatio * (1.0f - fullRatio);
        GUI.DrawTexture(new Rect(0, offset, size.x, size.y * mMaleRatio * (1.0f - fullRatio)), MaleTexture, ScaleMode.StretchToFill);
        offset += size.y * mMaleRatio * (1.0f - fullRatio);
        GUI.DrawTexture(new Rect(0, offset, size.x, size.y * mChildrenRatio * (1.0f - fullRatio)), ChildrenTexture, ScaleMode.StretchToFill);

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