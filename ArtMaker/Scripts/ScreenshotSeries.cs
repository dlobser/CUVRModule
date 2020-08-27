using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CUVR
{
    public class ScreenshotSeries : MonoBehaviour
    {
        public Color bgColor;
        public Vector2 divisions;
        public int resolution = 2048;
        float aspect;
        public bool rebuild;
        public bool destroy;

        int count;
        Vector3[] positions;
        GameObject container;
        ArtSquare artSquare;
        Camera cam;

        void Start()
        {
            artSquare = FindObjectOfType<ArtSquare>();
            if (artSquare == null)
                Debug.LogWarning("'ArtSquare' component missing");
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                rebuild = true;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                destroy = true;
            }
            if(destroy){
                if(container!=null)
                    Destroy(container);
                if(cam!=null)
                    Destroy(cam.gameObject);
                destroy = false;
            }
            if(rebuild){
                if (container != null)
                {
                    Destroy(container);
                    Destroy(cam.gameObject);
                }

                GameObject g = new GameObject();
                cam = g.AddComponent<Camera>();
                cam.orthographic = true;
                cam.orthographicSize = 1.5f;
                Camera.main.rect = new Rect(0, 0, 1.5f/aspect, 1.5f);

                cam.gameObject.AddComponent<PhysicsRaycaster>();
                g.transform.position = new Vector3(0, 1000, -10);
                g.name = "Cam";
                cam.backgroundColor = bgColor;
                cam.clearFlags = CameraClearFlags.Color;
                aspect = Camera.main.aspect;

                container = new GameObject();
                container.name = "ArtGrid";
                cam.transform.localPosition = new Vector3(divisions.x * aspect * .5f - (aspect * .5f), divisions.y * .5f + 1000 - .5f, -10);
                cam.orthographicSize = divisions.x * .5f;
                StartCoroutine(snap());
                rebuild = false;
            }
        }

        IEnumerator snap()
        {
            for (int i = 0; i < divisions.x; i++)
            {
                for (int j = 0; j < divisions.y; j++)
                {
                    Snap(i, j);
                    yield return null;
                }
            }

        }

        void Snap(float x, float y)
        {
            GameObject g = GameObject.CreatePrimitive(PrimitiveType.Quad);
            g.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Unlit/Texture"));
            g.transform.localPosition = new Vector3(x * aspect, y + 1000, 0);
            g.transform.localScale = new Vector3(aspect - .02f, 1 - .02f, 1);// Vector3.one * .98f;
            g.AddComponent<Interactable_ReRender>();
            g.AddComponent<SnapNewImage>();
            g.GetComponent<SnapNewImage>().Snap();
            if(x.Equals(0)&&y.Equals(0))
                g.GetComponent<SnapNewImage>().Snap();
            g.transform.parent = container.transform;
            count++;
        }
    }
}