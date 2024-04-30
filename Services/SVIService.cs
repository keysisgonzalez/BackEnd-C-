using Sabio.Data.Providers;
using Sabio.Models.Requests.SVIAddRequest;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public class SVIService : ISVIService
    {
        IDataProvider _data = null;

        public SVIService(IDataProvider data)
        {
            _data = data;
        }

        public int Add(SVIAddRequest model)
        {
            int id = 0;

            string procName = "dbo.Table1_Insert";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {

                col.AddWithValue("@FirstName", model.FirstName);
                col.AddWithValue("@LastName", model.LastName);
                col.AddWithValue("@Url", model.Url);

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                col.Add(idOut);

            }, returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object objId = returnCollection["@Id"].Value;
                
                int.TryParse(objId.ToString(), out id);
            });

            return id;
        }
    }
}
