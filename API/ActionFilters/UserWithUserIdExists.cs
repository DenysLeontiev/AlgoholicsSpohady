using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.ActionFilters
{
    public class UserWithUserIdExists : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repositoryManager;
        public UserWithUserIdExists(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string userId = (string)context.ActionArguments["userId"];
            var user = await _repositoryManager.User.GetUserByIdAsync(userId, trackChanges: true);

            if(user == null)
            {
                context.Result = new NotFoundObjectResult("User is not found");
            }

            context.HttpContext.Items.Add("user", user);

            await next();
        }
    }
}