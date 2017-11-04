using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyPL
{
    static class ReactiveUtils
    {

        public static IReactiveProperty<T> NewReactiveProperty<T>() => new ReactiveProperty<T>();

    }
    
}
