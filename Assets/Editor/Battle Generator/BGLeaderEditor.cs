using System.Collections;
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class BGLeaderEditor : EditorWindow
{
    private GUIStyle _headerlv1;

    private BGLoader _bgLoader;
    private BGMain _bGMainWindow;

    private GameObject _leaderSceneIndicator;
    private bool _objectChanged;

    private int _index;
    private string _presetPath = "leader_presets";
    private string _presetName = "leader_pr.asset";

    private TeamSettings _ts;
    private Vector2 scrollPosition;

    public BGMain BGMainWindow { set => _bGMainWindow = value; }
    public int Index { get => _index; set => _index = value; }

    private void OnEnable()
    {
        _headerlv1 = new GUIStyle
        {
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            fontSize = 13,
        };

        _objectChanged = true;
        SceneView.duringSceneGui += OnSceneGUI;
    }
    private void OnDisable()
    {
        if (_bgLoader != null)
            _bgLoader.Close();

        SceneView.duringSceneGui -= OnSceneGUI;

        //if(_leaderSceneIndicator)
        //    DestroyImmediate(_leaderSceneIndicator);
    }
    private void OnGUI()
    {
        if (_bGMainWindow != null)
        {
            _ts = _bGMainWindow.TeamSettings[_index];

            EditorGUILayout.LabelField("Leader", _headerlv1);
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            if(_ts.leaderEntity == null)
            {
                EditorGUILayout.HelpBox("El Gameobject debe contener componentes que apliquen las siguientes interfaces: " +
                                        "\n- IHealth\n- ISpeed\n- IMelee\n- IShoot\n- IVision", MessageType.Info);
            }
            EditorGUI.BeginChangeCheck();
            _ts.leaderEntity = (GameObject)EditorGUILayout.ObjectField("Leader Target", _ts.leaderEntity, typeof(GameObject), false);
            if (EditorGUI.EndChangeCheck())
            {
                if (_ts.leaderEntity == null || CheckIfMeetsRequirements(_ts.leaderEntity))
                {
                    _objectChanged = true;
                }
                else
                {
                    EditorUtility.DisplayDialog("Gameobject no compatible", "El Gameobject debe contener componentes que apliquen las siguientes interfaces:" +
                                                                            "\n- IHealth" +
                                                                            "\n- ISpeed" +
                                                                            "\n- IMelee" +
                                                                            "\n- IShoot" +
                                                                            "\n- IVision", "Aceptar");
                    _ts.leaderEntity = null;
                }
            }
            _ts.leaderPosition = EditorGUILayout.Vector3Field("Leader Position", _ts.leaderPosition);
            _ts.leaderHealth = EditorGUILayout.FloatField("Leader Health", _ts.leaderHealth);
            _ts.leaderSpeed = EditorGUILayout.FloatField("Leader Speed", _ts.leaderSpeed);
            _ts.leaderMeleeDamage = EditorGUILayout.FloatField("Leader Melee Damage", _ts.leaderMeleeDamage);
            _ts.leaderMeleeRate = EditorGUILayout.FloatField("Leader Melee Rate", _ts.leaderMeleeRate);
            _ts.leaderMeleeDistance = EditorGUILayout.FloatField("Leader Melee Distance", _ts.leaderMeleeDistance);
            _ts.leaderShootDamage = EditorGUILayout.FloatField("Leader Shoot Damage", _ts.leaderShootDamage);
            _ts.leaderShootRate = EditorGUILayout.FloatField("Leader Shoot Rate", _ts.leaderShootRate);
            _ts.leaderShootDistance = EditorGUILayout.FloatField("Leader Shoot Distance", _ts.leaderShootDistance);
            _ts.leaderVisionDistance = EditorGUILayout.FloatField("Leader Vision Distance", _ts.leaderVisionDistance);
            _ts.leaderVisionRangeAngles = EditorGUILayout.FloatField("Leader Vision Range Angles", _ts.leaderVisionRangeAngles);

            _bGMainWindow.TeamSettings[_index] = _ts;

            if (_leaderSceneIndicator != null)
                AssignValuesToEntity();

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(_ts.leaderEntity == null);
            if (GUILayout.Button("Save Preset", GUILayout.Width(100)))
            {
                SavePreset();
            }
            EditorGUI.EndDisabledGroup();
            if (GUILayout.Button("Load Preset", GUILayout.Width(100)))
            {
                OpenLoader();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
        }
    }

    private bool CheckIfMeetsRequirements(GameObject leaderEntity)
    {
        if (leaderEntity.GetComponent<IHealth>() == null) return false;
        else if (leaderEntity.GetComponent<ISpeed>() == null) return false;
        else if (leaderEntity.GetComponent<IMelee>() == null) return false;
        else if (leaderEntity.GetComponent<IShooter>() == null) return false;
        else if (leaderEntity.GetComponent<IVision>() == null) return false;
        else return true;
    }
    private void AssignValuesToEntity()
    {
        HashSet<Component> components = new HashSet<Component>();
        List<Component> componentList = new List<Component>();

        componentList.Add(_leaderSceneIndicator.GetComponent<IHealth>().HealthSettings(_ts.leaderHealth));
        componentList.Add(_leaderSceneIndicator.GetComponent<ISpeed>().SpeedSettings(_ts.leaderSpeed));
        componentList.Add(_leaderSceneIndicator.GetComponent<IMelee>().MeleeSettings(_ts.leaderMeleeDamage, _ts.leaderMeleeRate, _ts.leaderMeleeDistance));
        componentList.Add(_leaderSceneIndicator.GetComponent<IShooter>().ShootSettings(_ts.leaderShootDamage, _ts.leaderShootRate, _ts.leaderShootDistance));
        componentList.Add(_leaderSceneIndicator.GetComponent<IVision>().VisionSettings(_ts.leaderVisionDistance, _ts.leaderVisionRangeAngles));

        for (int i = 0; i < componentList.Count; i++)
        {
            if (!components.Add(componentList[i]))
            {
                componentList.RemoveAt(i);
                i--;
            }
            else
            {
                PrefabUtility.RecordPrefabInstancePropertyModifications(componentList[i]);
            }
        }
    }

    #region Save system
    private void SavePreset()
    {
        var scriptable = CreateInstance<BGLeaderPreset>();
        var path = _bGMainWindow.SaveFolderPath + "/" + _presetPath;

        LoadToScriptable(scriptable);

        if (!AssetDatabase.IsValidFolder(path))
        {
            AssetDatabase.CreateFolder(_bGMainWindow.SaveFolderPath, _presetPath);
        }
        path = path + "/" + _presetName;
        path = AssetDatabase.GenerateUniqueAssetPath(path);
        AssetDatabase.CreateAsset(scriptable, path);
    }
    private void OpenLoader()
    {
        _bgLoader = GetWindow<BGLoader>();
        _bgLoader.bgLoader += LoadFromScriptable;
        _bgLoader.AssetType = "BGLeaderPreset";
        _bgLoader.FolderPath = _bGMainWindow.SaveFolderPath + "/" + _presetPath;
        _bgLoader.Show();
    }
    void LoadToScriptable(BGLeaderPreset scriptable)
    {
        scriptable.leaderEntity = _ts.leaderEntity;
        scriptable.leaderPosition = _ts.leaderPosition;
        scriptable.leaderHealth = _ts.leaderHealth;
        scriptable.leaderSpeed = _ts.leaderSpeed;
        scriptable.leaderMeleeDamage = _ts.leaderMeleeDamage;
        scriptable.leaderMeleeRate = _ts.leaderMeleeRate;
        scriptable.leaderMeleeDistance = _ts.leaderMeleeDistance;
        scriptable.leaderShootDamage = _ts.leaderShootDamage;
        scriptable.leaderShootRate = _ts.leaderShootRate;
        scriptable.leaderShootDistance = _ts.leaderShootDistance;
        scriptable.leaderVisionDistance = _ts.leaderVisionDistance;
        scriptable.leaderVisionRangeAngles = _ts.leaderVisionRangeAngles;
    }
    void LoadFromScriptable(BGPreset scriptable)
    {
        if (scriptable.GetType() != typeof(BGLeaderPreset)) return;
        var castedScriptable = (BGLeaderPreset)scriptable;

        _ts = _bGMainWindow.TeamSettings[_index];

        _ts.leaderEntity = castedScriptable.leaderEntity;
        _ts.leaderPosition = castedScriptable.leaderPosition;
        _ts.leaderHealth = castedScriptable.leaderHealth;
        _ts.leaderSpeed = castedScriptable.leaderSpeed;
        _ts.leaderMeleeDamage = castedScriptable.leaderMeleeDamage;
        _ts.leaderMeleeRate = castedScriptable.leaderMeleeRate;
        _ts.leaderMeleeDistance = castedScriptable.leaderMeleeDistance;
        _ts.leaderShootDamage = castedScriptable.leaderShootDamage;
        _ts.leaderShootRate = castedScriptable.leaderShootRate;
        _ts.leaderShootDistance = castedScriptable.leaderShootDistance;
        _ts.leaderVisionDistance = castedScriptable.leaderVisionDistance;
        _ts.leaderVisionRangeAngles = castedScriptable.leaderVisionRangeAngles;

        _bGMainWindow.TeamSettings[_index] = _ts;
        _objectChanged = true;
        Repaint();
        SceneView.RepaintAll();
    }
    #endregion

    private void OnSceneGUI(SceneView sceneView)
    {
        _leaderSceneIndicator = _bGMainWindow.Leader[_index];

        if (_ts.leaderEntity == null)
        {
            if (_leaderSceneIndicator != null)
                Undo.DestroyObjectImmediate(_leaderSceneIndicator);

            return;
        }

        if (_objectChanged)
        {
            if (_leaderSceneIndicator != null)
                Undo.DestroyObjectImmediate(_leaderSceneIndicator);
            _leaderSceneIndicator = (GameObject)PrefabUtility.InstantiatePrefab(_ts.leaderEntity, _bGMainWindow.Container.transform);
            _objectChanged = false;

            Undo.RegisterCreatedObjectUndo(_leaderSceneIndicator, "Leader creado");
        }

        if (_leaderSceneIndicator == null) return;

        _leaderSceneIndicator.transform.position = _ts.leaderPosition;

        _bGMainWindow.Leader[_index] = _bGMainWindow.Container.Leaders[_index] = _leaderSceneIndicator;
    }
}
