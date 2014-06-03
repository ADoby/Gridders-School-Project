using UnityEngine;
using System.Collections;

public class VideoMenu : MonoBehaviour {
    
    
    private float Width;
    private float Height;

    public float minFOV = 70, maxFOV = 120;

    public FielOfViewChanger fovChanger;
    public DepthOfFieldScatter dofScript;
    public FastBloom bloomScript;
    public AntialiasingAsPostEffect AAScript;

    private bool vSyncEnabled = false;

	// Use this for initialization
	void Start () {
        StartCoroutine(UpdateGUIScale());
	}

    IEnumerator UpdateGUIScale()
    {

        Width = Screen.width * transform.localScale.x;
        Height = Screen.height * transform.localScale.y;

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(UpdateGUIScale());
    }

    private void UpdateSettings(){
        if (vSyncEnabled)
            QualitySettings.vSyncCount = 1;
        else
            QualitySettings.vSyncCount = 0;
    }

    void OnGUI()
    {
        GUI.Box(new Rect((Screen.width - Width) * transform.localPosition.x, (Screen.height - Height) * transform.localPosition.y, Width, Height), "");

        GUI.BeginGroup(new Rect((Screen.width - Width) * transform.localPosition.x, (Screen.height - Height) * transform.localPosition.y, Width, Height));

        GUILayout.Label("Current quality setting: " + QualitySettings.names[QualitySettings.GetQualityLevel()]);
        if (GUILayout.Button("Worse"))
        {
            QualitySettings.DecreaseLevel();
            UpdateSettings();
        }
        if (GUILayout.Button("Better"))
        {
            QualitySettings.IncreaseLevel();
            UpdateSettings();

        }
        GUILayout.Space(10);
        if (GUILayout.Button((dofScript.enabled ? "Disable" : "Enable") + "DOF (Too much bokeh here)"))
        {
            dofScript.enabled = !dofScript.enabled;
        }
        if (GUILayout.Button((bloomScript.enabled ? "Disable" : "Enable") + "Bloom (Uh its glowing everywhere)"))
        {
            bloomScript.enabled = !bloomScript.enabled;
        }
        if (GUILayout.Button((AAScript.enabled ? "Disable" : "Enable") + "AA (More softy edges)"))
        {
            AAScript.enabled = !AAScript.enabled;
        }
        if (GUILayout.Button((vSyncEnabled ? "Disable" : "Enable") + "VSync (freeze fps at 60 or so)"))
        {
            vSyncEnabled = !vSyncEnabled;
            UpdateSettings();
        }

        GUILayout.BeginHorizontal();
        fovChanger.startFOV = GUILayout.HorizontalSlider(fovChanger.startFOV, minFOV, maxFOV, GUILayout.MinWidth(100));
        GUILayout.Label("Current FOV: " + ((int)fovChanger.startFOV).ToString());
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Close"))
        {
            Screen.lockCursor = true;
            enabled = false;
        }
        GUI.EndGroup();
    }
}
