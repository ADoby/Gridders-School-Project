using UnityEngine;  
using System.Collections;  
  
public class AudioVisualizer : MonoBehaviour  
{  	
    //An AudioSource object so the music can be played  
    private AudioSource aSource;  
    //A float array that stores the audio samples  
    public float[] samples = new float[64];  
    //A reference to the cube prefab  
    public GameObject cube; 
    //The position of the current cube. Will also be the position of each point of the line.  
    private Vector3 cubePos;  
    //An array that stores the Transforms of all instantiated cubes  
    private Transform[] cubesTransform;  
    //The velocity that the cubes will drop  
    public Vector3 gravity = new Vector3(0.0f,0.25f,0.0f);  
  
	public float CubeHeight = 20.0f;
	public float multMovement = 4.0f;
	public float maxMovement = 10.0f;
	
	public float iMult = 10.0f;
	
	public bool smooth = false;
	
	private int allDivWanted = 0;
	
    void Awake ()  
    {  
        //Get and store a reference to the following attached components:  
        //AudioSource  
        this.aSource = GetComponent<AudioSource>();
    }  
  
    void Start()  
    {  
        //The cubesTransform array should be initialized with the same length as the samples array  
        cubesTransform = new Transform[samples.Length+1];
  
        //Create a temporary GameObject, that will serve as a reference to the most recent cloned cube  
        GameObject tempCube;
  
		allDivWanted = 8192/samples.Length;
		
		samples = new float[samples.Length/2];
		
        //For each sample  
        for(int i=0; i<samples.Length;i++)  
        {  
            //Instantiate a cube placing it at the right side of the previous one  
            tempCube = (GameObject) Instantiate(cube, new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z), Quaternion.identity);  
            //Get the recently instantiated cube Transform component  
            cubesTransform[i] = tempCube.transform;
            //Make the cube a child of this game object
            cubesTransform[i].parent = transform;
			int z = 0;
			if(i%2==0)
				z = 0;
			cubesTransform[i].localPosition = cubesTransform[i].localPosition + new Vector3(i*cubesTransform[i].localScale.x, 0, z) - (samples.Length/4) * new Vector3(1, 0, 0) - new Vector3(0, CubeHeight/2, 0);
			//cubesTransform[i].localScale = new Vector3(1,CubeHeight,1);
        }  
    }  
  
    void Update ()  
    {  
		
		float[] ALLsamples = new float[8192];
		
        //Obtain the samples from the frequency bands of the attached AudioSource
        aSource.GetSpectrumData(ALLsamples,0,FFTWindow.BlackmanHarris);
		
		
		
		//8192/64 = 128
		//128/2 = 64 == Alle 64 Schritte mach was
		int id = 0;
		
		for(int i=allDivWanted/2; i < 4096 - 1; i+=allDivWanted){
			//Alle zusammenrechnen und dann durchschnitt
			float amount = 0;
			for(int a = -allDivWanted/2; a < allDivWanted/2 - 1; a++){
				amount += ALLsamples[i+a];
			}
			amount /= allDivWanted;
			samples[id] = amount;
			id++;
		}
		
        //For each sample
        for(int i=0; i<samples.Length;i++)
        {  
            /*Set the cubePos Vector3 to the same value as the position of the corresponding
             * cube. However, set it's Y element according to the current sample.*/
            cubePos.Set(cubesTransform[i].localPosition.x, Mathf.Clamp(samples[i]*(multMovement+(i*i)*iMult),0,maxMovement), cubesTransform[i].localPosition.z);
  
            //If the new cubePos.y is greater than the current cube position
            if(cubePos.y >= cubesTransform[i].localPosition.y)
            {
                //Set the cube to the new Y position
                cubesTransform[i].localPosition = cubePos;
            }
            else
            {
                //The spectrum line is below the cube, make it fall
				if(cubesTransform[i].localPosition.y - gravity.y*Time.deltaTime > 0)
					cubesTransform[i].localPosition -= gravity*Time.deltaTime;
				else
					cubesTransform[i].localPosition = new Vector3(cubesTransform[i].localPosition.x, 0, cubesTransform[i].localPosition.z);
			}
        }  
    }  
}  