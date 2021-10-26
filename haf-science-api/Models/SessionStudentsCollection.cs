using Microsoft.Data.SqlClient.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Models
{
    public class SessionStudentsCollection : List<SessionStudents>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sqlRow = new SqlDataRecord(
                new SqlMetaData("UsuarioId", SqlDbType.Int),
                new SqlMetaData("SessionId", SqlDbType.Int),
                new SqlMetaData("NombreUsuario", SqlDbType.VarChar, 20),
                new SqlMetaData("NombreSesion", SqlDbType.VarChar, 50)
                );

            foreach (SessionStudents saveModel in this)
            {
                sqlRow.SetInt32(0, saveModel.UsuarioId);
                sqlRow.SetInt32(1, saveModel.SessionId);
                sqlRow.SetString(2, saveModel.NombreUsuario);
                sqlRow.SetString(3, saveModel.NombreSesion);

                yield return sqlRow;
            }
                
        }
    }
}
