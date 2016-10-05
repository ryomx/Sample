using UnityEngine;
using System.Collections;

public class BombController : MonoBehaviour {

	public GameObject prefab_HitEffect2;

	private SoundController	sound;
	// 手榴弾の爆発音No
	private int				bom_explosion_sound = 2;

	// Use this for initialization
	void Start () {

		sound = GameObject.Find("Sound").GetComponent< SoundController >();

		StartCoroutine("Bomb");		// コルーチン開始
	}
	
	IEnumerator Bomb(){
		yield return new WaitForSeconds(2.5f);		// 2.5秒、処理を待機.

		// ボムエフェクト発生
		GameObject effect = Instantiate(prefab_HitEffect2 , transform.position , Quaternion.identity) as GameObject;
		Destroy(effect , 1.0f);		// ボムエフェクトを、１秒後に消滅させる

		// 手榴弾の爆発音を鳴らす
		sound.SendMessage("soundRings" , bom_explosion_sound);

		// ボムによる攻撃処理
		bomAttack();

		Destroy(gameObject);
	}
	
	/**
	 * ボムによる攻撃処理
	 */
	private void bomAttack(){
		Collider[] targets = Physics.OverlapSphere(transform.position , 1.5f);	// 自分自身を中心に、半径1.5以内にいるColliderを探し、配列に格納.
		foreach(Collider obj in targets){		// targets配列を順番に処理 (その時に仮名をobjとする)
			if(obj.tag == "Object"){			// タグ名がObjectなら
				Destroy(obj.gameObject);		// そのゲームオブジェクトを消滅させる。
			}
		}
	}
}
