using System;
using System.Collections.Generic;
using UnityEngine;

namespace UHFPS.Scriptable
{
	[Serializable]
	public sealed class MapPlanLayer
	{
		[Serializable]
		public sealed class Room
		{
			public Sprite RoomSprite;
			public Bounds RoomBounds;
			public int RoomPlanID;
			public int BuildingPlanID;
			public RoomTitle RoomTitle;
			public List<Door> DoorsList;
		}

		[Serializable]
		public sealed class Door
        {
			public Vector3 Position;
			public Vector3 DoorSize;
			public int SecondRoomID;
        }

		[Serializable]
		public sealed class RoomTitle
        {
			public string Title;
			public Vector2 TitleOffset;
			public float TitleSize;
        }

		public List<Room> RoomList;
	}

	public class MapProfileAsset : ScriptableObject
	{
		public Sprite MapTexture;
		public Vector2 MapTextureSize;
		public Color MapBackground = Color.black;
		public Bounds MapBounds = new Bounds(Vector3.zero, Vector3.one);
		public MapPlanLayer[] MapLayers;
	}
}