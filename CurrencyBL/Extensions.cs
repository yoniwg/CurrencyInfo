using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyBL
{
    static class Extensions
    {
        /// <summary>
        /// Creates new observable that switches between obseravble provided by `f` on each
        /// value emmited by the this observable. In addition it ignores errors emmited by the switched 
        /// ibservables.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="obs"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static IObservable<R> SelectSwitchIgnoreErrors<T,R>(this IObservable<T> obs, Func<T, IObservable<R>> f)
        {
            return obs.SelectMany(t => f(t).Materialize())
                .Where(meterial => meterial.Kind == System.Reactive.NotificationKind.OnNext)
                .Select(meterial => meterial.Value);
        }

        /// <see cref="SelectSwitchIgnoreErrors{T, R}(IObservable{T}, Func{T, IObservable{R}})"/>
        public static IObservable<R> SelectSwitchElements<T,R>(this IObservable<T> obs, Func<T, Task<R>> f)
        {
            return SelectSwitchIgnoreErrors(obs, t => Observable.FromAsync(_ => f(t)));
        }

    }
}
