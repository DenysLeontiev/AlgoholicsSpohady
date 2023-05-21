using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ExtensionMethods
{
    public class UserParams // what user sends from client to the server
    {
        private const int MAX_PAGE_SIZE = 20;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 5;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > MAX_PAGE_SIZE) ? MAX_PAGE_SIZE : value;
            }
        }



    }
}