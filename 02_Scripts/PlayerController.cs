using UnityEngine;
using UnityEngine.UI;
using Fpsgame.Weapon;
using System.Collections;

namespace Fpsgame.Characters
{
    public class PlayerController : MonoBehaviour
    {
        public Canvas canvas;
        private CharacterController controller;         // キャラクターコンポーネント用の変数
        private Vector3 moveDirection = Vector3.zero;   // キャラ移動量
        private float speed = 5.0f;                 // 移動速度
        public float jumpPower = 35.0f;                 // 跳躍力
        private const float GRAVITY = 9.8f;         // 重力
        private const float rotationSpeed = 1.0f;            // プレイヤーの回転速度MIN

        private bool moveType = true;           // １人称視点動作：true , ３人称視点動作：false

        private GameObject target = null;               // ターゲット格納用の変数
        private int range = 30;                            // 攻撃可能距離（飛距離）

        public GameObject prefab_hitEffect1;            // 攻撃時のヒットエフェクト
        private Vector3 attackPoint;                // 攻撃位置
        
        private WeaponInfo weapon;                          // 武器

        public GameObject prefab_bomb;              // 手榴弾
        private bool isUsedBomb = false;            // 手榴弾の速射管理用

        public int gun_num;                 // 銃弾の残弾数
        private const int GUN_MAX_NUM = 30;         // 銃弾の最大弾数

        private SoundController sound;
        private int gun_sound = 0;              // 銃撃の音No
        private int bom_throw_sound = 1;        // 手榴弾を投げる音No.

        private UIController uiController;

        private GameObject weaponObject;

        // ボタン上にカーソルがあるとtrue
        private bool isButton = false;

        // 移動ボタンドラッグ中であるとtrue
        private bool isDrug = false;

        // 移動初期位置格納用
        public Vector2 moveStartPosition;
        // 移動現在位置格納用
        public Vector2 currentMovePosition;

        // 銃発射時の銃のけぞり時間
        private float gunAttackActionTime = 0.5f;

        // 銃発射時の銃のけぞり距離の調整値
        float gunRecoilNum = 0.01f;

        // 攻撃時のマシンガンの初期位置
        private Vector3 attacMachineGunkPos = new Vector3(0.25f, -0.3f, 0.7f);
        // 攻撃時のマシンガンの初期Rotation
        private Quaternion attacMachineGunkRot = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        //private Vector3 attacMachineGunkRot = new Vector3(0.0f, -180.0f, 0.0f);

        // 移動時のマシンガンの初期位置
        //    private Vector3 moveMachineGunPos = new Vector3(0.25f, -0.45f, 0.7f);
        //private Vector3 moveMachineGunPos = new Vector3(0.2f, -0.3f, 0.7f);
        private Vector3 moveMachineGunPos = new Vector3(0.13f, -0.08f, 0.8f);
        // 移動時のマシンガンの初期Rotation
        //    private Quaternion moveMachineGunkRot = Quaternion.Euler(-5.0f, -190.0f, 0.0f);
        //private Quaternion moveMachineGunkRot = Quaternion.Euler(0.0f, 178.0f, 0.0f);
        private Quaternion moveMachineGunkRot = Quaternion.Euler(0.0f, -5.0f, 0.0f);
        //private Vector3 moveMachineGunkRot = new Vector3(-5.0f, -190.0f, 0.0f);

        public GameObject prefab_MuzzleFlash;

        private Animator weaponAnim;

        private PlayerState playerState;

        public float playerStateNormalTime;

        private Vector2 playerMoveDirection;

        private int granadeFrontSpeed = 12;

        private int granadeUpSpeed = 6;

        // 移動回転速度最大値
        float speedSikiichiMax = 45.0f;

        //private RectTransform canvasRectTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();
        private RectTransform canvasRectTransform;

        //Canvas canvas = GameObject.Find("Canvas");
        //private RectTransform CanvasRect = canvas.GetComponent<RectTransform>();

        void awake() {
            //canvasRectTransform = canvas.GetComponent<RectTransform>();
        }

        void Start()
        {
            canvasRectTransform = canvas.GetComponent<RectTransform>();

            controller = GetComponent<CharacterController>();

            weapon = new WeaponInfo();

            uiController = GameObject.Find("GameRoot").GetComponent<UIController>();

            // 銃弾装填
            gun_num = GUN_MAX_NUM;

            // 弾数の表示変更
            //uiController.changeTextGunNum(gun_num)
            // テキストの初期化
            //uiController.initialize(weapon.getType() , gun_num , isUsedBomb);

            sound = GameObject.Find("Sound").GetComponent<SoundController>();

            // TODO 後で取得場所を変更
            weaponObject = transform.FindChild("FirstPersonCharacter").FindChild("WPN_AKM").gameObject;
            weaponAnim = weaponObject.GetComponent<Animator>();

            //weaponObject.transform.position = moveMachineGunPos;
            //weaponObject.transform.rotation = moveMachineGunkRot;

            this.ChangeWeapon((int)WeaponType.HAND_GUN);

            // Animator has not been initializedのエラー対応
            //weaponAnim.Rebind();

            //playerState = PlayerState.NORMAL;

            nonAttackBtn();

            playerMoveDirection = new Vector2(0.0f, 0.0f);

            moveStartPosition = new Vector2(0.0f, 0.0f);
            currentMovePosition = new Vector2(0.0f, 0.0f);

            
        }

        void Update()
        {
            // ターゲット情報を取得
            setTarget();

            if (target == null)
            {
                uiController.setTargetSize(false);
            }
            else
            {
                uiController.setTargetSize(true);
            }

            // カメラの視点変更
            if (moveType)
            {
                // １人称視点動作
                playerMove1rdParson();
                //			// １人称視点動作
                //			playerMove1rdParson();
            }
            else
            {
                // ３人称視点動作
                //	playerMove3rdParson();
            }

            //playerStateCheck();

            // のけぞり角度＋１を保険として条件に設定する
            //float nokezori = 10.0f + 1.0f;

            //Vector3 weaponD = weaponObject.transform.eulerAngles;
            //if (Mathf.DeltaAngle(0.0f, weaponObject.transform.eulerAngles.x) > 0.5f)
            // 武器攻撃によるのけぞり角度を取得
            float angle = Mathf.DeltaAngle(0.0f, weaponObject.transform.eulerAngles.x);
            Debug.Log("angle:" + angle);

            // 武器ののけぞりのローテーションにより符号が変わる
            float keisuu;
            if (true)
            {
                keisuu = 40.0f;
                if (angle < -0.5f)
                {
                    weaponObject.transform.Rotate(new Vector3(keisuu * Time.deltaTime, 0.0f, 0.0f));
                }
            }
            else
            {
                keisuu = -40.0f;
                if (angle > 0.5f)
                {
                    weaponObject.transform.Rotate(new Vector3(keisuu * Time.deltaTime, 0.0f, 0.0f));
                }
            }
        }

        public Vector2 getPlayerMoveDirection()
        {
            return this.playerMoveDirection;
        }

        // ターゲット情報を取得
        private void setTarget()
        {

            // ターゲットカーソルの位置を取得
            //Transform child_target = transform.FindChild ("target");
            //Ray ray = RectTransformUtility.ScreenPointToRay(Camera.main, transform.FindChild ("fire_target").position);
            Ray ray = RectTransformUtility.ScreenPointToRay(Camera.main, GameObject.Find("Canvas").transform.FindChild("RawImage_Target").position);

            // マウス位置から、カメラが見ている方向へと真っ直ぐ進む線を取得
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Ray ray = Camera.main.ScreenPointToRay(transform.position);
            // ヒット情報を格納する変数を作成
            RaycastHit hitInfo;

            // カメラから飛距離10（仮）の光線を出し、もし何かに当たった場合
            if (Physics.Raycast(ray, out hitInfo, range))
            {
                // その当たったオブジェクトのタグ名が Object の場合
                if (hitInfo.collider.gameObject.tag == "Object")
                {
                    // 当たったオブジェクトを、参照
                    target = hitInfo.collider.gameObject;
                    // 光線が当たった位置を取得
                    attackPoint = hitInfo.point;

                    // ターゲットが見つかったので、処理を抜ける
                    return;
                }
            }

            // Enemyが見つからないなら、null(空)にする
            target = null;
        }


        /**
	     * 左クリックで敵を攻撃
	     * 攻撃ボタンタップ時も当処理を呼び出す
	     */
        public void attack()
        {

            switch (weapon.getType())
            {

                // 武器が銃の場合
                //case 0:
                case WeaponType.HAND_GUN:
                    //this.attackBtn();

                    attackGun();
                    break;

                // 武器が手榴弾の場合
                //case 1:
                case WeaponType.GRENADE:
                    attackBomb();
                    break;
            }
        }


        /**
	     * 銃による攻撃
	     */
        private void attackGun()
        {

            // 残弾がないなら、以降は処理しない.
            if (gun_num == 0) { return; }

            weaponAttackAction();

            // 照準にターゲットが入っているなら
            if (target != null)
            {
                // 攻撃エフェクト発生
                GameObject effect = Instantiate(prefab_hitEffect1, attackPoint, Quaternion.identity) as GameObject;
                // 0.2秒待ってエフェクトを削除
                Destroy(effect, 0.2f);

                // ターゲットを消滅させる
                Destroy(target);
            }

            // 銃撃の音を鳴らす
            sound.SendMessage("soundRings", gun_sound);

            // 弾数を減らす
            gun_num--;

            // 弾数の表示変更
            uiController.changeTextGunNum(gun_num);

            // 弾切れの場合は銃弾の再装填コルーチン開始
            if (gun_num == 0)
            {
                StartCoroutine("reChargeGun");
            }
        }

        /**
	     * 手榴弾による攻撃
	     */
        private void attackBomb()
        {

            if (!isUsedBomb)
            {
                // 手榴弾の作成位置を取得（プレイヤー位置　+　プレイヤー正面にむけて１進んだ距離）
                Vector3 pos = transform.position + transform.TransformDirection(Vector3.forward);
                // 手榴弾を作成
                GameObject bom = Instantiate(prefab_bomb, pos, Quaternion.identity) as GameObject;

                // 手榴弾の移動速度。『プレイヤー正面に向けての速度ベクトル』。
                Vector3 bombSpeed = transform.TransformDirection(Vector3.forward) * granadeFrontSpeed;
                // 手榴弾の『高さ方向の速度』を加算
                bombSpeed += Vector3.up * granadeUpSpeed;
                // 手榴弾の速度を代入
                bom.GetComponent<Rigidbody>().velocity = bombSpeed;
                // 手榴弾の回転速度を代入.
                bom.GetComponent<Rigidbody>().angularVelocity = Vector3.forward * 7;

                // 手榴弾を投げる音を鳴らす
                sound.SendMessage("soundRings", bom_throw_sound);

                // ボムを使用不可能にする
                isUsedBomb = true;
                // 手榴弾のテキスト変更
                //uiController.changeTextBom(isUsedBomb);
                // 手榴弾の速射管理コルーチンを開始(数秒後、ボムを再び使用可にする)
                StartCoroutine("reChargeBomb");
            }
        }

        /**
	     * 手榴弾の速射管理コルーチン
	     */
        IEnumerator reChargeBomb()
        {
            // 2.5秒、処理を待機
            yield return new WaitForSeconds(2.5f);
            // 手榴弾使用フラグ初期化
            isUsedBomb = false;
            // 手榴弾のテキスト変更
            //uiController.changeTextBom(isUsedBomb);
        }

        /**
	     * 銃で攻撃時の銃の動き
	     */
        private void weaponAttackAction()
        {
            // 銃ののけぞり角度を取得
            Quaternion weaponDirection = Quaternion.Euler(-10.0f, -5.0f, 0.0f);

            // マズルフラッシュ発生位置を取得
            Vector3 effectMFPos = weaponObject.transform.FindChild("MuzzleFlashPos").position;

            // マズルフラッシュを生成
            GameObject effectMF = Instantiate(prefab_MuzzleFlash, effectMFPos, Quaternion.identity) as GameObject;

            // 0.1秒待ってエフェクトを削除
            Destroy(effectMF, 0.1f);

            // 銃ののけぞりを作成
            weaponObject.transform.localRotation = weaponDirection;
        }

        /**
	     * 手銃弾の再装填コルーチン
	     */
        IEnumerator reChargeGun()
        {
            // 3秒、処理を待機.
            yield return new WaitForSeconds(3.0f);
            // 銃弾装填
            gun_num = GUN_MAX_NUM;
            // 弾数の表示変更
            uiController.changeTextGunNum(gun_num);
        }

        //// 武器を変更
        public void ChangeWeapon(int weaponIndex)
        {
            WeaponType weaonType = (WeaponType)weaponIndex;
            weapon.SetWeaponType(weaonType);
            uiController.ChangeWeaponIcon(weaonType);
        }
        //public void changeWeapon(){

        //	// 武器タイプ変更
        //	weapon.changeWeapon();

        //	// 武器画像を変更
        //	uiController.changeRawImageWeapon(weapon.getType());

        //	// 武器テキスト表示を切り替える
        //	uiController.changeTextEnable(weapon.getType());
        //}

        // 1人称視点の移動
        private void playerMove1rdParson()
        {

            float upDown = 0.0f;
            float rightLeft = 0.0f;

            // ドラッグしているときは処理する
            if (this.isDrug)
            {

                float sikiichiMin = 20.0f;
                Vector3 inutPosition = Input.mousePosition;

                //RaycastHit2D hitInfo;
                //Ray ray = RectTransformUtility.ScreenPointToRay(Camera.main, inutPosition);
                //hitInfo = Physics2D.Raycast(tapPoint, -Vector2.up);
                

                // スクリーン座標からワールド座標に変換
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(inutPosition);
                // ワールド座標をCavas座標へ変換する
                Vector3 screenPos = Camera.main.WorldToViewportPoint(worldPos);
                // Cavas座標にするときはワールド座標をビューポート座標に変換し、
                // CanvasのRectTransformのサイズの1 /2を引く（座標の原点が違うため）
                Vector2 WorldObject_ScreenPosition = new Vector2(
                  ((screenPos.x * canvasRectTransform.sizeDelta.x) - (canvasRectTransform.sizeDelta.x * 0.5f)),
                  ((screenPos.y * canvasRectTransform.sizeDelta.y) - (canvasRectTransform.sizeDelta.y * 0.5f)));

                currentMovePosition.x = WorldObject_ScreenPosition.x;
                currentMovePosition.y = WorldObject_ScreenPosition.y;
                //Canvas.anchoredPosition = WorldObject_ScreenPosition;


                //Vector2 pos = new Vector2(tapPoint.x, tapPoint.y);
                // Collider2dが指定したワールド座標と接触しているかの判定
                //Collider2D collition2d = Physics2D.OverlapPoint(pos);
                //if (collition2d) {
                RaycastHit2D hitObject = Physics2D.Raycast(WorldObject_ScreenPosition, -Vector2.up);
//                    if (hitObject) {
//                        if (hitObject.collider.gameObject.tag == "UI_Move") {
                            //currentMovePosition.x = inutPosition.x;
                            //currentMovePosition.y = inutPosition.y;

                            if ((Input.mousePosition.y - moveStartPosition.y) > sikiichiMin)
                            {
                                upDown = 1;
                            }
                            else if ((Input.mousePosition.y - moveStartPosition.y) < -sikiichiMin)
                            {
                                upDown = -1;
                            }

                            float xDiff = Input.mousePosition.x - moveStartPosition.x;
                            if (xDiff > sikiichiMin)
                            {
                                rightLeft = 1;
                            }
                            else if (xDiff < -sikiichiMin)
                            {
                                rightLeft = -1;
                            }
//                        }
//                    }
                //}

//                // 移動ボタンタップの現在位置との比較（移動ボタンドラッグと同時タップの対応）
//                RaycastHit hitInfo;
//                Ray ray = RectTransformUtility.ScreenPointToRay(Camera.main, inutPosition);
                // カメラから光線を出し、もし何かに当たった場合
                //if (Physics.Raycast(ray, out hitInfo))
                //{
                    //// その当たったオブジェクトのタグ名が Object の場合
                    //if (hitInfo.collider.gameObject.tag == "UI_Move")
                    //{
                        //currentMovePosition.x = inutPosition.x;
                        //currentMovePosition.y = inutPosition.y;

                        //if ((Input.mousePosition.y - moveStartPosition.y) > sikiichi)
                        //{
                        //    //isMove = true;
                        //    upDown = 1;
                        //}
                        //else if ((Input.mousePosition.y - moveStartPosition.y) < -sikiichi)
                        //{
                        //    //isMove = true;
                        //    upDown = -1;
                        //}
                        //if ((Input.mousePosition.x - moveStartPosition.x) > sikiichi)
                        //{
                        //    //isMove = true;
                        //    rightLeft = 1;
                        //}
                        //else if ((Input.mousePosition.x - moveStartPosition.x) < -sikiichi)
                        //{
                        //    //isMove = true;
                        //    rightLeft = -1;
                        //}
//                    }
//                }

                // ▼▼▼移動量の取得▼▼▼
                // 移動ボタンドラッグ入力を取得し、移動量に代入
                moveDirection = new Vector3(0.0f, 0.0f, upDown);
                // プレイヤー基準の移動方向へ修正する
                moveDirection = transform.TransformDirection(moveDirection);
                // 移動速度を乗算
                moveDirection *= speed;

                // ▼▼▼プレイヤーの向き変更▼▼▼
                // 移動ボタンドラッグ入力を取得し、移動方向に代入
                Vector3 playerDirection = new Vector3(rightLeft, 0.0f, 0.0f);
                // プレイヤー基準の向きたい方向へ修正する
                playerDirection = transform.TransformDirection(playerDirection);
                if (playerDirection.magnitude > 0.1f)
                {
                    float rotSpeed = rotationSpeed + Mathf.Abs(xDiff) / 2.0f;
                    if (rotSpeed > speedSikiichiMax) {
                        rotSpeed = speedSikiichiMax;
                    }

                    // 向きたい方角をQuaternionn型に直す 
                    Quaternion q = Quaternion.LookRotation(playerDirection);
                    // 向きを q に向けてゆっくりと変化させる
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, q, rotSpeed * Time.deltaTime);
                }
            }

            playerMoveDirection.x = rightLeft;
            playerMoveDirection.y = upDown;

        }

        //	// 3人称視点の移動
        //	private void playerMove3rdParson(){
        //
        //		// ▼▼▼移動量の取得▼▼▼
        //		float y = moveDirection.y;
        //		// 左右上下のキー入力を取得し、移動量に代入
        //		moveDirection = new Vector3(Input.GetAxis("Horizontal") , 0.0f , Input.GetAxis("Vertical"));
        //		// 移動方向を取得
        //		Vector3 playerDir = moveDirection;
        //		// 移動速度を乗算
        //		moveDirection *= speed;
        //		
        //		// ▼▼▼重力／ジャンプ処理▼▼▼
        //		moveDirection.y += y;
        //		// 地面に設置していたら
        //		if(controller.isGrounded){
        //			// ジャンプ処理
        //			if(Input.GetKeyDown(KeyCode.Space)){
        //				moveDirection.y = jumpPower;
        //			}
        //		}
        //		// 重力を代入
        //		moveDirection.y -=  GRAVITY * Time.deltaTime;
        //		
        //		// ▼▼▼プレイヤーの向き変更▼▼▼
        //		if(playerDir.magnitude > 0.1f){
        //			// 向きたい方角をQuaternionn型に直す
        //			Quaternion q = Quaternion.LookRotation(playerDir);
        //			// 向きを q に向けてゆっくりと変化させる
        //			transform.rotation = Quaternion.RotateTowards(transform.rotation , q , rotationSpeed * Time.deltaTime);
        //		}
        //		
        //		// ▼▼▼移動処理▼▼▼
        //		// プレイヤー移動
        //		controller.Move(moveDirection * Time.deltaTime);
        //	}

        // 動作切り替え
        public void changeMoveType(bool type)
        {
            moveType = type;
        }

        /**
	     * 変数isButtonを書き換える
	     */
        public void setIsButton(bool button)
        {
            isButton = button;
        }
        //
        public void PushDown()
        {
            //this.push = true;

            Vector3 position = Input.mousePosition;
            //Debug.Log(position.x);
            //Debug.Log(position.y);

            // スタートポジションを設定
            moveStartPosition.x = Input.mousePosition.x;
            moveStartPosition.y = Input.mousePosition.y;

            //// 現在移動ポジションにスタートポジションを設定
            //currentMovePosition = moveStartPosition;
        }
        //
        //	public void PushUp(){
        //		this.push = false;
        //	}
        //
        //	public void setPushDirection(int directionType){
        //		this.pushDirection = directionType;
        //	}

        //	public void BeginDrag(){
        //		//Debug.Log("BeginDragイベント");
        //
        //	}

        public void Drag()
        {

            isDrug = true;

            //// 移動ボタンタップ中の現在位置更新
            //Vector3 inutPosition = Input.mousePosition;
            //if (Mathf.Abs(inutPosition.x - currentMovePosition.x) < sikiichiMax
            //    && Mathf.Abs(inutPosition.y - currentMovePosition.y) < sikiichiMax)
            //{
            //    currentMovePosition.x = inutPosition.x;
            //    currentMovePosition.y = inutPosition.y;
            //}
        }

        public void EndDrag()
        {
            //Debug.Log("EndDragイベント");
            // ドラッグ状態を解除
            isDrug = false;
        }
        
        public void PointerUp()
        {
            //isDrug = false;
        }

        //private void playerStateCheck() {
        //    if (playerState == PlayerState.ATTACK)
        //    {

        //        if (playerStateNormalTime > 0.0f)
        //        {
        //            playerStateNormalTime -= Time.deltaTime;
        //        }
        //        else
        //        {
        //            this.nonAttackBtn();
        //        }
        //    }
        //}

        //private void attackBtn() {
        //    if (playerState != PlayerState.ATTACK)
        //    {
        //        playerState = PlayerState.ATTACK;

        //        weaponObject.transform.localPosition = attacMachineGunkPos;
        //        weaponObject.transform.localRotation = attacMachineGunkRot;
        //        weaponObject.transform.eulerAngles = attacMachineGunkRot;
        //        weaponObject.transform.localRotation = attacMachineGunkRot;

        //    }

        //    playerStateNormalTime = 1.5f;
        //}

        private void nonAttackBtn()
        {

            playerState = PlayerState.NORMAL;

            weaponObject.transform.localPosition = moveMachineGunPos;
            //weaponObject.transform.localRotation = moveMachineGunkRot;
            //        weaponObject.transform.eulerAngles = moveMachineGunkRot;
            weaponObject.transform.localRotation = moveMachineGunkRot;

            playerStateNormalTime = 0.0f;
        }

        enum PlayerState
        {
            NORMAL,
            MOVE,
            ATTACK,
            MOVE_AND_ATTACK
        }
    }
}