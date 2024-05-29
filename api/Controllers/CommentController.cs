using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api;

[Route("api/comment")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepo;
    private readonly IStockRepository _stockRepo;
    public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
    {
        _commentRepo = commentRepo;
        _stockRepo = stockRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(){
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var comments = await _commentRepo.GetAllAsync();
        var commentDto = comments.Select(x => x.ToCommentDto());
        
        return Ok(commentDto);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id){
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var commentModel = await _commentRepo.GetByIdAsync(id);

        if(commentModel == null){
            return NotFound();
        }

        return Ok(commentModel.ToCommentDto());
    }

    [HttpPost("{stockId:int}")]
    public async Task<IActionResult> Create ([FromRoute] int stockId, [FromBody] CreateCommentRequestDto commentDto){
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if(!await _stockRepo.StockExists(stockId)){
            return BadRequest("Stock does not exist");
        }
        var commentModel = commentDto.ToCommentFromCreate(stockId);
        await _commentRepo.CreateAsync(commentModel);
        
        return CreatedAtAction(
            nameof(GetById),
            new { id = commentModel.Id }, // Route values for generating the URI
            commentModel.ToCommentDto()); // Comment data transfer object as the response body
    }

    [HttpPut]
    [Route("{commentId:int}")]
    public async Task<IActionResult> Update([FromRoute] int commentId, [FromBody] UpdateCommentRequestDto commentDto){
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

         var comment = await _commentRepo.UpdateAsync(commentId, commentDto.ToCommentFromUpdate());

         if(comment == null){
            return NotFound("Comment not found");
         }

         return Ok(comment?.ToCommentDto());

    }

    [HttpDelete]
    [Route("{commentId:int}")]
    public async Task<IActionResult> Delete([FromRoute] int commentId){
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var comment = await _commentRepo.DeleteAsync(commentId);
        if (comment == null){
            return NotFound(" id not found");
        }

        return Ok(comment.ToCommentDto());
    }
}
