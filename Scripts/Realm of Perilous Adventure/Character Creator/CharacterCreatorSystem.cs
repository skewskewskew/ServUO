using Server.Accounting;
using Server.Commands;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.CharacterCreator
{
    public class CharacterCreatorSystem
    {
        public static void Initialize()
        {
            EventSink.Login += new LoginEventHandler(EventSink_Login);
            CommandSystem.Register("CCCheat", AccessLevel.GameMaster, CharacterCreatorCheat_OnCommand);
            
        }

        [Usage("CCCheat NAME")]
        [Description("Sets your name to (NAME) and removes Character Creator GUMP")]
        public static void CharacterCreatorCheat_OnCommand(CommandEventArgs e)
        {
            string NAME = e.ArgString.Trim();
            Mobile m = e.Mobile;

            if (NAME.Length > 0)
            {
                m.Name = NAME;
                m.CloseAllGumps();
                m.Race = Race.Human;
                m.Hue = Race.Human.RandomSkinHue();
            }
        }
        public static void EventSink_Login(LoginEventArgs e)
        {
            if (e.Mobile != null)
            {
                if (e.Mobile.Name == "New Character" || !CharacterCreatorGump.CheckDupe(e.Mobile, e.Mobile.Name))
                {
                    e.Mobile.CantWalk = true;
                    e.Mobile.SendGump(new CharacterCreatorGump(e.Mobile));
                    e.Mobile.Blessed = true;
                }
                //else if(e.Mobile.Race == Race.Orc)
                //{
                //    if(e.Mobile.FindItemOnLayer(Layer.Face) == null)
                //    {
                //        OrcFace face = new OrcFace();
                //        e.Mobile.EquipItem(face);
                //        face.Movable = false;
                //        face.Hue = e.Mobile.Hue;
                //    }
                //}
            }
        }

        public static Type[] ItemTypes = new Type[]
        {
            typeof(BagOfFood), typeof(BagOfPotions),typeof(Bandage),typeof(BlankScroll),typeof(Board),
            typeof(IronIngot),typeof(Lockpick),typeof(BagOfReagents),typeof(BagOfNecroReagents),typeof(BookOfBushido),
            typeof(BookOfChivalry),typeof(MapmakersPen),typeof(FletcherTools),typeof(MortarPestle),typeof(Pickaxe),
            typeof(RollingPin),typeof(Tongs),typeof(Saw),typeof(ScribesPen),typeof(SewingKit),typeof(TinkersTools),typeof(MysticBook)
        };
    }
}
