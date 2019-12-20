using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using MyGameEngine;

namespace GameWithGraphicInterface
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private Dictionary<string, Army> armys;
        public static Dictionary<string, BattleUnitsStack> stacks;
        private readonly XmlFormatter xml;
        private const string directory = @"C:\2kurs1sem\OOP\Saves";

        public ObservableCollection<ArmyViewModel> Armys
        {
            get
            {
                var armyViewModels = new ObservableCollection<ArmyViewModel>();
                foreach (var armyViewModel in armys.Values.Select(army => new ArmyViewModel(army)))
                {
                    armyViewModel.ArmyChange += c;
                    armyViewModels.Add(armyViewModel);
                }

                return armyViewModels;
            }
        }

        private void c()
        {
            curScale = new Scale(stacks.Values);
            OnPropertyChanged("SelectedStack");
            OnPropertyChanged("CurScale");
        }

        private static Dictionary<string, Unit> units = new Dictionary<string, Unit>();
        public ObservableCollection<Unit> Units => new ObservableCollection<Unit>(units.Values);

        private static Scale curScale;
        public ObservableCollection<BattleUnitsStack> CurScale => new ObservableCollection<BattleUnitsStack>(curScale.Stacks);

        public BattleUnitsStack SelectedStack => curScale.Stacks.Any() ? curScale.GetStack() : null;

        private RelayCommand addArmyCommand;

        public RelayCommand AddArmyCommand =>
            addArmyCommand ?? (addArmyCommand = new RelayCommand(o =>
            {
                var army = (Army) o;
                if (armys.ContainsKey(army.Name))
                {
                    MessageBox.Show("Армия с таким названием уже существует");
                    return;
                }

                armys.Add(army.Name, army);
                OnPropertyChanged("Armys");
            }));

        private RelayCommand newGameCommand;

        public RelayCommand NewGameCommand =>
            newGameCommand ?? (newGameCommand = new RelayCommand(o =>
            {
                armys.Clear();
                stacks.Clear();
                OnPropertyChanged("Armys");
                c();
            }));

        private RelayCommand loadGameCommand;

        public RelayCommand LoadGameCommand =>
            loadGameCommand ?? (loadGameCommand = new RelayCommand(o =>
            {
                (armys, stacks) = xml.Open(directory, (string) o, units);
                OnPropertyChanged("Armys");
                c();
            }));

        private RelayCommand saveGameCommand;

        public RelayCommand SaveGameCommand =>
            saveGameCommand ?? (saveGameCommand = new RelayCommand(o =>
            {
                xml.Save(directory, (string) o, armys.Values);
                MessageBox.Show("Игра успешно сохранена");
            }));

        public ApplicationViewModel()
        {
            armys = new Dictionary<string, Army>();
            stacks = new Dictionary<string, BattleUnitsStack>();
            xml = new XmlFormatter();
            units = Loader.GetUnits(@"C:\2kurs1sem\OOP", "Units.db");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}