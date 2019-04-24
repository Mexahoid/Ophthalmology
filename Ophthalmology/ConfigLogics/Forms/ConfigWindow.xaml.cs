using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ophthalmology.ConfigLogics.Classes;

namespace Ophthalmology.ConfigLogics.Forms
{
    /// <summary>
    /// Логика взаимодействия для ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {
        private ConfigLogic _cfg;
        private ObservableCollection<string> _parameters;
        private string _rootFolder;
        private int _selectedIndex;

        public ConfigWindow()
        {
            InitializeComponent();
            DataContext = this;
            _cfg = ConfigLogic.Instance;
            if (_cfg.IsConfigPresent)
            {
                Tuple<string[], string> tpl = _cfg.GetParametersAndRoot();
                _parameters = new ObservableCollection<string>(tpl.Item1);
                _rootFolder = tpl.Item2;
                RootFolderTextBox.Text = _rootFolder;
                CheckData();
            }
            else
            {
                _parameters = new ObservableCollection<string>();
            }
            ParametersListListBox.ItemsSource = _parameters;
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
                _parameters.Add(ParameterNameTextBox.Text);
                ParameterNameTextBox.Text = "";
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
            if (!_cfg.IsConfigPresent)
            {
                _cfg.IsConfigPresent = true;
            }
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
            switch ((Selector.SelectedItem as TabItem)?.Tag)
            {
                case "Pars":
                    MainForm.MinHeight = 360;
                    MainForm.Height = 450;
                    MainForm.MaxHeight = 450;

                    MainForm.MinWidth = 550;
                    MainForm.Width = 800;
                    MainForm.MaxWidth = 800;
                    break;
                case "Image":
                    MainForm.MinHeight = 500;
                    MainForm.Height = 700;
                    MainForm.MaxHeight = 900;

                    MainForm.MinWidth = 900;
                    MainForm.Width = 1100;
                    MainForm.MaxWidth = 1200;
                    break;
                case "Report":
                    MainForm.MinHeight = 700;
                    MainForm.Height = 700;
                    MainForm.MaxHeight = 1300;

                    MainForm.MinWidth = 400;
                    MainForm.Width = 500;
                    MainForm.MaxWidth = 900;
                    break;
                default:
                    throw new Exception("Wtf");
            }
        }
    }
}
