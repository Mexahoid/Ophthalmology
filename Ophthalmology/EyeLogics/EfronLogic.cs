using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Ophthalmology.EyeLogics
{
    class EfronLogic
    {
        private readonly List<int> _diagnosis;
        private Dictionary<string, List<string>> _textedDiags;
        private List<string> _diagsItself;

        private string _currentDiag;
        private int _currentTextStage;
        private int _currentStage;

        public string CurrentDiag
        {
            set
            {
                _currentDiag = value;
                int ind = _diagsItself.IndexOf(_currentDiag);
                if (ind > -1)
                {
                    CurrentStage = _diagnosis[ind];
                }
            }
        }

        public int CurrentStage
        {
            get => _currentStage;
            set
            {
                _currentStage = value;
                int ind = _diagsItself.IndexOf(_currentDiag);
                if (ind > -1)
                {
                    _diagnosis[ind] = _currentStage;
                }
            }
        }

        public EfronLogic(List<int> curr)
        {
            FillTexts();
            if (curr == null)
            {
                _diagnosis = new List<int>();
                // 16 диагнозов, надо сразу инитить
                for (int i = 0; i < 16; i++)
                {
                    _diagnosis.Add(0);
                }
            }
            else
            {
                _diagnosis = curr;
            }
            CurrentDiag = _diagsItself[0];
            CurrentStage = _diagnosis[0];
        }

        public Tuple<List<string>, List<int>> GetDiagnosisTuple()
        {
            return Tuple.Create(_diagsItself, _diagnosis);
        }


        public List<string> GetDiagnosis(bool showNulls)
        {
            List<string> diags = new List<string>();
            for (int i = 0; i < 16; i++)
            {
                if (!showNulls && _diagnosis[i] == 0)
                    continue;
                diags.Add($"{_diagsItself[i]}: {_diagnosis[i]}");
            }

            return diags;
        }

        public void SwitchStage(bool next)
        {
            if (next)
            {
                if (_currentTextStage < 4)
                    _currentTextStage++;
            }
            else
            {
                if (_currentTextStage > 0)
                    _currentTextStage--;
            }
        }

        public string GetLink()
        {
            var path = Directory.GetCurrentDirectory();
            return $"{path}\\images\\{_diagsItself.IndexOf(_currentDiag)}{_currentTextStage}.jpg";
        }


        public string GetCurrentStageText()
        {
            return _textedDiags[_currentDiag][_currentTextStage];
        }

        private void FillTexts()
        {
            _textedDiags = new Dictionary<string, List<string>>();
            _diagsItself = new List<string>();

            List<string> stages = new List<string>
            {
                Properties.Resources.B0,
                Properties.Resources.B1,
                Properties.Resources.B2,
                Properties.Resources.B3,
                Properties.Resources.B4
            };
            _textedDiags.Add(Properties.Resources.B, stages);
            _diagsItself.Add(Properties.Resources.B);

            stages = new List<string>
            {
                Properties.Resources.Mgd0,
                Properties.Resources.Mgd1,
                Properties.Resources.Mgd2,
                Properties.Resources.Mgd3,
                Properties.Resources.Mgd4
            };
            _textedDiags.Add(Properties.Resources.Mgd, stages);
            _diagsItself.Add(Properties.Resources.Mgd);
            stages = new List<string>
            {
                Properties.Resources.Slk0,
                Properties.Resources.Slk1,
                Properties.Resources.Slk2,
                Properties.Resources.Slk3,
                Properties.Resources.Slk4
            };
            _textedDiags.Add(Properties.Resources.Slk, stages);
            _diagsItself.Add(Properties.Resources.Slk);
            stages = new List<string>
            {
                Properties.Resources.Ci0,
                Properties.Resources.Ci1,
                Properties.Resources.Ci2,
                Properties.Resources.Ci3,
                Properties.Resources.Ci4
            };
            _textedDiags.Add(Properties.Resources.Ci, stages);
            _diagsItself.Add(Properties.Resources.Ci);
            stages = new List<string>
            {
                Properties.Resources.Cu0,
                Properties.Resources.Cu1,
                Properties.Resources.Cu2,
                Properties.Resources.Cu3,
                Properties.Resources.Cu4
            };
            _textedDiags.Add(Properties.Resources.Cu, stages);
            _diagsItself.Add(Properties.Resources.Cu);
            stages = new List<string>
            {
                Properties.Resources.Ep0,
                Properties.Resources.Ep1,
                Properties.Resources.Ep2,
                Properties.Resources.Ep3,
                Properties.Resources.Ep4
            };
            _textedDiags.Add(Properties.Resources.Ep, stages);
            _diagsItself.Add(Properties.Resources.Ep);
            stages = new List<string>
            {
                Properties.Resources.Eb0,
                Properties.Resources.Eb1,
                Properties.Resources.Eb2,
                Properties.Resources.Eb3,
                Properties.Resources.Eb4
            };
            _textedDiags.Add(Properties.Resources.Eb, stages);
            _diagsItself.Add(Properties.Resources.Eb);
            stages = new List<string>
            {
                Properties.Resources.Cd0,
                Properties.Resources.Cd1,
                Properties.Resources.Cd2,
                Properties.Resources.Cd3,
                Properties.Resources.Cd4
            };
            _textedDiags.Add(Properties.Resources.Cd, stages);
            _diagsItself.Add(Properties.Resources.Cd);
            stages = new List<string>
            {
                Properties.Resources.Cr0,
                Properties.Resources.Cr1,
                Properties.Resources.Cr2,
                Properties.Resources.Cr3,
                Properties.Resources.Cr4
            };
            _textedDiags.Add(Properties.Resources.Cr, stages);
            _diagsItself.Add(Properties.Resources.Cr);
            stages = new List<string>
            {
                Properties.Resources.Lr0,
                Properties.Resources.Lr1,
                Properties.Resources.Lr2,
                Properties.Resources.Lr3,
                Properties.Resources.Lr4
            };
            _textedDiags.Add(Properties.Resources.Lr, stages);
            _diagsItself.Add(Properties.Resources.Lr);
            stages = new List<string>
            {
                Properties.Resources.Cn0,
                Properties.Resources.Cn1,
                Properties.Resources.Cn2,
                Properties.Resources.Cn3,
                Properties.Resources.Cn4
            };
            _textedDiags.Add(Properties.Resources.Cn, stages);
            _diagsItself.Add(Properties.Resources.Cn);
            stages = new List<string>
            {
                Properties.Resources.Em0,
                Properties.Resources.Em1,
                Properties.Resources.Em2,
                Properties.Resources.Em3,
                Properties.Resources.Em4
            };
            _textedDiags.Add(Properties.Resources.Em, stages);
            _diagsItself.Add(Properties.Resources.Em);
            stages = new List<string>
            {
                Properties.Resources.Co0,
                Properties.Resources.Co1,
                Properties.Resources.Co2,
                Properties.Resources.Co3,
                Properties.Resources.Co4
            };
            _textedDiags.Add(Properties.Resources.Co, stages);
            _diagsItself.Add(Properties.Resources.Co);
            stages = new List<string>
            {
                Properties.Resources.Cs0,
                Properties.Resources.Cs1,
                Properties.Resources.Cs2,
                Properties.Resources.Cs3,
                Properties.Resources.Cs4
            };
            _textedDiags.Add(Properties.Resources.Cs, stages);
            _diagsItself.Add(Properties.Resources.Cs);
            stages = new List<string>
            {
                Properties.Resources.Cst0,
                Properties.Resources.Cst1,
                Properties.Resources.Cst2,
                Properties.Resources.Cst3,
                Properties.Resources.Cst4
            };
            _textedDiags.Add(Properties.Resources.Cst, stages);
            _diagsItself.Add(Properties.Resources.Cst);
            stages = new List<string>
            {
                Properties.Resources.Pc0,
                Properties.Resources.Pc1,
                Properties.Resources.Pc2,
                Properties.Resources.Pc3,
                Properties.Resources.Pc4
            };
            _textedDiags.Add(Properties.Resources.Pc, stages);
            _diagsItself.Add(Properties.Resources.Pc);




        }

    }
}
