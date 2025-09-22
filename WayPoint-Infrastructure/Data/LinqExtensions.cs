using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WayPoint_Infrastructure.Data
{
    public static class LinqExtensions
    {
            public static IEnumerable<TResult> LeftJoin<TLeft, TRight, TKey, TResult>(
                this IEnumerable<TLeft> left,
                IEnumerable<TRight> right,
                Func<TLeft, TKey> leftKeySelector,
                Func<TRight, TKey> rightKeySelector,
                Func<TLeft, TRight?, TResult> resultSelector)
            {
                return from l in left
                       join r in right on leftKeySelector(l) equals rightKeySelector(r) into temp
                       from r in temp.DefaultIfEmpty()
                       select resultSelector(l, r);
            }
    }
}
