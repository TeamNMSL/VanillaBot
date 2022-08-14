using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunkong.Hoyolab.Avatar;

namespace VanillaForKonata.BotFunction.Games.Genshin
{
    public class CharModel
    {
        public CharModel(AvatarDetail chara)
        {
            mingzuo = chara.ActivedConstellationNumber.ToString();
            name=chara.Name;
            haogan = chara.Fetter.ToString();
            wuqi = new WeaponModel(chara.Weapon);
            LV=chara.Level.ToString();
            shengyiwu = new(chara.Reliquaries);
            element = chara.Element.ToString();
            icon = chara.Icon;
        }
        public string mingzuo = "";
        public string name = "";
        public string haogan = "";
        public WeaponModel wuqi = null;
        public string LV = "";
        public ReliquariesModel shengyiwu = null;
        public string element = "";
        public string icon;
    }

    public class ReliquariesModel
    {
        public List<ReliquarieModel> reliquaries=new();
        public ReliquariesModel(List<AvatarReliquary> p)
        {
            foreach (var item in p)
            {
                reliquaries.Add(new(item));
            }
        }
        public class ReliquarieModel
        {
            public string name;
            public string aff;
            public string lv;
            public string rat;
            public string pos;
            public string url;

            public ReliquarieModel(AvatarReliquary r)
            {
                name=r.ReliquarySet.Name;
                List<string> tmp=new();
                foreach (var item in r.ReliquarySet.Affixes)
                {
                    tmp.Add(item.Effect);
                }
                aff = string.Join("\n", tmp.ToArray());
                tmp.Clear();
                lv= r.Level.ToString();
                rat=r.Rarity.ToString();
                pos = r.Position.ToString();
                url = r.Icon;

            }
        }
    }

    public class WeaponModel
    {
        public string name;
        public string lv;
        public string affixLV;
        public string promoteLV;
        public string desc;
        public string url;
        public string xiyoudu;
        public WeaponModel(AvatarWeapon weapon)
        {
            affixLV=weapon.AffixLevel.ToString();
            promoteLV = weapon.PromoteLevel.ToString();
            desc = weapon.Description;
            url = weapon.Icon;
            xiyoudu=weapon.Rarity.ToString();
            lv=weapon.Level.ToString(); 
        }
    }
}
