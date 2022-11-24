using System;
using System.Collections.Generic;
using System.Threading;

namespace Server
{
    public class Storage
    {
        public string Add(byte[] path)
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
        public byte[] Get()
        {
            _lock.EnterReadLock();
            try
            {
                var bytes = new byte[_list.Count];
                for (var i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = _list[i];
                }
                return bytes;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private readonly List<byte> _list = new List<byte>();
    }
}