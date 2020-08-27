using UnityEngine;
using System.Collections;

namespace mover{
	public class killAfterTime : MonoBehaviour {

		public float time = 1;
		float count = 0;
		// Update is called once per frame
		void Update () {
			count += Time.deltaTime;
			if (count > time) {
				Destroy (this.gameObject);
			}
		}
	}
}