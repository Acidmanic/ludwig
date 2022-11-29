using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ludwig.Contracts
{
    public interface IStorage
    {

        void Store<T>(T value) where T : class, new();

        IEnumerable<T> All<T>() where T : class, new();
        
        IEnumerable<T> Find<T>(Expression<Func<T,bool>> expression) where T : class, new();
        
    }
}