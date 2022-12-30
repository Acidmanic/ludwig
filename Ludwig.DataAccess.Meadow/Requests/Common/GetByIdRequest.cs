using Ludwig.DataAccess.Meadow.Models;
using Meadow.Requests;

namespace Ludwig.DataAccess.Meadow.Requests.Common
{
    public class GetByIdRequest<TEntity, TId>:MeadowRequest<IdShell<TId>,TEntity> where TEntity : class, new()
    {
        private readonly bool _fullTree;
        
        public GetByIdRequest(TId id, bool fullTree) : base(true)
        {
            _fullTree = fullTree;
            
            // ReSharper disable once VirtualMemberCallInConstructor
            ToStorage = new IdShell<TId>
            {
                Id = id
            };
        }

        protected override bool FullTreeReadWrite()
        {
            return _fullTree;
        }
    }
}