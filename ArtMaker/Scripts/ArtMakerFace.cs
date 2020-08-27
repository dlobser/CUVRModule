using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR
{
    public class ArtMakerFace : ArtMakerTemplate
    {
        public Transform noses;
        public Transform eyes;
        public Transform mouths;
        public GameObject head;
        GameObject _head;

        public float mouthHeightMin;
        public float mouthHeightMax;

        public float mouthSizeMin;
        public float mouthSizeMax;

        public float noseSizeMin;
        public float noseSizeMax;

        public float eyeSizeMin;
        public float eyeSizeMax;

        public float eyeSeparationMin;
        public float eyeSeparationMax;

        public float eyeRotationMin;
        public float eyeRotationMax;

        GameObject LeftEye;
        GameObject RightEye;
        GameObject Nose;
        GameObject Mouth;

        public override void MakeArt()
        {
            _head = Instantiate(head);
            _head.transform.parent = root.transform;

            LeftEye = new GameObject();
            LeftEye.transform.parent = _head.transform;
            LeftEye.name = "LeftEye";

            RightEye = new GameObject();
            RightEye.transform.parent = _head.transform;
            RightEye.name = "RightEye";

            Nose = new GameObject();
            Nose.transform.parent = _head.transform;
            Nose.name = "Nose";

            Mouth = new GameObject();
            Mouth.transform.parent = _head.transform;
            Mouth.name = "Mouth";

            int randomEye = Random.Range(0, eyes.childCount);
            Transform t = Instantiate(eyes.GetChild(randomEye), LeftEye.transform);
            float scale = Random.Range(eyeSizeMin, eyeSizeMax);
            float sep = Random.Range(eyeSeparationMin, eyeSeparationMax);
            float rot = Random.Range(eyeRotationMin, eyeRotationMax);
            LeftEye.transform.localScale = new Vector3(scale, scale, scale);
            LeftEye.transform.localPosition = new Vector3(sep, .25f, -.5f);
            LeftEye.transform.localEulerAngles = new Vector3(0, 0, rot);

            t = Instantiate(eyes.GetChild(randomEye), RightEye.transform);
            RightEye.transform.localScale = new Vector3(scale, scale, scale);
            RightEye.transform.localPosition = new Vector3(-sep, .25f, -.5f);
            RightEye.transform.localEulerAngles = new Vector3(0, 0, -rot);

            t = Instantiate(noses.GetChild(Random.Range(0, noses.childCount)), Nose.transform);
            scale = Random.Range(noseSizeMin, noseSizeMax);
            Nose.transform.localScale = new Vector3(scale, scale, scale);
            Nose.transform.localPosition = new Vector3(0, 0, -.5f);

            t = Instantiate(mouths.GetChild(Random.Range(0, mouths.childCount)), Mouth.transform);
            scale = Random.Range(mouthSizeMin, mouthSizeMax);
            float pos = Random.Range(mouthHeightMin, mouthHeightMax);
            Mouth.transform.localScale = new Vector3(scale, scale, scale);
            Mouth.transform.localPosition = new Vector3(0, pos, -.5f);

        }
    }
}