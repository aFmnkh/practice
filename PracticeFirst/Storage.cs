using System;
using System.Collections.Generic;
using System.Threading;

namespace Server
{
    public class Storage
    {
        public string Add(string[] path)
        {
            _lock.EnterWriteLock();
            try
            {
                foreach (var p in path)
                {
                    _list.Add(p);
                }
                return "Файлы загружены";

            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
        public string[] Get()
        {
            _lock.EnterReadLock();
            try
            {
                var strings = new string[_list.Count];
                for (var i = 0; i < strings.Length; i++)
                {
                    strings[i] = _list[i];
                }
                return strings;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private readonly List<string> _list = new List<string>();
    }
}