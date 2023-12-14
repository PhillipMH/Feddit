using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Feddit_Domain.Connections
{
    public class Connection : IConnection
    {
        private readonly string _connectionString;
        private readonly SqlConnection _sqlConnection;

    }
}
