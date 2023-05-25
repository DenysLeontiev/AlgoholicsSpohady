using API.ActionFilters;
using API.DataTransferObjects;
using API.Entities;
using API.ExtensionMethods;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Authorize]
    public class MemoriesController : BaseApiController
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IQrCodeGenerator _qrCodeGenerator;
        private readonly IPhotoService _photoService;
        private readonly IDataShaper<User> _dataShaper;
        private readonly IMapper _mapper;

        public MemoriesController(IRepositoryManager repositoryManager,
                                  IQrCodeGenerator qrCodeGenerator,
                                  IPhotoService photoService,
                                  IDataShaper<User> dataShaper,
                                  IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _qrCodeGenerator = qrCodeGenerator;
            _photoService = photoService;
            _mapper = mapper;
            _dataShaper = dataShaper;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<MemoryDto>>> GetAllMyMemories([FromQuery] UserParams userParams)
        {
            var currentId = User.GetUserId();

            var pagedMemories = await _repositoryManager.Memory.GetAllMemoriesForUserAsync(userParams, currentId);

            Response.AddPaginationHeaders(new PaginationHeader(
                pagedMemories.CurrentPage,
                pagedMemories.PageSize,
                pagedMemories.TotalCount,
                pagedMemories.TotalPages));


            var memoriesToReturn = _mapper.Map<IEnumerable<MemoryDto>>(pagedMemories);

            return Ok(memoriesToReturn);
        }

        [HttpGet("{memoryId}")]
        public async Task<ActionResult> GetMemory(string memoryId)
        {
            var memory = await _repositoryManager.Memory.GetMemoryByIdAsync(memoryId, trackChanges: false);

            if (memory == null)
            {
                return NotFound("Memory is not found");
            }

            var memoryToReturn = _mapper.Map<MemoryDto>(memory);

            return Ok(memoryToReturn);
        }

        [ServiceFilter(typeof(MemoryWithMemoryIdExists))]
        [HttpGet("users-in-memory/{memoryId}")]
        public ActionResult GetUsersInMemory(string memoryId)
        {
            string fieldsString = "Id,UserName,Email";
            // var memory = await _repositoryManager.Memory.GetMemoryByIdAsync(memoryId, trackChanges: false);
            var memory = HttpContext.Items["memory"] as Memory;

            if (memory == null)
            {
                return NotFound("Memory is not found");
            }

            var usersInMemory = memory.Users;

            var shappedData = _dataShaper.ShapeData(usersInMemory, fieldsString);
            var parsedData = JsonConvert.DeserializeObject<IEnumerable<UserInMemoryDto>>(JsonConvert.SerializeObject(shappedData));

            return Ok(parsedData);
        }


        [HttpPost("add-memory")]
        public async Task<ActionResult> CreateMemory([FromForm] MemoryForCreationDto memoryForCreationDto)
        {
            // var .currentUsernameTest = User.FindFirst("name")?.Value;
            var currentId = User.GetUserId();
            var currentUsername = User.GetCurrentUserName();
            var user = await _repositoryManager.User.GetUserByIdAsync(currentId, true);

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
                OwnerId = user.Id,
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

        [ServiceFilter(typeof(MemoryWithMemoryIdExists))]
        [HttpPost("update-memory/{memoryId}")]
        public async Task<ActionResult> UpdateMemory(string memoryId, [FromBody] MemoryForUpdateDto memoryForUpdateDto)
        {
            var memory = HttpContext.Items["memory"] as Memory;

            if (string.IsNullOrWhiteSpace(memoryForUpdateDto.Title) ||
               string.IsNullOrWhiteSpace(memoryForUpdateDto.Description))
            {
                return BadRequest("Invalid data(empty fields)");
            }

            memory.Title = memoryForUpdateDto.Title;
            memory.Description = memoryForUpdateDto.Description;

            await _repositoryManager.SaveAsync();

            return Ok();
        }

        [HttpPost("add-user-to-memory")]
        public async Task<ActionResult> AddUserToMemory([FromBody] AddUserToMemoryDto addUserToMemoryDto)
        {
            string currentUsername = User.GetCurrentUserName();
            string currentId = User.GetUserId();

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
            
            if (currentId != memory.OwnerId)
            {
                return BadRequest($"Such user({currentUsername}) has to rights no do this");
            }

            var user = await _repositoryManager.User.GetUserByUsernameAsync(addUserToMemoryDto.UserName, true);

            if (memory.Users.Contains(user))
            {
                return BadRequest("Such Memory already has that user");
            }

            if (user == null)
            {
                return NotFound("User is not found");
            }

            memory.Users.Add(user);  //TODO: is this a correct logic
            user.Memories.Add(memory);

            await _repositoryManager.SaveAsync();

            return Ok();
        }

        [ServiceFilter(typeof(RemoveUserFromMemoryAttribute))]
        [HttpPost("remove-user-from-memory")] // removes particulal user from aprtic. memory by Ids
        public async Task<ActionResult> RemoveUserFromMemory([FromBody] RemoveUserFromMemoryDto removeUserFromMemory)
        {
            var memory = HttpContext.Items["memory"] as Memory;
            var user = HttpContext.Items["user"] as User;

            if (memory.OwnerId == user.Id)
            {
                return BadRequest("You can not remove owner from memory");
            }

            if (!memory.Users.Contains(user))
            {
                return BadRequest("User does not exist on this memory");
            }

            memory.Users.Remove(user);
            user.Memories.Remove(memory);

            await _repositoryManager.SaveAsync();

            return Ok();
        }


        //TODO: Create a method which will make a new owner of the memory
        [ServiceFilter(typeof(MemoryWithMemoryIdExists))]
        [HttpPost("set-new-owner/{memoryId}/{newOwnerId}")]
        public async Task<ActionResult> SetNewOwner(string newOwnerId, string memoryId)
        {
            var memory = HttpContext.Items["memory"] as Memory;

            var currentUserId = User.GetUserId();

            if(currentUserId == newOwnerId)
            {
                return BadRequest();
            }

            memory.OwnerId = newOwnerId;

            await _repositoryManager.SaveAsync();

            return Ok("New owner is set!");
        }

    }
}