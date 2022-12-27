using AutoMapper;
using Ludwig.Contracts.Models;
using Ludwig.DataAccess.Models;
using Ludwig.Presentation.Models;

namespace Ludwig.Presentation.Mapping
{
    public class UserStoryConversion:ITypeConverter<UserStoryDal,UserStory>,ITypeConverter<UserStory,UserStoryDal>
    {
        public UserStory Convert(UserStoryDal source, UserStory destination, ResolutionContext context)
        {
            if (source == null)
            {
                return null;
            }

            if (destination == null)
            {
                destination = new UserStory();
            }

            destination = context.Mapper.Map<UserStory>(source);

            destination.Priority = new Priority
            {
                Name = source.PriorityName,
                Value = source.PriorityValue
            };

            return destination;
        }

        public UserStoryDal Convert(UserStory source, UserStoryDal destination, ResolutionContext context)
        {
            if (source == null)
            {
                return null;
            }

            if (destination == null)
            {
                destination = new UserStoryDal();
            }

            destination = context.Mapper.Map<UserStoryDal>(source);

            destination.PriorityName = source.Priority?.Name;

            destination.PriorityValue = source.Priority?.Value ?? Priority.Medium.Value;

            return destination;
        }
    }
}