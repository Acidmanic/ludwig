using System.Collections.Generic;
using System.Linq;
using EnTier;
using EnTier.Repositories;
using EnTier.Repositories.Attributes;
using EnTier.Services;
using EnTier.UnitOfWork;
using Ludwig.Contracts.IssueManagement;
using Ludwig.Contracts.Models;
using Ludwig.DataAccess.Contracts.Repositories;
using Ludwig.DataAccess.Models;
using Ludwig.Presentation.Contracts;
using Ludwig.Presentation.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Ludwig.Presentation.Services
{
    public class UserStoryService : CrudService<UserStory, UserStoryDal, long, long>, IUserStoryService
    {
        private readonly IIssueManager _issueManager;

        public UserStoryService(EnTierEssence essence, IIssueManager issueManager) : base(essence)
        {
            _issueManager = issueManager;
        }
        // private ILogger Logger { get; set; } = NullLogger.Instance;


        // public UserStoryService(IUnitOfWork unitOfWork, IIssueManager issueManager)
        // {
        //     _unitOfWork = unitOfWork;
        //     _issueManager = issueManager;
        //
        //     _userStoryRepository = unitOfWork.GetCrudRepository<UserStory, long>();
        //     _storyUserRepository = unitOfWork.GetCrudRepository<StoryUser, long>();
        // }


        public override IEnumerable<UserStory> GetAll()
        {
            var all = base.GetAll().ToArray();

            foreach (var userStorey in all)
            {
                ReadFullTree(userStorey);
            }

            return all;
        }

        public override UserStory GetById(long id)
        {
            var item = base.GetById(id);

            ReadFullTree(item);

            return item;
        }

        private void ReadIssuesInto(UserStory item)
        {
            var issues = _issueManager.GetIssuesByUserStory(item.Title).Result;

            item.Issues = issues;
        }


        [KeepProperty(typeof(Priority))]
        public override UserStory Add(UserStory value)
        {
            if (value.Priority == null)
            {
                value.Priority = Priority.Medium;
            }

            WriteFullTreeChildren(value);
            
            var inserted = base.Add(value);

            value.Id = inserted.Id;
            
            return value;
        }


        private UserStory Clone(UserStory value)
        {
            return new UserStory
            {
                Id = value.Id,
                Issues = new List<Issue>(),
                Title = value.Title,
                CardColor = value.CardColor,
                StoryBenefit = value.StoryBenefit,
                StoryFeature = value.StoryFeature,
                StoryUser = new StoryUser
                {
                    Id = value.StoryUser.Id,
                    Name = value.StoryUser?.Name
                },
                StoryUserId = value.StoryUserId,
                Priority = new Priority
                {
                    Name = value.Priority.Name,
                    Value = value.Priority.Value
                }
            };
        }


        private void WriteFullTreeChildren(UserStory value)
        {

            if (value.StoryUser != null)
            {
                var sUserRepository = UnitOfWork.GetCrudRepository<StoryUser, long>();

                var incomingUser = new StoryUser
                {
                    Id = 0,
                    Name = value.StoryUser.Name
                };

                var foundUser = sUserRepository.Set(incomingUser);

                if (foundUser.Id != value.StoryUserId)
                {
                    value.StoryUser = foundUser;
                
                    value.StoryUserId = foundUser.Id;    
                }
                
                UnitOfWork.Complete();
            }
        }


        private void ReadFullTree(UserStory value)
        {
            var repository = UnitOfWork.GetCrudRepository<StoryUser, long>();
            
            value.StoryUser = repository.GetById(value.StoryUserId);

            ReadIssuesInto(value);

            if (value.Priority == null)
            {
                value.Priority = Priority.Medium;
            }
        }

        [KeepProperty(typeof(Priority))]
        public override UserStory Update(UserStory value)
        {
            if (value.Priority == null)
            {
                value.Priority = Priority.Medium;
            }

            WriteFullTreeChildren(value);
            
            var updated = base.Update(value);
            
            return updated;
        }

        public override UserStory Update(long id, UserStory value)
        {
            value.Id = id;

            return Update(value);
        }

        public override bool Remove(UserStory value)
        {
            return RemoveById(value.Id);
        }
    }
}