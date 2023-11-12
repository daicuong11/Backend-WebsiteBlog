using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsWebAPI.Api;
using NewsWebAPI.Entities;
using NewsWebAPI.MyExeption;
using NewsWebAPI.Repositorys;

namespace NewsWebAPI.Controllers
{
    [Route("api/likes")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly ILikeRepository likeRepository;
        private readonly IUserRepository userRepository;
        private readonly IArticleRepository articleRepository;
        [ActivatorUtilitiesConstructor]
        public LikeController(ILikeRepository likeRepository, IUserRepository userRepository, IArticleRepository articleRepository)
        {
            this.likeRepository = likeRepository;
            this.userRepository = userRepository;
            this.articleRepository = articleRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Like([FromBody] Like like)
        {
            try
            {

                User findUserById = await userRepository.GetById(like.UserID);
                if (findUserById == null)
                {
                    var res = new MyResponse<string>(false, "Không tìm thấy tác giả với id = " + like.UserID, "");
                    return NotFound(res);
                }
                Article findArticleById = await articleRepository.GetArticleById(like.ArticleID);
                if (findUserById == null)
                {
                    var res = new MyResponse<string>(false, "Không tìm thấy tác giả với id = " + like.UserID, "");
                    return NotFound(res);
                }
                //check liked => unlike
                Like findLikeByTwoId = await likeRepository.GetLikeByUserIdAndArticleId(like.UserID, like.ArticleID);
                if(findLikeByTwoId != null)
                {
                    await likeRepository.UnLike(findLikeByTwoId);
                    var res = new MyResponse<Like>(true, "Hủy thích thành công", null);
                    return Ok(res);

                }
                like.CreateAt = DateTime.Now;
                //Xữ lý khi user truyền vào thiếu thông tin (validate form)
                if (!ModelState.IsValid)
                {
                    var fError = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault();
                    if (fError != null)
                    {
                        var error = fError.ErrorMessage;
                        if (String.IsNullOrEmpty(error))
                        {
                            throw new ValidatorExeption(error);
                        }
                    }
                }


                Like newLike = await likeRepository.Like(like);
                var response = new MyResponse<Like>(true, "Thích thành công", newLike);
                return StatusCode(201, response);
            }
            catch (Exception ex)
            {
                var response = new MyResponse<string>(false, "Server error 500", ex.Message);
                return BadRequest(response);
            }
        }
    }
}
