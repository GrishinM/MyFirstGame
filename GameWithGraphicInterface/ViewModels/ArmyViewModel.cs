using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using MyGameEngine;
using static GameWithGraphicInterface.ApplicationViewModel;

namespace GameWithGraphicInterface
{
    public class ArmyViewModel : INotifyPropertyChanged
    {
        public delegate void MyHandler();

        public event MyHandler ArmyChange;

        public Army Army { get; }

        public ObservableCollection<BattleUnitsStack> Stacks
        {
            get
            {
                var stacks = new ObservableCollection<BattleUnitsStack>(Army.Stacks);
//                foreach (var stack in Army.Stacks)
//                {
//                    stacks.Add(new StackViewModel(stack, this));
//                }

                return stacks;
            }
        }

        private RelayCommand addStackCommand;

        public RelayCommand AddStackCommand =>
            addStackCommand ?? (addStackCommand = new RelayCommand(o =>
            {
                var (item1, item2, item3) = ((object, int, string)) o;
                var stack = new BattleUnitsStack((Unit) item1, item2, item3);
                if (stacks.ContainsKey(stack.Name))
                {
                    MessageBox.Show("Стек с таким названием уже существует");
                    return;
                }

                if (Army.Count == 6)
                {
                    MessageBox.Show("В этой армии уже максимальное количество стеков");
                    return;
                }

                Army.Add(stack);
                stacks.Add(stack.Name, stack);
                OnPropertyChanged("Stacks");
                ArmyChange?.Invoke();
            }));

        private RelayCommand removeStackCommand;

        public RelayCommand RemoveStackCommand =>
            removeStackCommand ?? (removeStackCommand = new RelayCommand(o =>
            {
                var stack = (BattleUnitsStack) o;
                Army.Remove(stack.Name);
                stacks.Remove(stack.Name);
                OnPropertyChanged("Stacks");
                ArmyChange?.Invoke();
            }));

        public ArmyViewModel(Army army)
        {
            Army = army;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}