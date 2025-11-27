using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.ChartingSystem.Data
{
    public class QueryRequest
    {
        public string? Content { get; set; }

        public QueryRequest()
        {
            Content = string.Empty;
        }

        public QueryRequest(string content)
        {
            Content = content;
        }
    }
}
