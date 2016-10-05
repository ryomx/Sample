using UnityEngine;
using Common;
using UnityEngine.UI;

/**
 * TitleSecene処理クラス
 */
public class TitleController : MonoBehaviour {

    /** Startテキスト */
    public Text textStart;
    /** Configテキスト */
    public Text textConfig;
    /** 画面遷移メソッド名 */
    private const string SCREEN_TRANSITION_METHOD = "ScreenTransition";

    /** 遷移先Secene名 */
    private string nextSceneName;

    /**
     * Startテキスト押下時の処理
     */
    public void OnStartButtonClicked()
    {
        nextSceneName = FpsConstants.SCENE_NAME_SELECT;

        Animator animator = textStart.GetComponent<Animator>();
        animator.Play(FpsConstants.TITLE_SELECT_ANIMATION);

        Invoke(SCREEN_TRANSITION_METHOD, FpsConstants.SCREEN_TRANSITION_TIME);
    }

    /**
     * Configテキスト押下時の処理
     */
    public void OnConfigButtonClicked()
    {
        nextSceneName = FpsConstants.SCENE_NAME_CONFIG;
        Animator animator = textConfig.GetComponent<Animator>();
        animator.Play(FpsConstants.TITLE_SELECT_ANIMATION);

        Invoke(SCREEN_TRANSITION_METHOD, FpsConstants.SCREEN_TRANSITION_TIME);
    }

    /**
     * Scene遷移処理
     */
    private void ScreenTransition()
    {
        Application.LoadLevel(nextSceneName);
    }
}
