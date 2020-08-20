using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class AddNamespace : MonoBehaviour
{
#if UNITY_EDITOR
    public TextAsset[] texts;
    public bool doit;

    void Start()
    {

        //string[] tee = AssetDatabase.FindAssets("*.cs");
        //texts = new TextAsset[tee.Length];
        //for (int i = 0; i < tee.Length; i++)
        //{
        //    texts[i] = AssetDatabase.LoadAssetAtPath(tee[i]);
        //}
    }

    void Update()
    {
        if (doit)
        {
            for (int i = 0; i < texts.Length; i++)
            {
                DOIT(texts[i]);
            }

            doit = false;
        }
    }
    void DOIT(TextAsset text)
    {

        string s = text.text;
        string[] ss = s.Split(new char[] { '\n' });
        string o = "";
        bool didName = false;

        bool dontName = false;
        for (int i = 0; i < ss.Length; i++)
        {
            if (ss[i].Contains("namespace"))
            {
                dontName = true;
            }


        }
        if (!dontName)
        {
            for (int i = 0; i < ss.Length; i++)
            {
                if (!didName && !ss[i].Contains("using"))
                {

                    o += "\n";
                    o += "namespace CUVR{\n";
                    didName = true;
                }

                o += ss[i] + "\n";
            }
            o += "\n}";

            File.WriteAllText(AssetDatabase.GetAssetPath(text), o);
            EditorUtility.SetDirty(text);
        }
        else
        {
            print(text.name + " Didn't write, namespace exists");
        }
    }
#endif
}
