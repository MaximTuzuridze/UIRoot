using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using TMPro;
using Core.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UIElementText : UIWindowElement
{
    public ParameterType ParamType;
    public string ParameterName;
    [HideInInspector] public bool AdditionalText;
    [HideInInspector] public string PreText;
    [HideInInspector] public string AfterText;

    private TextMeshProUGUI textMesh;

    public override void OnInitialize()
    {
        base.OnInitialize();
        textMesh = GetComponent<TextMeshProUGUI>();
        SetText();
    }

    public override void OnShowParentWindow()
    {
        base.OnShowParentWindow();
        UpdateText();
    }

    private void UpdateText()
    {
        if (string.IsNullOrEmpty(ParameterName))
            return;

        if (ParamType == ParameterType.String)
            UIRoot.Instance.Storage.ParameterUpdate<string>(ParameterName);
        else if (ParamType == ParameterType.Float)
            UIRoot.Instance.Storage.ParameterUpdate<float>(ParameterName);
        else
            UIRoot.Instance.Storage.ParameterUpdate<int>(ParameterName);
    }

    private void SetText()
    {
        if (string.IsNullOrEmpty(ParameterName))
            return;

        if (ParamType == ParameterType.String)
            UIRoot.Instance.Storage.OnParameterChange<string>(ParameterName,SetText);
        else if(ParamType == ParameterType.Float)
            UIRoot.Instance.Storage.OnParameterChange<float>(ParameterName, SetText);
        else
            UIRoot.Instance.Storage.OnParameterChange<int>(ParameterName, SetText);
    }

    private void SetText(string value) => textMesh.text = PreText + value + AfterText;

    private void SetText(float value) => textMesh.text = PreText + value.ToString() + AfterText;

    private void SetText(int value) => textMesh.text = PreText + value.ToString() + AfterText;

    public enum ParameterType
    {
        String,
        Int,
        Float            
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(UIElementText))]
public class MyScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var mText = target as UIElementText;

        mText.AdditionalText = GUILayout.Toggle(mText.AdditionalText, "Additional Text");

        using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(mText.AdditionalText)))
        {
            if (group.visible == true)
            {
                EditorGUILayout.PrefixLabel("Pre field");
                mText.PreText = EditorGUILayout.TextField(mText.PreText);
                EditorGUILayout.PrefixLabel("After field");
                mText.AfterText = EditorGUILayout.TextField(mText.AfterText);
            }
        }
    }
}
#endif
