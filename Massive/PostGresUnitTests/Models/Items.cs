using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Massive.PostgreSQL;

namespace PostGreSQLUnitTests.Models
{
    public class Items : DynamicModel
    {
        public Items()
            : base("Massive.PostGreSQLUnitTests")
        {
        }
    }
}
