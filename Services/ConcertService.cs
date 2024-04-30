using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models.Domain.Cars;
using Sabio.Models.Domain.Concerts;
using Sabio.Models.Requests.Concerts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public class ConcertService
    {
        IDataProvider _provider = null;

        public ConcertService(IDataProvider provider)
        {
            _provider = provider;
        }

        //DELETE METHOD
        public void Delete(int id)
        {
            string procName = "[dbo].[Concerts_Delete]";

            _provider.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection paramCol)
            {
                paramCol.AddWithValue("@Id", id);

            }, returnParameters: null);

        }

        //UPDATE METHOD
        public void Update(ConcertUpdateRequest model)
        {
            string procName = "[dbo].[Concerts_Update]";

            _provider.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection paramCol)
            {
                AddCommonParams(model, paramCol);

                paramCol.AddWithValue("@Id", model.Id);

            }, returnParameters: null);
           
        }

        //ADD/INSERT METHOD
        public int Add(ConcertAddRequest model)
        {
            int id = 0;

            string procName = "[dbo].[Concerts_Insert]";

            _provider.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection paramCol)
            {
                AddCommonParams(model, paramCol);

                SqlParameter idParamOut = new SqlParameter("@Id", SqlDbType.Int);
                idParamOut.Direction = ParameterDirection.Output;

                paramCol.Add(idParamOut);

            }, returnParameters: delegate (SqlParameterCollection returnedParamCol)
            {
                object objId = returnedParamCol["@Id"].Value;

                int.TryParse(objId.ToString(), out id);
            }
            );

            return id;
        }

        private static void AddCommonParams(ConcertAddRequest model, SqlParameterCollection paramCol)
        {
            paramCol.AddWithValue("@Name", model.Name);
            paramCol.AddWithValue("@Description", model.Description);
            paramCol.AddWithValue("@IsFree", model.IsFree);
            paramCol.AddWithValue("@Address", model.Address);
            paramCol.AddWithValue("@Cost", model.Cost);
            paramCol.AddWithValue("@DateOfEvent", model.DateOfEvent);
        }

        //GET/SELECT BY ID ALL METHOD 
        public Concert GetById(int id)
        {
            string procName = "[dbo].[Concerts_SelectById]";

            Concert aConcert = null;

            _provider.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCol)
            {
                paramCol.AddWithValue("@Id", id);

            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {                
                aConcert = SingleConcertMapper(reader);

            });

            return aConcert;

        }

        private static Concert SingleConcertMapper(IDataReader reader)
        {
            Concert aConcert = new Concert();

            int colIndex = 0;

            aConcert.Id = reader.GetSafeInt32(colIndex++);
            aConcert.Name = reader.GetSafeString(colIndex++);
            aConcert.Description = reader.GetSafeString(colIndex++);
            aConcert.IsFree = reader.GetSafeBool(colIndex++);
            aConcert.Address = reader.GetSafeString(colIndex++);
            aConcert.Cost = reader.GetSafeInt32(colIndex++);
            aConcert.DateOfEvent = reader.GetSafeDateTime(colIndex++);

            return aConcert;
        }

        //GET/SELECT ALL METHOD 
        public List<Concert> GetAll()
        {
            List<Concert> list = null;

            string procName = "[dbo].[Concerts_SelectAll]";

            _provider.ExecuteCmd(procName, inputParamMapper: null,
           singleRecordMapper: delegate (IDataReader reader, short set)
           {
               Concert aConcert = new Concert();

               int colIndex = 0;

               aConcert.Id = reader.GetSafeInt32(colIndex++);
               aConcert.Name = reader.GetSafeString(colIndex++);
               aConcert.Description = reader.GetSafeString(colIndex++);
               aConcert.IsFree = reader.GetSafeBool(colIndex++);
               aConcert.Address = reader.GetSafeString(colIndex++);
               aConcert.Cost = reader.GetSafeInt32(colIndex++);
               aConcert.DateOfEvent = reader.GetSafeDateTime(colIndex++);

               if (list == null)
               {
                   list = new List<Concert>();
               }
               list.Add(aConcert);

           });

            return list;

        }

    }
}
