using api.Models;

namespace api;

public class UpdateCommentRequestDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

}
