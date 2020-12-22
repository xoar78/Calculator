using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Models
{
    class Expression
    {
        public string Exp { get; set; }
        public string Value { get; set; }
        public Expression(string expression, string result)
        {
            Exp = expression;
            Value = result;
        }
    }
}
