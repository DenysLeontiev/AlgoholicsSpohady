using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DataTransferObjects;
using API.ExtensionMethods;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class LikesController : BaseApiController
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public LikesController(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        [HttpPost("like-memory/{memoryId}")]
        public async Task<ActionResult> LikeMemory(string memoryId)
        {
            var currentUserName = User.GetCurrentUserName();
            var currentUser = await _repositoryManager.User.GetUserByUsernameAsync(currentUserName, trackChanges: true);

            if (currentUser == null)
            {
                return NotFound("User is not found");
            }

            var likedMemory = await _repositoryManager.Memory.GetMemoryByIdAsync(memoryId, trackChanges: true);
            if (likedMemory == null)
            {
                return NotFound("Memory is not found");
            }

            if (!likedMemory.Users.Contains(currentUser))
            {
                return BadRequest("You have no rights to do that, because you do not belong to that memory");
            }

            if (currentUser.LikedMemories.Contains(likedMemory))
            {
                return BadRequest("You can`t like same memory twice");
            }

            currentUser.LikedMemories.Add(likedMemory);
            likedMemory.LikedByUsers.Add(currentUser);

            await _repositoryManager.SaveAsync();

            return Ok();
        }

        [HttpPost("dislike-memory/{memoryId}")]
        public async Task<ActionResult> DislikeMemory(string memoryId)
        {
            var currentUserName = User.GetCurrentUserName();
            var currentUser = await _repositoryManager.User.GetUserByUsernameAsync(currentUserName, trackChanges: true);

            if (currentUser == null)
            {
                return NotFound("User is not found");
            }

            var likedMemory = await _repositoryManager.Memory.GetMemoryByIdAsync(memoryId, trackChanges: true);
            if (likedMemory == null)
            {
                return NotFound("Memory is not found");
            }

            if (!likedMemory.Users.Contains(currentUser))
            {
                return BadRequest("You have no rights to do that, because you do not belong to that memory");
            }

            currentUser.LikedMemories.Remove(likedMemory);
            likedMemory.LikedByUsers.Remove(currentUser);

            await _repositoryManager.SaveAsync();

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<MemoryDto>>> GetLikedMemoriesForUser([FromQuery] UserParams userParams)
        {
            var currentUserName = User.GetCurrentUserName();
            var pagedMemories = await _repositoryManager.Memory.GetLikedMemoriesForUserAsync(userParams, currentUserName);

            Response.AddPaginationHeaders(new PaginationHeader(
                pagedMemories.CurrentPage,
                pagedMemories.PageSize,
                pagedMemories.TotalCount,
                pagedMemories.TotalPages));

            var memoriesToReturn = _mapper.Map<IEnumerable<LikedMemoryDto>>(pagedMemories);

            return Ok(memoriesToReturn);
        }
    }
}