using System.Collections;
using System.Security.Claims;
using API.DataTransferObjects;
using API.Entities;
using API.ExtensionMethods;
using API.Interfaces;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public MemoriesController(IRepositoryManager repositoryManager,
                                  IQrCodeGenerator qrCodeGenerator,
                                  IPhotoService photoService,
                                  IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _qrCodeGenerator = qrCodeGenerator;
            _photoService = photoService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemoryDto>>> GetAllMyMemories()
        {
            var currentUsername = User.GetCurrentUserName();

            var user = await _repositoryManager.User.GetUserByUsernameAsync(currentUsername, trackChanges: false);

            if (user == null)
            {
                return NotFound("User is not found");
            }

            var memoriesToReturn = _mapper.Map<IEnumerable<MemoryDto>>(user.Memories);

            return Ok(memoriesToReturn);
        }

        [HttpGet("{memoryId}")]
        public async Task<ActionResult> GetMemory(string memoryId) // TODO: Add DTOs for Photo,User,Memory
        {
            var memory = await _repositoryManager.Memory.GetMemoryByIdAsync(memoryId, trackChanges: false);

            if (memory == null)
            {
                return NotFound("Memory is not found");
            }

            var memoryToReturn = _mapper.Map<MemoryDto>(memory);

            return Ok(memoryToReturn);
        }


        [HttpPost("add-memory")]
        public async Task<ActionResult> CreateMemory([FromForm] MemoryForCreationDto memoryForCreationDto)
        {
            // var .currentUsernameTest = User.FindFirst("name")?.Value;
            var currentUsername = User.GetCurrentUserName();

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
            };

            user.Memories.Add(memoryToCreate);
            _repositoryManager.Memory.AddMemory(memoryToCreate);

            memoryToCreate.MemoryUrl = "https://localhost:7053/api/memories/" + memoryToCreate.Id;
            memoryToCreate.MemoryQrCode = _qrCodeGenerator.GenerateQrCode(memoryToCreate.MemoryUrl);

            await _repositoryManager.SaveAsync();

            return Ok(memoryToCreate);
        }

        [HttpPost("add-user-to-memory")]
        public async Task<ActionResult> AddUserToMemory([FromBody] AddUserToMemoryDto addUserToMemoryDto)
        {
            if (string.IsNullOrWhiteSpace(addUserToMemoryDto.MemoryId) ||
                string.IsNullOrWhiteSpace(addUserToMemoryDto.UserName))
            {
                return BadRequest("Invalid(empty) MemoryId or UserName field(s)");
            }

            var memory = await _repositoryManager.Memory.GetMemoryByIdAsync(addUserToMemoryDto.MemoryId, trackChanges: true);
            if (memory == null)
            {
                return NotFound("Memory is not found");
            }

            if(memory.Users.Where(u => u.UserName == addUserToMemoryDto.UserName).FirstOrDefault() != null)
            {
                return BadRequest("Such Memory already has that user");
            }

            var user = await _repositoryManager.User.GetUserByUsernameAsync(addUserToMemoryDto.UserName, true);
            if(user == null)
            {
                return NotFound("User is not found");
            }

            memory.Users.Add(user);
            user.Memories.Add(memory);

            await _repositoryManager.SaveAsync();

            return Ok("User is added to the memory");
        }
    }
}