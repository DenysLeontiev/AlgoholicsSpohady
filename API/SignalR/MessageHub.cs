using API.DataTransferObjects;
using API.Entities;
using API.ExtensionMethods;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    [Authorize] //TODO: Do we need that???
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MessageHub : Hub
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public MessageHub(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var memoryId = httpContext.Request.Query["memoryId"];
            var groupName = GetGroupName(memoryId);
            

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var messages = await _repositoryManager.Message.GetMessagesForMemoryWithNoPagination(memoryId);
            var messagesToReturn = _mapper.Map<IEnumerable<MessageDto>>(messages);

            await Clients.Group(groupName).SendAsync("ReceiveMemoryMessages", messagesToReturn);
        }

        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            if (string.IsNullOrWhiteSpace(createMessageDto.MessageText))
            {
                throw new HubException(); // Empty message
            }

            var memory = await _repositoryManager.Memory.GetMemoryByIdAsync(createMessageDto.MemoryId, true);
            if (memory == null)
            {
                throw new HubException("Memory is not found.Cant send message");
            }

            var currentUserId = Context.User.GetUserId();

            var user = await _repositoryManager.User.GetUserByIdAsync(currentUserId, trackChanges: false);
            if (user == null)
            {
                throw new HubException("User is not found.Can't send message");
            }

            if (!memory.Users.Contains(user))
            {
                throw new HubException();
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

            var groupName = GetGroupName(createMessageDto.MemoryId);
            await Clients.Group(groupName).SendAsync("SendNewMessage", _mapper.Map<MessageDto>(message));
        }

        private string GetGroupName(string memoryId)
        {
            return memoryId;
        }
    }
}