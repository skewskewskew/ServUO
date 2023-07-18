using Server.Mobiles;
using System;

namespace Server.Spells.Hedge

{
    public abstract class HedgeSummon<T> : NewSpell where T : BaseCreature
    {
        public HedgeSummon(Mobile caster, Item scroll, SpellInfo info)
            : base(caster, scroll, info)
        {
        }

        public override SpellCircle Circle => SpellCircle.First;
        public override double RequiredSkill => 10;
        public override int RequiredMana => 11;
        public override SkillName CompanionSkill => SkillName.Camping;
        public override double RequiredCompanionSkill => 50.0;

        public abstract int Sound { get; }
        public override bool CheckCast()
        {
            if (!base.CheckCast())
                return false;

            if ((Caster.Followers + 1) > Caster.FollowersMax)
            {
                Caster.SendLocalizedMessage(1074270); // You have too many followers to summon another one.
                return false;
            }

            return true;
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                TimeSpan duration = TimeSpan.FromMinutes(Caster.Skills.Spellweaving.Value / 24 + FocusLevel * 2);
                int summons = Math.Min(1 + FocusLevel, Caster.FollowersMax - Caster.Followers);

                for (int i = 0; i < summons; i++)
                {
                    BaseCreature bc;

                    try
                    {
                        bc = Activator.CreateInstance<T>();
                    }
                    catch (Exception e)
                    {
                        Diagnostics.ExceptionLogging.LogException(e);
                        break;
                    }

                    SpellHelper.Summon(bc, Caster, Sound, duration, false, false);
                }

                FinishSequence();
            }
        }
    }
}
