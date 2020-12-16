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

    private List<GameObject> _minionSceneIndicators;
    private bool _objectChanged;
    private bool _objectQtyPosChanged;
    private bool _meetFlockingRequirements;
    private bool _firstCodeRun = true;

    private int _index;
    private string _presetPath = "flocking_presets";
    private string _presetName = "flocking_pr.asset";

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

        SceneView.duringSceneGui += OnSceneGUI;

        _firstCodeRun = true;
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
            _minionSceneIndicators = _bGMainWindow.Minions[_index];
            if (_firstCodeRun)
            {
                if (_ts.minionEntity != null)
                    _meetFlockingRequirements = CheckIfMeetsFlockingRequirements(_ts.minionEntity);

                _firstCodeRun = false;
            }
            EditorGUILayout.LabelField("Minion Settings", _headerlv1);
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            if(_ts.minionEntity == null)
            {
                EditorGUILayout.HelpBox("El Gameobject debe contener componentes que apliquen las siguientes interfaces: " +
                                        "\n- IHealth\n- ISpeed\n- IMelee\n- IShoot\n- IVision", MessageType.Info);
            }
            EditorGUI.BeginChangeCheck();
            _ts.minionEntity = (GameObject)EditorGUILayout.ObjectField("Minion Prefab", _ts.minionEntity, typeof(GameObject), false);
            if (EditorGUI.EndChangeCheck())
            {
                if (_ts.minionEntity == null || CheckIfMeetsRequirements(_ts.minionEntity))
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
                    _ts.minionEntity = null;
                }

                if (_ts.minionEntity != null)
                    _meetFlockingRequirements = CheckIfMeetsFlockingRequirements(_ts.minionEntity);
            }
            EditorGUI.BeginChangeCheck();
            _ts.minionSpawnAreaPosition = EditorGUILayout.Vector3Field("Minion Spawn Area Position", _ts.minionSpawnAreaPosition);
            Vector2 area = EditorGUILayout.Vector2Field("Minion Spawn Area", new Vector2(_ts.minionSpawnAreaWidth, _ts.minionSpawnAreaLength));
            _ts.minionSpawnAreaWidth = area.x.ClampMinValue(.1f);
            _ts.minionSpawnAreaLength = area.y.ClampMinValue(.1f);
            _ts.minionsQuantityRow = EditorGUILayout.IntField("Minions Quantity", _ts.minionsQuantityRow).ClampMinValue(0);
            _ts.minionsQuantityColumn = EditorGUILayout.IntField("Minions Quantity", _ts.minionsQuantityColumn).ClampMinValue(0);
            if (EditorGUI.EndChangeCheck())
            {
                _objectQtyPosChanged = true;
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
            //Debug.Log((_ts.minionEntity == null) + "|" + _meetFlockingRequirements);
            if(_ts.minionEntity == null || !_meetFlockingRequirements)
            {
                EditorGUILayout.HelpBox("Para operar variables de flocking, el GameObject debe contener los siguientes componentes:" +
                                        "\n-Flock Entity\n-Leader Behaviour\n-Alineation Behaviour\n-Separation Behaviour" +
                                        "\n-Cohesion Behaviour\n-Avoidance Behaviour",MessageType.Info);
            }
            else
            {
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
            }

            _bGMainWindow.TeamSettings[_index] = _ts;
            _bGMainWindow.Minions[_index] = _bGMainWindow.Container.Minions[_index] = _minionSceneIndicators;
            _bGMainWindow.Container.FlockingGroup[_index].areaPosition = _ts.minionSpawnAreaPosition;
            _bGMainWindow.Container.FlockingGroup[_index].areaSize = new Vector3(_ts.minionSpawnAreaWidth, _ts.minionSpawnAreaLength);
            _bGMainWindow.Container.FlockingGroup[_index].quantityRow = _ts.minionsQuantityRow;
            _bGMainWindow.Container.FlockingGroup[_index].quantityColumn = _ts.minionsQuantityColumn;

            //Debug.Log(_minionSceneIndicators.Count);
            if (_minionSceneIndicators != null && _minionSceneIndicators.Count > 0)
            {
                for (int i = 0; i < _minionSceneIndicators.Count; i++)
                {
                    AssignValuesToEntity(_minionSceneIndicators[i]);
                    AssingFlockingValuesToEntity(_minionSceneIndicators[i]);
                }
            }

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
            EditorGUILayout.EndScrollView();
        }
    }

    private bool CheckIfMeetsRequirements(GameObject minionEntity)
    {
        if (minionEntity.GetComponent<IHealth>() == null) return false;
        else if (minionEntity.GetComponent<ISpeed>() == null) return false;
        else if (minionEntity.GetComponent<IMelee>() == null) return false;
        else if (minionEntity.GetComponent<IShooter>() == null) return false;
        else if (minionEntity.GetComponent<IVision>() == null) return false;
        else return true;
    }
    private bool CheckIfMeetsFlockingRequirements(GameObject minionEntity)
    {
        if (minionEntity.GetComponent<FlockEntity>() == null) return false;
        else if (minionEntity.GetComponent<LeaderBehavior>() == null) return false;
        else if (minionEntity.GetComponent<AlineationBehavior>() == null) return false;
        else if (minionEntity.GetComponent<SeparationBehavior>() == null) return false;
        else if (minionEntity.GetComponent<CohesionBehavior>() == null) return false;
        else if (minionEntity.GetComponent<AvoidanceBehavior>() == null) return false;
        else return true;
    }
    private void AssignValuesToEntity(GameObject minionEntity)
    {
        HashSet<Component> components = new HashSet<Component>();
        List<Component> componentList = new List<Component>();

        componentList.Add(minionEntity.GetComponent<IHealth>().HealthSettings(_ts.minionHealth));
        componentList.Add(minionEntity.GetComponent<ISpeed>().SpeedSettings(_ts.minionSpeed));
        componentList.Add(minionEntity.GetComponent<IMelee>().MeleeSettings(_ts.minionMeleeDamage, _ts.minionMeleeRate, _ts.minionMeleeDistance));
        componentList.Add(minionEntity.GetComponent<IShooter>().ShootSettings(_ts.minionShootDamage, _ts.minionShootRate, _ts.minionShootDistance));
        componentList.Add(minionEntity.GetComponent<IVision>().VisionSettings(_ts.minionVisionDistance, _ts.minionVisionRangeAngles));

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
    private void AssingFlockingValuesToEntity(GameObject minionEntity)
    {
        var flockEntity = minionEntity.GetComponent<FlockEntity>();
        var leaderBehaviour = minionEntity.GetComponent<LeaderBehavior>();
        var alineationBehavior = minionEntity.GetComponent<AlineationBehavior>();
        var separationBehavior = minionEntity.GetComponent<SeparationBehavior>();
        var cohesionBehavior = minionEntity.GetComponent<CohesionBehavior>();
        var avoidanceBehavior = minionEntity.GetComponent<AvoidanceBehavior>();

        flockEntity.maskEntity = _ts.flockEntityMask;
        flockEntity.radius = _ts.flockEntityRadius;
        leaderBehaviour.leaderWeight = _ts.flockLeaderBehaviourWeight;
        leaderBehaviour.target = _bGMainWindow.Leader[_index].transform;
        leaderBehaviour.minDistance = _ts.flockLeaderBehaviourMinDistance;
        alineationBehavior.alineationWeight = _ts.flockAlineationBehaviourWeight;
        separationBehavior.separationWeight = _ts.flockSeparationBehaviourWeight;
        separationBehavior.range = _ts.flockSeparationBehaviourRange;
        cohesionBehavior.cohesionWeight = _ts.flockCohesionBehaviourWeight;
        avoidanceBehavior.avoidanceWeight = _ts.flockAvoidanceBehaviourWeight;
        avoidanceBehavior.mask = _ts.flockAvoidanceBehaviourMask;
        avoidanceBehavior.range = _ts.flockAvoidanceBehaviourRange;

        PrefabUtility.RecordPrefabInstancePropertyModifications(flockEntity);
        PrefabUtility.RecordPrefabInstancePropertyModifications(leaderBehaviour);
        PrefabUtility.RecordPrefabInstancePropertyModifications(alineationBehavior);
        PrefabUtility.RecordPrefabInstancePropertyModifications(separationBehavior);
        PrefabUtility.RecordPrefabInstancePropertyModifications(cohesionBehavior);
        PrefabUtility.RecordPrefabInstancePropertyModifications(avoidanceBehavior);
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
        _objectChanged = _objectQtyPosChanged = true;

        if (_ts.minionEntity != null)
            _meetFlockingRequirements = CheckIfMeetsFlockingRequirements(_ts.minionEntity);

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

        if (_ts.minionEntity == null)
        {
            if(_minionSceneIndicators != null || _minionSceneIndicators.Count != 0)
            {
                for (int i = 0; i < _minionSceneIndicators.Count; i++)
                    Undo.DestroyObjectImmediate(_minionSceneIndicators[i]);
            }

            return;
        }
        if (_objectChanged)
        {
            if (_minionSceneIndicators != null && _minionSceneIndicators.Count != 0)
            {
                for (int i = 0; i < _minionSceneIndicators.Count; i++)
                {
                    if (_minionSceneIndicators[i] != null)
                        Undo.DestroyObjectImmediate(_minionSceneIndicators[i]);

                    _minionSceneIndicators[i] = (GameObject)PrefabUtility.InstantiatePrefab(_ts.minionEntity, _bGMainWindow.Container.transform);
                    Undo.RegisterCreatedObjectUndo(_minionSceneIndicators[i], "Minion creado");
                }
            }
        }
        if (_objectQtyPosChanged)
        {
            int totalQty = _ts.minionsQuantityRow * _ts.minionsQuantityColumn;
            int difference;

            if (_minionSceneIndicators.Count < totalQty)
            {
                difference = totalQty - _minionSceneIndicators.Count;
                for (int i = 0; i < difference; i++)
                {
                    _minionSceneIndicators.Add((GameObject)PrefabUtility.InstantiatePrefab(_ts.minionEntity, _bGMainWindow.Container.transform));
                    Undo.RegisterCreatedObjectUndo(_minionSceneIndicators[_minionSceneIndicators.Count - 1], "Minion creado");
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
                    {
                        Undo.DestroyObjectImmediate(_minionSceneIndicators[lastIndex]);
                    }

                    _minionSceneIndicators.RemoveAt(lastIndex);
                }
            }
        }

        if (_minionSceneIndicators == null && _minionSceneIndicators.Count == 0) return;

        if (_objectChanged || _objectQtyPosChanged)
            CalculatePositions();
        
        _objectChanged = false;
        _objectQtyPosChanged = false;
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
