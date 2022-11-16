using System.Linq;
using EnTier.UnitOfWork;
using Ludwig.Presentation.Contracts;
using Ludwig.Presentation.Models;

namespace Ludwig.Presentation.Services
{
    public class DatabaseExporterV1:IDatabaseExporter
    {

        private readonly IUnitOfWork _unitOfWork;

        public DatabaseExporterV1(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public object Export()
        {
            var database = new DatabaseV1
            {
                Stories = _unitOfWork.GetCrudRepository<UserStory,long>().All().ToList(),
                StoryUsers = _unitOfWork.GetCrudRepository<StoryUser,long>().All().ToList(),
                Version = 1
            };

            return database;
        }
    }
}