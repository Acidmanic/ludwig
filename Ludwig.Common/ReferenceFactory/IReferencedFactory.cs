using System;
using System.Collections.Generic;

namespace Ludwig.Common.ReferenceFactory
{

    public interface ITypeReference
    {
        List<Type> AvailableImplementations { get; }
    }
    
    public interface IReferencedFactory<TRef,TArg>:ITypeReference
    {
        
        TRef Make(TArg args);
        
    }
}