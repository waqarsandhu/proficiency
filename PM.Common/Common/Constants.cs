using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM.Common.Common
{
    public static class Constants
    {
        public const int DefaultPage = 1;
        public const int DefaultPageSize = 10;
        public const string BadRequestTitle = "Bad Request";
        public const string NotFoundTitle = "Not Found";
        public const string ProductIDErrorMessage = "Product ID must be greater than zero.";
        public const string ProductIDNotFoundMessage = "Product with ID {0} not found.";
        public const string UpdatedMessage = "Updated successfully.";
    }
}
