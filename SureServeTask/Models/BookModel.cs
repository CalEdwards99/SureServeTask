using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SureServeTask.Models
{
    public class BookModel
    {
        public class Book
        {
            public string _id { get; set; }
            public string name { get; set; }

            public string chapterName { get; set; }

        }

        //The returned json object from the API call.
        public List<object> docs { get; set; }



    }
}
