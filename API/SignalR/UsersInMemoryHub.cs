using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DataTransferObjects;
using API.Entities;
using API.ExtensionMethods;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace API.SignalR
{
    [Authorize] // TODO: Do we need that???
    public class UsersInMemoryHub : Hub
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IDataShaper<User> _dataShaper;

        public UsersInMemoryHub(IRepositoryManager repositoryManager, IDataShaper<User> dataShaper)
        {
            _repositoryManager = repositoryManager;
            _dataShaper = dataShaper;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var memoryId = httpContext.Request.Query["memoryId"];
            var groupName = GetGroupName(memoryId);
            string fieldsString = "Id,UserName,Email";

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName); // connectionId: The connection ID to add to a group.

            var memory = await _repositoryManager.Memory.GetMemoryByIdAsync(memoryId, trackChanges: true);

            var usersInMemory = memory.Users;
            var shappedData = _dataShaper.ShapeData(usersInMemory, fieldsString);
            var parsedData = JsonConvert.DeserializeObject<IEnumerable<UserInMemoryDto>>(JsonConvert.SerializeObject(shappedData));

            await Clients.Group(groupName).SendAsync("GetUsersInMemory", parsedData);
        }

        public async Task AddNewUserToMemory(AddUserToMemoryDto addUserToMemoryDto)
        {
            var httpContext = Context.GetHttpContext();
            string fieldsString = "Id,UserName,Email";

            string currentUsername = Context.User.GetCurrentUserName();
            string currentId = Context.User.GetUserId();

            if (string.IsNullOrWhiteSpace(addUserToMemoryDto.MemoryId) ||
                string.IsNullOrWhiteSpace(addUserToMemoryDto.UserName))
            {
                throw new HubException("Invalid(empty) MemoryId or UserName field(s)");
            }

            // memory, to which we are going to add new user
            var memory = await _repositoryManager.Memory.GetMemoryByIdAsync(addUserToMemoryDto.MemoryId, trackChanges: true);
            if (memory == null)
            {
                throw new HubException("Memory is not found");
            }

            if (currentId != memory.OwnerId)
            {
                throw new HubException($"Such user({currentUsername}) has no rights to do this");
            }

            var userToAdd = await _repositoryManager.User.GetUserByUsernameAsync(addUserToMemoryDto.UserName, true); // user we are going to add to memory

            if (memory.Users.Contains(userToAdd))
            {
                throw new HubException("Such Memory already has that user");
            }

            if (userToAdd == null)
            {
                throw new HubException("User is not found");
            }

            memory.Users.Add(userToAdd);  //TODO: is this a correct logic
            userToAdd.Memories.Add(memory);

            await _repositoryManager.SaveAsync();

            var groupName = GetGroupName(addUserToMemoryDto.MemoryId);

            var shappedData = _dataShaper.ShapeData(userToAdd, fieldsString); // new user we are going to add(here we make that object to only few fields)
            var parsedUserInMemory = JsonConvert.DeserializeObject<UserInMemoryDto>(JsonConvert.SerializeObject(shappedData)); // parse that user, so we can comfortably get this data in the client side of the application

            await Clients.Group(groupName).SendAsync("AddToNewToMemory", parsedUserInMemory);
        }

        public async Task RemoveUserFromMemory(RemoveUserFromMemoryDto removeUserFromMemory)
        {
            var memory = await _repositoryManager.Memory.GetMemoryByIdAsync(removeUserFromMemory.MemoryId, trackChanges: true);
            var userToRemove = await _repositoryManager.User.GetUserByUsernameAsync(removeUserFromMemory.UserName, trackChanges: true);
            string fieldsString = "Id,UserName,Email";
            var groupName = GetGroupName(removeUserFromMemory.MemoryId);

            if (memory.OwnerId == userToRemove.Id)
            {
                throw new HubException("You can not remove owner from memory");
            }

            if (!memory.Users.Contains(userToRemove))
            {
                throw new HubException("User does not exist on this memory");
            }

            memory.Users.Remove(userToRemove);
            userToRemove.Memories.Remove(memory);

            await _repositoryManager.SaveAsync();

            var shappedData = _dataShaper.ShapeData(userToRemove, fieldsString);
            var parsedUserInMemory = JsonConvert.DeserializeObject<UserInMemoryDto>(JsonConvert.SerializeObject(shappedData));

            await Clients.Group(groupName).SendAsync("RemoveUserFromMemory", parsedUserInMemory);
        }

        public async Task SetNewMemoryOwner(SetNewMemoryOwnerDto setNewMemoryOwnerDto)
        {
            var memory = await _repositoryManager.Memory.GetMemoryByIdAsync(setNewMemoryOwnerDto.MemoryId, trackChanges: true);
            var currentUserId = Context.User.GetUserId();

            string fieldsString = "Id,UserName,Email";
            var groupName = GetGroupName(setNewMemoryOwnerDto.MemoryId);

            if (currentUserId == setNewMemoryOwnerDto.NewOwnerId)
            {
                throw new HubException("You are alredy an owner");
            }

            memory.OwnerId = setNewMemoryOwnerDto.NewOwnerId;

            await _repositoryManager.SaveAsync();

            var usersInMemory = memory.Users;
            var shappedData = _dataShaper.ShapeData(usersInMemory, fieldsString);
            var parsedData = JsonConvert.DeserializeObject<IEnumerable<UserInMemoryDto>>(JsonConvert.SerializeObject(shappedData));

            await Clients.Group(groupName).SendAsync("SetNewMemoryOwner", parsedData);


        }

        private string GetGroupName(string memoryId)
        {
            return memoryId;
        }
    }
}