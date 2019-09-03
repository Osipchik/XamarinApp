using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Labs.ViewModels
{
    public class PickerViewModel
    {
        public List<PickerView> Measurements { get; set; }

        public PickerViewModel()
        {
            Measurements = GetMeasurements().OrderBy(t => t.Value).ToList();
        }

        public List<PickerView> GetMeasurements()
        {
            var measurements = new List<PickerView>()
            {
                new PickerView(){Key = 1, Value = "Seconds", Maximum = 120},
                new PickerView(){Key = 2, Value = "Minutes", Maximum = 180},
                new PickerView(){Key = 3, Value = "Hours", Maximum = 5}
            };

            return measurements;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private PickerView _selectedMeasurement { get; set; }
        public PickerView SelectedMeasurement
        {
            get { return _selectedMeasurement; }
            set
            {
                if (_selectedMeasurement != value)
                {
                    _selectedMeasurement = value;
                }
            }
        }

        private string _myMeasurement { get; set; }
        public string MyMeasurement
        {
            get { return _myMeasurement; }
            set
            {
                if (_myMeasurement != value)
                {
                    _myMeasurement = value;
                    OnPropertyChanged();
                }
            }
        }
    }

    public class PickerView
    {
        public int Key { get; set; }
        public string Value { get; set; }
        public int Maximum { get; set; }
    }
}
