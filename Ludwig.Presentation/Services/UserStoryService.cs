using System;
using System.Collections.Generic;
using System.Linq;
using EnTier.Repositories;
using EnTier.Repositories.Attributes;
using EnTier.Results;
using EnTier.UnitOfWork;
using Ludwig.Contracts.IssueManagement;
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

        private readonly IIssueManager _issueManager;
        //private readonly Jira _jira;


        public UserStoryService(IUnitOfWork unitOfWork, IIssueManager issueManager)
        {
            _unitOfWork = unitOfWork;
            _issueManager = issueManager;

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
            var issues = _issueManager.GetIssuesByUserStory(item.Title).Result;

            item.Issues = issues;
        }


        [KeepProperty(typeof(Priority))]
        public UserStory Add(UserStory value)
        {
            if (value.Priority == null)
            {
                value.Priority = Priority.Medium;
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


        private void WriteFullTree(UserStory value)
        {
            var insertee = Clone(value);

            if (insertee.StoryUser != null)
            {
                var incomingUser = insertee.StoryUser;

                var foundUser = _storyUserRepository.Find(u =>
                        u.Name != null && incomingUser.Name != null
                                       && u.Name.Trim().ToLower() == incomingUser.Name.Trim().ToLower())
                    .FirstOrDefault();

                StoryUser usingUser = null;
                
                if (foundUser!=null)
                {
                    usingUser = foundUser;
                }
                else
                {
                    //abandon last user in db, and create new user with given name
                    usingUser = _storyUserRepository.Add(insertee.StoryUser);
                }
                
                if (usingUser != null)
                {
                    insertee.StoryUser = usingUser;
                    insertee.StoryUserId = usingUser.Id;

                    _userStoryRepository.Update(insertee);
                }
            }
        }


        private void ReadFullTree(UserStory value)
        {
            value.StoryUser = _storyUserRepository.GetById(value.StoryUserId);

            ReadIssuesInto(value);

            if (value.Priority == null)
            {
                value.Priority = Priority.Medium;
            }
        }

        [KeepProperty(typeof(Priority))]
        public UserStory Update(UserStory value)
        {
            if (value.Priority == null)
            {
                value.Priority = Priority.Medium;
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