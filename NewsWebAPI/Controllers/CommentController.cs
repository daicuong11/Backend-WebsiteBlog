using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsWebAPI.Api;
using NewsWebAPI.Entities;
using NewsWebAPI.Repositorys;

namespace NewsWebAPI.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IArticleRepository _articleRepository;
        [ActivatorUtilitiesConstructor]
        public CommentController( ICommentRepository commentRepository , IUserRepository userRepository, IArticleRepository articleRepository)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _articleRepository = articleRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllCommentByParentCommentId([FromRoute] int id)
        {
            try
            {
                List<Comment> findAllCommentByParentCommentId = await _commentRepository.GetCommentByParentCommentID(id);
                var response = new MyResponse<List<Comment>>(true, "Danh sách bình luận trả lời của bình luận id = " + id, findAllCommentByParentCommentId);
                return Ok(response);
            }
            catch(Exception ex)
            {
                var response = new MyResponse<string>(false, "Server error 500", ex.Message);
                return BadRequest(response);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCommentByUserIdAndArticleId([FromQuery] int userId, [FromQuery] int articleId)
        {
            try
            {
                List<Comment> findAllCommentByUserIdAndArticle = await _commentRepository.GetCommentByUserIdAndArticleId(userId, articleId);
                var response = new MyResponse<List<Comment>>(true, "Danh sách bình luận userId = " + userId + " và articleId = " + articleId, findAllCommentByUserIdAndArticle );
                return Ok(response);
            }
            catch(Exception ex)
            {
                var response = new MyResponse<string>(false, "Server error 500", ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPost] 
        public async Task<IActionResult> CreateComment([FromBody] Comment comment)
        {
            try
            {
                User findUserById = await _userRepository.GetById(comment.UserID);
                if(findUserById == null)
                {
                    var res = new MyResponse<string>(false, "Không có user nào với id = " + comment.UserID, "");
                    return NotFound(res);
                }
                Article findArticleById = await _articleRepository.GetArticleById(comment.ArticleID);
                if (findArticleById == null)
                {
                    var res = new MyResponse<string>(false, "Không có bài viết nào với id = " + comment.ArticleID, "");
                    return NotFound(res);
                }
                return Ok();
            }
            catch(Exception ex)
            {
                var response = new MyResponse<string>(false, "Server error 500", ex.Message);
                return BadRequest(response);
            }
        }
    }
}
