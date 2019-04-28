using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophthalmology.EyeLogics
{
    /// <summary>
    /// Синглтон DiagnosiTextHolder
    /// </summary>
    public class DiagnosiTextHolder
    {
        /// <summary>
        /// Экземпляр синглтона DiagnosiTextHolder
        /// </summary>
        private static DiagnosiTextHolder _instance;

        /// <summary>
        /// Возвращает экземпляр синглтона класса DiagnosiTextHolder.
        /// </summary>
        public static DiagnosiTextHolder Instance => _instance ?? (_instance = new DiagnosiTextHolder());

        public Dictionary<string, List<string>> TextedDiags { get; private set; }

        public List<string> DiagsItself { get; private set; }

        /// <summary>
        /// Закрытый конструктор класса.
        /// </summary>
        private DiagnosiTextHolder()
        {
            FillTexts();
        }

        private void FillTexts()
        {
            TextedDiags = new Dictionary<string, List<string>>();
            DiagsItself = new List<string>();

            List<string> stages = new List<string>
            {
                Properties.Resources.B0,
                Properties.Resources.B1,
                Properties.Resources.B2,
                Properties.Resources.B3,
                Properties.Resources.B4
            };
            TextedDiags.Add(Properties.Resources.B, stages);
            DiagsItself.Add(Properties.Resources.B);

            stages = new List<string>
            {
                Properties.Resources.Mgd0,
                Properties.Resources.Mgd1,
                Properties.Resources.Mgd2,
                Properties.Resources.Mgd3,
                Properties.Resources.Mgd4
            };
            TextedDiags.Add(Properties.Resources.Mgd, stages);
            DiagsItself.Add(Properties.Resources.Mgd);
            stages = new List<string>
            {
                Properties.Resources.Slk0,
                Properties.Resources.Slk1,
                Properties.Resources.Slk2,
                Properties.Resources.Slk3,
                Properties.Resources.Slk4
            };
            TextedDiags.Add(Properties.Resources.Slk, stages);
            DiagsItself.Add(Properties.Resources.Slk);
            stages = new List<string>
            {
                Properties.Resources.Ci0,
                Properties.Resources.Ci1,
                Properties.Resources.Ci2,
                Properties.Resources.Ci3,
                Properties.Resources.Ci4
            };
            TextedDiags.Add(Properties.Resources.Ci, stages);
            DiagsItself.Add(Properties.Resources.Ci);
            stages = new List<string>
            {
                Properties.Resources.Cu0,
                Properties.Resources.Cu1,
                Properties.Resources.Cu2,
                Properties.Resources.Cu3,
                Properties.Resources.Cu4
            };
            TextedDiags.Add(Properties.Resources.Cu, stages);
            DiagsItself.Add(Properties.Resources.Cu);
            stages = new List<string>
            {
                Properties.Resources.Ep0,
                Properties.Resources.Ep1,
                Properties.Resources.Ep2,
                Properties.Resources.Ep3,
                Properties.Resources.Ep4
            };
            TextedDiags.Add(Properties.Resources.Ep, stages);
            DiagsItself.Add(Properties.Resources.Ep);
            stages = new List<string>
            {
                Properties.Resources.Eb0,
                Properties.Resources.Eb1,
                Properties.Resources.Eb2,
                Properties.Resources.Eb3,
                Properties.Resources.Eb4
            };
            TextedDiags.Add(Properties.Resources.Eb, stages);
            DiagsItself.Add(Properties.Resources.Eb);
            stages = new List<string>
            {
                Properties.Resources.Cd0,
                Properties.Resources.Cd1,
                Properties.Resources.Cd2,
                Properties.Resources.Cd3,
                Properties.Resources.Cd4
            };
            TextedDiags.Add(Properties.Resources.Cd, stages);
            DiagsItself.Add(Properties.Resources.Cd);
            stages = new List<string>
            {
                Properties.Resources.Cr0,
                Properties.Resources.Cr1,
                Properties.Resources.Cr2,
                Properties.Resources.Cr3,
                Properties.Resources.Cr4
            };
            TextedDiags.Add(Properties.Resources.Cr, stages);
            DiagsItself.Add(Properties.Resources.Cr);
            stages = new List<string>
            {
                Properties.Resources.Lr0,
                Properties.Resources.Lr1,
                Properties.Resources.Lr2,
                Properties.Resources.Lr3,
                Properties.Resources.Lr4
            };
            TextedDiags.Add(Properties.Resources.Lr, stages);
            DiagsItself.Add(Properties.Resources.Lr);
            stages = new List<string>
            {
                Properties.Resources.Cn0,
                Properties.Resources.Cn1,
                Properties.Resources.Cn2,
                Properties.Resources.Cn3,
                Properties.Resources.Cn4
            };
            TextedDiags.Add(Properties.Resources.Cn, stages);
            DiagsItself.Add(Properties.Resources.Cn);
            stages = new List<string>
            {
                Properties.Resources.Em0,
                Properties.Resources.Em1,
                Properties.Resources.Em2,
                Properties.Resources.Em3,
                Properties.Resources.Em4
            };
            TextedDiags.Add(Properties.Resources.Em, stages);
            DiagsItself.Add(Properties.Resources.Em);
            stages = new List<string>
            {
                Properties.Resources.Co0,
                Properties.Resources.Co1,
                Properties.Resources.Co2,
                Properties.Resources.Co3,
                Properties.Resources.Co4
            };
            TextedDiags.Add(Properties.Resources.Co, stages);
            DiagsItself.Add(Properties.Resources.Co);
            stages = new List<string>
            {
                Properties.Resources.Cs0,
                Properties.Resources.Cs1,
                Properties.Resources.Cs2,
                Properties.Resources.Cs3,
                Properties.Resources.Cs4
            };
            TextedDiags.Add(Properties.Resources.Cs, stages);
            DiagsItself.Add(Properties.Resources.Cs);
            stages = new List<string>
            {
                Properties.Resources.Cst0,
                Properties.Resources.Cst1,
                Properties.Resources.Cst2,
                Properties.Resources.Cst3,
                Properties.Resources.Cst4
            };
            TextedDiags.Add(Properties.Resources.Cst, stages);
            DiagsItself.Add(Properties.Resources.Cst);
            stages = new List<string>
            {
                Properties.Resources.Pc0,
                Properties.Resources.Pc1,
                Properties.Resources.Pc2,
                Properties.Resources.Pc3,
                Properties.Resources.Pc4
            };
            TextedDiags.Add(Properties.Resources.Pc, stages);
            DiagsItself.Add(Properties.Resources.Pc);
        }

    }
}
