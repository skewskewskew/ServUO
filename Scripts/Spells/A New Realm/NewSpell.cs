using Server.Items;
using System;

namespace Server.Spells
{
    public abstract class NewSpell : Spell
    {
		private int m_CastTimeFocusLevel;

		private static readonly int[] m_ManaTable = new int[] { 4, 6, 9, 11, 14, 20, 40, 50 };
        private const double ChanceOffset = 20.0, ChanceLength = 100.0 / 7.0;
        public NewSpell(Mobile caster, Item scroll, SpellInfo info)
            : base(caster, scroll, info)
        {
        }

		public abstract double RequiredSkill { get; }
		public abstract double RequiredCompanionSkill { get; }
		public abstract int RequiredMana { get; }
		//public abstract int RequiredTithing { get; }
		public override SkillName CastSkill => SkillName.Magery;
		public override SkillName DamageSkill => SkillName.EvalInt;
		public override SkillName CompanionSkill => SkillName.EvalInt;
		public override SkillName DefenseSkill => SkillName.MagicResist;

		public override bool ClearHandsOnCast => false;
		public override int CastRecoveryBase => 7;
		public abstract SpellCircle Circle { get; }
        public override TimeSpan CastDelayBase => TimeSpan.FromMilliseconds(((4 + (int)Circle) * CastDelaySecondsPerTick) * 1000);
        public override bool ConsumeReagents()
        {
            if (base.ConsumeReagents())
                return true;

            if (ArcaneGem.ConsumeCharges(Caster, 1))
                return true;

            return false;
        }
		public virtual int FocusLevel => m_CastTimeFocusLevel;

		// Note: ComputeKarmaAward() is in NecromancerSpell

		public override void GetCastSkills(out double min, out double max, out double compmin, out double compmax)
		{
			// This is taken from necromancy.
			min = RequiredSkill;
			max = Scroll != null ? min : RequiredSkill + 40.0;
			compmin = RequiredCompanionSkill;
			compmax = Scroll != null ? compmin : RequiredSkill + 40.0;
		}

        public override int GetMana()
        {
            if (Scroll is BaseWand)
                return 0;

            return m_ManaTable[(int)Circle];
        }

        public virtual bool CheckResisted(Mobile target)
        {
            double n = GetResistPercent(target);

            n /= 100.0;

            if (n <= 0.0)
                return false;

            if (n >= 1.0)
                return true;

            int maxSkill = (1 + (int)Circle) * 10;
            maxSkill += (1 + ((int)Circle / 6)) * 25;

            if (target.Skills[SkillName.MagicResist].Value < maxSkill)
                target.CheckSkill(SkillName.MagicResist, 0.0, target.Skills[SkillName.MagicResist].Cap);

            return (n >= Utility.RandomDouble());
        }

        public virtual double GetResistPercentForCircle(Mobile target, SpellCircle circle)
        {
            double value = GetResistSkill(target);
            double firstPercent = value / 5.0;
            double secondPercent = value - (((Caster.Skills[CastSkill].Value - 20.0) / 5.0) + (1 + (int)circle) * 5.0);

            return (firstPercent > secondPercent ? firstPercent : secondPercent) / 2.0; // Seems should be about half of what stratics says.
        }

        public virtual double GetResistPercent(Mobile target)
        {
            return GetResistPercentForCircle(target, Circle);
        }
    }
}
