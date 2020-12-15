using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

public class BGFlockingEditor : EditorWindow
{
    private GUIStyle _headerlv1;

    private BGLoader _bgLoader;
    private BGMain _bGMainWindow;

    private List<GameObject> _minionSceneIndicators = new List<GameObject>();
    private bool _objectChanged;
    private bool _objectQtyChanged;

    private int _index;
    private string _presetPath = "flocking_presets";
    private string _presetName = "flocking_pr.asset";

    private TeamSettings _ts;

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

        SceneView.duringSceneGui += OnSceneGUI;
    }
    private void OnDisable()
    {
        if (_bgLoader != null)
            _bgLoader.Close();

        SceneView.duringSceneGui -= OnSceneGUI;
    }
    private void OnGUI()
    {
        if (_bGMainWindow != null)
        {
            _ts = _bGMainWindow.TeamSettings[_index];
            EditorGUILayout.LabelField("Minion Settings", _headerlv1);
            EditorGUI.BeginChangeCheck();
            _ts.minionEntity = (GameObject)EditorGUILayout.ObjectField("Minion Prefab", _ts.minionEntity, typeof(GameObject), false);
            if (EditorGUI.EndChangeCheck())
            {
                _objectChanged = true;
            }
            _ts.minionSpawnAreaPosition = EditorGUILayout.Vector3Field("Minion Spawn Area Position", _ts.minionSpawnAreaPosition);
            Vector2 area = EditorGUILayout.Vector2Field("Minion Spawn Area", new Vector2(_ts.minionSpawnAreaWidth, _ts.minionSpawnAreaLength));
            _ts.minionSpawnAreaWidth = area.x.ClampMinValue(.1f);
            _ts.minionSpawnAreaLength = area.y.ClampMinValue(.1f);
            EditorGUI.BeginChangeCheck();
            _ts.minionsQuantityRow = EditorGUILayout.IntField("Minions Quantity", _ts.minionsQuantityRow).ClampMinValue(0);
            _ts.minionsQuantityColumn = EditorGUILayout.IntField("Minions Quantity", _ts.minionsQuantityColumn).ClampMinValue(0);
            if (EditorGUI.EndChangeCheck())
            {
                _objectQtyChanged = true;
            }
            _ts.minionHealth = EditorGUILayout.FloatField("Minion Health", _ts.minionHealth).ClampMinValue(.1f);
            _ts.minionSpeed = EditorGUILayout.FloatField("Minion Speed", _ts.minionSpeed).ClampMinValue(.1f);
            _ts.minionMeleeDamage = EditorGUILayout.FloatField("Minion Melee Damage", _ts.minionMeleeDamage).ClampMinValue(.1f);
            _ts.minionMeleeRate = EditorGUILayout.FloatField("Minion Melee Rate", _ts.minionMeleeRate).ClampMinValue(.1f);
            _ts.minionMeleeDistance = EditorGUILayout.FloatField("Minion Melee Distance", _ts.minionMeleeDistance).ClampMinValue(.1f);
            _ts.minionShootDamage = EditorGUILayout.FloatField("Minion Shoot Damage", _ts.minionShootDamage).ClampMinValue(.1f);
            _ts.minionShootRate = EditorGUILayout.FloatField("Minion Shoot Rate", _ts.minionShootRate).ClampMinValue(.1f);
            _ts.minionShootDistance = EditorGUILayout.FloatField("Minion Shoot Distance", _ts.minionShootDistance).ClampMinValue(.1f);
            _ts.minionVisionDistance = EditorGUILayout.FloatField("Minion Vision Distance", _ts.minionVisionDistance).ClampMinValue(.1f);
            _ts.minionVisionRangeAngles = EditorGUILayout.FloatField("Minion Vision Range Angles", _ts.minionVisionRangeAngles).ClampMinValue(.1f);

            EditorGUILayout.LabelField("Flocking Settings", _headerlv1);
            _ts.flockEntityRadius = EditorGUILayout.FloatField("Flock Entity Radius", _ts.flockEntityRadius).ClampMinValue(.1f);

            LayerMask tempMask = EditorGUILayout.MaskField("Flock Entity Mask", InternalEditorUtility.LayerMaskToConcatenatedLayersMask(_ts.flockEntityMask), InternalEditorUtility.layers);
            _ts.flockEntityMask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);

            _ts.flockLeaderBehaviourWeight = EditorGUILayout.FloatField("Flock Leader Behaviour Weight", _ts.flockLeaderBehaviourWeight).ClampMinValue(.1f);
            _ts.flockLeaderBehaviourMinDistance = EditorGUILayout.FloatField("Flock Leader Behaviour Min Distance", _ts.flockLeaderBehaviourMinDistance).ClampMinValue(.1f);
            _ts.flockAlineationBehaviourWeight = EditorGUILayout.FloatField("Flock Alineation Behaviour Weight", _ts.flockAlineationBehaviourWeight).ClampMinValue(.1f);
            _ts.flockSeparationBehaviourWeight = EditorGUILayout.FloatField("Flock Separation Behaviour Weight", _ts.flockSeparationBehaviourWeight).ClampMinValue(.1f);
            _ts.flockSeparationBehaviourRange = EditorGUILayout.FloatField("Flock Separation Behaviour Range", _ts.flockSeparationBehaviourRange).ClampMinValue(.1f);
            _ts.flockCohesionBehaviourWeight = EditorGUILayout.FloatField("Flock Cohesion Behaviour Weight", _ts.flockCohesionBehaviourWeight).ClampMinValue(.1f);
            _ts.flockAvoidanceBehaviourWeight = EditorGUILayout.FloatField("Flock Avoidance Behaviour Weight", _ts.flockAvoidanceBehaviourWeight).ClampMinValue(.1f);

            LayerMask tempMask2 = EditorGUILayout.MaskField("Flock Avoidance Behaviour Mask", InternalEditorUtility.LayerMaskToConcatenatedLayersMask(_ts.flockAvoidanceBehaviourMask), InternalEditorUtility.layers);
            _ts.flockAvoidanceBehaviourMask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask2);

            _ts.flockAvoidanceBehaviourRange = EditorGUILayout.FloatField("Flock Avoidance Behaviour Range", _ts.flockAvoidanceBehaviourRange).ClampMinValue(.1f);

            _bGMainWindow.TeamSettings[_index] = _ts;

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(_ts.minionEntity == null);
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
        }
    }

    #region Save and Load
    private void SavePreset()
    {
        var scriptable = CreateInstance<BGFlockingPreset>();
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
        _bgLoader.AssetType = "BGFlockingPreset";
        _bgLoader.FolderPath = _bGMainWindow.SaveFolderPath + "/" + _presetPath;
        _bgLoader.Show();
    }
    void LoadToScriptable(BGFlockingPreset scriptable)
    {
        scriptable.minionEntity = _ts.minionEntity;

        scriptable.minionSpawnAreaPosition = _ts.minionSpawnAreaPosition;
        scriptable.minionSpawnAreaWidth = _ts.minionSpawnAreaWidth;
        scriptable.minionSpawnAreaLength = _ts.minionSpawnAreaLength;
        scriptable.minionsQuantityRow = _ts.minionsQuantityRow;
        scriptable.minionsQuantityColumn = _ts.minionsQuantityColumn;
        scriptable.minionHealth = _ts.minionHealth;
        scriptable.minionSpeed = _ts.minionSpeed;
        scriptable.minionMeleeDamage = _ts.minionMeleeDamage;
        scriptable.minionMeleeRate = _ts.minionMeleeRate;
        scriptable.minionMeleeDistance = _ts.minionMeleeDistance;
        scriptable.minionShootDamage = _ts.minionShootDamage;
        scriptable.minionShootRate = _ts.minionShootRate;
        scriptable.minionShootDistance = _ts.minionShootDistance;
        scriptable.minionVisionDistance = _ts.minionVisionDistance;
        scriptable.minionVisionRangeAngles = _ts.minionVisionRangeAngles;

        scriptable.flockEntityRadius = _ts.flockEntityRadius;
        scriptable.flockEntityMask = _ts.flockEntityMask;
        scriptable.flockLeaderBehaviourWeight = _ts.flockLeaderBehaviourWeight;
        scriptable.flockLeaderBehaviourMinDistance = _ts.flockLeaderBehaviourMinDistance;
        scriptable.flockAlineationBehaviourWeight = _ts.flockAlineationBehaviourWeight;
        scriptable.flockSeparationBehaviourWeight = _ts.flockSeparationBehaviourWeight;
        scriptable.flockSeparationBehaviourRange = _ts.flockSeparationBehaviourRange;
        scriptable.flockCohesionBehaviourWeight = _ts.flockCohesionBehaviourWeight;
        scriptable.flockAvoidanceBehaviourWeight = _ts.flockAvoidanceBehaviourWeight;
        scriptable.flockAvoidanceBehaviourMask = _ts.flockAvoidanceBehaviourMask;
        scriptable.flockAvoidanceBehaviourRange = _ts.flockAvoidanceBehaviourRange;
    }
    void LoadFromScriptable(BGPreset scriptable)
    {
        if (scriptable.GetType() != typeof(BGFlockingPreset)) return;
        var castedScriptable = (BGFlockingPreset)scriptable;

        _ts = _bGMainWindow.TeamSettings[_index];

        _ts.minionEntity = castedScriptable.minionEntity;
        _ts.minionSpawnAreaPosition = castedScriptable.minionSpawnAreaPosition;
        _ts.minionSpawnAreaWidth = castedScriptable.minionSpawnAreaWidth;
        _ts.minionSpawnAreaLength = castedScriptable.minionSpawnAreaLength;
        _ts.minionsQuantityRow = castedScriptable.minionsQuantityRow;
        _ts.minionsQuantityColumn = castedScriptable.minionsQuantityColumn;
        _ts.minionHealth = castedScriptable.minionHealth;
        _ts.minionSpeed = castedScriptable.minionSpeed;
        _ts.minionMeleeDamage = castedScriptable.minionMeleeDamage;
        _ts.minionMeleeRate = castedScriptable.minionMeleeRate;
        _ts.minionMeleeDistance = castedScriptable.minionMeleeDistance;
        _ts.minionShootDamage = castedScriptable.minionShootDamage;
        _ts.minionShootRate = castedScriptable.minionShootRate;
        _ts.minionShootDistance = castedScriptable.minionShootDistance;
        _ts.minionVisionDistance = castedScriptable.minionVisionDistance;
        _ts.minionVisionRangeAngles = castedScriptable.minionVisionRangeAngles;

        _ts.flockEntityRadius = castedScriptable.flockEntityRadius;
        _ts.flockEntityMask = castedScriptable.flockEntityMask;
        _ts.flockLeaderBehaviourWeight = castedScriptable.flockLeaderBehaviourWeight;
        _ts.flockLeaderBehaviourMinDistance = castedScriptable.flockLeaderBehaviourMinDistance;
        _ts.flockAlineationBehaviourWeight = castedScriptable.flockAlineationBehaviourWeight;
        _ts.flockSeparationBehaviourWeight = castedScriptable.flockSeparationBehaviourWeight;
        _ts.flockSeparationBehaviourRange = castedScriptable.flockSeparationBehaviourRange;
        _ts.flockCohesionBehaviourWeight = castedScriptable.flockCohesionBehaviourWeight;
        _ts.flockAvoidanceBehaviourWeight = castedScriptable.flockAvoidanceBehaviourWeight;
        _ts.flockAvoidanceBehaviourMask = castedScriptable.flockAvoidanceBehaviourMask;
        _ts.flockAvoidanceBehaviourRange = castedScriptable.flockAvoidanceBehaviourRange;

        _bGMainWindow.TeamSettings[_index] = _ts;
        _objectChanged = _objectQtyChanged = true;
        Repaint();
        SceneView.RepaintAll();
    }
    #endregion

    private void OnSceneGUI(SceneView sceneView)
    {
        //Area de spawneo minions
        var MyPosForward = _ts.minionSpawnAreaPosition + Vector3.forward * _ts.minionSpawnAreaLength;
        var MyPosRight = _ts.minionSpawnAreaPosition + Vector3.right * _ts.minionSpawnAreaWidth;
        Handles.DrawDottedLine(_ts.minionSpawnAreaPosition, MyPosForward, 2);
        Handles.DrawDottedLine(_ts.minionSpawnAreaPosition, MyPosRight, 2);
        Handles.DrawDottedLine(MyPosForward, MyPosForward + Vector3.right * _ts.minionSpawnAreaWidth, 2);
        Handles.DrawDottedLine(MyPosRight, MyPosRight + Vector3.forward * _ts.minionSpawnAreaLength, 2);

        if (_ts.minionEntity == null) return;

        if (_objectChanged)
        {
            if (_minionSceneIndicators != null && _minionSceneIndicators.Count != 0)
            {
                for (int i = 0; i < _minionSceneIndicators.Count; i++)
                {
                    if (_minionSceneIndicators[i] != null)
                        DestroyImmediate(_minionSceneIndicators[i]);
                    _minionSceneIndicators[i] = (GameObject)PrefabUtility.InstantiatePrefab(_ts.minionEntity);
                }
            }
        }
        if (_objectQtyChanged)
        {
            int totalQty = _ts.minionsQuantityRow * _ts.minionsQuantityColumn;
            int difference;

            if (_minionSceneIndicators.Count < totalQty)
            {
                difference = totalQty - _minionSceneIndicators.Count;
                for (int i = 0; i < difference; i++)
                {
                    _minionSceneIndicators.Add((GameObject)PrefabUtility.InstantiatePrefab(_ts.minionEntity));
                }
            }
            else if (_minionSceneIndicators.Count > totalQty)
            {
                difference = _minionSceneIndicators.Count - totalQty;
                int lastIndex;
                for (int i = 0; i < difference; i++)
                {
                    lastIndex = _minionSceneIndicators.Count - 1;

                    if (_minionSceneIndicators[lastIndex] != null)
                        DestroyImmediate(_minionSceneIndicators[lastIndex]);

                    _minionSceneIndicators.RemoveAt(lastIndex);
                }
            }
        }

        if (_minionSceneIndicators == null && _minionSceneIndicators.Count == 0) return;

        if (_objectChanged || _objectQtyChanged)
            CalculatePositions();
        
        _objectChanged = false;
        _objectQtyChanged = false;

    }

    private void CalculatePositions()
    {
        //_calculatingPositions = true;

        float xPos, zPos;
        int rows = _ts.minionsQuantityRow;
        int columns = _ts.minionsQuantityColumn;
        Vector3 originPoint = _ts.minionSpawnAreaPosition;

        var rowDivision = _ts.minionSpawnAreaLength / (rows + 1);
        var columnDivision = _ts.minionSpawnAreaWidth / (columns + 1);

        Vector3 position;

        //_waypointData = new List<WaypointData>();
        int indexMinions = 0;
        for (int r = 0; r < rows; r++)
        {
            zPos = (r + 1) * rowDivision + originPoint.z;
            for (int c = 0; c < columns; c++)
            {
                xPos = (c + 1) * columnDivision + originPoint.x;
                position = new Vector3(xPos, originPoint.y, zPos);

                if (_minionSceneIndicators != null)
                    _minionSceneIndicators[indexMinions].transform.position = position;

                indexMinions++;
            }
        }

        //_calculatingPositions = false;
    }
}
