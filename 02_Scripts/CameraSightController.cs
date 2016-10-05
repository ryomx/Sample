using UnityEngine;
using Fpsgame.Characters;
using System.Collections;

public class CameraSightController : MonoBehaviour {

	//private PlayerController player;
	//private CameraController mainCamera , subCamera;
	//private bool cameraSight = true;		// １人称視点：true  , ３人称視点：flase

	//void Start () {
	//	player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
	//	mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();
	//	subCamera = GameObject.FindWithTag("SubCamera").GetComponent<CameraController>();

	//	// 視点切り替え命令
	//	changeCameraSight();
	//}

	//void Update () {
	//	if(Input.GetKeyDown(KeyCode.Tab)){
	//		cameraSight	= !cameraSight;
	//		// 視点切り替え命令
	//		changeCameraSight();
	//	}
	//}
	
	//// 視点切り替え命令
	//private void changeCameraSight(){
	//	// プレイヤーの動作モードタイプ変更
	//	player.changeMoveType(cameraSight);
	//	// メインカメラの視点切り替え
	//	mainCamera.changeSight(cameraSight);
	//	// サブカメラの視点切り替え
	//	subCamera.changeSight(!cameraSight);
	//}
}
