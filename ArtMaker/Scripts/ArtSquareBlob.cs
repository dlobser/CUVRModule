using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Art
{
    public class ArtSquareBlob : MonoBehaviour
    {
        public int amount = 10;
        public int resolution = 30;
        public float blobSize = .1f;
        MCBlob blob;
        GameObject root;

        void Start()
        {
            root = new GameObject();
            blob = GetComponent<MCBlob>();
            blob.dimX = blob.dimY = blob.dimZ = resolution;
            blob.inBlobs = new Transform[amount];
            for (int i = 0; i < amount; i++)
            {
                GameObject g = new GameObject();
                g.transform.parent = root.transform;
                g.transform.localScale = Vector3.one * .1f * Random.Range(.4f, 1f);
                g.transform.localPosition = Random.insideUnitSphere * .4f;
                g.transform.parent = root.transform;
                blob.inBlobs[i] = g.transform;
            }
            blob.Regen();
        }

		private void Update()
		{
            for (int i = 0; i < amount; i++)
            {
                Transform g = root.transform.GetChild(i);
                //g.transform.localScale = Vector3.one * .1f * Random.Range(.4f, 1f);
                //g.transform.localPosition = Random.insideUnitSphere * .4f;
                //g.transform.parent = root.transform;
                blob.inBlobs[i] = g.transform;
            }
            blob.Regen();
		}
	}
}