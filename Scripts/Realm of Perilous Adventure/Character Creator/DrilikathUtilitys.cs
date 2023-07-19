using Server.Accounting;
using Server.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Server.Drilikath
{
    public static class DrilikathUtilitys
    {
        private static Dictionary<string, int> m_Skills;

		public static Dictionary<string, int> GetSkillsList(Mobile from)
        {
           if(m_Skills == null)
            {
                m_Skills = new Dictionary<string, int>();

                foreach(Skill skill in from.Skills)
                {
                    m_Skills.Add(skill.Name, skill.SkillID);
                }
            }
         
                return m_Skills;
        }

        public static Dictionary<string, int> GetRestrictedSkillsList(Mobile from) // skills minus magic skills.
        {

            List<string> exclude = new List<string> { "Bushido", "Chivalry", "Evaluating Intelligence", "Imbuing", "Inscription",
                "Magery", "Meditation", "Mysticism", "Necromancy", "Ninjitsu", "Spellweaving",
                "Spirit Speak", "Healing", "Stealth", "Animal Lore" };


            if (m_Skills == null)
            {
                m_Skills = new Dictionary<string, int>();

                foreach (Skill skill in from.Skills)
                {
                    if (!exclude.Contains(skill.Name))
                    m_Skills.Add(skill.Name, skill.SkillID);
                }
            }

            return m_Skills;
        }

        public static void Initialize()
        {

        }

        private const int RGB_MAX = 255; // Reduce this for a darker range
        private const int RGB_MIN = 0; // Increase this for a lighter range

        public static string RemoveCamelCase(string name)
        {
            return Regex.Replace(name, "(\\B[A-Z])", " $1");
        }
        public static string getColorProp(int cur, int max)
        {

            double Percent = (double)cur / max;
            if (Percent > 1)
                Percent = 1;
            int Current = (int)(Percent * 100);

            if (Current > 100)
                Current = 100;
            // Work out the percentage of red and green to use (i.e. a percentage
            // of the range from RGB_MIN to RGB_MAX)
            var redPercent = Math.Min(200 - (Current * 2), 100) / 100f;
            var greenPercent = Math.Min(Current * 2, 100) / 100f;

            // Now convert those percentages to actual RGB values in the range
            // RGB_MIN - RGB_MAX
            var red = RGB_MIN + ((RGB_MAX - RGB_MIN) * redPercent);
            var green = RGB_MIN + ((RGB_MAX - RGB_MIN) * greenPercent);

            if (red < 0)
            {
                red *= -1;
            }
            if (red > 255)
            {
                red = 255;
            }
            if (green < 0)
            {
                green *= -1;
            }
            if (green > 255)
            {
                green = 255;
            }
            // Return string with our property and color. 
            return "<BASEFONT COLOR=" + ColorTranslator.ToHtml(Color.FromArgb((int)red, (int)green, RGB_MIN)) + ">" + cur + "<BASEFONT COLOR=#FFFFFF>";

        }
        public static string getColorCurMax(int cur, int max)
        {

            double Percent = (double)cur / max;
            if (Percent > 1)
                Percent = 1;
            int Current = (int)(Percent * 100);

            if (Current > 100)
                Current = 100;
            // Work out the percentage of red and green to use (i.e. a percentage
            // of the range from RGB_MIN to RGB_MAX)
            var redPercent = Math.Min(200 - (Current * 2), 100) / 100f;
            var greenPercent = Math.Min(Current * 2, 100) / 100f;

            // Now convert those percentages to actual RGB values in the range
            // RGB_MIN - RGB_MAX
            var red = RGB_MIN + ((RGB_MAX - RGB_MIN) * redPercent);
            var green = RGB_MIN + ((RGB_MAX - RGB_MIN) * greenPercent);

            if (red < 0)
            {
                red *= -1;
            }
            if (red > 255)
            {
                red = 255;
            }
            if (green < 0)
            {
                green *= -1;
            }
            if (green > 255)
            {
                green = 255;
            }
            // Return string with our property and color. 
            return "<BASEFONT COLOR=" + ColorTranslator.ToHtml(Color.FromArgb((int)red, (int)green, RGB_MIN)) + ">" + cur + " / " + max + "<BASEFONT COLOR=#FFFFFF>";

        }
        public static string getColorProp(string name, string color)
        {
            // Return string with our property and color. 
            return "<BASEFONT COLOR=#" + color + ">" + name + "<BASEFONT COLOR=#FFFFFF>";
        }

        public static int GetBalance(Mobile m)
        {
            double balance = 0;

            if (AccountGold.Enabled && m.Account != null)
            {
                int goldStub;
                m.Account.GetGoldBalance(out goldStub, out balance);

                if (balance > Int32.MaxValue)
                {
                    return Int32.MaxValue;
                }
            }

            Container bank = m.FindBankNoCreate();

            if (bank != null)
            {
                var gold = bank.FindItemsByType<Gold>();
                var checks = bank.FindItemsByType<BankCheck>();

                balance += gold.Aggregate(0.0, (c, t) => c + t.Amount);
                balance += checks.Aggregate(0.0, (c, t) => c + t.Worth);
            }

            return (int)Math.Max(0, Math.Min(Int32.MaxValue, balance));
        }
        public static string AddCommas(int num)
        {
            return string.Format("{0:n0}", num);
        }


    }
}
