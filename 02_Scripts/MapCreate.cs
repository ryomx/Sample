using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapCreate : MonoBehaviour {

	// プレイヤーオブジェクト格納用
	private	GameObject		player;

	// ブロックのサイズ
	private	Vector3			BL_SIZE		= new Vector3(4,4,4);

	// 作成したブロックを入れるフォルダー
	private GameObject block_folder;

	// 作成したアイテムを入れるフォルダー
	private GameObject		item_folder;

	// 作成した敵を入れるフォルダー
	private GameObject		enemy_folder;

	// アイテム格納用のプレファブ配列
	public	GameObject[]	prefab_ITEM;
	//	// マップに配置した各種アイテム(壁、障害物など)の格納用
	//	private	GameObject[,]	map_ITEM;
//	// 壁のサイズ
//	private	Vector3			WALL_SIZE	= new Vector3(4,8,4);
//	
//	// 柱のサイズ
//	private	Vector3			PILLAR_SIZE	= new Vector3(2,8,2);
//	
//	// ダメージエリアのサイズ
//	private	Vector3			DAMAGE_AREA_SIZE	= new Vector3(4, 0.02f, 4);
//	
//	// オブジェクトのサイズ
//	private	Vector3			OBJECT_SIZE	= new Vector3(1.5f, 1.5f, 1.5f);
	
	// アイテム格納配列のインデックス（壁）
	private const int ITEM_INDEX_WALL = 0;
	
	// アイテム格納配列のインデックス（柱）
	private const int ITEM_INDEX_PILLAR = 1;
	
	// アイテム格納配列のインデックス（ダメージエリア）
	private const int ITEM_INDEX_DAMAGE_AREA = 2;
	
	// アイテム格納配列のインデックス（オブジェクト）
	private const int ITEM_INDEX_OBJECT = 3;
	
	// 床ブロック格納用のプレファブ配列
	public	GameObject[]	prefab_BL;
	// マップに配置したブロックの格納用
	private	GameObject[,]	map_BL;
	//	// マップ横幅のブロック数
	//	private int				map_size_x	= 10;
	//	// マップ奥幅のブロック数
	//	private int				map_size_z	= 10;
	// マップ横幅のブロック数
	private const int		MAP_SIZE_X	= 25;
	// マップ奥幅のブロック数
	private const int		MAP_SIZE_Z	= 25;
	//	// 実際のマップ横幅のブロック数
	//	private int				MAP_SIZE_X;
	//	// 実際のマップ奥幅のブロック数
	//	private int				MAP_SIZE_Z;

//	// マップ横幅の半分
//	private int				MAP_HARFSIZE_X;
//	// マップ奥幅の半分
//	private int				MAP_HARFSIZE_Z;

	// マップアイテム情報
	MapItemInfo mapItemInfo;

	float timer = 0.0f;

//	// 柱生成場所情報
//	private List<int[]> pillarPosList;
//
//	// ダメージエリア生成場所情報
//	private List<int[]> damageAreaPosList;

	// Use this for initialization
	void Start () {

		// プレイヤーオブジェクト格納
		player = GameObject.FindGameObjectWithTag("Player");

		// 作成したブロックを入れるフォルダー(空オブジェクト)を作成
		block_folder = new GameObject();
		// 名前を変更
		block_folder.name = "BL_Folder";

		// 作成したアイテムを入れるフォルダー(空オブジェクト)を作成
		item_folder = new GameObject();
		// 名前を変更
		item_folder.name = "ITEM_Folder";

		// 作成した敵を入れるフォルダー(空オブジェクト)を作成
		enemy_folder = new GameObject();
		// 名前を変更
		enemy_folder.name = "ENEMY_Folder";

		mapItemInfo = new MapItemInfo (MapType.STANDARD);

//		// マップサイズ設定
//		/mapSizeCrrection();
		// 初期ブロック配置
		initBlockArrangement();
		// 初期アイテム配置
//		initItemArrangement();

		// 柱生成
		createThings (mapItemInfo.pillarPosList, ITEM_INDEX_PILLAR, "PILLAR", item_folder);
		// オブジェクト生成
		createThings (mapItemInfo.objectPosList, ITEM_INDEX_OBJECT, "ENEMY", enemy_folder);

		// プレイヤーの初期位置設定
		initPlayerArrangement();

	}

	void Update(){
		// プレイヤー座標取得／差分取得／ブロック更新
		//renewalBL(changedCordinatePosXZ());
		//renewalBL(changedCordinatePosZ());

		// 敵ゲームオブジェクトがなくなった場合、再配置する
		if (enemy_folder.transform.childCount == 0) {
			this.reCreateEnemy ();
		}
	}


//	/**
//	 * マップサイズ設定(偶数なら、マップサイズを+1して奇数にする)
//	 */
//	private void mapSizeCrrection(){
//		MAP_SIZE_X = (map_size_x % 2 == 1) ? map_size_x : map_size_x+1;
//		MAP_SIZE_Z = (map_size_z % 2 == 1) ? map_size_z : map_size_z+1;
//
//		MAP_HARFSIZE_X = Mathf.FloorToInt(MAP_SIZE_X/2);
//		MAP_HARFSIZE_Z = Mathf.FloorToInt(MAP_SIZE_Z/2);
//	}

	/**
	 * 初期ブロック配置
	 */
	private void initBlockArrangement(){

		// MAP_SIZE分の配列を作成
		map_BL = new GameObject[MAP_SIZE_X , MAP_SIZE_Z];
		
		// X列のブロックを順の配置するための変数
		int n = 0;
		// Z列のブロックを順の配置するための変数
		int m = 0;
		
		for(int z = 0 ; z < MAP_SIZE_Z ; z++){
			
			for(int x = 0 ; x < MAP_SIZE_X ; x++){
				// ブロック位置の算出
				Vector3 block_pos = new Vector3( BL_SIZE.x * x , -BL_SIZE.y / 2 , BL_SIZE.z * z);
				// プレハブ作成
				GameObject block = Instantiate(prefab_BL[n] , block_pos , Quaternion.identity) as GameObject;
				
				// 作成したブロックの名前変更
				block.name = "BL[" + x + "," + z + "]";
				// 作成したブロックの親、フォルダーにする
				block.transform.parent = block_folder.transform;
				
				// 作成したブロックを、マップ配列に格納
				map_BL[x,z] = block;
				
				// 次に作るブロックの番号
				n = (n+1) % prefab_BL.Length;

				// ★☆★ 壁の作成 ★☆★
				//if(z == 0 || z == map_size_z - 1 || x == 0 || x == map_size_x - 1){
				if(z == 0 || z == MAP_SIZE_Z - 1  || x == 0 || x == MAP_SIZE_X - 1){
					// ブロック位置の算出
					Vector3 wall_pos = new Vector3( BL_SIZE.x * x , prefab_ITEM[ITEM_INDEX_WALL].transform.localScale.y / 2 , BL_SIZE.z * z);
					// プレハブ作成
					GameObject wall = Instantiate(prefab_ITEM[ITEM_INDEX_WALL] , wall_pos , Quaternion.identity) as GameObject;

					// 作成したアイテムの名前変更
					wall.name = "ITEM[" + x % MAP_SIZE_X + "," + z + "]_WALL";
					// 作成したアイテムの親、フォルダーにする
					wall.transform.parent = item_folder.transform;
					
//					map_ITEM[x % map_size_x , z] = wall;
				}
//
//				// 柱のインスタンス作成判定
//				if(isCreatePosition(x, z, mapItemInfo.pillarPosList)){
//
//					this.createThings(x, z, ITEM_INDEX_PILLAR, "PILLAR", item_folder);
//				}
//				//this.createPillar(x, z);
//
////				// ダメージエリアの作成
////				this.createDamageArea(x, z);
//
//				// オブジェクトのインスタンス作成判定
//				if(isCreatePosition(x, z, mapItemInfo.objectPosList)){
//					
//					this.createThings(x, z, ITEM_INDEX_OBJECT, "OBJECT", enemy_folder);
//				}
////				// オブジェクトの作成
////				this.createObject(x, z);
			}
			// Z列の最初に来るブロックの番号
			m = (m+1) % prefab_BL.Length;
			n = m;
		}

//		// MAP_SIZE分の配列を作成
//		map_BL = new GameObject[MAP_SIZE_X , MAP_SIZE_Z];
//
//		// X列のブロックを順の配置するための変数
//		int n = 0;
//		// Z列のブロックを順の配置するための変数
//		int m = 0;
//		
//		for(int z=0 ; z< MAP_SIZE_Z ; z++){
//
//			for(int x=0 ; x< MAP_SIZE_X ; x++){
//				// ブロック位置の算出
//				Vector3 block_pos = new Vector3( BL_SIZE.x * x , -BL_SIZE.y / 2 , BL_SIZE.z * z);
//				// プレハブ作成
//				GameObject block = Instantiate(prefab_BL[n] , block_pos , Quaternion.identity) as GameObject;
//
//				// 作成したブロックの名前変更
//				block.name = "BL[" + x + "," + z + "]";
//				// 作成したブロックの親、フォルダーにする
//				block.transform.parent = block_folder.transform;
//
//				// 作成したブロックを、マップ配列に格納
//				map_BL[x,z] = block;
//
//				// 次に作るブロックの番号
//				n = (n+1) % prefab_BL.Length;
//			}
//			// Z列の最初に来るブロックの番号
//			m = (m+1) % prefab_BL.Length;
//			n = m;
//		}
	}

	/**
	 * プレイヤーの初期位置設定
	 */
	private void initPlayerArrangement(){

//		// X位置 = マップサイズの半分
//		pos_now.x = Mathf.FloorToInt(MAP_SIZE_X/2);
//		// Z位置 = マップサイズの半分
//		pos_now.z = Mathf.FloorToInt(MAP_SIZE_Z/2);
//
//		// プレイヤーの位置変更
//		player.transform.position = new Vector3(pos_now.x * BL_SIZE.x , player.transform.position.y , pos_now.z * BL_SIZE.z);

		// X位置 = マップサイズの半分
		int playerPos_x = Mathf.FloorToInt(MAP_SIZE_X/2);
		// Z位置 = マップサイズの半分
		int playerPos_z = Mathf.FloorToInt(MAP_SIZE_Z/2);
		
		// プレイヤーの位置変更
		//player.transform.position = new Vector3(playerPos_x * BL_SIZE.x , player.transform.position.y , playerPos_z * BL_SIZE.z);
	}
//
//	/**
//	 * 柱生成処理
//	 */
//	private void createPillar(int pos_x, int pos_z){
//
//		// 柱格納配列をループ
//		foreach(int[] pos in mapItemInfo.pillarPosList){
//
//			// 柱生成場所と位置が同じ場合、柱を生成する
//			if(pos_x == pos[0] && pos_z == pos[1]){
//
//				// ブロック位置の算出
//				Vector3 pillar_pos = new Vector3( BL_SIZE.x * pos_x , prefab_ITEM[ITEM_INDEX_PILLAR].transform.localScale.y / 2 , BL_SIZE.z * pos_z);
//				// プレハブ作成
//				GameObject pillar = Instantiate(prefab_ITEM[ITEM_INDEX_PILLAR] , pillar_pos , Quaternion.identity) as GameObject;
//				
//				// 作成したアイテムの名前変更
//				pillar.name = "ITEM[" + pos_x + "," + pos_z + "]_PILLAR";
//				// 作成したアイテムの親、フォルダーにする
//				pillar.transform.parent = item_folder.transform;
//				
//			}
//		}
//	}
//	
//	/**
//	 * ダメージエリア生成処理
//	 */
//	private void createDamageArea(int pos_x, int pos_z){
//		
//		// ダメージエリア格納配列をループ
//		foreach(int[] pos in mapItemInfo.damageAreaPosList){	
//
//			// ダメージエリア生成場所と位置が同じ場合、ダメージエリアを生成する
//			if(pos_x == pos[0] && pos_z == pos[1]){
//				
//				// ダメージエリア位置の算出
//				Vector3 damageArea_pos = new Vector3( BL_SIZE.x * pos_x , prefab_ITEM[ITEM_INDEX_DAMAGE_AREA].transform.localScale.y / 2 , BL_SIZE.z * pos_z);
//				// プレハブ作成
//				GameObject damageArea = Instantiate(prefab_ITEM[ITEM_INDEX_DAMAGE_AREA] , damageArea_pos , Quaternion.identity) as GameObject;
//				
//				// 作成したアイテムの名前変更
//				damageArea.name = "ITEM[" + pos_x + "," + pos_z + "]_DAMAGEAREA";
//				// 作成したアイテムの親、フォルダーにする
//				damageArea.transform.parent = item_folder.transform;
//			}
//		}
//	}
//
//	/**
//	 * オブジェクト生成処理
//	 */
//	private void createObject(int pos_x, int pos_z){
//		
//		// ダメージエリア格納配列をループ
//		foreach(int[] pos in mapItemInfo.objectPosList){
//			
//			// ダメージエリア生成場所と位置が同じ場合、オブジェクトを生成する
//			if(pos_x == pos[0] && pos_z == pos[1]){
//				
//				// オブジェクト位置の算出
//				Vector3 object_pos = new Vector3( BL_SIZE.x * pos_x , prefab_ITEM[ITEM_INDEX_OBJECT].transform.localScale.y / 2 , BL_SIZE.z * pos_z);
//				// プレハブ作成
//				GameObject objItem = Instantiate(prefab_ITEM[ITEM_INDEX_OBJECT] , object_pos , Quaternion.identity) as GameObject;
//				
//				// 作成したアイテムの名前変更
//				objItem.name = "ITEM[" + pos_x + "," + pos_z + "]_OBJECT";
//				// 作成したアイテムの親、フォルダーにする
//				objItem.transform.parent = enemy_folder.transform;
//			}
//		}
//	}
//
//	/**
//	 * プレハブからインスタンス生成する場所かどうかのチェック
//	 */
//	private bool isCreatePosition(int pos_x, int pos_z, List<int[]> positionList){
//
//		bool rtnVal = false;
//
//		// インスタンス生成場所配列をループ
//		foreach (int[] pos in positionList) {
//			
//			// インスタンス生成場所と位置が同じ場合、戻り値にtrueを設定
//			if (pos_x == pos [0] && pos_z == pos [1]) {
//				rtnVal = true;
//				break;
//			}
//		}
//		return rtnVal;
//	}
//

	/**
	 * プレハブからインスタンスを生成
	 */
	private void createThings(List<int[]> positionList, int createIndex, string objName, GameObject parentFolder){

		// インスタンス生成場所配列をループ
		foreach (int[] pos in positionList) {
			// インスタンス生成位置の算出
			Vector3 createPos = new Vector3( BL_SIZE.x * pos[0] , prefab_ITEM[createIndex].transform.localScale.y / 2 , BL_SIZE.z * pos[1]);
			// インスタンス生成
			GameObject createObj = Instantiate(prefab_ITEM[createIndex] , createPos , Quaternion.identity) as GameObject;
			
			// 作成したアイテムの名前変更
			createObj.name = objName + "[" + pos[0] + "," + pos[1] + "]";
			// 作成したアイテムの親、フォルダーを設定
			createObj.transform.parent = parentFolder.transform;
		}
	}

	/**
	 * 敵を再配置する処理
	 */
	private void reCreateEnemy(){
		
		timer += Time.deltaTime;
		
		if (timer > 0.3f){
			// オブジェクト生成
			createThings (mapItemInfo.objectPosList, ITEM_INDEX_OBJECT, "ENEMY", enemy_folder);
			timer = 0.0f;
		}
	}

//	/**
//	 * プレイヤー座標を取得し、前回座標との差分を返す
//	 */
//	private Pos_xz changedCordinatePosXZ(){
//		// 差分用
//		Pos_xz ret_pos;
//
//		// 前回座標を取得
//		pos_before = pos_now;
//		// 現在座標Xを取得
//		pos_now.x = Mathf.FloorToInt((player.transform.position.x + BL_SIZE.x/2) / BL_SIZE.x);
//		// 現在座標Zを取得
//		pos_now.z = Mathf.FloorToInt((player.transform.position.z + BL_SIZE.z/2) / BL_SIZE.z);
//
//		// Xの座標差分を取得
//		ret_pos.x = pos_now.x - pos_before.x;
//		// Zの座標差分を取得
//		ret_pos.z = pos_now.z - pos_before.z;
//
//		// 差分を返す
//		return ret_pos;
//	}
	
//	/**
//	 * 初期アイテム配置
//	 */
//	private void initItemArrangement(){
//		map_ITEM = new GameObject[MAP_SIZE_X , MAP_SIZE_Z];	// MAP_SIZE分の配列を作成.
//		
//		// 左右端に壁を作成
//		for(int z=0 ; z< MAP_SIZE_Z ; z++){
//			for(int x=0 ; x< MAP_SIZE_X ; x++){
//				// 一番端以外の列は処理せず、次のループへ
//				if(x !=0 && x != MAP_SIZE_X-1){ continue; }
//
//				// アイテムの位置算出
//				Vector3 wall_pos = new Vector3( WALL_SIZE.x * x , WALL_SIZE.y / 2 , WALL_SIZE.z * z);
//				// プレハブ作成
//				GameObject wall = Instantiate(prefab_ITEM[0] , wall_pos , Quaternion.identity) as GameObject;
//				// 作成したアイテムの名前変更
//				wall.name = "ITEM[" + x + "," + 0 + "]_WALL";
//				// 作成したアイテムの親、フォルダーにする
//				wall.transform.parent = item_folder.transform;
//
//				// 作成したアイテムを、マップ配列に格納
//				map_ITEM[x,z] = wall;
//			}
//		}
//	}

//	/**
//	 * プレイヤー座標を取得し、前回座標との差分を返す(Zのみ更新)
//	 */
//	private Pos_xz changedCordinatePosZ(){
//
//		// 差分用
//		Pos_xz ret_pos;
//
//		// 前回座標を取得
//		pos_before = pos_now;
//		// 現在座標は常に中央
//		pos_now.x = MAP_HARFSIZE_X;
//		// 現在座標Zを取得
//		pos_now.z = Mathf.FloorToInt((player.transform.position.z + BL_SIZE.z/2) / BL_SIZE.z);
//
//		// Xの座標差分は常に0
//		ret_pos.x = 0;
//		// Zの座標差分を取得
//		ret_pos.z = pos_now.z - pos_before.z;
//
//		// Zの座標差分がマイナスなら
//		if(ret_pos.z < 0){
//			// 現在座標Zは前回と同じまま
//			pos_now.z = pos_before.z;
//			// Zの座標差分なし
//			ret_pos.z = 0;
//		}
//
//		// 差分を返す
//		return ret_pos;
//	}

//	/**
//	 * 座標差分を取得し、プレイヤーの現在座標から床ブロックを削除／作成
//	 */
//	private void renewalBL(Pos_xz change_pos){

		/*
		// 列方向のブロック削除／作成
		if(change_pos.x != 0){

			int newBL_posX = pos_now.x;
			// 新規ブロック列の位置 = (現在座標 ± マップ幅÷2)
			newBL_posX += (change_pos.x > 0) ? MAP_HARFSIZE_X : -MAP_HARFSIZE_X;
			// 操作列 = (新規ブロック位置 % マップ幅)
			int x = newBL_posX % MAP_SIZE_X;
			// 操作列が負の値の場合は、マップ幅を足して正値にする
			if(x < 0){ x += MAP_SIZE_X; }

			// 新規ブロック行の初期位置 = (現在座標 - マップ幅÷2)
			int newBL_posZ = pos_now.z - MAP_HARFSIZE_Z;
			// 配列の初期座標(行)の取得
			int map_z = newBL_posZ % MAP_SIZE_Z;
			// 負の場合は正値にする
			if(map_z < 0){ map_z += MAP_SIZE_Z; }

			// 最初に作るブロックの番号算出
			int n = (pos_now.x + pos_now.z) % prefab_BL.Length;
			if(n <0){ n += prefab_BL.Length;}

			// (操作列の)全行に対し操作
			for(int z=0 ; z< MAP_SIZE_Z ; z++){
				// 配列の中に入っているブロックを削除
				Destroy(map_BL[x , (z+map_z)%MAP_SIZE_Z].gameObject);

				// ブロック位置の算出
				Vector3 block_pos = new Vector3( BL_SIZE.x * newBL_posX , -BL_SIZE.y / 2 , BL_SIZE.z * (newBL_posZ + z));
				// プレハブ作成
				GameObject block = Instantiate(prefab_BL[n] , block_pos , Quaternion.identity) as GameObject;

				// 作成したブロックの名前変更
				block.name = "BL[" + x + "," + (z+map_z)%MAP_SIZE_Z + "]";
				// 作成したブロックの親、フォルダーにする
				block.transform.parent = block_folder.transform;
				// 作成したブロックを、マップ配列に格納
				map_BL[x , (z+map_z)%MAP_SIZE_Z] = block;

				// 次に作るブロックの番号
				n = (n+1) % prefab_BL.Length;
			}
		}
		*/
		
//		// 行方向のブロック削除／作成
//		if(change_pos.z != 0){
//
//			//int newBL_posZ = pos_now.z += (change_pos.z > 0) ? MAP_HARFSIZE_Z : -MAP_HARFSIZE_Z;
//
//
//
//			/*
//			int newBL_posZ = pos_now.z;
//			// 新規ブロック行の位置 = (現在座標 ± マップ幅÷2)
//			newBL_posZ += (change_pos.z > 0) ? MAP_HARFSIZE_Z : -MAP_HARFSIZE_Z;
//
//			// 操作列 = (新規ブロック位置 % マップ幅)
//			int z = newBL_posZ % MAP_SIZE_Z;
//			// 操作列が負の値の場合は、マップ幅を足して正値にする
//			if(z < 0){ z += MAP_SIZE_Z; }
//
//			// 新規ブロック列の初期位置 = (現在座標 - マップ幅÷2)
//			int newBL_posX = pos_now.x - MAP_HARFSIZE_X;
//			// 配列の初期座標(列)の取得
//			int map_x = newBL_posX % MAP_SIZE_X;
//			// 負の場合は正値にする
//			if(map_x < 0){ map_x += MAP_SIZE_X; }
//
//			// 最初に作るブロックの番号算出
//			int n = (pos_now.x + pos_now.z) % prefab_BL.Length;
//			if(n <0){ n += prefab_BL.Length;}
//
//			// (操作列の)全行に対し操作
//			for(int x=0 ; x< MAP_SIZE_X ; x++){
//				// 配列の中に入っているブロックを削除
//				Destroy(map_BL[(x+map_x)%MAP_SIZE_X , z].gameObject);
//
//				// ブロック位置の算出
//				Vector3 block_pos = new Vector3( BL_SIZE.x * (newBL_posX + x) , -BL_SIZE.y / 2 , BL_SIZE.z * newBL_posZ);
//				// プレハブ作成
//				GameObject block = Instantiate(prefab_BL[n] , block_pos , Quaternion.identity) as GameObject;
//
//				// 作成したブロックの名前変更
//				block.name = "BL[" + (x+map_x)%MAP_SIZE_X + "," + z + "]";
//				// 作成したブロックの親、フォルダーにする
//				block.transform.parent = block_folder.transform;
//
//				// 作成したブロックを、マップ配列に格納
//				map_BL[(x+map_x)%MAP_SIZE_X , z] = block;
//
//				// 次に作るブロックの番号
//				n = (n+1) % prefab_BL.Length;
//				*/
//
////				// ★★★壁の更新★★★
////				if(x != 0 && x != MAP_SIZE_X-1){ continue; }
////
////				// 配列の中に入っているアイテムを削除
////				Destroy(map_ITEM[(x + map_x)%MAP_SIZE_X , z].gameObject);
////
////				// ブロック位置の算出
////				Vector3 wall_pos = new Vector3( WALL_SIZE.x * (newBL_posX + x) , WALL_SIZE.y / 2 , WALL_SIZE.z * newBL_posZ);
////				// プレハブ作成
////				GameObject wall = Instantiate(prefab_ITEM[0] , wall_pos , Quaternion.identity) as GameObject;
////
////				// 作成したアイテムの名前変更
////				wall.name = "ITEM[" + (x+map_x)%MAP_SIZE_X + "," + z + "]_WALL";
////				// 作成したアイテムの親、フォルダーにする
////				wall.transform.parent = item_folder.transform;
////				
////				map_ITEM[(x+map_x)%MAP_SIZE_X , z] = wall;
////			}
//		}
//	}
	
}
