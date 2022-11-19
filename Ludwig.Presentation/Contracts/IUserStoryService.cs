using System;
using EnTier.Services;
using Ludwig.Presentation.Models;
using Microsoft.AspNetCore.Http;

namespace Ludwig.Presentation.Contracts
{
    public interface IUserStoryService:ICrudService<UserStory,long>
    {
        
    }
}