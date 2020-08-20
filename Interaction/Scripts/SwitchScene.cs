using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CUVR{

    public class SwitchScene : MonoBehaviour
    {
        public Button button;
        int numScenes;
        int whichScene = 1;

        void Start()
        {
            numScenes = SceneManager.sceneCountInBuildSettings;
        }

        void Update()
        {
            if (button.buttonDown)
            {
                whichScene++;
                if (whichScene >= numScenes)
                    whichScene = 1;
                Debug.Log(numScenes);
                button.buttonDown = false;
                SceneManager.LoadScene(whichScene);
            }
        }
    }
}