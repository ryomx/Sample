using System.Collections;
using System.Collections.Generic;

public class MapItemInfo {

	private List<int[]> PILLAR_POSITION_STD = new List<int[]>(){
		new int[]{2, 2},
		new int[]{2, 7},
		new int[]{2, 12},
		new int[]{4, 4},
		new int[]{7, 2},
		new int[]{7, 12},
		new int[]{10, 4},
		new int[]{10, 10},
		new int[]{12, 2},
		new int[]{12, 7},
		new int[]{12, 12}
	};

	private List<int[]> DAMAGE_AREA_POSITION_STD = new List<int[]>(){
		new int[]{2, 5},
		new int[]{2, 10},
		new int[]{4, 2},
		new int[]{5, 12},
		new int[]{5, 7},
		new int[]{7, 5},
		new int[]{7, 9},
		new int[]{9, 2},
		new int[]{9, 7},
		new int[]{10, 12},
		new int[]{12, 4},
		new int[]{12, 9}
	};

	
	private List<int[]> OBJECT_POSITION_STD = new List<int[]>(){
		new int[]{2, 4},
		new int[]{3, 8},
		new int[]{4, 6},
		new int[]{4, 12},
		new int[]{5, 11},
		new int[]{6, 3},
		new int[]{6, 9},
		new int[]{8, 4},
		new int[]{9, 10},
		new int[]{11, 2},
		new int[]{11, 6},
		new int[]{11, 8},
		new int[]{11, 11}
	};

	// 柱生成場所情報
	public List<int[]> pillarPosList{ get; set;}
	
	// ダメージエリア生成場所情報
	public List<int[]> damageAreaPosList{ get; set;}

	// オブジェクト生成場所情報
	public List<int[]> objectPosList{ get; set;}

	//private MapItemInfo (){}

	public MapItemInfo(MapType mapType){

		switch(mapType){

		case MapType.STANDARD:

			pillarPosList = PILLAR_POSITION_STD;
			damageAreaPosList = DAMAGE_AREA_POSITION_STD;
			objectPosList = OBJECT_POSITION_STD;
			break;
		}
	}
}

public enum MapType
{
	STANDARD
}

