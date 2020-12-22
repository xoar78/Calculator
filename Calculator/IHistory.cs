using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Linq;
using Calculator.Models;

namespace Calculator.Interfaces
{
    interface IHistory
    {
        ObservableCollection<Expression> Values { get; }
        void Add(Expression expression);
        void Clear();
    }
}
