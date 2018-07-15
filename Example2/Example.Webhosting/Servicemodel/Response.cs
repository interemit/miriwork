using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Example.Webhosting.Servicemodel
{
    public class Response : IResponse
    {
        public string ErrorMessage { get; set; }
    }
}
