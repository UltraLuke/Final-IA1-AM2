using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WPLoad : EditorWindow
{
    public delegate void WayPointsLoader(string fileName);
    public WayPointsLoader wpLoader;

    string _saveFolderPath;
    List<WaypointsInfo> _waypointsInfos;
    public string SaveFolderPath { set => _saveFolderPath = value; }

    bool _folderExists;

    private void OnEnable()
    {
        _waypointsInfos = new List<WaypointsInfo>();
    }

    private void OnGUI()
    {
        if (_saveFolderPath != null && (_waypointsInfos == null || _waypointsInfos.Count <= 0))
        {
            var wpInfosGUID = AssetDatabase.FindAssets("t:WaypointsInfo");

            for (int i = 0; i < wpInfosGUID.Length; i++)
            {
                var wpPath = AssetDatabase.GUIDToAssetPath(wpInfosGUID[i]);
                var wp = AssetDatabase.LoadAssetAtPath<WaypointsInfo>(wpPath);
                _waypointsInfos.Add(wp);
            }
        }
        else if (_waypointsInfos != null && _waypointsInfos.Count > 0)
        {
            EditorGUILayout.LabelField("Seleccione el grupo de waypoints a cargar");
            for (int i = 0; i < _waypointsInfos.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.ObjectField(_waypointsInfos[i], typeof(WaypointsInfo), false);
                if (GUILayout.Button("Load"))
                {
                    if (wpLoader != null)
                    {
                        wpLoader(_waypointsInfos[i].name + ".asset");
                        Close();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
