using api.Data;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDBContext _context;
    public CommentRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<Comment> CreateAsync(Comment commentModel)
    {
        await _context.Comments.AddAsync(commentModel);
        await _context.SaveChangesAsync();
        return commentModel;
    }

    public async Task<Comment?> DeleteAsync(int id)
    {
        var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
        if(comment == null){
            return null;
        }
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
        
        return comment;
    }

    public async Task<List<Comment>> GetAllAsync()
    {
        var stockModel = await _context.Comments.ToListAsync();
        
        return stockModel;
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        var commentModel = await _context.Comments.FindAsync(id);
        
        return commentModel;
    }

    public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
    {
        var existingComment = await _context.Comments.FindAsync(id);
        if(existingComment == null){
            return null;
        }

        existingComment.Title = commentModel.Title;
        existingComment.Content = commentModel.Content;

        await _context.SaveChangesAsync();

        return existingComment;

    }

    
}
