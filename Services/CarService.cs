using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models.Domain.Cars;
using Sabio.Models.Requests.Cars;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public class CarService
    {
        IDataProvider _provider = null;

        public CarService (IDataProvider provider)
        {
            _provider = provider;
        }

        //DELETE METHOD 
        public void Delete(int id)
        {
            string procName = "[dbo].[Cars_Delete]";            

            _provider.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection paramCol)
            {
                paramCol.AddWithValue("@Id", id);

            }, returnParameters: null);
          
        }

        //UPDATE METHOD NON REFACTORED
        public void Update(CarUpdateRequest model)
        {
            string procName = "[dbo].[Cars_Update]";

            _provider.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection paramCol)
            {
                paramCol.AddWithValue("@Make", model.Make);
                paramCol.AddWithValue("@Model", model.Model);
                paramCol.AddWithValue("@Year", model.Year);              
                paramCol.AddWithValue("@Id", model.Id); //Added

            }, returnParameters: null);
            
        }

        //ADD/INSERT METHOD NON REFACTORED
        public int Add(CarAddRequest model, int manufacturedId)
        {
            int id = 0;

            string procName = "[dbo].[Cars_Insert]";

            _provider.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection paramCol)
            {               
                paramCol.AddWithValue("@Make", model.Make);
                paramCol.AddWithValue("@Model", model.Model);
                paramCol.AddWithValue("@Year", model.Year);
                paramCol.AddWithValue("@IsUsed", model.IsUsed);
                paramCol.AddWithValue("@ManufacturerId", manufacturedId);

                SqlParameter idParamOut = new SqlParameter("@Id", SqlDbType.Int);
                idParamOut.Direction = ParameterDirection.Output;

                paramCol.Add(idParamOut);

            }, returnParameters: delegate (SqlParameterCollection returnedParamCol)
            {
                object objId = returnedParamCol["@Id"].Value;

                Int32.TryParse(objId.ToString(), out id);
            }
            );

            return id;

        }

        //GET/SELECT BY ID METHOD NON REFACTORED
        public Car GetById(int id)
        {
            string procName = "[dbo].[Cars_SelectById]";

            Car aCar = null;

            _provider.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCol)
            {
                paramCol.AddWithValue("@Id", id);

            }, singleRecordMapper: delegate(IDataReader reader, short set)
            {
                aCar = new Car();

                int colIndex = 0;

                aCar.Id = reader.GetSafeInt32(colIndex++);
                aCar.Make = reader.GetSafeString(colIndex++);
                aCar.Model = reader.GetSafeString(colIndex++);
                aCar.Year = reader.GetSafeInt32(colIndex++);
                aCar.IsUsed = reader.GetSafeBool(colIndex++);
                aCar.DateCreated = reader.GetSafeDateTime(colIndex++);
                aCar.DateModified = reader.GetSafeDateTime(colIndex++);
            }
            );

            return aCar;
        }

        //GET/SELECT ALL METHOD NON REFACTORED
        public List<Car> GetAll()
        {
        List<Car> carList = null;

        string procName = "[dbo].[Cars_SelectAll]";

        _provider.ExecuteCmd(procName, inputParamMapper: null,
            singleRecordMapper: delegate (IDataReader reader, short set)
        {
            Car aCar = new Car();

            int colIndex = 0;

            aCar.Id = reader.GetSafeInt32(colIndex++);
            aCar.Make = reader.GetSafeString(colIndex++);
            aCar.Model = reader.GetSafeString(colIndex++);
            aCar.Year = reader.GetSafeInt32(colIndex++);
            aCar.IsUsed = reader.GetSafeBool(colIndex++);
            aCar.DateCreated = reader.GetSafeDateTime(colIndex++);
            aCar.DateModified = reader.GetSafeDateTime(colIndex++);

            if (carList == null)
            {
                carList = new List<Car>();
            }
            carList.Add(aCar);
        }
        );

        return carList;
        }


    }
}
