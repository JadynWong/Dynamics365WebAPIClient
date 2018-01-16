using System;
using System.Collections.Generic;
using System.Text;

namespace D365WebApiClient.Standard.WebApiQueryOptions.Options.Filter
{
    public sealed class FilterExpression
    {
        private LogicalOperator _filterOperator;
        private IList<ConditionExpression> _conditions;
        private IList<FilterExpression> _filters;

        public FilterExpression()
        {
        }

        public FilterExpression(LogicalOperator filterOperator)
        {
            this.FilterOperator = filterOperator;
        }


        public LogicalOperator FilterOperator
        {
            get
            {
                return this._filterOperator;
            }
            set
            {
                this._filterOperator = value;
            }
        }

        public IList<ConditionExpression> Conditions
        {
            get
            {
                if (this._conditions == null)
                    this._conditions = new List<ConditionExpression>();
                return this._conditions;
            }
            private set
            {
                this._conditions = value;
            }
        }

        public IList<FilterExpression> Filters
        {
            get
            {
                if (this._filters == null)
                    this._filters = new List<FilterExpression>();
                return this._filters;
            }
            private set
            {
                this._filters = value;
            }
        }

        public void AddCondition(string attributeName, ConditionOperator conditionOperator, int value)
        {
            this.Conditions.Add(new ConditionExpression(attributeName, conditionOperator, value));
        }
        public void AddCondition(string attributeName, ConditionOperator conditionOperator, long value)
        {
            this.Conditions.Add(new ConditionExpression(attributeName, conditionOperator, value));
        }
        public void AddCondition(string attributeName, ConditionOperator conditionOperator, decimal value)
        {
            this.Conditions.Add(new ConditionExpression(attributeName, conditionOperator, value));
        }
        public void AddCondition(string attributeName, ConditionOperator conditionOperator, double value)
        {
            this.Conditions.Add(new ConditionExpression(attributeName, conditionOperator, value));
        }
        public void AddCondition(string attributeName, ConditionOperator conditionOperator, string value)
        {
            this.Conditions.Add(new ConditionExpression(attributeName, conditionOperator, value));
        }
        public void AddCondition(string attributeName, ConditionOperator conditionOperator, Guid value)
        {
            this.Conditions.Add(new ConditionExpression(attributeName, conditionOperator, value));
        }
        public void AddCondition(string attributeName, ConditionOperator conditionOperator, bool value)
        {
            this.Conditions.Add(new ConditionExpression(attributeName, conditionOperator, value));
        }
        public void AddCondition(string attributeName, ConditionOperator conditionOperator, DateTime value, bool onlyDate = false)
        {
            this.Conditions.Add(new ConditionExpression(attributeName, conditionOperator, value, onlyDate));
        }

        //public void AddCondition(string entityName, string attributeName, ConditionOperator conditionOperator, params object[] values)
        //{
        //    this.Conditions.Add(new ConditionExpression(entityName, attributeName, conditionOperator, values));
        //}

        public void AddCondition(ConditionExpression condition)
        {
            this.Conditions.Add(condition);
        }

        public FilterExpression AddFilter(LogicalOperator logicalOperator)
        {
            FilterExpression filterExpression = new FilterExpression();
            filterExpression.FilterOperator = logicalOperator;
            this.Filters.Add(filterExpression);
            return filterExpression;
        }

        public void AddFilter(FilterExpression childFilter)
        {
            if (childFilter == null)
                return;
            this.Filters.Add(childFilter);
        }

        public override string ToString()
        {

            if (_conditions.Count == 0)
                return string.Empty;
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("(");
            foreach (var condition in _conditions)
            {
                if (stringBuilder.Length != 1)
                {
                    switch (this.FilterOperator)
                    {
                        case LogicalOperator.LogicalAnd:
                            stringBuilder.Append(" and ");
                            break;
                        case LogicalOperator.LogicalOr:
                            stringBuilder.Append(" or ");
                            break;
                        case LogicalOperator.LogicalNegation:
                            stringBuilder.Append(" not ");
                            break;
                    }
                }
                stringBuilder.Append($"({condition})");
            }

            var filters = FilterToString(_filters);
            if (_filters.Count > 0)
            {
                switch (this.FilterOperator)
                {
                    case LogicalOperator.LogicalAnd:
                        stringBuilder.Append(" and ");
                        break;
                    case LogicalOperator.LogicalOr:
                        stringBuilder.Append(" or ");
                        break;
                    case LogicalOperator.LogicalNegation:
                        stringBuilder.Append(" not ");
                        break;
                }

                stringBuilder.Append(filters);
            }

            stringBuilder.Append(")");
            return stringBuilder.ToString();
        }

        public string FilterToString(IList<FilterExpression> filters)
        {
            if (Filters.Count == 0)
            {
                return null;
            }

            var stringBuilder = new StringBuilder();
            stringBuilder.Append("(");
            foreach (var filter in Filters)
            {
                stringBuilder.Append(filter);
            }
            stringBuilder.Append(")");
            return stringBuilder.ToString();
        }
    }
}
