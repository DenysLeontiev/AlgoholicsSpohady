using System.Security.Claims;
using API.DataTransferObjects;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{   
    [Authorize]
    public class MemoriesController : BaseApiController
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IQrCodeGenerator _qrCodeGenerator;
        private readonly IPhotoService _photoService;

        public MemoriesController(IRepositoryManager repositoryManager, IQrCodeGenerator qrCodeGenerator, IPhotoService photoService)
        {
            _repositoryManager = repositoryManager;
            _qrCodeGenerator = qrCodeGenerator;
            _photoService = photoService;
        }

        [HttpGet("{memoryId}")]
        public async Task<ActionResult> GetMemory(string memoryId) // TODO: Add DTOs for Photo,User,Memory
        {
            var memory = await _repositoryManager.Memory.GetMemoryByIdAsync(memoryId, trackChanges: false);

            if(memory == null)
            {
                return NotFound("Memory is not found");
            }

            return Ok(memory);
        }


        [HttpPost("add-memory")]
        public async Task<ActionResult> CreateMemory([FromForm] MemoryForCreationDto memoryForCreationDto)
        {
            var currentUsername = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _repositoryManager.User.GetUserByUsernameAsync(currentUsername, true);

            if (user == null)
            {
                return NotFound("User is not found");
            }

            List<Photo> photosToAdd = new();

            foreach (var file in memoryForCreationDto.Files)
            {
                var result = await _photoService.UploadAsync(file);

                var photo = new Photo
                {
                    PhotoUrl = result.SecureUrl.AbsoluteUri,
                    PublicId = result.PublicId,
                };

                photosToAdd.Add(photo);
            }

            Memory memoryToCreate = new Memory
            {
                Title = memoryForCreationDto.Title,
                Description = memoryForCreationDto.Description,
                OwnerUserName = currentUsername,
                Photos = photosToAdd,
                DateCreated = DateTime.Now,
                // MemoryUrl = "https://localho" //TODO Finish Implemetation:
            };
            
            user.Memories.Add(memoryToCreate);
            _repositoryManager.Memory.AddMemory(memoryToCreate);

            await _repositoryManager.SaveAsync();

            return Ok(memoryToCreate);
        }
    }
}