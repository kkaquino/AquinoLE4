using BlogDataLibrary.Data;
using BlogDataLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly SqlData _db;

        public PostController(SqlData db)
        {
            _db = db;
        }

        [AllowAnonymous]
        [HttpGet("list")]
        public ActionResult<List<ListPostModel>> ListPosts()
        {
            var posts = _db.ListPosts();
            return Ok(posts);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<ListPostModel> ShowPostDetails(int id)
        {
            var post = _db.ShowPostDetails(id);
            return Ok(post);
        }

        [Authorize]
        [HttpPost("add")]
        public ActionResult AddPost([FromBody] PostForm form)
        {
            PostModel post = new PostModel();
            post.Title = form.Title;
            post.Body = form.Body;
            post.DateCreated = DateTime.Now;
            post.UserId = GetCurrentUserId();
            _db.AddPost(post);

            return Ok("Post created.");
        }

        private int GetCurrentUserId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                string id = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                if (id != null)
                    return Convert.ToInt32(id);
            }
            return 0;
        }
    }
}
