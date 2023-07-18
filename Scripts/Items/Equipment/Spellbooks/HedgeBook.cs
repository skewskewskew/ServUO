namespace Server.Items
{
    public class HedgeBook : Spellbook
    {
        [Constructable]
        public HedgeBook()
            : this((ulong)0)
        {
        }

        [Constructable]
        public HedgeBook(ulong content)
            : base(content, 0x2D9D)
        {
        }

        public HedgeBook(Serial serial)
            : base(serial)
        {
        }

        public override SpellbookType SpellbookType => SpellbookType.Mystic;
        public override int BookOffset => 800;
        public override int BookCount => 64;

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }
}