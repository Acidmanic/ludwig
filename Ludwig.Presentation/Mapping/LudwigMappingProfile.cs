using AutoMapper;
using Ludwig.Contracts.Models;
using Ludwig.DataAccess.Models;
using Ludwig.Presentation.Authentication;
using Ludwig.Presentation.Models;

namespace Ludwig.Presentation.Mapping
{
    public class LudwigMappingProfile:Profile
    {

        public LudwigMappingProfile()
        {
            CreateMap<UserStory,UserStory>();
            
            CreateMap<UserStoryDal,UserStory>().ConvertUsing<UserStoryConversion>();
            
            CreateMap<RequestUpdate,RequestUpdateDal>().ReverseMap();
            
            CreateMap<AuthorizationRecord,AuthorizationRecordDal>().ReverseMap();
            
            CreateMap<UserStory,UserStoryDal>().ConvertUsing<UserStoryConversion>();
        }
    }
}