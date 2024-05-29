using api.Models;

namespace api;

public interface ICommentRepository
{
    Task<List<Comment>> GetAllAsync();
    Task<Comment?> GetByIdAsync(int id);
    Task<Comment> CreateAsync(Comment CommentModel);
    Task<Comment?> UpdateAsync(int id,Comment commentModel);
    Task<Comment?> DeleteAsync(int id);
}
