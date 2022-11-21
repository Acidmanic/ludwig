using System;
using System.Collections.Generic;
using System.Linq;
using EnTier.Repositories;
using EnTier.Repositories.Attributes;
using EnTier.Results;
using EnTier.UnitOfWork;
using Ludwig.Contracts.Models;
using Ludwig.Presentation.Contracts;
using Ludwig.Presentation.Extensions;
using Ludwig.Presentation.Models;
using Microsoft.AspNetCore.Http;

namespace Ludwig.Presentation.Services
{
    public class UserStoryService : IUserStoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICrudRepository<UserStory, long> _userStoryRepository;
        private readonly ICrudRepository<StoryUser, long> _storyUserRepository;
        private readonly Jira _jira;


        public UserStoryService(IUnitOfWork unitOfWork, Jira jira)
        {
            _unitOfWork = unitOfWork;
            _jira = jira;

            _userStoryRepository = unitOfWork.GetCrudRepository<UserStory, long>();
            _storyUserRepository = unitOfWork.GetCrudRepository<StoryUser, long>();
        }


        public IEnumerable<UserStory> GetAll()
        {
            var all = _userStoryRepository.All();

            foreach (var userStorey in all)
            {
                ReadFullTree(userStorey);
            }

            return all;
        }

        public UserStory GetById(long id)
        {
            var item = _userStoryRepository.GetById(id);

            ReadFullTree(item);

            return item;
        }

        private void ReadIssuesInto(UserStory item)
        {
            var issues = _jira.IssuesByUserStory(item.Title).Result;

            issues.ForEach(i => i.Fields.Clear());

            item.Issues = issues;
        }


        [KeepProperty(typeof(Priority))]
        public UserStory Add(UserStory value)
        {

            if (value.Priority == null)
            {
                value.Priority=Priority.Medium;
            }
            
            var inserted = _userStoryRepository.Add(value);

            WriteFullTree(inserted);

            _unitOfWork.Complete();

            return inserted;
        }


        private UserStory Clone(UserStory value)
        {
            return new UserStory
            {
                Id = value.Id,
                Issues = new List<JiraIssue>(),
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


        private void WriteFullTree(UserStory value)
        {
            var insertee = Clone(value);
            
            if (insertee.StoryUser != null)
            {
                var foundUser = FindUser(insertee.StoryUser);

                StoryUser user = null;

                if (foundUser)
                {
                    user = foundUser.Primary;
                }
                else if (foundUser.Secondary)
                {
                    user = _storyUserRepository.Add(foundUser.Primary);
                }

                if (user != null)
                {
                    insertee.StoryUser = user;
                    insertee.StoryUserId = user.Id;

                    _userStoryRepository.Update(insertee);
                }
            }
        }

        private Result<StoryUser, bool> FindUser(StoryUser insertedStoryUser)
        {
            if (insertedStoryUser != null && !string.IsNullOrEmpty(insertedStoryUser.Name) &&
                !string.IsNullOrWhiteSpace(insertedStoryUser.Name))
            {
                if (insertedStoryUser.Id > 0)
                {
                    var user = _storyUserRepository.GetById(insertedStoryUser.Id);

                    if (user != null)
                    {
                        return new Result<StoryUser, bool>(true, false, user);
                    }
                }
                else
                {
                    var user = _storyUserRepository.Find(u =>
                            u.Name != null && insertedStoryUser.Name != null
                                           && u.Name.Trim().ToLower() == insertedStoryUser.Name.Trim().ToLower())
                        .FirstOrDefault();

                    if (user != null)
                    {
                        return new Result<StoryUser, bool>(true, false, user);
                    }
                }

                return new Result<StoryUser, bool>(false, true, insertedStoryUser);
            }
            else
            {
                return new Result<StoryUser, bool>(false, false, null);
            }
        }


        private void ReadFullTree(UserStory value)
        {
            value.StoryUser = _storyUserRepository.GetById(value.StoryUserId);

            ReadIssuesInto(value);
            
            if (value.Priority == null)
            {
                value.Priority=Priority.Medium;
            }
        }

        [KeepProperty(typeof(Priority))]
        public UserStory Update(UserStory value)
        {
            if (value.Priority == null)
            {
                value.Priority=Priority.Medium;
            }
            
            var updated = _userStoryRepository.Update(value);

            WriteFullTree(updated);

            _unitOfWork.Complete();

            return updated;
        }

        public UserStory Update(long id, UserStory value)
        {
            value.Id = id;

            return Update(value);
        }

        public bool Remove(UserStory value)
        {
            return RemoveById(value.Id);
        }

        public bool RemoveById(long id)
        {
            return _userStoryRepository.Remove(id);
        }
        
    }
}