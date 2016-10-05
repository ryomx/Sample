using UnityEngine;
using System.Collections;

namespace Fpsgame.Weapon
{
    public class WeaponInfo
    {

        /** 武器タイプ */
        private WeaponType weaponType;

        //   private int type	= 0;			// 武器タイプ
        //private int num		= 2;            // 武器の種類数


        // 武器設定
        public void SetWeaponType(WeaponType weaponType)
        {
            this.weaponType = weaponType;

        }

        // 武器を変更
        //public void changeWeapon(){
        //	type = (type + 1) % num;
        //	Debug.Log("現在の武器：" + type);
        //}

        // 武器タイプを取得
        //public int getType(){
        //	return type;
        //}
        public WeaponType getType()
        {
            return weaponType;
        }

    }

    public enum WeaponType
    {
        HAND_GUN = 0,
        MACHINE_GUN = 1,
        GRENADE = 2
    }
}