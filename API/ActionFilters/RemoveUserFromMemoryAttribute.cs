using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DataTransferObjects;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.ActionFilters
{
    public class RemoveUserFromMemoryAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repositoryManager;

        public RemoveUserFromMemoryAttribute(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var removeUserFromMemoryDto = context.ActionArguments["removeUserFromMemory"] as RemoveUserFromMemoryDto;

            if (removeUserFromMemoryDto == null)
            {
                context.Result = new NotFoundObjectResult("removeUserFromMemoryDto parametr does not exists");
            }

            var memoryId = removeUserFromMemoryDto.MemoryId;
            var userName = removeUserFromMemoryDto.UserName;

            if (string.IsNullOrWhiteSpace(memoryId) ||
                string.IsNullOrWhiteSpace(userName))
            {
                context.Result = new BadRequestObjectResult("Field(s) is(are) empty");
            }

            var memory = await _repositoryManager.Memory.GetMemoryByIdAsync(memoryId, trackChanges: true);
            if(memory == null)
            {
                context.Result = new NotFoundObjectResult("Memory with such id is not found");
            }

            var user = await _repositoryManager.User.GetUserByUsernameAsync(userName, trackChanges: true);
            if(user == null)
            {
                context.Result = new NotFoundObjectResult("User with such userName is not found");
            }
            
            context.HttpContext.Items.Add("memory", memory);
            context.HttpContext.Items.Add("user", user);

            await next();
        }
    }
}