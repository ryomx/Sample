using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {

	private	AudioSource		audioSource;	// AudioSorceコンポーネント格納用
	public	AudioClip[]		sound;			// 効果音の格納用

	// Use this for initialization
	void Start () {
		// AudioSorceコンポーネントを追加し、変数に代入
		audioSource	= gameObject.AddComponent< AudioSource >();
		// 音のループなし
		audioSource.loop = false;
	}

	/**
	 * 渡された番号の音を鳴らす
	 */
	private void soundRings(int value){

		// もし指定された番号が負の値なら、処理を抜ける.
		if(value < 0){ return; }

		// もし指定された番号が、sound配列に格納されている数-1より大きいなら、処理を抜ける
		if(value > sound.Length - 1){ return; }

		// もし中身に何も入っていなければ、処理を抜ける。
		if(sound[value] == null){ return; }

		// 該当の音を一回だけ再生
		audioSource.PlayOneShot(sound[value]);
	}
}
