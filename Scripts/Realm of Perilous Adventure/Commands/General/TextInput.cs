using Server.Mobiles;
using Server.Gumps;
using Server.Network;

namespace Server.Commands
{
    public class Input
    {
        public static void Initialize()
        {
            CommandSystem.Register("TextInput", AccessLevel.GameMaster, new CommandEventHandler(InputGump_OnCommand));
        }

        [Usage("TextInput")]
        [Description("Gump For Long Inputs.")]
        public static void InputGump_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile is PlayerMobile)
                e.Mobile.SendGump(new InputGump((PlayerMobile)e.Mobile, "", "" ));
        }

        public class InputGump : Gump
        {
            private PlayerMobile pm_From;
            public InputGump(PlayerMobile from, string message, string input) : base(0, 0)
            {
                pm_From = from;
                from.CloseGump(typeof(InputGump));
                Resizable = false;
                AddPage(0);

                AddBackground(25, 10, 590, 100, 5054);

                AddLabel(45, 85, 38, message);

                AddLabel(555, 85, 10, @"Submit");
                AddButton(520, 85, 4005, 4007, 1, GumpButtonType.Reply, 0);
                
                AddBackground(40, 20, 560, 60, 9350);
                if (input.Length>1)
                { AddTextEntry(45, 20, 545, 100, 0, 0, @message); }
                else
                { AddTextEntry(45, 20, 545, 100, 0, 0, @""); }
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                if (sender == null || info == null) return;

                string say = info.TextEntries[0].Text;

                if (say.Length > 235)
                {
                    pm_From.SendGump(new InputGump(pm_From, "Your message is too long, please keep it under 235 characters.", say));
                    return;
                }
                else
                {
                    pm_From.DoSpeech(say.Length.ToString(), new int[0], MessageType.Regular, 0);
                }
                


            }
        }
    }
}


