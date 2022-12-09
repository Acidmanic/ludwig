using System;
using System.Threading.Tasks;

namespace Ludwig.Common.Utilities
{

    public class LazyCacheRetryNulls<T> : LazyCache<T>
    {
        protected LazyCacheRetryNulls()
        {
            AcceptNulls = false;
        }
    }
    public class LazyCache<T>
    {
        private Func<T> _provider = () => default;

        private T _value;
        protected bool AcceptNulls = true;
        private bool _initiated = false;
        private readonly object _instanceLock = new object();
        private static readonly object SingletonLock = new object();
        private static LazyCache<T> _instance = null;

        protected LazyCache()
        {
        }


        public static LazyCache<T> Instance
        {
            get
            {
                lock (SingletonLock)
                {
                    if (_instance == null)
                    {
                        _instance = new LazyCache<T>();
                    }
                }

                return _instance;
            }
        }


        public void SetProvider(Func<T> provider)
        {
            lock (_instanceLock)
            {
                _provider = provider;
            }
        }

        public void SetProvider(Task<T> provider)
        {
            lock (_instanceLock)
            {
                _provider = () => provider.Result;
            }
        }

        public T Value
        {
            get
            {
                lock (_instanceLock)
                {
                    if (!_initiated)
                    {
                        _value = _provider();

                        _initiated = AcceptNulls || _value != null;
                    }

                    return _value;
                }
            }
        }
    }
}