using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyPL.ViewModels
{
    abstract class AbstractVM : INotifyPropertyChanged, INotifyDataErrorInfo
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;


        public bool HasErrors => throw new NotImplementedException();

        public IEnumerable GetErrors(string propertyName) => throw new NotImplementedException();

        private readonly IDictionary<string, object> values = new Dictionary<string, object>();

        protected void SetValue<T>(Expression<Func<T>> propertyGetter, T value, Action afterSet)
        {
            SetValue(propertyGetter, value);
            afterSet();
        }

        protected void SetValue<T>(Expression<Func<T>> propertyGetter, T value)
        {
            var propertyName = GetPropertyName(propertyGetter);
            values[propertyName] = value;
            NotifyPropertyChange(propertyName);
        }

        private void NotifyPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected T GetValue<T>(Expression<Func<T>> propertyGetter)
        {
            var propertyName = GetPropertyName(propertyGetter);
            object value = default(T);
            values.TryGetValue(propertyName, out value);
            return (T) value;
        }

        private string GetPropertyName(LambdaExpression expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new InvalidOperationException();
            }
            return memberExpression.Member.Name;
        }
    }
}
