using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowParticlesAlongSpline : MonoBehaviour {
	
	ParticleSystem m_System;
	ParticleSystem.Particle[] m_Particles;
	public Transform ControlParent;
	Transform[] pos;
	public Transform endTarget;
	Vector3[] endTargetPos;
	[Tooltip("X:start time Y: end time Z: amount W: LerpTime")]
	public Vector4 lerpToTarget;
	Vector3[] scales;
	public Texture2D image;
	public Color lerpToColor;
	public float amountToLerpColor;
	//public float t = 0;

	void Start(){
		pos = new Transform[ControlParent.childCount];
		scales = new Vector3[pos.Length];

		SetupControls ();
		ControlsToScales ();
	}

	Vector3 GetNoisePosition(float id){
		return new Vector3 (
			Mathf.PerlinNoise(id,id*1.2f)-.5f,
			Mathf.PerlinNoise(id*1.3f,id*1.5f)-.5f,
			Mathf.PerlinNoise(id*.75f,id*.55f)-.5f

		);
	}

	void SetupEndTargets(){
		for (int i = 0; i < endTargetPos.Length; i++) {
			Vector2 c = FindPixel ();
//			Debug.Log (c);
			endTargetPos [i] = new Vector3 (c.x, c.y, 0);
		}
	}

	Vector2 FindPixel(){
		Color b = Color.black;
		float counter = 1000;
		Vector2 r = Vector2.zero;
		while (b.Equals (Color.black) && counter > 0) {
			Vector2 R = new Vector2 (Random.Range (0, image.width), Random.Range (0, image.height));
			b = image.GetPixel ((int)R.x,(int)R.y);
			r = new Vector2 ((((float)R.x / (float)image.width) - .5f), (((float)R.y / (float)image.height) - .5f));
			counter--;
		}
		return r;
	}
	void SetupControls(){
		for (int i = 0; i < ControlParent.childCount; i++) {
			pos [i] = ControlParent.GetChild (i);
		}
	}
	void ControlsToScales(){
		for (int i = 0; i < pos.Length; i++) {
			scales [i] = pos [i].localScale;
		}
	}

	void OnDrawGizmos()
	{
		// Draw a yellow sphere at the transform's position
		Gizmos.color = Color.yellow;
		for (int i = 0; i < 29; i++)
		{
			if(pos!=null)
				if(pos.Length>0)
					Gizmos.DrawLine(
						CatmullRomSpline.GetSplinePos(pos, (float)i/30),
						CatmullRomSpline.GetSplinePos(pos, ((float)i+1)/30));
		}

	}


	private void LateUpdate()
	{
		InitializeIfNeeded();
		ControlsToScales ();

		// GetParticles is allocation free because we reuse the m_Particles buffer between updates
		int numParticlesAlive = m_System.GetParticles(m_Particles);

		// Change only the particles that are alive
		for (int i = 0; i < numParticlesAlive; i++)
		{
			float id = (float)m_Particles [i].randomSeed * .00001f;

			int index = (int)(m_Particles [i].randomSeed % numParticlesAlive);
			float s = Mathf.Min (1, ((m_Particles [i].startLifetime - m_Particles [i].remainingLifetime) / m_Particles [i].startLifetime));// * lerpToTarget.w);
			Vector3 offset = GetNoisePosition (id) * CatmullRomSpline.GetSplinePos (scales, s).magnitude;

			float timeLerp = map (s, lerpToTarget.x, lerpToTarget.y, 0, 1);
			m_Particles [i].position = 
				//Vector3.Lerp(
				(CatmullRomSpline.GetSplinePos(pos, s) + offset);
				//endTarget.localToWorldMatrix.MultiplyPoint (endTargetPos[i]),
				//Mathf.SmoothStep(0,1,timeLerp*lerpToTarget.z)
			//);
			//m_Particles [i].color = Color.Lerp (m_Particles [i].color, lerpToColor, timeLerp * amountToLerpColor);
		}

		// Apply the particle changes to the particle system
		m_System.SetParticles(m_Particles, numParticlesAlive);

	}

	float map(float s, float a1, float a2, float b1, float b2)
	{
		return b1 + (s-a1)*(b2-b1)/(a2-a1);
	}

	void InitializeIfNeeded()
	{
		if (m_System == null) {
			m_System = GetComponent<ParticleSystem> ();
			endTargetPos = new Vector3[m_System.main.maxParticles];
			SetupEndTargets ();
		}

		if (m_Particles == null || m_Particles.Length < m_System.main.maxParticles)
			m_Particles = new ParticleSystem.Particle[m_System.main.maxParticles];


	}

}
