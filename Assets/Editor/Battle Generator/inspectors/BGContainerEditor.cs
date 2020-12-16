using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BGContainer))]
public class BGContainerEditor : Editor
{
    GUIStyle _title;
    GUIStyle _headerlv1;
    GUIStyle _headerlv2;

    BGContainer _bGContainer;

    bool minionfoldedOut1;
    bool minionfoldedOut2;

    string _folderPath = "Assets/battle_generator_saves/container_saves";
    string _fileName = "cont_save.asset";

    private void OnEnable()
    {
        _bGContainer = (BGContainer)target;

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
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Battle Generator Container", _title);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Leaders", _headerlv1);
        for (int i = 0; i < _bGContainer.leaders.Count; i++)
        {
            _bGContainer.leaders[i] = (GameObject)EditorGUILayout.ObjectField("Leader " + i, _bGContainer.leaders[i], typeof(GameObject), false);
        }
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Minions", _headerlv1);
        minionfoldedOut1 = EditorGUILayout.Foldout(minionfoldedOut1, "Minion Group 1", true);
        MinionGroup currMinionGroup;
        FlockingGroup currFlockingGroup;
        if (minionfoldedOut1)
        {
            currMinionGroup = _bGContainer.minionGroups[0];
            for (int j = 0; j < currMinionGroup.minions.Count; j++)
            {
                currMinionGroup.minions[j] = (GameObject)EditorGUILayout.ObjectField("Minion " + j, currMinionGroup.minions[j], typeof(GameObject), false);
            }
            var rect = EditorGUILayout.GetControlRect(true, 1);
            EditorGUI.DrawRect(rect, Color.gray);
            currFlockingGroup = _bGContainer.flockingGroup[0];
            currFlockingGroup.areaPosition = EditorGUILayout.Vector3Field("Area Position", currFlockingGroup.areaPosition);
            EditorGUILayout.LabelField("Area Size");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("W");
            currFlockingGroup.areaSize.x = EditorGUILayout.FloatField(currFlockingGroup.areaSize.x);
            GUILayout.Label("L");
            currFlockingGroup.areaSize.y = EditorGUILayout.FloatField(currFlockingGroup.areaSize.y);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.LabelField("Quantity");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.IntField(currFlockingGroup.quantityRow);
            //EditorGUILayout.LabelField("X");
            GUILayout.Label("X");
            EditorGUILayout.IntField(currFlockingGroup.quantityColumn);
            EditorGUILayout.EndHorizontal();
        }
        minionfoldedOut2 = EditorGUILayout.Foldout(minionfoldedOut2, "Minion Group 2", true);
        if (minionfoldedOut2)
        {
            currMinionGroup = _bGContainer.minionGroups[1];
            for (int j = 0; j < currMinionGroup.minions.Count; j++)
            {
                currMinionGroup.minions[j] = (GameObject)EditorGUILayout.ObjectField("Minion " + j, currMinionGroup.minions[j], typeof(GameObject), false);
            }
            var rect = EditorGUILayout.GetControlRect(true, 1);
            EditorGUI.DrawRect(rect, Color.gray);
            currFlockingGroup = _bGContainer.flockingGroup[1];
            currFlockingGroup.areaPosition = EditorGUILayout.Vector3Field("Area Position", currFlockingGroup.areaPosition);
            EditorGUILayout.LabelField("Area Size");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("W");
            currFlockingGroup.areaSize.x = EditorGUILayout.FloatField(currFlockingGroup.areaSize.x);
            GUILayout.Label("L");
            currFlockingGroup.areaSize.y = EditorGUILayout.FloatField(currFlockingGroup.areaSize.y);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.LabelField("Quantity");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.IntField(currFlockingGroup.quantityRow);
            GUILayout.Label("X");
            EditorGUILayout.IntField(currFlockingGroup.quantityColumn);
            EditorGUILayout.EndHorizontal();
        }

        GUI.color = Color.green;
        if (GUILayout.Button("Save container"))
        {
            var scriptable = ScriptableObject.CreateInstance<BGContainerSave>();
            var leaders = scriptable.leaders;
            var minions = scriptable.minions;
            var tsso = scriptable.ts;

            for (int i = 0; i < _bGContainer.leaders.Count; i++)
            {
                var currLeader = _bGContainer.leaders[i];
                var currTs = tsso[i];

                leaders[i] = PrefabUtility.GetCorrespondingObjectFromSource(currLeader);
                currTs.leaderPosition = currLeader.transform.position;
                currTs.leaderHealth = currLeader.GetComponent<IHealth>().GetHealth();
                currTs.leaderSpeed = currLeader.GetComponent<ISpeed>().GetSpeed();
                currLeader.GetComponent<IMelee>().GetMeleeData(out currTs.leaderMeleeDamage, out currTs.leaderMeleeRate, out currTs.leaderMeleeDistance);
                currLeader.GetComponent<IShooter>().GetShootData(out currTs.leaderShootDamage, out currTs.leaderShootRate, out currTs.leaderShootDistance);
                currLeader.GetComponent<IVision>().GetVisionData(out currTs.leaderVisionDistance, out currTs.leaderVisionRangeAngles);

                tsso[i] = currTs;
            }
            for (int i = 0; i < _bGContainer.minionGroups.Count; i++)
            {
                var minionGroup = _bGContainer.minionGroups[i];
                var flockingGroup = _bGContainer.flockingGroup[i];
                var currTs = tsso[i];

                //Debug.Log(minionGroup.minions.Count);
                for (int j = 0; j < minionGroup.minions.Count; j++)
                {
                    var minionData = new MinionData
                    {
                        minion = PrefabUtility.GetCorrespondingObjectFromSource(minionGroup.minions[j]),
                        position = minionGroup.minions[j].transform.position
                    };
                    minions[i].Add(minionData);
                }

                currTs.minionSpawnAreaPosition = flockingGroup.areaPosition;
                currTs.minionSpawnAreaWidth = flockingGroup.areaSize.x;
                currTs.minionSpawnAreaLength = flockingGroup.areaSize.y;
                currTs.minionsQuantityRow = flockingGroup.quantityRow;
                currTs.minionsQuantityColumn = flockingGroup.quantityColumn;
                currTs.minionHealth = minionGroup.minions[0].GetComponent<IHealth>().GetHealth();
                currTs.minionSpeed = minionGroup.minions[0].GetComponent<ISpeed>().GetSpeed();
                minionGroup.minions[0].GetComponent<IMelee>().GetMeleeData(out currTs.minionMeleeDamage, out currTs.minionMeleeRate, out currTs.minionMeleeDistance);
                minionGroup.minions[0].GetComponent<IShooter>().GetShootData(out currTs.minionShootDamage, out currTs.minionShootRate, out currTs.minionShootDistance);
                minionGroup.minions[0].GetComponent<IVision>().GetVisionData(out currTs.minionVisionDistance, out currTs.minionVisionRangeAngles);
                currTs.flockEntityRadius = minionGroup.minions[0].GetComponent<FlockEntity>().radius;
                currTs.flockEntityMask = minionGroup.minions[0].GetComponent<FlockEntity>().maskEntity;
                currTs.flockLeaderBehaviourWeight = minionGroup.minions[0].GetComponent<LeaderBehavior>().leaderWeight;
                currTs.flockLeaderBehaviourMinDistance = minionGroup.minions[0].GetComponent<LeaderBehavior>().minDistance;
                currTs.flockAlineationBehaviourWeight = minionGroup.minions[0].GetComponent<AlineationBehavior>().alineationWeight;
                currTs.flockSeparationBehaviourWeight = minionGroup.minions[0].GetComponent<SeparationBehavior>().separationWeight;
                currTs.flockSeparationBehaviourRange = minionGroup.minions[0].GetComponent<SeparationBehavior>().range;
                currTs.flockCohesionBehaviourWeight = minionGroup.minions[0].GetComponent<CohesionBehavior>().cohesionWeight;
                currTs.flockAvoidanceBehaviourWeight = minionGroup.minions[0].GetComponent<AvoidanceBehavior>().avoidanceWeight;
                currTs.flockAvoidanceBehaviourMask = minionGroup.minions[0].GetComponent<AvoidanceBehavior>().mask;
                currTs.flockAvoidanceBehaviourRange = minionGroup.minions[0].GetComponent<AvoidanceBehavior>().range;

                tsso[i] = currTs;
            }
            scriptable.leaders = leaders;
            scriptable.minions = minions;
            scriptable.ts = tsso;

            if (!AssetDatabase.IsValidFolder(_folderPath))
            {
                var stripedPath = _folderPath.Split('/');
                string parentFolder = "";
                for (int i = 0; i < stripedPath.Length - 1; i++)
                {
                    if (i > 0)
                        parentFolder += "/";

                    parentFolder += stripedPath[i];
                }

                AssetDatabase.CreateFolder(parentFolder, stripedPath[stripedPath.Length - 1]);
            }
            var path = _folderPath + "/" + _fileName;
            path = AssetDatabase.GenerateUniqueAssetPath(path);
            AssetDatabase.CreateAsset(scriptable, path);
        }
    }
}
