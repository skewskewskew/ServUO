#region Header
//   Vorspire    _,-'/-'/  BuffIconTest.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2015  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using System;
using System.IO;

using Server.Mobiles;
using Server.Network;

using VitaNex;
using VitaNex.IO;
using VitaNex.SuperGumps.UI;
#endregion

namespace Server
{
    public static class BuffIconTest
    {
        private static readonly short[] _Icons = ((BuffIcon)0).GetValues<short>();

        public static void Initialize()
        {
            CommandUtility.Register(
                "TestBuffs",
                AccessLevel.Administrator,
                e => NextIcon((PlayerMobile)e.Mobile, 1, IOUtility.EnsureFile(VitaNexCore.DataDirectory + "/BuffIcons.txt", true)));
        }

        private static void NextIcon(PlayerMobile m, int index, FileInfo file)
        {
            var uid = (short)(1000 + index);

            if (index > 1)
            {
                m.Send(new TestRemoveBuffPacket(m, (short)(uid - 1)));
            }

            m.Send(new TestAddBuffPacket(m, uid));

            var input = _Icons.Contains(uid) ? ((BuffIcon)uid).ToString() : String.Empty;

            new InputDialogGump(m)
            {
                Modal = false,
                CanMove = false,
                InputText = input,
                Title = "Buff #" + index + " (" + uid + ")",
                Html =
                    "Enter a description for the currently displayed Buff Icon and click OK to continue, or Cancel to stop testing.\n\n" +
                    "Data will be appended to '" + file + "'",
                AcceptHandler = b => NextIcon(m, index + 1, file),
                CancelHandler = b => m.Send(new TestRemoveBuffPacket(m, uid)),
                Callback = (b, val) => file.AppendText(false, val + " = " + uid + ",")
            }.Send();
        }

        private sealed class TestAddBuffPacket : Packet
        {
            public TestAddBuffPacket(Mobile mob, short iconID)
                : base(0xDF)
            {
                EnsureCapacity(44);
                m_Stream.Write(mob.Serial);

                m_Stream.Write(iconID); //ID
                m_Stream.Write((short)0x1); //Type 0 for removal. 1 for add 2 for Data

                m_Stream.Fill(4);

                m_Stream.Write(iconID); //ID
                m_Stream.Write((short)0x01); //Type 0 for removal. 1 for add 2 for Data

                m_Stream.Fill(4);

                m_Stream.Write((short)0); //Time in seconds

                m_Stream.Fill(3);
                m_Stream.Write(-1);
                m_Stream.Write(-1);

                m_Stream.Fill(10);
            }
        }

        private sealed class TestRemoveBuffPacket : Packet
        {
            public TestRemoveBuffPacket(Mobile mob, short iconID)
                : base(0xDF)
            {
                EnsureCapacity(13);
                m_Stream.Write(mob.Serial);

                m_Stream.Write(iconID); //ID
                m_Stream.Write((short)0x0); //Type 0 for removal. 1 for add 2 for Data

                m_Stream.Fill(4);
            }
        }
    }
}
