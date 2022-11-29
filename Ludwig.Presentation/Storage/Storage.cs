using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using EnTier.UnitOfWork;
using Ludwig.Contracts;

namespace Ludwig.Presentation.Storage
{
    public class Storage:IStorage
    {

        private int lastId;
        private object _locker = new object();
        private List<object> _objects = new List<object>();


        public Storage()
        {
            
        }


        public void Store<T>(T value) where T : class, new()
        {
            this._objects.Add(value);
        }

        public IEnumerable<T> All<T>()where T : class, new()
        {
            var result = new List<T>();

            foreach (object o in _objects)
            {
                if (o is T t)
                {
                    result.Add(t);
                }
            }

            return result;
        }

        public IEnumerable<T> Find<T>(Expression<Func<T, bool>> expression)where T : class, new()
        {
            var result = new List<T>();

            var func = expression.Compile();

            foreach (object o in _objects)
            {
                if (o is T t)
                {
                    if (func(t))
                    {
                        result.Add(t);
                    }
                }
            }

            return result;
        }
    }
}