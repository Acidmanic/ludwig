using EnTier.UnitOfWork;

namespace ApiEmbassy.Services
{
    public class Embassy
    {

        private readonly IUnitOfWork _unitOfWork;

        public Embassy(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ClientAmbassador GetAmbassador()
        {
            return new ClientAmbassador(_unitOfWork);
        }
    }
}