using System.ComponentModel;
using System.Runtime.CompilerServices;
using MyGameEngine;
using static GameWithGraphicInterface.ArmyViewModel;

namespace GameWithGraphicInterface
{
    public class StackViewModel: INotifyPropertyChanged
    {
        public BattleUnitsStack stack { get; }
        public ArmyViewModel arm;
        
        private RelayCommand removeStackCommand;
        
        public RelayCommand RemoveStackCommand =>
            removeStackCommand ?? (removeStackCommand = new RelayCommand(o =>
            {
                var s = (StackViewModel) o;
                s.stack.Army.Remove(s.stack.Name);
                arm.OnPropertyChanged("Stacks");
            }));
        
        public StackViewModel(BattleUnitsStack stack, ArmyViewModel army)
        {
            this.stack = stack;
            arm = army;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}