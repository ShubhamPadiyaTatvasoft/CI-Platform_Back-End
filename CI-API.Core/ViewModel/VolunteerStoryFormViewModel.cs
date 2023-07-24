using Microsoft.AspNetCore.Http;

namespace CI_API.Core.ViewModel
{
    public class VolunteerStoryFormViewModel

    {
        public long UserId { get; set; }
        public long Id { get; set; }

        public long MissionId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string theme { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public IEnumerable<IFormFile> Images { get; set; } = Enumerable.Empty<IFormFile>();
        public long?[]? ToBeDeletedIds { get; set; }

        public string[]? VideoUrls { get; set; } = null;
    }
}
