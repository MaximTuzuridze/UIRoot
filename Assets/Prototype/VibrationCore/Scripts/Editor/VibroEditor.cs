using MoreMountains.NiceVibrations;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;


namespace Prototype.Vibration
{
    [CustomEditor(typeof(Vibro))]
    public class VibroEditor : Editor
    {
        Vibro _target;

        SerializedProperty patternProperty;

        SerializedProperty amplitudesProperty;

        public void OnEnable()
        {
            _target = (Vibro)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (_target == null)
                _target = (Vibro)target;

            patternProperty = serializedObject.FindProperty("pattern");

            amplitudesProperty = serializedObject.FindProperty("amplitudes");

            Draw();

            serializedObject.ApplyModifiedProperties();
        }

        public void Draw()
        {
            GUILayout.BeginVertical("box");
            {
                _target.Type = (TVibration)EditorGUILayout.EnumPopup("Тип вібрації(власна чи створена):", _target.Type);

                if (_target.Type == TVibration.Common)
                {
                    _target.HapticTypes = (HapticTypes)EditorGUILayout.EnumPopup("Тип створеної вібрації:", _target.HapticTypes);
                }
                else
                {
                    EditorGUILayout.PropertyField(patternProperty, new GUIContent("Патерни: "), true);

                    EditorGUILayout.PropertyField(amplitudesProperty, new GUIContent("Амплітуди: "), true);
                }

            }
            GUILayout.EndVertical();
        }
    }
}
