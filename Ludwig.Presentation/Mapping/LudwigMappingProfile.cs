using AutoMapper;
using Ludwig.DataAccess.Models;
using Ludwig.Presentation.Models;

namespace Ludwig.Presentation.Mapping
{
    public class LudwigMappingProfile:Profile
    {

        public LudwigMappingProfile()
        {
            CreateMap<UserStoryDal,UserStory>().ConvertUsing<UserStoryConversion>();
            
            CreateMap<UserStory,UserStoryDal>().ConvertUsing<UserStoryConversion>();
        }
    }
}