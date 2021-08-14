using Microsoft.AspNetCore.Mvc;
using System;
using API.Contracts.V1;
using API.Models.V1.Request;
using API.Models.V1.Response;
using AowCore.Domain;
using System.Threading.Tasks;
using API.Services;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using API.Helpers;
using Microsoft.AspNetCore.Cors;

namespace API.Controllers.V1
{
   [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
   [EnableCors("AllPolicy")]
   [Authorize]
    public class CompaniesController : Controller
    {
        private readonly ICompanyService _postService;

        public CompaniesController(ICompanyService postService)
        {
            _postService = postService;
        }

        [HttpGet(ApiRoutes.Companies.GetAll)]
        public async Task<IActionResult> Index()
        {
            return Ok(await _postService.GetAll());
        }

        [HttpGet(ApiRoutes.Companies.Get)]
        public async Task<IActionResult> GetById([FromRoute]Guid postId)
        {
            var post = await _postService.GetPostById(postId);

            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpPost(ApiRoutes.Companies.Create)]
        public async Task<IActionResult> Create(CreatePostsRequest createPostsRequest, CancellationToken cancellationToken)
        {
            var post = new Company { CompanyName = createPostsRequest.Name };

            var userId = HttpContext.GetUserId();
            await _postService.CreatePost(post, cancellationToken);

            var baseurl = $"{HttpContext.Request.Scheme }";
            var locationUrl = baseurl + "/" + ApiRoutes.Companies.Get.Replace("{postId}", post.Id.ToString());

            var response = new PostsResponse { Id = post.Id };

            return Created(locationUrl, response);
        }

        [HttpPut(ApiRoutes.Companies.Update)]
        public async Task<IActionResult> Update([FromRoute]Guid postId, [FromBody] UpdatePostsRequest updatePostsRequest, CancellationToken cancellationToken)
        {
            var cmp = await _postService.GetPostById(postId);
            if (cmp == null)
            {
                return NotFound();
            }
            cmp.CompanyName = updatePostsRequest.Name;


            var updated = await _postService.UpdatePost(cmp, cancellationToken);
            if (updated)
                return Ok(cmp);

            return NotFound();
        }

        [HttpDelete(ApiRoutes.Companies.Delete)]
        public async Task<IActionResult> Delete([FromRoute]Guid postId, CancellationToken cancellationToken)
        {
            var cmp = await _postService.GetPostById(postId);
            if (cmp == null)
            {
                return NotFound();
            }
            var deleted = await _postService.DeletePost(cmp, cancellationToken);

            if (deleted)
                return Ok(new AuthSuccessResponse
                {                    
                    Success = true
                });

            return NotFound();
        }
    }
}