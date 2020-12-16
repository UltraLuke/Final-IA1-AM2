using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class BGMain : EditorWindow
{
    GUIStyle _title;
    GUIStyle _headerlv1;
    GUIStyle _headerlv2;

    string _saveFolderPath = "Assets/battle_generator_saves";
    //string _savePresetName;

    PathFindingEditor _pathFindingEditorWindow;
    BGLeaderEditor _bGLeaderEditorWindow;
    BGFlockingEditor _bGFlockingEditorWindow;
    BGContainer _container;

    int _tsIndex;
    bool[] _pressedTeams = new bool[2];
    TeamSettings[] _teamSettings = new TeamSettings[2];
    List<List<GameObject>> _minions = new List<List<GameObject>>() { null, null };
    List<GameObject> _leaders = new List<GameObject>() { null, null };

    public TeamSettings[] TeamSettings { get => _teamSettings; set => _teamSettings = value; }
    public string SaveFolderPath { get => _saveFolderPath; }
    public List<List<GameObject>> Minions { get => _minions; set => _minions = value; }
    public List<GameObject> Leader { get => _leaders; set => _leaders = value; }
    public BGContainer Container { get => _container; set => _container = value; }

    [MenuItem("Tools/Battle Generator")]
    public static void OpenWindow()
    {
        var window = GetWindow<BGMain>();
        window.Show();
    }

    private void OnEnable()
    {
        CreateDirectories();
        _container = GetContainer();
        LoadContainerData(_container, out _leaders, out _minions, out _teamSettings);

        _tsIndex = -1;

        _title = new GUIStyle
        {
            fontStyle = FontStyle.BoldAndItalic,
            alignment = TextAnchor.MiddleCenter,
            fontSize = 15,
        };
        _headerlv1 = new GUIStyle
        {
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            fontSize = 13,
        };
        _headerlv2 = new GUIStyle
        {
            fontStyle = FontStyle.Bold,
        };

        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void LoadContainerData(BGContainer container, out List<GameObject> leaders, out List<List<GameObject>> minions, out TeamSettings[] ts)
    {
        ts = new TeamSettings[2];

        if(container.Leaders == null || container.Leaders.Count < 2)
            container.Leaders = leaders = new List<GameObject>() { null, null };
        else
        {
            leaders = container.Leaders;
            for (int i = 0; i < ts.Length; i++)
            {
                ts[i].leaderEntity = PrefabUtility.GetCorrespondingObjectFromSource(leaders[i]);
                ts[i].leaderPosition = leaders[i].transform.position;
                ts[i].leaderHealth = leaders[i].GetComponent<IHealth>().GetHealth();
                ts[i].leaderSpeed = leaders[i].GetComponent<ISpeed>().GetSpeed();
                leaders[i].GetComponent<IMelee>().GetMeleeData(out ts[i].leaderMeleeDamage, out ts[i].leaderMeleeRate, out ts[i].leaderMeleeDistance);
                leaders[i].GetComponent<IShooter>().GetShootData(out ts[i].leaderShootDamage, out ts[i].leaderShootRate, out ts[i].leaderShootDistance);
                leaders[i].GetComponent<IVision>().GetVisionData(out ts[i].leaderVisionDistance, out ts[i].leaderVisionRangeAngles);
            }
        }

        if (container.Minions == null || container.Minions.Count < 2)
            container.Minions = minions = new List<List<GameObject>>() { new List<GameObject>(), new List<GameObject>() };
        else
        {
            minions = container.Minions;
            for (int i = 0; i < ts.Length; i++)
            {
                GameObject minionReference = minions[i][0];
                FlockingGroup fg = container.FlockingGroup[i];
                ts[i].minionEntity = PrefabUtility.GetCorrespondingObjectFromSource(minionReference);
                ts[i].minionSpawnAreaPosition = fg.areaPosition;
                ts[i].minionSpawnAreaWidth = fg.areaSize.x;
                ts[i].minionSpawnAreaLength = fg.areaSize.y;
                ts[i].minionsQuantityRow = fg.quantityRow;
                ts[i].minionsQuantityColumn = fg.quantityColumn;
                ts[i].minionHealth = minionReference.GetComponent<IHealth>().GetHealth();
                ts[i].minionSpeed = minionReference.GetComponent<ISpeed>().GetSpeed();
                minionReference.GetComponent<IMelee>().GetMeleeData(out ts[i].minionMeleeDamage, out ts[i].minionMeleeRate, out ts[i].minionMeleeDistance);
                minionReference.GetComponent<IShooter>().GetShootData(out ts[i].minionShootDamage, out ts[i].minionShootRate, out ts[i].minionShootDistance);
                minionReference.GetComponent<IVision>().GetVisionData(out ts[i].minionVisionDistance, out ts[i].minionVisionRangeAngles);

                ts[i].flockEntityRadius = minionReference.GetComponent<FlockEntity>().radius;
                ts[i].flockEntityMask = minionReference.GetComponent<FlockEntity>().maskEntity;
                ts[i].flockLeaderBehaviourWeight = minionReference.GetComponent<LeaderBehavior>().leaderWeight;
                ts[i].flockLeaderBehaviourMinDistance = minionReference.GetComponent<LeaderBehavior>().minDistance;
                ts[i].flockAlineationBehaviourWeight = minionReference.GetComponent<AlineationBehavior>().alineationWeight;
                ts[i].flockSeparationBehaviourWeight = minionReference.GetComponent<SeparationBehavior>().separationWeight;
                ts[i].flockSeparationBehaviourRange = minionReference.GetComponent<SeparationBehavior>().range;
                ts[i].flockCohesionBehaviourWeight = minionReference.GetComponent<CohesionBehavior>().cohesionWeight;
                ts[i].flockAvoidanceBehaviourWeight = minionReference.GetComponent<AvoidanceBehavior>().avoidanceWeight;
                ts[i].flockAvoidanceBehaviourMask = minionReference.GetComponent<AvoidanceBehavior>().mask;
                ts[i].flockAvoidanceBehaviourRange = minionReference.GetComponent<AvoidanceBehavior>().range;
            }
        }

    }

    private BGContainer GetContainer()
    {
        var container = FindObjectOfType<BGContainer>();
        if(container == null)
        {
            container = new GameObject("BC_container").AddComponent<BGContainer>();
            container.transform.position = Vector3.zero;
        }
        return container;
    }

    private void OnDisable()
    {
        CloseEditingSubwindows();

        if (_pathFindingEditorWindow != null)
            _pathFindingEditorWindow.Close();

        SceneView.duringSceneGui -= OnSceneGUI;
    }
    private void OnGUI()
    {
        EditorGUILayout.LabelField("BATTLE GENERATOR", _title);

        var normalGUIColor = GUI.color;
        var backgroundColor = GUI.backgroundColor;
        var normalBGColor = backgroundColor;
        EditorGUILayout.BeginHorizontal();
        GUI.color = new Color(0, 0.82f, 0.93f);
        EditorGUI.BeginChangeCheck();
        if (_pressedTeams[0] = GUILayout.Toggle(_pressedTeams[0], "Team 1", "Button"))
        {
            _pressedTeams[1] = false;
            backgroundColor = new Color(0, 0.82f, 0.93f);
        }
        GUI.color = new Color(0.92f, 0.69f, 0f);
        if (_pressedTeams[1] = GUILayout.Toggle(_pressedTeams[1], "Team 2", "Button"))
        {
            _pressedTeams[0] = false;
            backgroundColor = new Color(0.92f, 0.69f, 0f);
        }
        if (EditorGUI.EndChangeCheck())
        {
            CloseEditingSubwindows();
        }
        GUI.color = normalGUIColor;
        GUI.backgroundColor = backgroundColor;
        _tsIndex = -1;
        for (int i = 0; i < _pressedTeams.Length; i++)
        {
            if (_pressedTeams[i])
            {
                _tsIndex = i;
                break;
            }
        }
        EditorGUILayout.EndHorizontal();


        var rect = EditorGUILayout.GetControlRect(false, 1);
        if (_tsIndex >= 0)
        {
            EditorGUI.DrawRect(rect, Color.gray);
            EditorGUILayout.LabelField("Team Settings", _headerlv1);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("Leader", "Opciones relacionadas al Leader del equipo")))
            {
                if(_bGLeaderEditorWindow == null)
                    _bGLeaderEditorWindow = GetWindow<BGLeaderEditor>();

                _bGLeaderEditorWindow.BGMainWindow = this;
                _bGLeaderEditorWindow.Index = _tsIndex;
                _bGLeaderEditorWindow.Show();
            }
            EditorGUI.BeginDisabledGroup(_teamSettings[_tsIndex].leaderEntity == null);
            if(GUILayout.Button(new GUIContent("Flocking", "Opciones relacionadas a los minions del equipo.\nPara acceder debe haber un GameObject Leader asignado.")))
            {
                if (_bGFlockingEditorWindow == null)
                    _bGFlockingEditorWindow = GetWindow<BGFlockingEditor>();

                _bGFlockingEditorWindow.BGMainWindow = this;
                _bGFlockingEditorWindow.Index = _tsIndex;
                _bGFlockingEditorWindow.Show();
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
        }

        rect = EditorGUILayout.GetControlRect(false, 1);
        EditorGUI.DrawRect(rect, Color.gray);

        GUI.backgroundColor = normalBGColor;

        EditorGUILayout.LabelField("Pathfinding", _headerlv1);
        if (GUILayout.Button(new GUIContent("Pathfinding Settings", "Herramienta de edicion de pathfinding")))
        {
            _pathFindingEditorWindow = GetWindow<PathFindingEditor>();
            _pathFindingEditorWindow.wantsMouseMove = true;
            _pathFindingEditorWindow.Show();
        }
    }

    private void CreateDirectories()
    {
        if (!AssetDatabase.IsValidFolder(_saveFolderPath))
        {
            var splittedPath = _saveFolderPath.Split('/');
            AssetDatabase.CreateFolder(splittedPath[0], splittedPath[1]);
        }
    }

    private void CloseEditingSubwindows()
    {
        if (_bGLeaderEditorWindow != null)
            _bGLeaderEditorWindow.Close();

        if (_bGFlockingEditorWindow != null)
            _bGFlockingEditorWindow.Close();
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
    }
}
