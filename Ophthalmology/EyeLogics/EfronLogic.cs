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

        private readonly Dictionary<string, List<string>> _textedDiags;
        private readonly List<string> _diagsItself;

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
            var th = DiagnosiTextHolder.Instance;
            _textedDiags = th.TextedDiags;
            _diagsItself = th.DiagsItself;

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
    }
}
