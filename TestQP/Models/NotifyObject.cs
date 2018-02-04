using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TestQP
{
    public class NotifyObject : INotifyPropertyChanged
    {
        // Events
        public event PropertyChangedEventHandler PropertyChanged;

        // Methods
        protected void OnPropertyChanged<T>(Expression<Func<T>> property)
        {
            PropertyInfo member = (property.Body as MemberExpression).Member as PropertyInfo;
            if (null == member)
            {
                throw new ArgumentException("The lambda expression property should point to a valid Property");
            }
            this.OnPropertyChanged(member.Name);
        }

        //public void OnPropertyChanged(string propertyName)
        //{
        //    if (this.PropertyChanged != null)
        //    {
        //        this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
