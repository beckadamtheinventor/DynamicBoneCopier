#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;

public class DynamicBoneCopier : MonoBehaviour
{
    [CustomEditor(typeof(DynamicBoneCopier))]
    public class MeshCombinerEditor : Editor
    {
        public const string infostring = "Dynamic Bone Tree Copier";
        public const string copyright = "MIT License (C)2024 BeckATI";
        public override void OnInspectorGUI()
        {
            DynamicBoneCopier combiner = (DynamicBoneCopier)target;
            GUILayout.Label(infostring);
            GUILayout.Label(copyright);
            if (GUILayout.Button("Build"))
            {
                combiner.Build();
            }
            base.OnInspectorGUI();
        }
    }

	public Transform m_source;
	public Transform m_target;

    public void Build()
    {
        if (m_target == null || m_source == null)
        {
            Debug.LogError("Both source and target gameobjects must be set.");
            return;
        }
        CopyTree(m_source, m_target);
    }

    public Transform FindChildWithName(Transform tree, String name, bool recursive=false)
    {
        for (int i = 0; i < tree.childCount; i++)
        {
            Transform item = tree.GetChild(i);
            if (item.childCount > 0 && recursive)
            {
                Transform rv = FindChildWithName(item, name, recursive);
                if (rv != null)
                {
                    return rv;
                }
            }
            if (item.name == name)
            {
                return item;
            }
        }
        return null;
    }

    public void CopyTree(Transform source, Transform target)
    {
        for (int i = 0; i < source.childCount; i++)
        {
            Transform item = source.GetChild(i);
            Transform targetItem = FindChildWithName(target, item.name);
            if (targetItem == null)
            {
                continue;
            }
            if (item.childCount > 0)
            {
                CopyTree(item, targetItem);
            }
            DynamicBone db = item.GetComponent<DynamicBone>();
            if (db != null)
            {
                DynamicBone targetDb = targetItem.gameObject.GetComponent<DynamicBone>();
                if (targetDb == null)
                {
                    targetDb = targetItem.gameObject.AddComponent<DynamicBone>();
                }
                targetDb.m_Colliders = db.m_Colliders;
                targetDb.m_Damping = db.m_Damping;
                targetDb.m_DampingDistrib= db.m_DampingDistrib;
                targetDb.m_DistanceToObject = db.m_DistanceToObject;
                targetDb.m_DistantDisable= db.m_DistantDisable;
                targetDb.m_Elasticity = db.m_Elasticity;
                targetDb.m_ElasticityDistrib = db.m_ElasticityDistrib;
                targetDb.m_EndLength = db.m_EndLength;
                targetDb.m_EndOffset = db.m_EndOffset;
                targetDb.m_Exclusions = db.m_Exclusions;
                targetDb.m_Force = db.m_Force;
                targetDb.m_FreezeAxis = db.m_FreezeAxis;
                targetDb.m_Gravity = db.m_Gravity;
                targetDb.m_Inert = db.m_Inert;
                targetDb.m_InertDistrib = db.m_InertDistrib;
                targetDb.m_Radius = db.m_Radius;
                targetDb.m_RadiusDistrib = db.m_RadiusDistrib;
                targetDb.m_ReferenceObject = db.m_ReferenceObject;
                targetDb.m_Stiffness = db.m_Stiffness;
                targetDb.m_StiffnessDistrib = db.m_StiffnessDistrib;
                targetDb.m_UpdateMode = db.m_UpdateMode;
                targetDb.m_UpdateRate = db.m_UpdateRate;

                if (db.m_Root == null)
                {
                    continue;
                }
                Transform rootObj = FindChildWithName(target, db.m_Root.name);
                if (rootObj == null)
                {
                    rootObj = FindChildWithName(m_target, db.m_Root.name, true);
                }
                if (rootObj == null)
                {
                    Debug.LogError($"Failed to locate target bone with name \"{db.m_Root.name}\"");
                } else
                {
                    targetDb.m_Root = rootObj;
                }
            }
            DynamicBoneCollider dbc = item.GetComponent<DynamicBoneCollider>();
            if (dbc != null)
            {
                DynamicBoneCollider targetDbc = targetItem.gameObject.GetComponent<DynamicBoneCollider>();
                if (targetDbc == null)
                {
                    targetDbc = targetItem.gameObject.AddComponent<DynamicBoneCollider>();
                }
                targetDbc.m_Bound = dbc.m_Bound;
                targetDbc.m_Center = dbc.m_Center;
                targetDbc.m_Direction = dbc.m_Direction;
                targetDbc.m_Height = dbc.m_Height;
                targetDbc.m_Radius = dbc.m_Radius;
            }
            DynamicBoneColliderBase dbcb = item.GetComponent<DynamicBoneColliderBase>();
            if (dbcb != null)
            {
                DynamicBoneColliderBase targetDbc = targetItem.gameObject.GetComponent<DynamicBoneColliderBase>();
                if (targetDbc == null)
                {
                    targetDbc = targetItem.gameObject.AddComponent<DynamicBoneColliderBase>();
                }
                targetDbc.m_Bound = dbcb.m_Bound;
                targetDbc.m_Center = dbcb.m_Center;
                targetDbc.m_Direction= dbcb.m_Direction;
            }
            DynamicBonePlaneCollider dbpc = item.GetComponent<DynamicBonePlaneCollider>();
            if (dbpc != null)
            {
                DynamicBonePlaneCollider targetDbc = targetItem.gameObject.GetComponent<DynamicBonePlaneCollider>();
                if (targetDbc == null)
                {
                    targetDbc = targetItem.gameObject.AddComponent<DynamicBonePlaneCollider>();
                }
                targetDbc.m_Bound = dbpc.m_Bound;
                targetDbc.m_Center= dbpc.m_Center;
                targetDbc.m_Direction = dbpc.m_Direction;
            }
        }
    }
}
#endif