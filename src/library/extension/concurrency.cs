using System;
using System.Collections.Generic;

namespace OdinSdk.BaseLib.Extension
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class CExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static SynchronizedCollection<T> AddRange<T>(this SynchronizedCollection<T> @this, IEnumerable<T> items)
        {
            lock (@this.SyncRoot)
            {
                foreach (var _item in items)
                    @this.Add(_item);

                return @this;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static int RemoveAll<T>(this SynchronizedCollection<T> @this, Predicate<T> match)
        {
            var _result = 0;

            lock (@this.SyncRoot)
            {
                var _size = @this.Count;

                var _count = _size;
                for (var _offset = 0; _offset < _count; _offset++)
                {
                    if (match(@this[_offset]) == true)
                    {
                        @this.RemoveAt(_offset);

                        _count--;
                        _offset--;
                    }
                }

                _result = _size - _count;
            }

            return _result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="item"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static bool UpdateOrInsert<T>(this SynchronizedCollection<T> @this, T item, Predicate<T> match)
        {
            var _result = false;

            lock (@this.SyncRoot)
            {
                for (var _offset = 0; _offset < @this.Count; _offset++)
                {
                    if (match(@this[_offset]) == true)
                    {
                        @this[_offset] = item;
                        _result = true;

                        break;
                    }
                }

                if (_result == false)
                    @this.Add(item);
            }

            return _result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static bool Exists<T>(this SynchronizedCollection<T> @this, Predicate<T> match)
        {
            var _result = false;

            lock (@this.SyncRoot)
            {
                foreach (var _items in @this)
                {
                    if (match(_items) == true)
                    {
                        _result = true;
                        break;
                    }
                }
            }

            return _result;
        }
    }
}