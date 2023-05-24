using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.ActionFilters;
using API.DataTransferObjects;
using API.Entities;
using API.ExtensionMethods;
using API.Helpers;
using API.Interfaces;
using API.Interfaces.MessageInterfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MessagesController : BaseApiController
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public MessagesController(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        [ServiceFilter(typeof(MemoryWithMemoryIdExists))]
        [HttpGet("{memoryId}")]
        public async Task<ActionResult> GetMessagesForMemory([FromQuery] MessageParams messageParams, string memoryId)
        {
            var messages = await _repositoryManager.Message.GetMessagesForMemory(memoryId, messageParams);

            Response.AddPaginationHeaders(new PaginationHeader(
                messages.CurrentPage,
                messages.PageSize,
                messages.TotalCount,
                messages.TotalPages));


            var messagesToReturn = _mapper.Map<IEnumerable<MessageDto>>(messages);

            return Ok(messagesToReturn);
        }

        [HttpPost("add-message")]
        public async Task<ActionResult> AddMessage([FromBody] CreateMessageDto createMessageDto) // TODO: replace to MessageRepos
        {
            if (string.IsNullOrWhiteSpace(createMessageDto.MessageText))
            {
                return BadRequest(); // Emty message
            }

            var memory = await _repositoryManager.Memory.GetMemoryByIdAsync(createMessageDto.MemoryId, true);
            if (memory == null)
            {
                return NotFound("Memory is not found.Cant send message");
            }

            var currentUserId = User.GetUserId();

            var user = await _repositoryManager.User.GetUserByIdAsync(currentUserId, trackChanges: false);
            if (user == null)
            {
                return NotFound("User is not found.Can't send message");
            }

            if (!memory.Users.Contains(user))
            {
                return BadRequest();
            }

            var message = new Message
            {
                DateSend = DateTime.Now,
                Sender = user,
                SenderId = currentUserId,
                SenderUsername = user.UserName,
                Text = createMessageDto.MessageText,
            };

            memory.Messages.Add(message);
            _repositoryManager.Message.CreateMessage(message);

            await _repositoryManager.SaveAsync();

            return Ok();
        }
    }
}