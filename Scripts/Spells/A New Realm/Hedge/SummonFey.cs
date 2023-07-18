using Server.Mobiles;
using System;

namespace Server.Spells.Hedge

{
    public class SummonFeySpell : HedgeSummon<ArcaneFey>
    {
        private static readonly SpellInfo m_Info = new SpellInfo(
            "Summon Fey", "Alalithra",
            -1);
        public SummonFeySpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override SpellCircle Circle => SpellCircle.First;
        public override double RequiredSkill => 10;
        public override int RequiredMana => 11;
        public override SkillName CompanionSkill => SkillName.Camping;
        public override double RequiredCompanionSkill => 50.0;

        public override TimeSpan CastDelayBase => TimeSpan.FromSeconds(1.5);

        public override int Sound => 0x217;
    }
}
