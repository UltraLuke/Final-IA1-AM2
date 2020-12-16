using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

public class BGLoader : EditorWindow
{
    public delegate void BGLoaderDelegate(BGPreset obj);
    public BGLoaderDelegate bgLoader;

    string _assetType;
    string _folderPath;

    List<BGPreset> _presetList;
    
    Vector2 scrollPos;

    public string AssetType { get => _assetType; set => _assetType = value; }
    public string FolderPath { get => _folderPath; set => _folderPath = value; }

    private void OnEnable()
    {
        ShowModalUtility();
        _presetList = new List<BGPreset>();
    }

    private void OnGUI()
    {
        if(focusedWindow != this)
        {
            Close();
        }
        if(_assetType != null && _folderPath != null && (_presetList == null || _presetList.Count <= 0))
        {
            var leaderPresetGUIs = AssetDatabase.FindAssets("t:" + _assetType);
            for (int i = 0; i < leaderPresetGUIs.Length; i++)
            {
                var presetPath = AssetDatabase.GUIDToAssetPath(leaderPresetGUIs[i]);
                var preset = AssetDatabase.LoadAssetAtPath<BGPreset>(presetPath);
                _presetList.Add(preset);
            }
        }
        else if(_presetList != null && _presetList.Count > 0)
        {
            EditorGUILayout.LabelField("Seleccione el item a cargar");
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width), GUILayout.Height(position.height - 130));

            for (int i = 0; i < _presetList.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.ObjectField(_presetList[i], typeof(Object), false);
                if (GUILayout.Button("Load"))
                {
                    if(bgLoader != null)
                    {
                        bgLoader(_presetList[i]);
                        Close();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }
        else
        {
            EditorGUILayout.HelpBox("No hay presets guardados", MessageType.Info);
        }
    }
}