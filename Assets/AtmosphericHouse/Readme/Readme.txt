//*****  (©) Finward Studios 2021. All rights reserved. *****\\

Built-In Render Pipeline Instrcutions:
Recommended Color Space: Linear (Project Settings -> Player -> Other Settings -> Color Space)
Recommended Rendering Path: Deferred
Import Post Processing from Package Manager. Assign Post Processing profiles from BuiltIn_RP -> BuiltIn_Profiles folder. 

HDRP Instructions:
1. Import asset to an existing HDRP project.
2. Import AtmosphericHouseHDRP.unitypackage file from Assets -> Import Package -> Custom Package.
3. Replace all files.
4. Shaders and scene should update to HDRP.
5. Search for HDRenderPipelineAsset (In settings folder by default).
Add Diffusion_cotton_thin and Diffusion_foliage from folder AtmosphericHouse -> HDRP_profiles to Diffusion Profile List in HDRenderPipelineAsset.

HDRP issues:
Some materials look strange, such as plants and curtains?
This is due to missing Diffusion profiles from your HDRP asset file. Package has two Diffusion profiles that needs to be added to your HDRP asset.
Please see step 5. from HDRP Instructions.

URP Instructions:
1. Import asset to an existing URP project.
2. Import AtmosphericHouseURP.unitypackage file from Assets -> Import Package -> Custom Package.
3. Replace all files.
4. Shaders and scene should update to URP.

URP issues:
When baking lightning, decals produce weird shadows to walls.
How to fix?
Change Decals_corner_clean, Decals_corner_worn materials to URP/Lit transparent shader whan baking lightning and change back to FinwardStudios/Decal after bake is done.


Material Swapper script:
Available from Tools -> Finward Studios -> Material Swapper.

How to use?
Place all prefabs in hierarchy under one game object if you want to change all materials at once.
Drag that gameobject from scene hierarchy to GameObject slot.
Make sure Include Children is on.
Hit Swap Materials to swap all clean materials to worn materials.
Reverse Current/Assigned Strings changes selection so that worn materials will be swapped to clean and vice versa.

Mode Toggle: Toggle between simple and normal mode.
How to use?
You can change for example all "Wall_B_plaster_clean" materials to "Wall_B_Wallpaper_A_clean" by entering
Wall_B_plaster_clean to Current Material string and Wall_B_Wallpaper_A_clean to Assigned Material string.


Material Blend shader:
MultiMask texture channels:
R - Height
G - Grunge
B - Triplanar
A - Triplanar 2
VertexPaint uses vertex alpha channel.


Prefab Swapper script:
Script is attached to each modular prefab and is found under Transform component. Visible in inspector by selecting modular prefabs from the scene.
Prefab list can be modified from SwapPrefabList.cs in case you want to add your own custom prefabs.

How to use?
Select prefab from scene or hierarchy. From inspector find SwapPrefab script which is attached to the prefab.
Select Category you want.
Select Prefab you want the prefab to be swapped to and it will be swapped immediately.
Variation shows available variations for currently selected prefab. These variations are for example broken floor planks or walls that are broken.
Note that not all prefabs have variations.


Modular prefabs will snap to grid.
Use ProGrid or Ctrl + L to open AutoSnap script.


Light Optimize script:
Will disable light component based on distance or height compared to MainCamera tag.
Distance From Object if you want to specify another object where the distance is calculated from.
Floor Height will disable light component if MainCamera is +-2 units (meters) from Y axis compared to the light.


Doors:
If you wish to have doors that can be opened in both directions, take the following steps:
1. Open each door prefab separately from Prefabs -> Doors folder.
2. Hide or delete Door_mesh.
3. Activate Door_mesh_multidirectional.
3. Close the prefab.
Note: There are no scripts or animations included for multidirectional doors, but the meshes have their pivots and size adjusted for multidirectional use.




For questions and feedback please email to: support@finwardstudios.com

Thanks to Jan Procházka for writing Prefab Swapper script. Jan's email again.jbc@gmail.com

