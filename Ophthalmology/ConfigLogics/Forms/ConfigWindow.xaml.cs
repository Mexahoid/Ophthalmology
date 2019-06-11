using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Ophthalmology.ConfigLogics.Classes;

namespace Ophthalmology.ConfigLogics.Forms
{
    /// <summary>
    /// Логика взаимодействия для ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {
        private ConfigLogic _cfg;
        private readonly ObservableCollection<string> _parameters;
        private string _rootFolder;
        private int _selectedIndex;
        private int _fontSize = 14;
        private FontFamily _font = new FontFamily("Times New Roman");
        private readonly ObservableCollection<string> _names;

        private class TagObj
        {
            public string Alias { get; set; }
            public string Parameter { get; set; }
        }

        private List<TagObj> _tagobjs;

        public ConfigWindow()
        {
            InitializeComponent();
            DataContext = this;
            DocumentTemplate.FontFamily = _font;
            DocumentTemplate.FontSize = _fontSize;
            Paragraph p = DocumentTemplate.Document.Blocks.FirstBlock as Paragraph;
            p.LineHeight = 10;
            //FontBox.ItemsSource = Fonts.SystemFontFamilies;
            //FontBox.Text = "Times New Roman";

            _cfg = ConfigLogic.Instance;
            _tagobjs = new List<TagObj>();
            _names = new ObservableCollection<string>();

            var tn = _cfg.GetTemplateNames();
            foreach (string s in tn)
            {
                _names.Add(s);
            }

            TemplatesListbox.ItemsSource = _names;

            if (_cfg.IsConfigPresent)
            {
                Tuple<string[], string> tpl = _cfg.GetParametersAndRoot();
                _parameters = new ObservableCollection<string>(tpl.Item1);
                //

                var aliases = _cfg.GetTemplateAliases();

                foreach (var alias in aliases)
                {
                    _tagobjs.Add(new TagObj
                    {
                        Parameter = alias[1],
                        Alias = alias[0]
                    });
                }
                //
                _rootFolder = tpl.Item2;
                RootFolderTextBox.Text = _rootFolder;
                CheckData();
            }
            else
            {
                _parameters = new ObservableCollection<string>();
            }
            ParametersListListBox.ItemsSource = _parameters;
            ParametersDataGrid.ItemsSource = _tagobjs;
        }

        private void CheckData()
        {
            string ButtonText = "";

            if (_parameters.Count != 0)
            {
                _cfg.ParametersTyped = true;
            }
            else
            {
                _cfg.ParametersTyped = false;
                ButtonText += "Отсутствуют параметры. ";
            }

            if (!string.IsNullOrEmpty(_rootFolder))
            {
                _cfg.RootFolderTyped = true;
                if (_cfg.ParametersTyped)
                {
                    SaveButton.IsEnabled = true;
                    ButtonText = "Сохранить";
                }
            }
            else
            {
                _cfg.RootFolderTyped = false;
                ButtonText += "Не выбрана корневая папка.";
            }

            SaveButton.Content = ButtonText;
        }

        private void AddChangeParameterButton_Click(object sender, RoutedEventArgs e)
        {
            if (_cfg.IsAdding)
            {
                if(!_parameters.Contains(ParameterNameTextBox.Text))
                {
                    _parameters.Add(ParameterNameTextBox.Text);
                    ParameterNameTextBox.Text = "";
                }
            }
            else
            {
                _parameters[_selectedIndex] = ParameterNameTextBox.Text;
            }
            CheckData();
        }

        private void SelectRootFolderButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog ofd = new System.Windows.Forms.FolderBrowserDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _rootFolder = ofd.SelectedPath;
                RootFolderTextBox.Text = _rootFolder;
            }
            CheckData();
        }

        private void RemoveParameterButton_Click(object sender, RoutedEventArgs e)
        {
            _parameters.RemoveAt(_selectedIndex);
            _selectedIndex = -1;
            ParameterNameTextBox.Text = "";
            CheckData();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _cfg.CreateConfig(_parameters.ToArray(), _rootFolder);
            if (_cfg.IsConfigPresent)
                return;
            _cfg.IsConfigPresent = true;
            DialogResult = true;
        }

        private void ParametersListListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ParametersListListBox.SelectedIndex < 0)
            {
                return;
            }
            _selectedIndex = ParametersListListBox.SelectedIndex;
            ParameterNameTextBox.Text = _parameters[_selectedIndex];
            RemoveParameterButton.IsEnabled = true;
            AddChangeParameterButton.Content = "Изменить";
            _cfg.IsAdding = false;
        }

        private void CancelParameterButton_OnClick(object sender, RoutedEventArgs e)
        {
            ParametersListListBox.SelectedIndex = -1;
            _selectedIndex = -1;
            RemoveParameterButton.IsEnabled = false;
            AddChangeParameterButton.Content = "Добавить";
            ParameterNameTextBox.Text = "";
            _cfg.IsAdding = true;
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((Selector.SelectedItem as TabItem)?.Tag is "Pars")
            {
                MainForm.MinHeight = 360;
                //MainForm.Height = 450;
                MainForm.MaxHeight = 450;

                MainForm.MinWidth = 550;
                //MainForm.Width = 800;
                MainForm.MaxWidth = 800;
            }
            else
            {
                MainForm.MinHeight = 700;
                //MainForm.Height = 700;
                MainForm.MaxHeight = 1700;

                MainForm.MinWidth = 500;
                //MainForm.Width = 650;
                MainForm.MaxWidth = 1300;
            }
        }

        private void NudUp_Click(object sender, RoutedEventArgs e)
        {
            if (_fontSize < 100)
                _fontSize++;
            //FontSizeTB.Text = _fontSize.ToString();
            Update();
        }

        private void NudDown_Click(object sender, RoutedEventArgs e)
        {
            if (_fontSize > 2)
                _fontSize--;
            //FontSizeTB.Text = _fontSize.ToString();
            Update();
        }

        private void FontBox_DropDownClosed(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(FontBox.Text))
                //return;
            //_font = new FontFamily(FontBox.Text);
            Update();
        }

        private void Update()
        {
            var target = DocumentTemplate;

            if (target == null)
                return;

            // Make sure we have a selection. Should have one even if there is no text selected.
            // Check whether there is text selected or just sitting at cursor
            if (target.Selection.IsEmpty)
            {
                // Check to see if we are at the start of the textbox and nothing has been added yet
                if (target.Selection.Start.Paragraph == null)
                {
                    // Add a new paragraph object to the richtextbox with the fontsize
                    Paragraph p = new Paragraph
                    {
                        FontSize = _fontSize,
                        FontFamily = _font
                    };
                    target.Document.Blocks.Add(p);
                }
                else
                {
                    // Get current position of cursor
                    TextPointer curCaret = target.CaretPosition;
                    // Get the current block object that the cursor is in
                    Block curBlock = target.Document.Blocks.FirstOrDefault(x => x.ContentStart.CompareTo(curCaret) == -1 && x.ContentEnd.CompareTo(curCaret) == 1);
                    if (curBlock != null)
                    {
                        Paragraph curParagraph = curBlock as Paragraph;
                        // Create a new run object with the fontsize, and add it to the current block
                        Run newRun = new Run
                        {
                            FontSize = _fontSize,
                            FontFamily = _font
                        };
                        curParagraph?.Inlines.Add(newRun);
                        // Reset the cursor into the new block. 
                        // If we don't do this, the font size will default again when you start typing.
                        target.CaretPosition = newRun.ElementStart;
                    }
                }
            }
            else // There is selected text, so change the fontsize of the selection
            {
                TextRange selectionTextRange = new TextRange(target.Selection.Start, target.Selection.End);
                selectionTextRange.ApplyPropertyValue(TextElement.FontSizeProperty, _fontSize.ToString());
                selectionTextRange.ApplyPropertyValue(TextElement.FontFamilyProperty, _font);
            }
        }

        private void UpdateTemplatesList()
        {
            _names.Clear();

            var tn = _cfg.GetTemplateNames();
            foreach (string s in tn)
            {
                _names.Add(s);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string name = TemplateNameTextbox.Text;

            TextRange tr = new TextRange(
                DocumentTemplate.Document.ContentStart, DocumentTemplate.Document.ContentEnd);

            ConfigLogic.Instance.AddTemplate(name, tr);
            UpdateTemplatesList();
        }

        private void SaveTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            var on = TemplatesListbox.SelectedItems[0].ToString();
            var nn = TemplateNameTextbox.Text;
            TextRange tr = new TextRange(
                DocumentTemplate.Document.ContentStart, DocumentTemplate.Document.ContentEnd);

            ConfigLogic.Instance.EditTemplate(on, nn, tr);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            ConfigLogic.Instance.DeleteTemplate(TemplateNameTextbox.Text);
            DocumentTemplate.Document.Blocks.Clear();
            TemplateNameTextbox.Text = "";
            UpdateTemplatesList();
            DeleteButton.IsEnabled = false;
        }

        private void TemplatesListbox_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (TemplatesListbox.SelectedItems.Count < 1)
                return;
            var si = TemplatesListbox.SelectedItems[0];
            if (si == null)
                return;
            var n = si.ToString();

            TextRange tr = new TextRange(
                DocumentTemplate.Document.ContentStart, DocumentTemplate.Document.ContentEnd);

            ConfigLogic.Instance.LoadTemplate(n, tr);
            TemplateNameTextbox.Text = n;

            DeleteButton.IsEnabled = true;
        }
    }
}
