using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.Collections;


namespace exp3
{
    [CustomEditor(typeof(TransformUniversal))]
    public class TransformUniversalEditor : Editor
    {

        TransformUniversal script;

        //Translate vars
        AnimBool doTranslate;
        AnimBool translateUseBounds;
        AnimBool doTranslateOscillate;
        AnimBool doTranslateNoise;
        AnimBool doRotate;
        AnimBool doRotateOscillate;
        AnimBool doRotateNoise;
        AnimBool doScale;
        AnimBool scaleUseBounds;
        AnimBool doScaleOscillate;
        AnimBool doScaleNoise;


        void OnEnable()
        {

            script = (TransformUniversal)target;
            
            //Translate props
            doTranslate = new AnimBool(script.doTranslate);
            doTranslate.valueChanged.AddListener(Repaint);
            translateUseBounds = new AnimBool(script.translateUseBounds);
            translateUseBounds.valueChanged.AddListener(Repaint);
            doTranslateOscillate = new AnimBool(script.doTranslateOscillate);
            doTranslateOscillate.valueChanged.AddListener(Repaint);;
            doTranslateNoise = new AnimBool(script.doTranslateNoise);
            doTranslateNoise.valueChanged.AddListener(Repaint);
            doRotate = new AnimBool(script.doRotate);
            doRotate.valueChanged.AddListener(Repaint);
            doRotateOscillate = new AnimBool(script.doRotateOscillate);
            doRotateOscillate.valueChanged.AddListener(Repaint);
            doRotateNoise = new AnimBool(script.doRotateNoise);
            doRotateNoise.valueChanged.AddListener(Repaint);
            doScale = new AnimBool(script.doScale);
            doScale.valueChanged.AddListener(Repaint);
            scaleUseBounds = new AnimBool(script.scaleUseBounds);
            scaleUseBounds.valueChanged.AddListener(Repaint);
            doScaleOscillate = new AnimBool(script.doScaleOscillate);
            doScaleOscillate.valueChanged.AddListener(Repaint);
            doScaleNoise = new AnimBool(script.doScaleNoise);
            doScaleNoise.valueChanged.AddListener(Repaint);

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space();

            script.globalTimeScale = EditorGUILayout.FloatField("Global Time Scale", script.globalTimeScale);
            
            EditorGUILayout.Space();

            this.displayTranslateFields();
            this.displayTranslateOscillateFields();
            this.displayTranslateNoiseFields();

            EditorGUILayout.Space();

            this.displayRotateFields();
            this.displayRotateOscillateFields();
            this.displayRotateNoiseFields();

            EditorGUILayout.Space();

            this.displayScaleFields();
            this.displayScaleOscillateFields();
            this.displayScaleNoiseFields();

        }

        void displayTranslateFields()
        {
            doTranslate.target = EditorGUILayout.ToggleLeft("Translate", doTranslate.target);
            script.doTranslate = doTranslate.target;

            if (EditorGUILayout.BeginFadeGroup(doTranslate.faded))
            {
                EditorGUI.indentLevel++;
                //Translate speed
                script.translateSpeed = EditorGUILayout.Vector3Field("Translate Speed", script.translateSpeed);

                //Toggle use bounds
                translateUseBounds.target = EditorGUILayout.ToggleLeft("Use Bounds", translateUseBounds.target);
                script.translateUseBounds = translateUseBounds.target;

                if (EditorGUILayout.BeginFadeGroup(translateUseBounds.faded))
                {
                    EditorGUI.indentLevel++;

                    script.pingPong = EditorGUILayout.Toggle("Ping Pong", script.pingPong);
                    script.translateOffset = EditorGUILayout.Vector3Field("Offset", script.translateOffset);
                    script.translateUpperBounds = EditorGUILayout.Vector3Field("Upper Bounds", script.translateUpperBounds);
                    script.translateLowerBounds = EditorGUILayout.Vector3Field("Lower Bounds", script.translateLowerBounds);

                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndFadeGroup();

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFadeGroup();
        }

        void displayTranslateOscillateFields()
        {
            doTranslateOscillate.target = EditorGUILayout.ToggleLeft("Translate Oscillate", doTranslateOscillate.target);
            script.doTranslateOscillate = doTranslateOscillate.target;

            if (EditorGUILayout.BeginFadeGroup(doTranslateOscillate.faded))
            {
                EditorGUI.indentLevel++;
                script.translateOscillateUpperBounds = EditorGUILayout.Vector3Field("Upper Bounds", script.translateOscillateUpperBounds);
                script.translateOscillateLowerBounds = EditorGUILayout.Vector3Field("Lower Bounds", script.translateOscillateLowerBounds);
                script.translateOscillateSpeed = EditorGUILayout.Vector3Field("Speed", script.translateOscillateSpeed);
                script.translateOscillateOffset = EditorGUILayout.Vector3Field("Offset", script.translateOscillateOffset);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFadeGroup();
        }

        void displayTranslateNoiseFields()
        {
            doTranslateNoise.target = EditorGUILayout.ToggleLeft("Translate Noise", doTranslateNoise.target);
            script.doTranslateNoise = doTranslateNoise.target;

            if (EditorGUILayout.BeginFadeGroup(doTranslateNoise.faded))
            {
                EditorGUI.indentLevel++;
                script.translateNoiseUpperBounds = EditorGUILayout.Vector3Field("Upper Bounds", script.translateNoiseUpperBounds);
                script.translateNoiseLowerBounds = EditorGUILayout.Vector3Field("Lower Bounds", script.translateNoiseLowerBounds);
                script.translateNoiseSpeed = EditorGUILayout.Vector3Field("Speed", script.translateNoiseSpeed);
                script.translateNoiseOffset = EditorGUILayout.Vector3Field("Offset", script.translateNoiseOffset);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFadeGroup();
        }

        void displayRotateFields()
        {
            doRotate.target = EditorGUILayout.ToggleLeft("Rotate", doRotate.target);
            script.doRotate = doRotate.target;

            if (EditorGUILayout.BeginFadeGroup(doRotate.faded))
            {
                EditorGUI.indentLevel++;
                script.rotate = EditorGUILayout.Vector3Field("rotation", script.rotate);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFadeGroup();
        }

        void displayRotateOscillateFields()
        {
            doRotateOscillate.target = EditorGUILayout.ToggleLeft("Rotate Oscillate", doRotateOscillate.target);
            script.doRotateOscillate = doRotateOscillate.target;

            if (EditorGUILayout.BeginFadeGroup(doRotateOscillate.faded))
            {
                EditorGUI.indentLevel++;
                script.rotateOscillateUpperBounds = EditorGUILayout.Vector3Field("Upper Bounds", script.rotateOscillateUpperBounds);
                script.rotateOscillateLowerBounds = EditorGUILayout.Vector3Field("Lower Bounds", script.rotateOscillateLowerBounds);
                script.rotateOscillateSpeed = EditorGUILayout.Vector3Field("Speed", script.rotateOscillateSpeed);
                script.rotateOscillateOffset = EditorGUILayout.Vector3Field("Offset", script.rotateOscillateOffset);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFadeGroup();
        }

        void displayRotateNoiseFields()
        {
            doRotateNoise.target = EditorGUILayout.ToggleLeft("Rotate Noise", doRotateNoise.target);
            script.doRotateNoise = doRotateNoise.target;

            if (EditorGUILayout.BeginFadeGroup(doRotateNoise.faded))
            {
                EditorGUI.indentLevel++;
                script.rotateNoiseUpperBounds = EditorGUILayout.Vector3Field("Upper Bounds", script.rotateNoiseUpperBounds);
                script.rotateNoiseLowerBounds = EditorGUILayout.Vector3Field("Lower Bounds", script.rotateNoiseLowerBounds);
                script.rotateNoiseSpeed = EditorGUILayout.Vector3Field("Speed", script.rotateNoiseSpeed);
                script.rotateNoiseOffset = EditorGUILayout.Vector3Field("Offset", script.rotateNoiseOffset);
                
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFadeGroup();
        }

        void displayScaleFields()
        {
            doScale.target = EditorGUILayout.ToggleLeft("Scale", doScale.target);
            script.doScale = doScale.target;

            if (EditorGUILayout.BeginFadeGroup(doScale.faded))
            {
                EditorGUI.indentLevel++;

                script.useInitialScale = EditorGUILayout.Toggle("Use Initial Scale", script.useInitialScale);
                script.scale = EditorGUILayout.Vector3Field("Scale", script.scale);
                script.scaleOffset = EditorGUILayout.Vector3Field("Scale Offset", script.scaleOffset);

                scaleUseBounds.target = EditorGUILayout.ToggleLeft("Use Bounds", scaleUseBounds.target);
                script.scaleUseBounds = scaleUseBounds.target;


                if (EditorGUILayout.BeginFadeGroup(scaleUseBounds.faded))
                {
                    EditorGUI.indentLevel++;

                    script.scalePingPong = EditorGUILayout.Toggle("Ping Pong", script.scalePingPong);
                    script.scaleDirection = EditorGUILayout.Vector3Field("Direction", script.scaleDirection);
                    script.scaleUpperBounds = EditorGUILayout.Vector3Field("Upper Bounds", script.scaleUpperBounds);
                    script.scaleLowerBounds = EditorGUILayout.Vector3Field("Lower Bounds", script.scaleLowerBounds);

                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndFadeGroup();

                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFadeGroup();
        }

        void displayScaleOscillateFields()
        {
            doScaleOscillate.target = EditorGUILayout.ToggleLeft("Scale Oscillate",doScaleOscillate.target);
            script.doScaleOscillate = doScaleOscillate.target;

            if (EditorGUILayout.BeginFadeGroup(doScaleOscillate.faded))
            {
                EditorGUI.indentLevel++;
                script.scaleOscillateUpperBounds = EditorGUILayout.Vector3Field("Upper Bounds", script.scaleOscillateUpperBounds);
                script.scaleOscillateLowerBounds = EditorGUILayout.Vector3Field("Lower Bounds", script.scaleOscillateLowerBounds);
                script.scaleOscillateSpeed = EditorGUILayout.Vector3Field("Speed", script.scaleOscillateSpeed);
                script.scaleOscillateOffset = EditorGUILayout.Vector3Field("Offset", script.scaleOscillateOffset);

                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFadeGroup();
        }
            
        void displayScaleNoiseFields()
        {
            doScaleNoise.target = EditorGUILayout.ToggleLeft("Scale Noise", doScaleNoise.target);
            script.doScaleNoise = doScaleNoise.target;

            if (EditorGUILayout.BeginFadeGroup(doScaleNoise.faded))
            {
                EditorGUI.indentLevel++;

                script.scaleNoiseUpperBounds = EditorGUILayout.Vector3Field("Upper Bounds", script.scaleNoiseUpperBounds);
                script.scaleNoiseLowerBounds = EditorGUILayout.Vector3Field("Lower Bounds", script.scaleNoiseLowerBounds);
                script.scaleNoiseSpeed = EditorGUILayout.Vector3Field("Speed", script.scaleNoiseSpeed);
                script.scaleNoiseOffset = EditorGUILayout.Vector3Field("Offset", script.scaleNoiseOffset);

                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFadeGroup();
        }
    }
}

