using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics365WebApi.WebApiQueryOption.Options.Filter
{
    public sealed class ConditionExpression
    {
        private string _attributeName;

        private ConditionOperator _conditionOperator;

        //private IList<object> _values;
        private object _value;

        //private string _entityName;
        private bool _onlyDate;

        public ConditionExpression()
        {
        }

        public ConditionExpression(string attributeName, ConditionOperator conditionOperator, int value)
        {
            Create(attributeName, conditionOperator, value);
        }

        public ConditionExpression(string attributeName, ConditionOperator conditionOperator, long value)
        {
            Create(attributeName, conditionOperator, value);
        }

        public ConditionExpression(string attributeName, ConditionOperator conditionOperator, double value)
        {
            Create(attributeName, conditionOperator, value);
        }

        public ConditionExpression(string attributeName, ConditionOperator conditionOperator, decimal value)
        {
            Create(attributeName, conditionOperator, value);
        }

        public ConditionExpression(string attributeName, ConditionOperator conditionOperator, string value)
        {
            Create(attributeName, conditionOperator, value);
        }

        public ConditionExpression(string attributeName, ConditionOperator conditionOperator, DateTime value,
            bool onlyDate = false)
        {
            OnlyDate = onlyDate;
            Create(attributeName, conditionOperator, value);
        }

        public ConditionExpression(string attributeName, ConditionOperator conditionOperator, bool value)
        {
            Create(attributeName, conditionOperator, value);
        }

        public ConditionExpression(string attributeName, ConditionOperator conditionOperator, Guid value)
        {
            Create(attributeName, conditionOperator, value);
        }

        private void Create(string attributeName, ConditionOperator conditionOperator, object value)
        {
            this._attributeName = attributeName;
            this._conditionOperator = conditionOperator;
            this._value = value;
        }

        public string AttributeName
        {
            get { return this._attributeName; }
            set { this._attributeName = value; }
        }

        public ConditionOperator Operator
        {
            get { return this._conditionOperator; }
            set { this._conditionOperator = value; }
        }

        public object Value
        {
            get
            {
                if (this._value == null)
                    this._value = new object();
                return this._value;
            }
            private set { this._value = value; }
        }

        public bool OnlyDate
        {
            get { return this._onlyDate; }
            set { this._onlyDate = value; }
        }

        public override string ToString()
        {
            string value;

            if (Value == null)
            {
                value = "null";
            }
            else
            {
                var valueType = Value.GetType();
                if (valueType == typeof(string))
                {
                    value = string.IsNullOrWhiteSpace(Value as string) ? "''" : $"'{Value}'";
                }
                else if (valueType == typeof(int) || valueType == typeof(double) || valueType == typeof(decimal) ||
                         valueType == typeof(long))
                {
                    value = $"{Value}";
                }
                else if (valueType == typeof(Guid))
                {
                    value = $"{Value:D}";
                }
                else if (valueType == typeof(DateTime))
                {
                    var dateTime = ((DateTime) Value);
                    if (OnlyDate)
                    {
                        value = $"{Value:yyyy-MM-dd}";
                    }
                    else
                    {
                        dateTime = dateTime.ToUniversalTime();
                        value = $"{Value:yyyy-MM-ddTHH:mm:ssZ}";
                    }
                }
                else if (valueType == typeof(bool))
                {
                    var bools = (bool) Value;
                    value = bools ? "true" : "false";
                }
                else
                {
                    throw new ArgumentException($"不支持的类型,{valueType.FullName}", nameof(Value));
                }
            }

            switch (this.Operator)
            {
                case ConditionOperator.Equal:
                    return $"{AttributeName} eq {value}";
                case ConditionOperator.NotEqual:
                    return $"{AttributeName} ne {value}";
                case ConditionOperator.GreaterThan:
                    return $"{AttributeName} gt {value}";
                case ConditionOperator.GreaterThanOrQqual:
                    return $"{AttributeName} ge {value}";
                case ConditionOperator.LessThan:
                    return $"{AttributeName} lt {value}";
                case ConditionOperator.LessThanOrQqual:
                    return $"{AttributeName} le {value}";
                case ConditionOperator.StartsWith:
                    return $"startswith({AttributeName},'({value})')";
                case ConditionOperator.EndsWith:
                    return $"endswith({AttributeName},'({value})')";
                case ConditionOperator.Contains:
                    return $"contains({AttributeName},'({value})')";
                default:
                    throw new ArgumentException($"不支持的操作,{this.Operator}", $"Operator");
            }
        }
    }
}