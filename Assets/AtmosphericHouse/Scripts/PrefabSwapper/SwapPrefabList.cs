using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrefabSwapper
{

    //*****  (©) Finward Studios 2021. All rights reserved. *****\\

    public class SwapPrefabList : ScriptableObject
    {
        // Add folders to look the prefabs from: { "Assets/Folder_A", "Assets/Folder_B" },
        public static string[] prefabsFolders = { "Assets/AtmosphericHouse/Prefabs/Building", "Assets/AtmosphericHouse/Prefabs/Doors", "Assets/AtmosphericHouse/Prefabs/Props", "Assets/AtmosphericHouse/Prefabs/Lights", "Assets/AtmosphericHouse/Prefabs/Decals" };

        [System.Serializable]
        public class SwapPrefabProperties
        {

            public string prefabName;
            public string category;
            public string[] variations;
            public string[] swapToPrefab;
            public string[] swapToPrefabButtonName;
            public string[] addAdditional;
            public string[] addAdditionalButtonName;
            public Vector3[] addAdditionalPos;
            public Vector3[] addAdditionalRot;
            public string[] spawnAlong;
            public Vector3[] spawnAlongPos;
            public Vector3[] spawnAlongRot;

            public SwapPrefabProperties(
                string _PrefabName,
                string _Category,
                string[] _Variations,
                string[] _AwapToPrefab,
                string[] _SwapToPrefabButtonName,
                string[] _AddAdditional,
                string[] _AddAdditionalButtonName,
                Vector3[] _AddAdditionalPos,
                Vector3[] _AddAdditionalRot,
                string[] _spawnAlong,
                Vector3[] _spawnAlongPos,
                Vector3[] _spawnAlongRot
                )

            {
                prefabName = _PrefabName;
                category = _Category;
                variations = _Variations;
                swapToPrefab = _AwapToPrefab;
                swapToPrefabButtonName = _SwapToPrefabButtonName;
                addAdditional = _AddAdditional;
                addAdditionalButtonName = _AddAdditionalButtonName;
                addAdditionalPos = _AddAdditionalPos;
                addAdditionalRot = _AddAdditionalRot;
                spawnAlong = _spawnAlong;
                spawnAlongPos = _spawnAlongPos;
                spawnAlongRot = _spawnAlongRot;
            }
        }

        public static SwapPrefabProperties[] swapPrefabProperties = new SwapPrefabProperties[]
        {
            
            new SwapPrefabProperties(
                //prefabName
                "WallOut_1x",
                //category
                "WallOut",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Foundation_1x" },
                //addAdditionalButtonName
                new string[] { "Foundation" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, -0.5f, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { "WallIn_A_1x" },
                //  spawnAlongPos
                new Vector3[] { new Vector3(-1, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 180, 0) }
                )
            ,
            new SwapPrefabProperties(
                //prefabName
                "WallOut_2x",
                //category
                "WallOut",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Foundation_2x" },
                //addAdditionalButtonName
                new string[] { "Foundation" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, -0.5f, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { "WallIn_A_2x" },
                //  spawnAlongPos
                new Vector3[] { new Vector3(-2, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 180, 0) }
                )
                ,
            new SwapPrefabProperties(
                //  prefabName
                "WallOut_3x",
                //  category
                "WallOut",
                //  variations
                new string[] { },
                //  swapToPrefab
                new string[] { },
                //  swapToPrefabButtonName
                new string[] { },
                //  addAdditional
                new string[] { "Foundation_3x" },
                //  addAdditionalButtonName
                new string[] { "Foundation" },
                //a  ddAdditionalPos
                new Vector3[] { new Vector3(0, -0.5f, 0) },
                //  addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { "WallIn_A_3x" },
                //  spawnAlongPos
                new Vector3[] { new Vector3(-3, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 180, 0) }
                )
            ,
            new SwapPrefabProperties(
                //prefabName
                "WallOut_3x_door",
                //category
                "WallOut",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Foundation_3x" },
                //addAdditionalButtonName
                new string[] { "Foundation" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, -0.5f, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { "WallIn_A_3x_door" },
                //  spawnAlongPos
                new Vector3[] { new Vector3(-3, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 180, 0) }
                )
            ,
            new SwapPrefabProperties(
                //prefabName
                "WallOut_3x_door_windows",
                //category
                "WallOut",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Foundation_3x" },
                //addAdditionalButtonName
                new string[] { "Foundation" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, -0.5f, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { "WallIn_A_3x_doordouble" },
                //  spawnAlongPos
                new Vector3[] { new Vector3(-3, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 180, 0) }
                )
            ,
            new SwapPrefabProperties(
                //prefabName
                "WallOut_3x_doordouble",
                //category
                "WallOut",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Foundation_3x" },
                //addAdditionalButtonName
                new string[] { "Foundation" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, -0.5f, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { "WallIn_A_3x_doordouble" },
                //  spawnAlongPos
                new Vector3[] { new Vector3(-3, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 180, 0) }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallOut_3x_window_static",
                //category
                "WallOut",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Foundation_3x", "Windowshutters"  },
                //addAdditionalButtonName
                new string[] { "Foundation", "Windowshutters" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, -0.5f, 0), new Vector3(-1.5f, 1.5f, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { "WallIn_A_3x_window" },
                //  spawnAlongPos
                new Vector3[] { new Vector3(-3, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 180, 0) }
                )
               ,
            new SwapPrefabProperties(
                //prefabName
                "WallOut_3x_window_vinyl",
                //category
                "WallOut",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Foundation_3x", "Windowshutters"  },
                //addAdditionalButtonName
                new string[] { "Foundation", "Windowshutters" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, -0.5f, 0), new Vector3(-1.5f, 1.5f, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { "WallIn_A_3x_window" },
                //  spawnAlongPos
                new Vector3[] { new Vector3(-3, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 180, 0) }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallOut_3x_windowdouble_static",
                //category
                "WallOut",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Foundation_3x", "Windowshutters_windowdouble"  },
                //addAdditionalButtonName
                new string[] { "Foundation", "Windowshutters" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, -0.5f, 0), new Vector3(-1.5f, 1.5f, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { "WallIn_A_3x_windowdouble" },
                //  spawnAlongPos
                new Vector3[] { new Vector3(-3, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 180, 0) }
                )
                            ,
            new SwapPrefabProperties(
                //prefabName
                "WallOut_3x_windowdouble_vinyl",
                //category
                "WallOut",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Foundation_3x", "Windowshutters_windowdouble"  },
                //addAdditionalButtonName
                new string[] { "Foundation", "Windowshutters" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, -0.5f, 0), new Vector3(-1.5f, 1.5f, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { "WallIn_A_3x_windowdouble" },
                //  spawnAlongPos
                new Vector3[] { new Vector3(-3, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 180, 0) }
                )
                           ,
            new SwapPrefabProperties(
                //prefabName
                "WallOut_3x_windowsmall_vinyl",
                //category
                "WallOut",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Foundation_3x" },
                //addAdditionalButtonName
                new string[] { "Foundation" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, -0.5f, 0), new Vector3(-1.5f, 1.5f, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { "WallIn_A_3x_windowsmall" },
                //  spawnAlongPos
                new Vector3[] { new Vector3(-3, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 180, 0) }
                )
               ,
            new SwapPrefabProperties(
                //prefabName
                "WallOut_6x",
                //category
                "WallOut",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Foundation_6x" },
                //addAdditionalButtonName
                new string[] { "Foundation" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, -0.5f, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { "WallIn_A_6x" },
                //  spawnAlongPos
                new Vector3[] { new Vector3(-6, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 180, 0) }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallOut_6x_windowtriple",
                //category
                "WallOut",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Foundation_6x" },
                //addAdditionalButtonName
                new string[] { "Foundation" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, -0.5f, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { "WallIn_A_6x_windowtriple" },
                //  spawnAlongPos
                new Vector3[] { new Vector3(-6, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 180, 0) }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallOut_corner",
                //category
                "WallOut",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, -0.5f, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_A_1x",
                //category
                "WallIn_A",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { "WallIn_B_1x" },
                //swapToPrefabButtonName
                new string[] { "WallIn_B" },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_A_2x",
                //category
                "WallIn_A",
                //variations
                new string[] { "WallIn_A_2x_bent_A", "WallIn_A_2x_broken_A" },
                //swapToPrefab
                new string[] { "WallIn_B_2x" },
                //swapToPrefabButtonName
                new string[] { "WallIn_B" },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_A_3x",
                //category
                "WallIn_A",
                //variations
                new string[] { "WallIn_A_3x_bent_A", "WallIn_A_3x_broken_A", "WallIn_A_3x_broken_B", "WallIn_A_3x_broken_C", "WallIn_A_3x_broken_D" },
                //swapToPrefab
                new string[] { "WallIn_B_3x" },
                //swapToPrefabButtonName
                new string[] { "WallIn_B" },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_A_3x_door",
                //category
                "WallIn_A",
                //variations
                new string[] { "WallIn_A_3x_door_bent_A", "WallIn_A_3x_door_broken_A", "WallIn_A_3x_door_broken_B", "WallIn_A_3x_door_broken_C" },
                //swapToPrefab
                new string[] { "WallIn_B_3x_door" },
                //swapToPrefabButtonName
                new string[] { "WallIn_B" },
                //addAdditional
                new string[] { "Doorframe_in_single", "Door_parent_A_L", "Floor_trim_door", "Decal_wallIn_A_3x_door" },
                //addAdditionalButtonName
                new string[] { "Doorframe", "Door", "Floor_trim", "Decal" },
                //addAdditionalPos
                new Vector3[] { new Vector3(-1.5f, 0, 0), new Vector3(-1.5f, 0, 0), new Vector3(-1.5f, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_A_3x_doordouble",
                //category
                "WallIn_A",
                //variations
                new string[] { "WallIn_A_3x_doordouble_bent_A" , "WallIn_A_3x_doordouble_broken_A" },
                //swapToPrefab
                new string[] { "WallIn_B_3x_doordouble" },
                //swapToPrefabButtonName
                new string[] { "WallIn_B" },
                //addAdditional
                new string[] { "Doorframe_in_double", "Door_parent_A_double", "Floor_trim_doordouble", "Decal_wallIn_A_3x_doordouble", "AreaLight_door_double_prefab", },
                //addAdditionalButtonName
                new string[] { "Doorframe", "Doors", "Floor_trim", "Decal", "Area Light"},
                //addAdditionalPos
                new Vector3[] { new Vector3(-1.5f, 0, 0), new Vector3(-1.5f, 0, 0), new Vector3(-1.5f, 0, 0), new Vector3(0, 0, 0), new Vector3(-1.5f, 1.5f, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_A_3x_recessed",
                //category
                "WallIn_A",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { "WallIn_B_3x_recessed" },
                //swapToPrefabButtonName
                new string[] { "WallIn_B" },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
            ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_A_3x_window",
                //category
                "WallIn_A",
                //variations
                new string[] { "WallIn_A_3x_window_bent_A", "WallIn_A_3x_window_broken_A", "WallIn_A_3x_window_broken_B", "WallIn_A_3x_window_broken_C" },
                //swapToPrefab
                new string[] { "WallIn_B_3x_window" },
                //swapToPrefabButtonName
                new string[] { "WallIn_B" },
                //addAdditional
                new string[] { "Windowblinds_open_A", "Curtains_short_singlewindow", "Curtains_long_singlewindow", "AreaLight_window_single_prefab" },
                //addAdditionalButtonName
                new string[] { "Windowblinds", "Curtains Short", "Curtains Long", "Area Light" },
                //addAdditionalPos
                new Vector3[] { new Vector3(-1.5f, 2, 0), new Vector3(-1.5f, 2.33f, 0.1f), new Vector3(-1.5f, 2.33f, 0.1f), new Vector3(-1.5f, 1.5f, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_A_3x_windowdouble",
                //category
                "WallIn_A",
                //variations
                new string[] { "WallIn_A_3x_windowdouble_bent_A", "WallIn_A_3x_windowdouble_broken_A" },
                //swapToPrefab
                new string[] { "WallIn_B_3x_windowdouble" },
                //swapToPrefabButtonName
                new string[] { "WallIn_B" },
                //addAdditional
                new string[] { "Windowblinds_open_A", "Windowblinds_open_A", "Curtains_short_doublewindow", "Curtains_long_doublewindow", "AreaLight_window_double_prefab" },
                //addAdditionalButtonName
                new string[] { "Windowblinds Left", "Windowblinds Right", "Curtains Short", "Curtains Long", "Area Light" },
                //addAdditionalPos
                new Vector3[] { new Vector3(-1, 2, 0), new Vector3(-2, 2, 0), new Vector3(-1.5f, 2.33f, 0.1f), new Vector3(-1.5f, 2.33f, 0.1f), new Vector3(-1.5f, 1.5f, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                            ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_A_3x_windowsmall",
                //category
                "WallIn_A",
                //variations
                new string[] { "WallIn_A_3x_windowsmall_broken_A" },
                //swapToPrefab
                new string[] { "WallIn_B_3x_windowsmall" },
                //swapToPrefabButtonName
                new string[] { "WallIn_B" },
                //addAdditional
                new string[] { "AreaLight_window_singlesmall_prefab" },
                //addAdditionalButtonName
                new string[] { "Area Light" },
                //addAdditionalPos
                new Vector3[] { new Vector3(-1.5f, 1.5f, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_A_6x",
                //category
                "WallIn_A",
                //variations
                new string[] { "WallIn_A_6x_bent_A", "WallIn_A_6x_broken_A", "WallIn_A_6x_broken_B" },
                //swapToPrefab
                new string[] { "WallIn_B_6x" },
                //swapToPrefabButtonName
                new string[] { "WallIn_B" },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_A_6x_windowtriple",
                //category
                "WallIn_A",
                //variations
                new string[] { "WallIn_A_6x_windowtriple_bent_A", "WallIn_A_6x_windowtriple_broken_A" },
                //swapToPrefab
                new string[] { "WallIn_B_6x_windowtriple" },
                //swapToPrefabButtonName
                new string[] { "WallIn_B" },
                //addAdditional
                new string[] { "Windowblinds_windowtriple", "Curtains_short_triplewindow", "Curtains_long_triplewindow", "AreaLight_window_triple_prefab" },
                //addAdditionalButtonName
                new string[] { "Windowblinds", "Curtains Short", "Curtains Long", "Area Lights" },
                //addAdditionalPos
                new Vector3[] { new Vector3(-3, 2, 0), new Vector3(-3, 2.33f, 0.1f), new Vector3(-3, 2.33f, 0.1f), new Vector3(-3, 1.5f, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_A_column_small",
                //category
                "WallIn_A",
                //variations
                new string[] { "WallIn_A_column_small_broken_A", "WallIn_A_column_small_broken_B" },
                //swapToPrefab
                new string[] { "WallIn_B_column_small" },
                //swapToPrefabButtonName
                new string[] { "WallIn_B" },
                //addAdditional
                new string[] { "Decal_wallIn_A_column_small" },
                //addAdditionalButtonName
                new string[] { "Decal" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                 ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_A_column_medium",
                //category
                "WallIn_A",
                //variations
                new string[] { "WallIn_A_column_medium_broken_A", "WallIn_A_column_medium_broken_B" },
                //swapToPrefab
                new string[] { "WallIn_B_column_medium" },
                //swapToPrefabButtonName
                new string[] { "WallIn_B" },
                //addAdditional
                new string[] { "Decal_wallIn_A_column_medium"},
                //addAdditionalButtonName
                new string[] { "Decal" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_A_columnhalf_small",
                //category
                "WallIn_A",
                //variations
                new string[] { "WallIn_A_columnhalf_small_broken_A", "WallIn_A_columnhalf_small_broken_B" },
                //swapToPrefab
                new string[] { "WallIn_B_columnhalf_small" },
                //swapToPrefabButtonName
                new string[] { "WallIn_B" },
                //addAdditional
                new string[] { "Decal_wallIn_A_columnhalf_small" },
                //addAdditionalButtonName
                new string[] { "Decal" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
              ,
               
            new SwapPrefabProperties(
                //prefabName
                "WallIn_A_columnhalf_medium",
                //category
                "WallIn_A",
                //variations
                new string[] { "WallIn_A_columnhalf_medium_broken_A", "WallIn_A_columnhalf_medium_broken_B" },
                //swapToPrefab
                new string[] { "WallIn_B_columnhalf_medium" },
                //swapToPrefabButtonName
                new string[] { "WallIn_B" },
                //addAdditional
                new string[] { "Decal_wallIn_A_columnhalf_medium" },
                //addAdditionalButtonName
                new string[] { "Decal" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_A_columnhalf_large",
                //category
                "WallIn_A",
                //variations
                new string[] { "WallIn_A_columnhalf_large_broken_A", "WallIn_A_columnhalf_large_broken_B" },
                //swapToPrefab
                new string[] { "WallIn_B_columnhalf_large" },
                //swapToPrefabButtonName
                new string[] { "WallIn_B" },
                //addAdditional
                new string[] { "Decal_wallIn_A_columnhalf_large" },
                //addAdditionalButtonName
                new string[] { "Decal" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_A_corner_outSmall",
                //category
                "WallIn_A",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { "WallIn_B_corner_outSmall" },
                //swapToPrefabButtonName
                new string[] { "WallIn_B" },
                //addAdditional
                new string[] { "Decal_wallIn_A_corner_outSmall" },
                //addAdditionalButtonName
                new string[] { "Decal" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_A_corner_outLarge",
                //category
                "WallIn_A",
                //variations
                new string[] { "WallIn_A_corner_outLarge_broken_A", "WallIn_A_corner_outLarge_broken_B"},
                //swapToPrefab
                new string[] { "WallIn_B_corner_outLarge" },
                //swapToPrefabButtonName
                new string[] { "WallIn_B" },
                //addAdditional
                new string[] { "Decal_wallIn_A_corner_outLarge" },
                //addAdditionalButtonName
                new string[] { "Decal" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_A_corner_outPatch",
                //category
                "WallIn_A",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { "WallIn_B_corner_outPatch" },
                //swapToPrefabButtonName
                new string[] { "WallIn_B" },
                //addAdditional
                new string[] { "Decal_wallIn_A_corner_outPatch" },
                //addAdditionalButtonName
                new string[] { "Decal" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )

                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_B_1x",
                //category
                "WallIn_B",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { "WallIn_A_1x" },
                //swapToPrefabButtonName
                new string[] { "WallIn_A" },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_B_2x",
                //category
                "WallIn_B",
                //variations
                new string[] { "WallIn_B_2x_bent_A", "WallIn_B_2x_broken_A" },
                //swapToPrefab
                new string[] { "WallIn_A_2x" },
                //swapToPrefabButtonName
                new string[] { "WallIn_A" },
                //addAdditional
                new string[] {  },
                //addAdditionalButtonName
                new string[] {  },
                //addAdditionalPos
                new Vector3[] {  },
                //addAdditionalRot
                new Vector3[] {  },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_B_3x",
                //category
                "WallIn_B",
                //variations
                new string[] { "WallIn_B_3x_bent_A", "WallIn_B_3x_broken_A", "WallIn_B_3x_broken_B", "WallIn_B_3x_broken_C", "WallIn_B_3x_broken_D" },
                //swapToPrefab
                new string[] { "WallIn_A_3x" },
                //swapToPrefabButtonName
                new string[] { "WallIn_A" },
                //addAdditional
                new string[] {  },
                //addAdditionalButtonName
                new string[] {  },
                //addAdditionalPos
                new Vector3[] {  },
                //addAdditionalRot
                new Vector3[] {  },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_B_3x_door",
                //category
                "WallIn_B",
                //variations
                new string[] { "WallIn_B_3x_door_bent_A", "WallIn_B_3x_door_broken_A", "WallIn_B_3x_door_broken_B", "WallIn_B_3x_door_broken_C" },
                //swapToPrefab
                new string[] { "WallIn_A_3x_door" },
                //swapToPrefabButtonName
                new string[] { "WallIn_A" },
                //addAdditional
                new string[] { "Doorframe_in_single", "Door_parent_A_L", "Floor_trim_door", "Decal_wallIn_B_3x_door" },
                //addAdditionalButtonName
                new string[] { "Doorframe", "Door", "Floor_trim", "Decal" },
                //addAdditionalPos
                new Vector3[] { new Vector3(-1.5f, 0, 0), new Vector3(-1.5f, 0, 0), new Vector3(-1.5f, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_B_3x_doordouble",
                //category
                "WallIn_B",
                //variations
                new string[] { "WallIn_B_3x_doordouble_bent_A", "WallIn_B_3x_doordouble_broken_A" },
                //swapToPrefab
                new string[] { "WallIn_A_3x_doordouble" },
                //swapToPrefabButtonName
                new string[] { "WallIn_A" },
                //addAdditional
                new string[] { "Doorframe_in_double", "Door_parent_A_double", "Floor_trim_doordouble", "Decal_wallIn_B_3x_doordouble", "AreaLight_door_double_prefab", },
                //addAdditionalButtonName
                new string[] { "Doorframe", "Doors", "Floor_trim", "Decal", "Area Light"},
                //addAdditionalPos
                new Vector3[] { new Vector3(-1.5f, 0, 0), new Vector3(-1.5f, 0, 0), new Vector3(-1.5f, 0, 0), new Vector3(0, 0, 0), new Vector3(-1.5f, 1.5f, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_B_3x_recessed",
                //category
                "WallIn_B",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { "WallIn_A_3x_recessed" },
                //swapToPrefabButtonName
                new string[] { "WallIn_A" },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_B_3x_window",
                //category
                "WallIn_B",
                //variations
                new string[] { "WallIn_B_3x_window_bent_A", "WallIn_B_3x_window_broken_A", "WallIn_B_3x_window_broken_B", "WallIn_B_3x_window_broken_C"},
                //swapToPrefab
                new string[] { "WallIn_A_3x_window" },
                //swapToPrefabButtonName
                new string[] { "WallIn_A" },
                //addAdditional
                new string[] { "Windowblinds_open_A", "Curtains_short_singlewindow", "Curtains_long_singlewindow", "AreaLight_window_single_prefab" },
                //addAdditionalButtonName
                new string[] { "Windowblinds", "Curtains Short", "Curtains Long", "Area Light" },
                //addAdditionalPos
                new Vector3[] { new Vector3(-1.5f, 2, 0), new Vector3(-1.5f, 2.33f, 0.1f), new Vector3(-1.5f, 2.33f, 0.1f), new Vector3(-1.5f, 1.5f, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_B_3x_windowdouble",
                //category
                "WallIn_B",
                //variations
                new string[] { "WallIn_B_3x_windowdouble_bent_A","WallIn_B_3x_windowdouble_broken_A" },
                //swapToPrefab
                new string[] { "WallIn_A_3x_windowdouble" },
                //swapToPrefabButtonName
                new string[] { "WallIn_A" },
                //addAdditional
                new string[] { "Windowblinds_open_A", "Windowblinds_open_A", "Curtains_short_doublewindow", "Curtains_long_doublewindow", "AreaLight_window_double_prefab" },
                //addAdditionalButtonName
                new string[] { "Windowblinds Left", "Windowblinds Right", "Curtains Short", "Curtains Long", "Area Light" },
                //addAdditionalPos
                new Vector3[] { new Vector3(-1, 2, 0), new Vector3(-2, 2, 0), new Vector3(-1.5f, 2.33f, 0.1f), new Vector3(-1.5f, 2.33f, 0.1f), new Vector3(-1.5f, 1.5f, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                            ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_B_3x_windowsmall",
                //category
                "WallIn_B",
                //variations
                new string[] { "WallIn_B_3x_windowsmall_broken_A" },
                //swapToPrefab
                new string[] { "WallIn_A_3x_windowsmall" },
                //swapToPrefabButtonName
                new string[] { "WallIn_A" },
                //addAdditional
                new string[] { "AreaLight_window_singlesmall_prefab" },
                //addAdditionalButtonName
                new string[] { "Area Light" },
                //addAdditionalPos
                new Vector3[] { new Vector3(-1.5f, 1.5f, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_B_6x",
                //category
                "WallIn_B",
                //variations
                new string[] { "WallIn_B_6x_bent_A", "WallIn_B_6x_broken_A", "WallIn_B_6x_broken_B" },
                //swapToPrefab
                new string[] { "WallIn_A_6x" },
                //swapToPrefabButtonName
                new string[] { "WallIn_A" },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                            ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_B_6x_windowtriple",
                //category
                "WallIn_B",
                //variations
                new string[] { "WallIn_B_6x_windowtriple_bent_A", "WallIn_B_6x_windowtriple_broken_A" },
                //swapToPrefab
                new string[] { "WallIn_A_6x_windowtriple" },
                //swapToPrefabButtonName
                new string[] { "WallIn_A" },
                //addAdditional
                new string[] { "Windowblinds_windowtriple", "Curtains_short_triplewindow", "Curtains_long_triplewindow", "AreaLight_window_triple_prefab" },
                //addAdditionalButtonName
                new string[] { "Windowblinds", "Curtains Short", "Curtains Long", "Area Lights" },
                //addAdditionalPos
                new Vector3[] { new Vector3(-3, 2, 0), new Vector3(-3, 2.33f, 0.1f), new Vector3(-3, 2.33f, 0.1f), new Vector3(-3, 1.5f, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_B_column_small",
                //category
                "WallIn_B",
                //variations
                new string[] { "WallIn_B_column_small_broken_A", "WallIn_B_column_small_broken_B" },
                //swapToPrefab
                new string[] { "WallIn_A_column_small" },
                //swapToPrefabButtonName
                new string[] { "WallIn_A" },
                //addAdditional
                new string[] { "Decal_wallIn_B_column_small" },
                //addAdditionalButtonName
                new string[] { "Decal" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_B_column_medium",
                //category
                "WallIn_B",
                //variations
                new string[] { "WallIn_B_column_medium_broken_A", "WallIn_B_column_medium_broken_B" },
                //swapToPrefab
                new string[] { "WallIn_A_column_medium" },
                //swapToPrefabButtonName
                new string[] { "WallIn_A" },
                //addAdditional
                new string[] { "Decal_wallIn_B_column_medium" },
                //addAdditionalButtonName
                new string[] { "Decal" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_B_columnhalf_small",
                //category
                "WallIn_B",
                //variations
                new string[] { "WallIn_B_columnhalf_small_broken_A", "WallIn_B_columnhalf_small_broken_B" },
                //swapToPrefab
                new string[] { "WallIn_A_columnhalf_small" },
                //swapToPrefabButtonName
                new string[] { "WallIn_A" },
                //addAdditional
                new string[] { "Decal_wallIn_B_columnhalf_small"  },
                //addAdditionalButtonName
                new string[] { "Decal" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                            ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_B_columnhalf_medium",
                //category
                "WallIn_B",
                //variations
                new string[] { "WallIn_B_columnhalf_medium_broken_A", "WallIn_B_columnhalf_medium_broken_B" },
                //swapToPrefab
                new string[] { "WallIn_A_columnhalf_medium" },
                //swapToPrefabButtonName
                new string[] { "WallIn_A" },
                //addAdditional
                new string[] { "Decal_wallIn_B_columnhalf_medium"},
                //addAdditionalButtonName
                new string[] { "Decal" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                            ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_B_columnhalf_large",
                //category
                "WallIn_B",
                //variations
                new string[] { "WallIn_B_columnhalf_large_broken_A", "WallIn_B_columnhalf_large_broken_B"},
                //swapToPrefab
                new string[] { "WallIn_A_columnhalf_large" },
                //swapToPrefabButtonName
                new string[] { "WallIn_A" },
                //addAdditional
                new string[] { "Decal_wallIn_B_columnhalf_large" },
                //addAdditionalButtonName
                new string[] { "Decal" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_B_corner_outSmall",
                //category
                "WallIn_B",
                //variations
                new string[] {  },
                //swapToPrefab
                new string[] { "WallIn_A_corner_outSmall" },
                //swapToPrefabButtonName
                new string[] { "WallIn_A" },
                //addAdditional
                new string[] { "Decal_wallIn_B_corner_outSmall" },
                //addAdditionalButtonName
                new string[] { "Decal" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
            ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_B_corner_outLarge",
                //category
                "WallIn_B",
                //variations
                new string[] { "WallIn_B_corner_outLarge_broken_A", "WallIn_B_corner_outLarge_broken_B" },
                //swapToPrefab
                new string[] { "WallIn_A_corner_outLarge" },
                //swapToPrefabButtonName
                new string[] { "WallIn_A" },
                //addAdditional
                new string[] { "Decal_wallIn_B_corner_outLarge" },
                //addAdditionalButtonName
                new string[] { "Decal" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_B_corner_outPatch",
                //category
                "WallIn_B",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { "WallIn_A_corner_outPatch" },
                //swapToPrefabButtonName
                new string[] { "WallIn_A" },
                //addAdditional
                new string[] { "Decal_wallIn_B_corner_outPatch"},
                //addAdditionalButtonName
                new string[] { "Decal" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_C_1x",
                //category
                "WallIn_C",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                            ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_C_2x",
                //category
                "WallIn_C",
                //variations
                new string[] { "WallIn_C_2x_bent_A" },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                                        ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_C_3x",
                //category
                "WallIn_C",
                //variations
                new string[] { "WallIn_C_3x_bent_A" },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_C_3x_door",
                //category
                "WallIn_C",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Doorframe_in_single", "Door_parent_A_L", "Floor_trim_door" },
                //addAdditionalButtonName
                new string[] { "Doorframe", "Door", "Floor_trim" },
                //addAdditionalPos
                new Vector3[] { new Vector3(-1.5f, 0, 0), new Vector3(-1.5f, 0, 0), new Vector3(-1.5f, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                            ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_C_3x_doordouble",
                //category
                "WallIn_C",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Doorframe_in_double", "Door_parent_A_double", "Floor_trim_doordouble" },
                //addAdditionalButtonName
                new string[] { "Doorframe", "Doors", "Floor_trim" },
                //addAdditionalPos
                new Vector3[] { new Vector3(-1.5f, 0, 0), new Vector3(-1.5f, 0, 0), new Vector3(-1.5f, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(-1.5f, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_C_6x",
                //category
                "WallIn_C",
                //variations
                new string[] { "WallIn_C_6x_bent_A" },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_C_column_small",
                //category
                "WallIn_C",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Decal_wallIn_C_column_small" },
                //addAdditionalButtonName
                new string[] { "Decal" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_C_column_medium",
                //category
                "WallIn_C",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Decal_wallIn_C_column_medium" },
                //addAdditionalButtonName
                new string[] { "Decal" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_C_columnhalf_small",
                //category
                "WallIn_C",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Decal_wallIn_C_columnhalf_small" },
                //addAdditionalButtonName
                new string[] { "Decal" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_C_columnhalf_medium",
                //category
                "WallIn_C",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Decal_wallIn_C_columnhalf_medium" },
                //addAdditionalButtonName
                new string[] { "Decal" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_C_columnhalf_large",
                //category
                "WallIn_C",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Decal_wallIn_C_columnhalf_large" },
                //addAdditionalButtonName
                new string[] { "Decal" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_C_corner_outSmall",
                //category
                "WallIn_C",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Decal_wallIn_C_corner_outSmall" },
                //addAdditionalButtonName
                new string[] { "Decal" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_C_corner_outLarge",
                //category
                "WallIn_C",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Decal_wallIn_C_corner_outLarge" },
                //addAdditionalButtonName
                new string[] { "Decal" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                            ,
            new SwapPrefabProperties(
                //prefabName
                "WallIn_C_corner_outPatch",
                //category
                "WallIn_C",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )

                ,
            new SwapPrefabProperties(
                //prefabName
                "Floor_1x3",
                //category
                "Floor",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Ceiling_1x3" },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 3, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Floor_2x3",
                //category
                "Floor",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Ceiling_2x3" },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 3, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Floor_3x3",
                //category
                "Floor",
                //variations
                new string[] { "Floor_3x3_broken_A", "Floor_3x3_broken_B" },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Ceiling_3x3" },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 3, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Floor_4x3",
                //category
                "Floor",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Ceiling_4x3" },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 3, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                            ,
            new SwapPrefabProperties(
                //prefabName
                "Floor_6x3",
                //category
                "Floor",
                //variations
                new string[] { "Floor_6x3_broken_A" },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Ceiling_6x3" },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 3, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Floor_6x3_stairs_L",
                //category
                "Floor",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Ceiling_6x3" },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(6, 3, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Floor_6x3_stairs_R",
                //category
                "Floor",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Ceiling_6x3" },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 3, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                                        ,
            new SwapPrefabProperties(
                //prefabName
                "Floor_6x6",
                //category
                "Floor",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Ceiling_6x6" },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 3, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Ceiling_1x3",
                //category
                "Ceiling",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { },
                //addAdditionalRot
                new Vector3[] { },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Ceiling_2x3",
                //category
                "Ceiling",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { },
                //addAdditionalRot
                new Vector3[] { },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Ceiling_3x3",
                //category
                "Ceiling",
                //variations
                new string[] { "Ceiling_3x3_broken_A", "Ceiling_3x3_broken_B", "Ceiling_3x3_broken_C" },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { },
                //addAdditionalRot
                new Vector3[] { },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Ceiling_4x3",
                //category
                "Ceiling",
                //variations
                new string[] { "Ceiling_4x3_broken_A" },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { },
                //addAdditionalRot
                new Vector3[] { },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                            ,
            new SwapPrefabProperties(
                //prefabName
                "Ceiling_6x3",
                //category
                "Ceiling",
                //variations
                new string[] { "Ceiling_6x3_broken_A"},
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { },
                //addAdditionalRot
                new Vector3[] { },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Ceiling_6x3_stairs_L",
                //category
                "Ceiling",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { },
                //addAdditionalRot
                new Vector3[] { },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Ceiling_6x3_stairs_R",
                //category
                "Ceiling",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { },
                //addAdditionalRot
                new Vector3[] { },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                                        ,
            new SwapPrefabProperties(
                //prefabName
                "Ceiling_6x6",
                //category
                "Ceiling",
                //variations
                new string[] { "Ceiling_6x6_broken_A", "Ceiling_6x6_broken_B"},
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { },
                //addAdditionalRot
                new Vector3[] { },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Ceiling_3x_trim",
                //category
                "Ceiling",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { },
                //addAdditionalRot
                new Vector3[] { },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Roof_A_6x",
                //category
                "Roof",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { "Roof_A_9x", "Roof_A_12x", "Roof_A_6x_gable" },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Roof_A_6x_corner",
                //category
                "Roof",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { "Roof_A_9x_corner", "Roof_A_12x_corner" },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Roof_A_6x_corner_T",
                //category
                "Roof",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { "Roof_A_9x_corner_T", "Roof_A_12x_corner_T" },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Roof_A_6x_gable",
                //category
                "Roof",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { "Roof_A_9x_gable", "Roof_A_12x_gable", "Roof_A_6x" },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Gutter_pipe_1story", "Gutter_pipe_2story", "Gutter_pipe_abovePorch", "Gutter_pipe_1story", "Gutter_pipe_2story", "Gutter_pipe_abovePorch" },
                //addAdditionalButtonName
                new string[] { "Gutter_pipe_1story_R", "Gutter_pipe_2story_R", "Gutter_pipe_abovePorch_R", "Gutter_pipe_1story_L", "Gutter_pipe_2story_L", "Gutter_pipe_abovePorch_L", },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(-0.5f, 0, -6), new Vector3(-0.5f, 0, -6), new Vector3(-0.5f, 0, -6) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 180, 0), new Vector3(0, 180, 0), new Vector3(0, 180, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Roof_A_9x",
                //category
                "Roof",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { "Roof_A_6x", "Roof_A_12x", "Roof_A_9x_gable" },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Roof_A_9x_corner",
                //category
                "Roof",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { "Roof_A_6x_corner", "Roof_A_12x_corner" },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Roof_A_9x_corner_T",
                //category
                "Roof",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { "Roof_A_6x_corner_T", "Roof_A_12x_corner_T" },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Roof_A_9x_gable",
                //category
                "Roof",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { "Roof_A_6x_gable", "Roof_A_12x_gable", "Roof_A_9x"},
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Gutter_pipe_1story", "Gutter_pipe_2story", "Gutter_pipe_abovePorch", "Gutter_pipe_1story", "Gutter_pipe_2story", "Gutter_pipe_abovePorch" },
                //addAdditionalButtonName
                new string[] { "Gutter_pipe_1story_R", "Gutter_pipe_2story_R", "Gutter_pipe_abovePorch_R", "Gutter_pipe_1story_L", "Gutter_pipe_2story_L", "Gutter_pipe_abovePorch_L", },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(-0.5f, 0, -9), new Vector3(-0.5f, 0, -9), new Vector3(-0.5f, 0, -9) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 180, 0), new Vector3(0, 180, 0), new Vector3(0, 180, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Roof_A_12x",
                //category
                "Roof",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { "Roof_A_6x", "Roof_A_9x", "Roof_A_12x_gable" },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Roof_A_12x_corner",
                //category
                "Roof",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { "Roof_A_6x_corner", "Roof_A_9x_corner" },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Roof_A_12x_corner_T",
                //category
                "Roof",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { "Roof_A_6x_corner_T", "Roof_A_9x_corner_T" },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Roof_A_12x_gable",
                //category
                "Roof",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { "Roof_A_6x_gable", "Roof_A_9x_gable", "Roof_A_12x" },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Gutter_pipe_1story", "Gutter_pipe_2story", "Gutter_pipe_abovePorch", "Gutter_pipe_1story", "Gutter_pipe_2story", "Gutter_pipe_abovePorch" },
                //addAdditionalButtonName
                new string[] { "Gutter_pipe_1story_R", "Gutter_pipe_2story_R", "Gutter_pipe_abovePorch_R", "Gutter_pipe_1story_L", "Gutter_pipe_2story_L", "Gutter_pipe_abovePorch_L", },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(-0.5f, 0, -12), new Vector3(-0.5f, 0, -12), new Vector3(-0.5f, 0, -12) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 180, 0), new Vector3(0, 180, 0), new Vector3(0, 180, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Porch_A_3x",
                //category
                "Porch_A",
                //variations
                new string[] { "Porch_A_3x_broken_A", "Porch_A_3x_broken_B", "Porch_A_3x_broken_C", "Porch_A_3x_broken_D" },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                            ,
            new SwapPrefabProperties(
                //prefabName
                "Porch_A_3x_noColumn",
                //category
                "Porch_A",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Porch_A_3x_corner",
                //category
                "Porch_A",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] {  },
                //swapToPrefabButtonName
                new string[] {  },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Porch_A_3x_end_entrance_L",
                //category
                "Porch_A",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] {  },
                //swapToPrefabButtonName
                new string[] {  },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Porch_A_3x_end_entrance_R",
                //category
                "Porch_A",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] {  },
                //swapToPrefabButtonName
                new string[] {  },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Porch_A_3x_end_L",
                //category
                "Porch_A",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] {  },
                //swapToPrefabButtonName
                new string[] {  },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Porch_A_3x_end_R",
                //category
                "Porch_A",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] {  },
                //swapToPrefabButtonName
                new string[] {  },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Porch_A_3x_entrance",
                //category
                "Porch_A",
                //variations
                new string[] { "Porch_A_3x_entrance_broken_A" },
                //swapToPrefab
                new string[] {  },
                //swapToPrefabButtonName
                new string[] {  },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Porch_A_6x_entrance_separate",
                //category
                "Porch_A",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] {  },
                //swapToPrefabButtonName
                new string[] {  },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Foundation_1x",
                //category
                "Foundation",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Foundation_2x",
                //category
                "Foundation",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Foundation_3x",
                //category
                "Foundation",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Foundation_6x",
                //category
                "Foundation",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Stairs_basement",
                //category
                "Stairs",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Floor_6x3"  },
                //addAdditionalButtonName
                new string[] {  },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, -3, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, -90, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Stairs_main_L",
                //category
                "Stairs",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Floor_6x3_stairs_L", "Ceiling_6x3_stairs_L" },
                //addAdditionalButtonName
                new string[] { "Floor", "Ceiling" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 3, 0), new Vector3(0, 3, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Stairs_main_R",
                //category
                "Stairs",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Floor_6x3_stairs_R", "Ceiling_6x3_stairs_R" },
                //addAdditionalButtonName
                new string[] { "Floor", "Ceiling" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 3, 0), new Vector3(0, 3, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Stairs_straight_L",
                //category
                "Stairs",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Floor_6x3_stairs_L", "Ceiling_6x3_stairs_L" },
                //addAdditionalButtonName
                new string[] { "Floor", "Ceiling" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 3, 0), new Vector3(0, 3, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Stairs_straight_R",
                //category
                "Stairs",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Floor_6x3_stairs_R", "Ceiling_6x3_stairs_R" },
                //addAdditionalButtonName
                new string[] { "Floor", "Ceiling" },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 3, 0), new Vector3(0, 3, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) ,new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Door_parent_A_L",
                //category
                "Doors",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { "Door_parent_A_R", "Door_parent_B_L", "Door_parent_B_R", "Door_parent_C_L", "Door_parent_C_R" },
                //swapToPrefabButtonName
                new string[] { "DoorA Right", "DoorB Left" , "DoorB Right", "DoorC Left", "DoorC Right" },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { },
                //addAdditionalRot
                new Vector3[] { },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Door_parent_A_R",
                //category
                "Doors",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { "Door_parent_A_L", "Door_parent_B_L", "Door_parent_B_R", "Door_parent_C_L", "Door_parent_C_R" },
                //swapToPrefabButtonName
                new string[] { "DoorA Left", "DoorB Left" , "DoorB Right", "DoorC Left", "DoorC Right" },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { },
                //addAdditionalRot
                new Vector3[] { },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                            ,
            new SwapPrefabProperties(
                //prefabName
                "Door_parent_B_L",
                //category
                "Doors",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { "Door_parent_A_L", "Door_parent_A_R", "Door_parent_B_R", "Door_parent_C_L", "Door_parent_C_R" },
                //swapToPrefabButtonName
                new string[] { "DoorA Left", "DoorA Right" , "DoorB Right", "DoorC Left", "DoorC Right" },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { },
                //addAdditionalRot
                new Vector3[] { },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                                        ,
            new SwapPrefabProperties(
                //prefabName
                "Door_parent_B_R",
                //category
                "Doors",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { "Door_parent_A_L", "Door_parent_A_R", "Door_parent_B_L", "Door_parent_C_L", "Door_parent_C_R" },
                //swapToPrefabButtonName
                new string[] { "DoorA Left", "DoorA Right" , "DoorB Left", "DoorC Left", "DoorC Right" },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { },
                //addAdditionalRot
                new Vector3[] { },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Door_parent_C_L",
                //category
                "Doors",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { "Door_parent_A_L", "Door_parent_A_R", "Door_parent_B_L", "Door_parent_B_R", "Door_parent_C_R" },
                //swapToPrefabButtonName
                new string[] { "DoorA Left", "DoorA Right" , "DoorB Left", "DoorB Right", "DoorC Right" },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { },
                //addAdditionalRot
                new Vector3[] { },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Door_parent_C_R",
                //category
                "Doors",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { "Door_parent_A_L", "Door_parent_A_R", "Door_parent_B_L", "Door_parent_B_R", "Door_parent_C_L" },
                //swapToPrefabButtonName
                new string[] { "DoorA Left", "DoorA Right" , "DoorB Left", "DoorB Right", "DoorC Left" },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { },
                //addAdditionalRot
                new Vector3[] { },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Door_parent_A_double",
                //category
                "Doors",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { "Door_parent_B_double", "Door_parent_C_double", "Door_parent_sliding_double" },
                //swapToPrefabButtonName
                new string[] { "DoorB double", "DoorC double", "Door Sliding double" },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { },
                //addAdditionalRot
                new Vector3[] { },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                            ,
            new SwapPrefabProperties(
                //prefabName
                "Door_parent_B_double",
                //category
                "Doors",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { "Door_parent_A_double", "Door_parent_C_double", "Door_parent_sliding_double" },
                //swapToPrefabButtonName
                new string[] { "DoorA double", "DoorC double", "Door Sliding double" },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { },
                //addAdditionalRot
                new Vector3[] { },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                            ,
            new SwapPrefabProperties(
                //prefabName
                "Door_parent_C_double",
                //category
                "Doors",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { "Door_parent_A_double", "Door_parent_B_double", "Door_parent_sliding_double" },
                //swapToPrefabButtonName
                new string[] { "DoorA double", "DoorB double", "Door Sliding double" },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { },
                //addAdditionalRot
                new Vector3[] { },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                 ,
            new SwapPrefabProperties(
                //prefabName
                "Door_parent_sliding_double",
                //category
                "Doors",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { "Door_parent_A_double", "Door_parent_B_double", "Door_parent_C_double" },
                //swapToPrefabButtonName
                new string[] { "DoorA double", "DoorB double", "DoorC double" },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { },
                //addAdditionalRot
                new Vector3[] { },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { },
                //  spawnAlongRot
                new Vector3[] { }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Ceiling_wood_3x_single",
                //category
                "Extras",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] {  },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 0, 0) }
                )
                            ,
            new SwapPrefabProperties(
                //prefabName
                "Ceiling_wood_3x3",
                //category
                "Extras",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] {  },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 0, 0) }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Ceiling_wood_9x3",
                //category
                "Extras",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] {  },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 0, 0) }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Ceiling_wood_pilar",
                //category
                "Extras",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] {  },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 0, 0) }
                )
                            ,
            new SwapPrefabProperties(
                //prefabName
                "Fireplace",
                //category
                "Extras",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] {  },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 0, 0) }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Kitchen_cabinet_A",
                //category
                "Extras",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Kitchen_cabinet_end_L", "Kitchen_cabinet_end_R" },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(-1, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 0, 0) }
                )
                            ,
            new SwapPrefabProperties(
                //prefabName
                "Kitchen_cabinet_B",
                //category
                "Extras",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Kitchen_cabinet_end_L", "Kitchen_cabinet_end_R" },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(-1, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 0, 0) }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Kitchen_cabinet_C",
                //category
                "Extras",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Kitchen_cabinet_end_L", "Kitchen_cabinet_end_R" },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(-1, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 0, 0) }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Kitchen_cabinet_corner",
                //category
                "Extras",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { "Kitchen_cabinet_end_L", "Kitchen_cabinet_end_R" },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(1, 0, 0), new Vector3(0, 0, 1) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 90.0f, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 0, 0) }
                )
                            ,
            new SwapPrefabProperties(
                //prefabName
                "Kitchen_cabinet_shelf_L",
                //category
                "Extras",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(1, 0, 0), new Vector3(0, 0, 1) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 90.0f, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 0, 0) }
                )
                            ,
            new SwapPrefabProperties(
                //prefabName
                "Kitchen_cabinet_shelf_R",
                //category
                "Extras",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(1, 0, 0), new Vector3(0, 0, 1) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 90.0f, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 0, 0) }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Kitchen_counter_A",
                //category
                "Extras",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 0, 0) }
                )
                            ,
            new SwapPrefabProperties(
                //prefabName
                "Kitchen_counter_B",
                //category
                "Extras",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 0, 0) }
                )
                                        ,
            new SwapPrefabProperties(
                //prefabName
                "Kitchen_counter_dishwasher",
                //category
                "Extras",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 0, 0) }
                )
                                                    ,
            new SwapPrefabProperties(
                //prefabName
                "Kitchen_counter_sink",
                //category
                "Extras",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 0, 0) }
                )
                                        ,
            new SwapPrefabProperties(
                //prefabName
                "Kitchen_counter_corner",
                //category
                "Extras",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 0, 0) }
                )
                                                    ,
            new SwapPrefabProperties(
                //prefabName
                "Kitchen_counter_separate",
                //category
                "Extras",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] { },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 0, 0) }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Pipe_1x",
                //category
                "Extras",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] {  },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 0, 0) }
                )
                            ,
            new SwapPrefabProperties(
                //prefabName
                "Pipe_3x",
                //category
                "Extras",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] {  },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 0, 0) }
                )
                                        ,
            new SwapPrefabProperties(
                //prefabName
                "Pipe_3x_valve",
                //category
                "Extras",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] {  },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 0, 0) }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Pipe_corner_L",
                //category
                "Extras",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] {  },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 0, 0) }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Pipe_corner_R",
                //category
                "Extras",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] {  },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 0, 0) }
                )
                            ,
            new SwapPrefabProperties(
                //prefabName
                "Pipe_corner_T",
                //category
                "Extras",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] {  },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 0, 0) }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Pipe_end",
                //category
                "Extras",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] {  },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 0, 0) }
                )
                            ,
            new SwapPrefabProperties(
                //prefabName
                "Windowblinds_open_A",
                //category
                "Extras",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { "Windowblinds_open_B", "Windowblinds_open_C", "Windowblinds_closed" },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] {  },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 0, 0) }
                )
                                        ,
            new SwapPrefabProperties(
                //prefabName
                "Windowblinds_open_B",
                //category
                "Extras",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { "Windowblinds_open_A", "Windowblinds_open_C", "Windowblinds_closed" },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] {  },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 0, 0) }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Windowblinds_open_C",
                //category
                "Extras",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { "Windowblinds_open_A", "Windowblinds_open_B", "Windowblinds_closed" },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] {  },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 0, 0) }
                )
                ,
            new SwapPrefabProperties(
                //prefabName
                "Windowblinds_closed",
                //category
                "Extras",
                //variations
                new string[] { },
                //swapToPrefab
                new string[] { "Windowblinds_open_A", "Windowblinds_open_B", "Windowblinds_open_C" },
                //swapToPrefabButtonName
                new string[] { },
                //addAdditional
                new string[] {  },
                //addAdditionalButtonName
                new string[] { },
                //addAdditionalPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //addAdditionalRot
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlong
                new string[] { },
                //  spawnAlongPos
                new Vector3[] { new Vector3(0, 0, 0) },
                //  spawnAlongRot
                new Vector3[] { new Vector3(0, 0, 0) }
                )
        };
    }
}

//*****  (©) Finward Studios 2021. All rights reserved. *****\\
//*****     SwapPrefab written by: Jan Procházka  *****\\