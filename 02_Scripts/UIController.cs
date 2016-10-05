using UnityEngine;
using System.Collections;
using Fpsgame.Weapon;
using UnityEngine.UI;
using Common;

public class UIController : MonoBehaviour {

	// 残弾表示
	public Text[]		text_gun_num;

    // 武器アイコンの配列
    public Image[] ImageWeaponIcon;

    // 武器アイコン背景の配列
    public Image[] ImageWeaponIconBG;

    public RawImage target_image;

	/**
	 * Start()よりも先に実行される初期化関数
	 */
	void Awake(){
		// Animatorコンポーネントの取得
//		anim_TextBom = GameObject.Find ("Text_Bom").GetComponent< Animator >();
	}

	void Start(){
		// ジャイロの使用を指定
		Input.gyro.enabled = true;
	}

	void Update(){
		//// ジャイロの傾きを取得
		//Quaternion q = Input.gyro.attitude;
		//Quaternion qq = Quaternion.AngleAxis (90.0f, Vector3.left);
		//this.transform.localRotation = qq * q;
	}

	/**
	 * 斬弾数を変更
	 */
	public void changeTextGunNum(int num){
		// text_gun_num配列の中身を順に操作
		foreach(Text t in text_gun_num){
			// 中身がnullでないなら
			if(t != null){
				// テキストを変更
				t.text = "残弾：" + num;
			}
		}
	}

    /**
    * 武器アイコンの色を選択中のものを強調するように変更
    */
    public void ChangeWeaponIcon(WeaponType weaponType)
    {
        foreach(Image image in ImageWeaponIconBG)
        {
            image.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }

        foreach (Image image in ImageWeaponIcon)
        {
            image.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        }

        switch (weaponType)
        {
            case WeaponType.HAND_GUN:
                ImageWeaponIconBG[0].color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                ImageWeaponIcon[0].color = new Color(1.0f, 1.0f, 1.0f, 0.8f);
                break;

            case WeaponType.MACHINE_GUN:
                ImageWeaponIconBG[1].color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                ImageWeaponIcon[1].color = new Color(1.0f, 1.0f, 1.0f, 0.8f);
                break;

            case WeaponType.GRENADE:
                ImageWeaponIconBG[2].color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                ImageWeaponIcon[2].color = new Color(1.0f, 1.0f, 1.0f, 0.8f);
                break;
        }
    }
	
	/**
	 * テキスト初期化用
	 */
	public void initialize(int type ,int num , bool used){
		// 武器タイプによるテキストの表示オン／オフ
		//changeTextEnable(type);

		// 残弾数を変更
		changeTextGunNum(num);
		// 手榴弾のテキスト変更
		//changeTextBom(used);
	}

	/**
	 * ターゲット位置変更
	 */
	public void setTargetPosition(int index){

		Vector3 newPosition = target_image.transform.position;

		if (index == 0) {
			newPosition.x += 1.0f;

		} else {
			newPosition.y += 1.0f;
		}

		target_image.transform.position = newPosition;
	}

	/**
	 * ターゲットサイズ変更
	 */
	public void setTargetSize(bool isSet){
		if (isSet) {
			target_image.transform.localScale = new Vector3 (1.15f, 1.15f, 1.15f);
		} else {
			target_image.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
		}
	}

    /**
     * タイトルボタン押下時の処理
     */
    public void OnGoTitleButtonClicked()
    {
        Application.LoadLevel(FpsConstants.SCENE_NAME_TITLE);
    }
}
