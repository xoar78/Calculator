using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Controls;
using Calculator.Interfaces;
using Calculator.Models;
using Calculator;

namespace Calculator
{
    class ViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IHistory History { get; }
        public IMemory Mem { get; }

        public ViewModel()
        {
            //History = new HistoryDB();
            //History = new HistoryInFile();
            History = new HistoryInRAM();
            //Mem = new MemoryDB();
            //Mem = new MemoryInFile();
            Mem = new MemoryInRAM();
        }

        private string textValue = "";
        public string TextValue
        {
            get => textValue;
            set
            {
                textValue = value;
                OnPropertyChanged(nameof(TextValue));
            }
        }

        private bool isComma = false;

        public ICommand AddToMemory
        {
            get => new RelayCommand(() =>
            {
                Mem.Add(TextValue);
            }, () => TextValue.Length > 0);
        }

        public ICommand SumMem
        {
            get => new RelayCommand(() =>
            {
                string result = ParserCalculator.Calculate(TextValue);
                if (result == "Error")
                    result = "";
                TextValue = result;
                Mem.Increase(Mem.Count - 1, TextValue);
            }, () => string.IsNullOrEmpty(TextValue) == false && Mem.Any());
        }

        public ICommand SubMem
        {
            get => new RelayCommand(() =>
            {
                string result = ParserCalculator.Calculate(TextValue);
                if (result == "Error")
                    result = "";
                TextValue = result;

                Mem.Decrease(Mem.Count - 1, TextValue);
            }, () => string.IsNullOrEmpty(TextValue) == false && Mem.Any());
        }

        public ICommand RemoveFromMemory
        {
            get => new RelayCommand(() =>
            {
                Mem.RemoveAtIndex(Mem.Count - 1);
            }, () => Mem.Any());
        }

        public ICommand TakeMemory
        {
            get => new RelayCommand<TextBox>((x) =>
            {
                TextValue = x.Text;
            }, x => true);
        }

        public ICommand AddNumber
        {
            get => new RelayCommand<string>(x =>
            {
                if ("+-*/".IndexOf(x) != -1 && "+-*/".IndexOf(TextValue[TextValue.Length - 1]) != -1)
                    return;
                if (x == "," && TextValue[TextValue.Length - 1] == ',')
                    return;
                if (x == "," && isComma == true)
                    return;
                if (x == ",")
                    isComma = true;
                TextValue += x;
                if ("+-*/".IndexOf(x) != -1)
                    isComma = false;
            }, x => true);
        }

        public ICommand Calculate
        {
            get => new RelayCommand<string>(x =>
            {
                if (TextValue == "") return;
                string result = ParserCalculator.Calculate(TextValue);
                if (TextValue != result)
                    History.Add(new Calculator.Models.Expression(textValue + " = ", result));
                if (result == "Error")
                    result = "";
                TextValue = result;
                isComma = false;
            }, x => ParserCalculator.Valid(TextValue));
        }

        public ICommand Brackets
        {
            get => new RelayCommand<string>(x =>
            {
                TextValue += x;
            }, x => true);
        }

        public ICommand Back
        {
            get => new RelayCommand(() =>
            {
                if (TextValue == "") return;
                if (TextValue[TextValue.Length - 1] == ',')
                    isComma = false;
                TextValue = TextValue.Remove(TextValue.Length - 1);
            }, () => true);
        }

        public ICommand Clear
        {
            get => new RelayCommand(() =>
            {
                TextValue = "";
                isComma = false;
            }, () => true);
        }

        public ICommand ClearAll
        {
            get => new RelayCommand(() =>
            {
                Mem.Clear();
                History.Clear();
            }, () => Mem.Values.Count > 0 || History.Values.Count > 0);
        }

        public Dictionary<string, string> ErrorCollection { get; private set; } = new Dictionary<string, string>();
        public string Error => null;

        public string this[string name]
        {
            get
            {
                string messageError = null;
                if (name == "TextValue")
                    if (!ParserCalculator.Valid(TextValue))
                        messageError = "Error";
                if (ErrorCollection.ContainsKey(name))
                    ErrorCollection[name] = messageError;
                else if (messageError != null)
                    ErrorCollection.Add(name, messageError);
                OnPropertyChanged(nameof(ErrorCollection));
                return messageError;
            }
        }
    }
}
