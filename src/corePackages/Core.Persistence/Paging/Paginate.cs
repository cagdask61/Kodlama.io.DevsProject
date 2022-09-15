using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persistence.Paging
{
    public class Paginate<T> : IPaginate<T>
    {

        internal Paginate(IEnumerable<T> source, int index, int size, int from)
        {
            var enumerable = source as T[] ?? source.ToArray();

            if (from > index)
                throw new ArgumentException($"indexForm: {from} > pageIndex: {index}, must indexForm <= pageIndex");

            if (source is IQueryable<T> querable)
            {
                Index = index;
                Size = size;
                From = from;
                Count = querable.Count();
                Pages = (int)Math.Ceiling(Count / (double)Size);

                Items = querable.Skip((Index - From) * Size).Take(Size).ToList();
            }
            else
            {
                Index = index;
                Size = size;
                From = from;
                Count = enumerable.Count();
                Pages = (int)Math.Ceiling(Count / (double)Size);

                Items = enumerable.Skip((Index - From) * Size).Take(Size).ToList();
            }

        }

        internal Paginate()
        {
            //Items = Array.Empty<T>();
            Items = new T[0];

        }

        public int From { get; set; }

        public int Index { get; set; }

        public int Size { get; set; }

        public int Count { get; set; }

        public int Pages { get; set; }

        public IList<T> Items { get; set; }

        public int CurrentCount => Items.Count;

        public bool HasPrevious => Index - From > 0;

        public bool HasNext => Index - From + 1 < Pages;
    }

    internal class Paginate<TSource, TResult> : IPaginate<TResult>
    {

        public Paginate(IEnumerable<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> converter, int index, int size, int from)
        {

            var enumerable = source as TSource[] ?? source.ToArray();

            if (from > index)
                throw new ArgumentException($"Form: {from} > Index: {index}, must From <= Index");

            if (source is IQueryable<TSource> querable)
            {
                Index = index;
                Size = size;
                From = from;
                Count = querable.Count();
                Pages = (int)Math.Ceiling(Count / (double)Size);

                var items = querable.Skip((Index - From) * Size).Take(Size).ToArray();

                Items = new List<TResult>(converter(items));
            }
            else
            {
                Index = index;
                Size = size;
                From = from;
                Count = enumerable.Count();
                Pages = (int)Math.Ceiling(Count / (double)Size);

                var items = enumerable.Skip((Index - From) * Size).Take(Size).ToArray();

                Items = new List<TResult>(converter(items));
            }
        }

        public Paginate(IPaginate<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> converter)
        {
            Index = source.Index;
            Size = source.Size;
            From = source.From;
            Count = source.Count;
            Pages = source.Pages;

            Items = new List<TResult>(converter(source.Items));
        }

        public int From { get; }

        public int Index { get; }

        public int Size { get; }

        public int Count { get; }

        public int Pages { get; }

        public IList<TResult> Items { get; }

        public int CurrentCount => Items.Count;

        public bool HasPrevious => Index - From > 0;

        public bool HasNext => Index - From + 1 < Pages;
    }

    public static class Paginate
    {
        public static IPaginate<T> Empty<T>()
        {
            return new Paginate<T>();
        }

        public static IPaginate<TResult> From<TResult, TSource>(IPaginate<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> converter)
        {
            return new Paginate<TSource, TResult>(source, converter);
        }

    }
}
