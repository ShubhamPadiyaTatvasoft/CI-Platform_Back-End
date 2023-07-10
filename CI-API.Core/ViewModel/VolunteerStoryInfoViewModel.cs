namespace CI_API.Core.ViewModel
{
    public class VolunteerStoryInfoViewModel
    {
        public long Id { get; set; }

        public long MissionId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Theme { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public IEnumerable<StoryMediaInfoViewModel>? StoryMediaInfoList { get; set; }
    }
}
