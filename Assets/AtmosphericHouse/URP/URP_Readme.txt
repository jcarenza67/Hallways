URP Instructions:
1. Import asset to an existing URP project.
2. Import AtmosphericHouseURP.unitypackage file from Assets -> Import Package -> Custom Package.
3. Replace all files.
4. Shaders and scene should update to URP.

URP issues:
When baking lightning, decals produce weird shadows to walls.
How to fix?
Change Decals_corner_clean, Decals_corner_worn materials to URP/Lit transparent shader whan baking lightning and change back to FinwardStudios/Decal after bake is done.


Recommended UniversalRP-HighQuality.asset settings:

Lightning:
Main Light Shadow Resolution: 4096
Additional Lights Per Object Limit: 8
Additional Lights Shadow Resolution: 4096

Shadows:
Distance: 80
Cascades: Two Cascades
Normal Bias: 0.5
Post Processing: Grading Mode: High Dynamic Range