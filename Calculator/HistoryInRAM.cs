using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Calculator.Interfaces;

namespace Calculator.Models
{
    class HistoryInRAM : IHistory
    {
        public ObservableCollection<Expression> Values { get; }
        public HistoryInRAM()
        {
            Values = new ObservableCollection<Expression>();
        }
        public void Add(Expression expression)
        {
            Values.Add(expression);
        }
        public void Clear()
        {
            if (Values.Count > 0)
                Values.Clear();
        }
    }
}
