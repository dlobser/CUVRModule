using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmo_WireSphere : MonoBehaviour {
	void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position, transform.localScale.x*.5f);
	}
}
