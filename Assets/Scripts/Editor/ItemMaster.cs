﻿using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ItemMaster : EditorWindow
{
    private GameObject ManagerPrefab;
    private ItemManager itemManager;
    public List<WizardFightItem> Items = new List<WizardFightItem>();

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/Wizard Fight/Wizard Fight Items")]
    public static void ShowWindow() {
        //Show existing window instance. If one doesn't exist, make one.
        GetWindow(typeof(ItemMaster));
    }

    public void OnEnable() {
        var data = EditorPrefs.GetString("WFItems", JsonUtility.ToJson(this, false));
        JsonUtility.FromJsonOverwrite(data, this);
        if (ManagerPrefab == null) {
            ManagerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/ItemManager.prefab");
            itemManager = ManagerPrefab.GetComponent<ItemManager>();
        }
    }

    public void OnDisable() {
        var data = JsonUtility.ToJson(this, false);
        EditorPrefs.SetString("WFItems", data);
        if (itemManager != null) {
            itemManager.items = Items.ToList();
        }
    }

    private void OnLostFocus() {
        UpdatePrefab();
    }

    void OnGUI() {
        if (GUILayout.Button("Assign Item IDs")) {
            AssignIDs();
        }
        SerializedObject obj = new SerializedObject(this);
        SerializedProperty prop = obj.FindProperty("Items");
        GUILayout.Label("Items", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(prop, true);
        bool noManager = ManagerPrefab == null;
        obj.ApplyModifiedProperties();
        UpdatePrefab();
    }

    public void AssignIDs() {
        UpdatePrefab();
    }

    private void UpdatePrefab() {
        itemManager.items = Items.ToList();
        for (int i = 0; i < itemManager.items.Count; i++) {
            GameObject world = PrefabUtility.GetCorrespondingObjectFromOriginalSource(itemManager.items[i].WorldModel);
            GameObject held = PrefabUtility.GetCorrespondingObjectFromOriginalSource(itemManager.items[i].HeldModel);
            world.GetComponent<Item>().Id = i;
            held.GetComponent<HeldItem>().Id = i;
            EditorUtility.SetDirty(world);
            EditorUtility.SetDirty(held);
        }
        EditorUtility.SetDirty(ManagerPrefab);
    }
}