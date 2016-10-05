using UnityEngine;
using Fpsgame.Characters;
using System.Collections;

namespace Fpsgame.Main
{
    public class CameraController : MonoBehaviour
    {

        private PlayerController player;

        // Use this for initialization
        void Start()
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }

        // カメラ切り替え
        public void changeSight(bool type)
        {
            if (type)
            {
                // １人称カメラへの切り替え
                changeCameraMode1stPerson();
            }
            else
            {
                // ３人称カメラへの切り替え
                changeCameraMode3stPerson();
            }
        }

        // １人称カメラへの切り替え
        private void changeCameraMode1stPerson()
        {
            // 親オブジェクトがいない場合
            if (transform.parent == null)
            {
                // 自身の親オブジェクトに、playerオブジェクトを指定
                transform.parent = player.transform;
                // カメラの相対位置を零に
                transform.localPosition = Vector3.zero;
                // カメラの相対角度を零に(プレイヤーが向いている方向に)
                transform.localEulerAngles = Vector3.zero;
            }
        }

        // ３人称カメラへの切り替え
        private void changeCameraMode3stPerson()
        {
            // 親オブジェクトが存在する場合
            if (transform.parent != null)
            {
                // 親子関係を解除
                transform.parent = null;
                // カメラ位置を基本視点に変更
                transform.position = new Vector3(0, 4, -10);
                // カメラ角度を基本視点に変更
                transform.localEulerAngles = Vector3.zero;
            }
        }
    }

}
