using UnityEngine;
using Common;

/**
 * ConfigSecene処理クラス
 */
public class ConfigController : MonoBehaviour {

    /**
     * 設定ボタン押下時の処理
     */
    public void OnSetButtonClicked()
    {
        Application.LoadLevel(FpsConstants.SCENE_NAME_TITLE);
    }

    /**
     * キャンセルボタン押下時の処理
     */
    public void OnCancelButtonClicked()
    {
        Application.LoadLevel(FpsConstants.SCENE_NAME_TITLE);
    }
}
