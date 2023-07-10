using Microsoft.AspNetCore.Http;

namespace CI_API.Core.ViewModel
{
    public class VolunteerStoryFormViewModel
    {
        public long Id { get; set; }

        public long MissionId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string theme { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string[]? Images { get; set; } = null!;

        public long?[]? ToBeDeletedIds { get; set; }

        public string[]? VideoUrls { get; set; } = null;
    }
}
