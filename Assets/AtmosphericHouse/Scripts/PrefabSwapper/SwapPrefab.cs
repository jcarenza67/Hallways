using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.ExceptionServices;
using TMPro;

namespace PrefabSwapper
{

    //*****  (©) Finward Studios 2021. All rights reserved. *****\\

    [AddComponentMenu("PrefabSwapper/SwapPrefab")]
    [ExecuteInEditMode]
    public class SwapPrefab : MonoBehaviour
    {
        public static List<string> categoryList;
        public static List<string> prefabList;
        public static List<string> variationList;

        public bool placed = false;
        public string category;
        public string prefab;
        public string variation;
        public string[] quickSwaps;
        public string[] quickSwapsBtns;
        public string[] additionals;
        public string[] additionalsBtns;
        public Vector3[] additionalsPos;
        public Vector3[] additionalsRot;
        public string[] spawnAlongs;
        public Vector3[] spawnAlongsPos;
        public Vector3[] spawnAlongsRot;

        string exCategory;
        string exPrefab;
        string exVariation;

        public void CreateCategoryList()
        {
            categoryList = new List<string>();

            foreach (SwapPrefabList.SwapPrefabProperties _prefab in SwapPrefabList.swapPrefabProperties)
            {
                if (!categoryList.Contains(_prefab.category))
                    categoryList.Add(_prefab.category);
            }
        }

        public void CreatePrefabList()
        {
            prefabList = new List<string>();

            foreach (SwapPrefabList.SwapPrefabProperties _prefab in SwapPrefabList.swapPrefabProperties)
            {
                if (_prefab.category == category)
                    if (!prefabList.Contains(_prefab.prefabName))
                        prefabList.Add(_prefab.prefabName);
            }
        }

        public void CreateVariationList()
        {
            variationList = new List<string>();
            variationList.Add(prefab);

            foreach (SwapPrefabList.SwapPrefabProperties _prefab in SwapPrefabList.swapPrefabProperties)
            {
                if (_prefab.category == category)
                    if (_prefab.prefabName == prefab)
                        foreach (string variation in _prefab.variations)
                            if (!variationList.Contains(variation))
                                variationList.Add(variation);
            }
        }

        public void FindPrefabProperties()
        {
            //Debug.Log("looking for prefab properties");
            foreach (SwapPrefabList.SwapPrefabProperties _prefab in SwapPrefabList.swapPrefabProperties)
            {
                if (_prefab.prefabName == transform.name)
                {
                    variation = _prefab.prefabName;
                    GetPrefabProperties(_prefab);
                    return;
                }
            }

            foreach (SwapPrefabList.SwapPrefabProperties _prefab in SwapPrefabList.swapPrefabProperties)
                foreach (string _variation in _prefab.variations)
                    if ( _variation == transform.name)
                    {
                        variation = _variation;
                        GetPrefabProperties(_prefab);
                        return;
                    }
            //CleanPrefabProperties();
        }

        public void CleanPrefabProperties()
        {
            category = "";
            prefab = "";

            quickSwaps = new string[0];
            quickSwapsBtns = new string[0];

            additionals = new string[0];
            additionalsBtns = new string[0];
            additionalsPos = new Vector3[0];
            additionalsRot = new Vector3[0];

            spawnAlongs = new string[0];
            spawnAlongsPos = new Vector3[0];
            spawnAlongsRot = new Vector3[0];

            exCategory = "";
            exPrefab = "";
            exVariation = "";
        }

        public void GetPrefabProperties(SwapPrefabList.SwapPrefabProperties _prefab)
        {
            category = _prefab.category;
            prefab = _prefab.prefabName;

            quickSwaps = _prefab.swapToPrefab;
            quickSwapsBtns = _prefab.swapToPrefabButtonName;

            additionals = _prefab.addAdditional;
            additionalsBtns = _prefab.addAdditionalButtonName;
            additionalsPos = _prefab.addAdditionalPos;
            additionalsRot = _prefab.addAdditionalRot;

            spawnAlongs = _prefab.spawnAlong;
            spawnAlongsPos = _prefab.spawnAlongPos;
            spawnAlongsRot = _prefab.spawnAlongRot;

            exCategory = category;
            exPrefab = prefab;
            exVariation = variation;
        }

        public void Initialize()
        {
            FindPrefabProperties();

            CreateCategoryList();
            CreatePrefabList();
            CreateVariationList();
        }


#if UNITY_EDITOR
        public void Awake()
        {
            Initialize();
        }

        private void OnValidate()
        {

            Event e = Event.current;

            if (e != null)
            {
                if (e.type == EventType.ExecuteCommand && e.commandName == "Duplicate")
                {
                    placed = true;
                }
            }

            CreateCategoryList();
            CreatePrefabList();
            CreateVariationList();
        }
#endif
    }
}

//*****  (©) Finward Studios 2021. All rights reserved. *****\\
//*****     SwapPrefab written by: Jan Procházka  *****\\