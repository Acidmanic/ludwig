using System.Collections.Generic;
using System.Linq;
using EnTier.Repositories;
using EnTier.Results;
using EnTier.UnitOfWork;
using Ludwig.Presentation.Contracts;
using Ludwig.Presentation.Models;

namespace Ludwig.Presentation.Services
{
    public class UserStoryService:IUserStoryService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICrudRepository<UserStory, long> _userStoryRepository;
        private readonly ICrudRepository<StoryUser, long> _storyUserRepository;
        
        public UserStoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _userStoryRepository = unitOfWork.GetCrudRepository<UserStory, long>();
            _storyUserRepository = unitOfWork.GetCrudRepository<StoryUser, long>();
        }


        
        
        public IEnumerable<UserStory> GetAll()
        {
            var all =  _userStoryRepository.All();

            foreach (var userStorey in all)
            {
                ReadFullTree(userStorey);
            }

            return all;
        }

        public UserStory GetById(long id)
        {
            var item =  _userStoryRepository.GetById(id);
            
            ReadFullTree(item);

            return item;
        }

        public UserStory Add(UserStory value)
        {
            var inserted = _userStoryRepository.Add(value);

            WriteFullTree(inserted);

            _unitOfWork.Complete();
            
            return inserted;
        }

        private void WriteFullTree(UserStory inserted)
        {
            if (inserted.StoryUser != null)
            {
                var foundUser = FindUser(inserted.StoryUser);

                StoryUser user = null;
                
                if (foundUser)
                {
                     user = foundUser.Primary;
                     
                }else if (foundUser.Secondary)
                {
                    user = _storyUserRepository.Add(foundUser.Primary);
                }

                if (user != null)
                {
                    inserted.StoryUser = user;
                    inserted.StoryUserId = user.Id;

                    _userStoryRepository.Update(inserted);
                }
            }
        }

        private Result<StoryUser,bool> FindUser(StoryUser insertedStoryUser)
        {
            if (insertedStoryUser != null && !string.IsNullOrEmpty(insertedStoryUser.Name) && !string.IsNullOrWhiteSpace(insertedStoryUser.Name) )
            {
                if (insertedStoryUser.Id > 0)
                {
                    var user = _storyUserRepository.GetById(insertedStoryUser.Id);

                    if (user != null)
                    {
                        return new Result<StoryUser,bool>(true,false,user);
                    }
                }
                else
                {
                    var user = _storyUserRepository.Find(u => 
                        u.Name!=null && insertedStoryUser.Name !=null 
                        && u.Name.Trim().ToLower()== insertedStoryUser.Name.Trim().ToLower())
                        .FirstOrDefault();

                    if (user != null)
                    {
                        return new Result<StoryUser,bool>(true,false,user);
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
            
        }

        public UserStory Update(UserStory value)
        {
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