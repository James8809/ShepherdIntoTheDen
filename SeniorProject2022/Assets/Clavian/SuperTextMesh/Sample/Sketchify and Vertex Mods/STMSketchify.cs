using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SuperTextMesh))]
public class STMSketchify : MonoBehaviour {

	public SuperTextMesh stm;
	[Range(0.001f,8f)]
	public float sketchDelay = 0.25f;
	private float sketchLastTime = -1.0f;
	public float sketchAmount = 0.025f;
	private Vector3[] storedVerts = new Vector3[0];
	public bool unscaledTime = true;


	public void Reset()
	{
		//get stm component
		stm = GetComponent<SuperTextMesh>();
	}
	void Awake()
	{
		sketchLastTime = -1f;
	}

	void OnEnable()
	{
		stm.OnVertexMod += SketchifyVerts;
	}
	void OnDisable()
	{
		stm.OnVertexMod -= SketchifyVerts;
	}
	public void SketchifyVerts(Vector3[] verts, Vector3[] middles, Vector3[] positions){
		//fill array first time
		if(storedVerts.Length != verts.Length){
			System.Array.Resize(ref storedVerts, verts.Length); //expand array to fill

			//store verts so they can be applied until the timer rolls over
			for(int i=0, iL=verts.Length; i<iL; i++){
				storedVerts[i].x = verts[i].x + Random.Range(-sketchAmount,sketchAmount);
				storedVerts[i].y = verts[i].y + Random.Range(-sketchAmount,sketchAmount);
				storedVerts[i].z = verts[i].z + Random.Range(-sketchAmount,sketchAmount);
				
			}
		}
		//round time to nearest multiple of delay
		float newTime = Mathf.Floor((unscaledTime ? Time.unscaledTime : Time.time) / sketchDelay) * sketchDelay;
		if(newTime != sketchLastTime){ //update random state if time has changed or it's the first time
			sketchLastTime = newTime;
			//update stored verts
			if(storedVerts.Length != verts.Length)
                System.Array.Resize(ref storedVerts, verts.Length); //expand array to fill


			for(int i=0, iL=verts.Length; i<iL; i++){
				storedVerts[i].x = verts[i].x + Random.Range(-sketchAmount,sketchAmount);
				storedVerts[i].y = verts[i].y + Random.Range(-sketchAmount,sketchAmount);
				storedVerts[i].z = verts[i].z + Random.Range(-sketchAmount,sketchAmount);
				
			}
		}
		//use stored verts
		for(int i=0, iL=verts.Length; i<iL; i++){
			verts[i].x = storedVerts[i].x;
			verts[i].y = storedVerts[i].y;
			verts[i].z = storedVerts[i].z;
		}
	}
}
