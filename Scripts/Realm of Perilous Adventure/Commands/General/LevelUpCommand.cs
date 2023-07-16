using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using VitaNex;
using VitaNex.SuperGumps;
using System;

namespace Server.Commands
{
	public class LevelUp
	{
		public static void Initialize()
		{
			CommandSystem.Register("LevelUp", AccessLevel.GameMaster, new CommandEventHandler(LevelUpGump_OnCommand));
		}

		[Usage("LevelUp")]
		[Description("Opens the Level Up gump.")]
		public static void LevelUpGump_OnCommand(CommandEventArgs e)
		{

			PlayerMobile pm = e.Mobile as PlayerMobile;

			if (pm == null || pm.Deleted)
			{
				return;
			}

			if (pm.ESLevelSpent >= pm.ESLevel)
				pm.SendMessage("You do not have any unspent levels.");

			var gump = new VitaNex.SuperGumps.UI.SkillSelectionGump(pm)
			{

				// limit the number of skills that can be selected at once
				Limit = 3


			};

			// ignore skills
			gump.IgnoredSkills.Add(SkillName.Alchemy);

			// callback method is called after the accept handler
			gump.Callback = skills =>
			{
				foreach (var skillName in skills)
				{
					var skill = gump.User.Skills[skillName];
					// do something with 'skill'


					//var statgump = new StatCodexUI;
				}
			};

			gump.Send();
		}

		//public sealed class StatCodexUI : SuperGump
		//{
		//	public const int StatMin = 10, StatMax = 125;

		//	public int StatInc { get; private set; }

		//	public int StatsCap { get { return User.StatCap; } }

		//	private int _Str = StatMin;
		//	private int _Dex = StatMin;
		//	private int _Int = StatMin;

		//	public int Str { get { return _Str; } set { _Str = Math.Max(StatMin, Math.Min(StatMax, value)); } }
		//	public int Dex { get { return _Dex; } set { _Dex = Math.Max(StatMin, Math.Min(StatMax, value)); } }
		//	public int Int { get { return _Int; } set { _Int = Math.Max(StatMin, Math.Min(StatMax, value)); } }

		//	public int StatTotal { get { return Str + Dex + Int; } }
		//	public int StatPoints { get { return StatsCap - StatTotal; } }

		//	public bool CanAddStat { get { return StatPoints > 0; } }
		//	public bool CanRemoveStat { get { return StatTotal > StatMin; } }

		//	public bool CanAddStr { get { return Str < StatMax; } }
		//	public bool CanRemoveStr { get { return Str > StatMin; } }

		//	public bool CanAddDex { get { return Dex < StatMax; } }
		//	public bool CanRemoveDex { get { return Dex > StatMin; } }

		//	public bool CanAddInt { get { return Int < StatMax; } }
		//	public bool CanRemoveInt { get { return Int > StatMin; } }

			//public StatCodexUI(Mobile user, StatCodex codex)
			//	: base(user)
			//{
			//	Codex = codex;

			//	StatInc = 10;

			//	Str = User.RawStr;
			//	Dex = User.RawDex;
			//	Int = User.RawInt;

			//	FixStats();
			//}

		}
}


