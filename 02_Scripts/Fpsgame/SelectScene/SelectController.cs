using UnityEngine;
using Common;

/**
 * SelectSecene処理クラス
 */
public class SelectController : MonoBehaviour {

    [SerializeField]
    private GameObject[] characters;

    private int charaSelectIndex = 0;

    public Vector3 pos;

    void Start()
    {
        pos = new Vector3(-5.0f, -2.0f, 0.0f);
        this.setCharaActive();
    }

    /**
     * キャラクター選択左ボタン押下時の処理
    */
    public void OnCharaSelectLButtonClicked()
    {
        charaSelectIndex = (charaSelectIndex - 1) % characters.Length;
        if (charaSelectIndex < 0)
        {
            charaSelectIndex = characters.Length + charaSelectIndex;
        }
        this.setCharaActive();
    }

    /**
     * キャラクター選択右ボタン押下時の処理
    */
    public void OnCharaSelectRButtonClicked()
    {
        charaSelectIndex = (charaSelectIndex + 1) % characters.Length;
        this.setCharaActive();
    }

    /**
     * キャラクター表示状態設定
    */
    private void setCharaActive()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            if (i == charaSelectIndex)
            {
                characters[i].transform.position = pos;
                characters[i].SetActive(true);
            }
            else
            {
                characters[i].SetActive(false);
            }
        }
    }

    /**
     * スタートボタン押下時の処理
     */
    public void OnStartButtonClicked()
    {
        Application.LoadLevel(FpsConstants.SCENE_NAME_MAIN);
    }

    /**
     * 戻るボタン押下時の処理
     */
    public void OnBackButtonClicked()
    {
        Application.LoadLevel(FpsConstants.SCENE_NAME_TITLE);
    }
}
