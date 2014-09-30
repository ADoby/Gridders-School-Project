using UnityEngine;
using System.Collections;

public class VideoMenu : MonoBehaviour {

    private float PosX = 0, PosY = 0;
    private float Width;
    private float Height;

    public float minFOV = 60, maxFOV = 120;

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

        Width = 200;
        Height = Screen.height;

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
        GUI.Box(new Rect(PosX, PosY, Width, Height), "");

        float marginX = 10;

        GUI.BeginGroup(new Rect(PosX + marginX, PosY, Width, Height));

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
        if (GUILayout.Button((dofScript.enabled ? "Disable" : "Enable") + " depth of field"))
        {
            dofScript.enabled = !dofScript.enabled;
        }
        if (GUILayout.Button((bloomScript.enabled ? "Disable" : "Enable") + " bloom"))
        {
            bloomScript.enabled = !bloomScript.enabled;
        }
        if (GUILayout.Button((AAScript.enabled ? "Disable" : "Enable") + " AA"))
        {
            AAScript.enabled = !AAScript.enabled;
        }
        if (GUILayout.Button((vSyncEnabled ? "Disable" : "Enable") + " VSync"))
        {
            vSyncEnabled = !vSyncEnabled;
            UpdateSettings();
        }

        fovChanger.startFOV = GUILayout.HorizontalSlider(fovChanger.startFOV, minFOV, maxFOV, GUILayout.MinWidth(100));
        GUILayout.Label("Current FOV: " + ((int)fovChanger.startFOV).ToString());

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Close"))
        {
            Screen.lockCursor = true;
            enabled = false;
        }

        if (GUILayout.Button("Exit Game"))
        {
            Application.Quit();
        }
        GUI.EndGroup();
    }
}
