using AutoMapper;
using Ludwig.Contracts.Models;
using Ludwig.DataAccess.Models;
using Ludwig.Presentation.Models;

namespace Ludwig.Presentation.Mapping
{
    public class UserStoryConversion : ITypeConverter<UserStoryDal, UserStory>, ITypeConverter<UserStory, UserStoryDal>
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

            destination.Id = source.Id;
            destination.Title = source.Title;
            destination.CardColor = source.CardColor;
            destination.StoryBenefit = source.StoryBenefit;
            destination.StoryFeature = source.StoryFeature;
            destination.IsDone = source.IsDone;
            destination.StoryUser = source.StoryUser == null ? null : context.Mapper.Map<StoryUser>(source.StoryUser);
            destination.StoryUserId = source.StoryUserId;
            
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

            destination.Id = source.Id;
            destination.Title = source.Title;
            destination.CardColor = source.CardColor;
            destination.StoryBenefit = source.StoryBenefit;
            destination.StoryFeature = source.StoryFeature;
            destination.IsDone = source.IsDone;
            destination.StoryUser = source.StoryUser == null ? null : context.Mapper.Map<StoryUser>(source.StoryUser);
            destination.StoryUserId = source.StoryUserId;

            destination.PriorityName = source.Priority?.Name;

            destination.PriorityValue = source.Priority?.Value ?? Priority.Medium.Value;

            return destination;
        }
    }
}