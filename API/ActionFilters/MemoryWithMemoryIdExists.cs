using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.ActionFilters
{
    public class MemoryWithMemoryIdExists : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repositoryManager;
        public MemoryWithMemoryIdExists(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var memoryId = (string)context.ActionArguments["memoryId"];
            var memory = await _repositoryManager.Memory.GetMemoryByIdAsync(memoryId, trackChanges: true);

            if(memory == null)
            {
                context.Result = new NotFoundObjectResult("Memory is not found");
            }
            else
            {
                context.HttpContext.Items.Add("memory", memory);
            }

            await next();
        }
    }
}