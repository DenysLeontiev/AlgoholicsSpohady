using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Helpers;

namespace API.ExtensionMethods
{
    public class UserParams : PaginationParams // what user sends from client to the server
    {
        public string SearchTerm { get; set; } = "";
        public string OrderByField { get; set; } = "DateCreated";
        public string OrderByType { get; set; } = "ASC"; // "DESC"
    }
}