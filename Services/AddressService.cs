using Sabio.Data.Providers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Sabio.Data;
using Sabio.Models.Requests.Addresses;
using System.Net;
using System.Reflection.PortableExecutable;
using Sabio.Models.Domain.Addresses;
using Sabio.Services.Interfaces;



namespace Sabio.Services
{
    public class AddressService : IAddressService
    {
        IDataProvider _data = null;
        public AddressService(IDataProvider data)
        {
            _data = data;
        }

        //DELETE METHOD
        public void Delete(int id)
        {
            string procName = "[dbo].[Sabio_Addresses_DeleteById]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);

            }, returnParameters: null);

        }


        //UPDATE METHOD REFACTORED
        public void Update(AddressUpdateRequest model)
        {
            string procName = "[dbo].[Sabio_Addresses_Update]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);

                col.AddWithValue("@Id", model.Id);

            }, returnParameters: null);

        }

        private static void AddCommonParams(AddressUpdateRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@LineOne", model.LineOne);
            col.AddWithValue("@SuiteNumber", model.SuiteNumber);
            col.AddWithValue("@City", model.City);
            col.AddWithValue("@State", model.State);
            col.AddWithValue("@PostalCode", model.PostalCode);
            col.AddWithValue("@IsActive", model.IsActive);
            col.AddWithValue("@Lat", model.Lat);
            col.AddWithValue("@Long", model.Long);

        }

        //ADD/INSERT METHOD REFACTORED
        public int Add(AddressAddRequest model, int currentUserId)
        {

            int id = 0;

            string procName = "[dbo].[Sabio_Addresses_Insert]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);

                //and 1 Output

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                col.Add(idOut);

            }, returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object oId = returnCollection["@Id"].Value;

                int.TryParse(oId.ToString(), out id);

            });

            return id;
        }

        private static void AddCommonParams(AddressAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@LineOne", model.LineOne);
            col.AddWithValue("@SuiteNumber", model.SuiteNumber);
            col.AddWithValue("@City", model.City);
            col.AddWithValue("@State", model.State);
            col.AddWithValue("@PostalCode", model.PostalCode);
            col.AddWithValue("@IsActive", model.IsActive);
            col.AddWithValue("@Lat", model.Lat);
            col.AddWithValue("@Long", model.Long);
        }

        //GET/SELECT BY ID METHOD
        public Address Get(int id)
        {
            string procName = "[dbo].[Sabio_Addresses_SelectById]";

            Address address = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                //a mapper takes data in one shape and produces a second shape

                paramCollection.AddWithValue("@Id", id);
            }, delegate (IDataReader reader, short set) //single record mapper
            {
                address = new Address();

                int startingIndex = 0;

                address.Id = reader.GetSafeInt32(startingIndex++);
                address.LineOne = reader.GetSafeString(startingIndex++);
                address.SuiteNumber = reader.GetSafeInt32(startingIndex++);
                address.City = reader.GetSafeString(startingIndex++);
                address.State = reader.GetSafeString(startingIndex++);
                address.PostalCode = reader.GetSafeString(startingIndex++);
                address.IsActive = reader.GetSafeBool(startingIndex++);
                address.Lat = reader.GetSafeDouble(startingIndex++);
                address.Long = reader.GetSafeDouble(startingIndex++);

            }
            );

            return address;
        }

        //SELECT ALL METHOD       
        public List<Address> GetRandomAddresses()
        {
            List<Address> list = null;

            string procName = "[dbo].[Sabio_Addresses_SelectRandom50]";

            _data.ExecuteCmd(procName, inputParamMapper: null
            , singleRecordMapper: delegate (IDataReader reader, short set) //single record mapper
            {
                Address aAddress = MapSingleAddress(reader);

                if (list == null)
                {
                    list = new List<Address>();
                }

                list.Add(aAddress);

            });

            return list;
        }

        private static Address MapSingleAddress(IDataReader reader)
        {
            Address aAddress = new Address();

            int startingIndex = 0;

            aAddress.Id = reader.GetSafeInt32(startingIndex++);
            aAddress.LineOne = reader.GetSafeString(startingIndex++);
            aAddress.SuiteNumber = reader.GetSafeInt32(startingIndex++);
            aAddress.City = reader.GetSafeString(startingIndex++);
            aAddress.State = reader.GetSafeString(startingIndex++);
            aAddress.PostalCode = reader.GetSafeString(startingIndex++);
            aAddress.IsActive = reader.GetSafeBool(startingIndex++);
            aAddress.Lat = reader.GetSafeDouble(startingIndex++);
            aAddress.Long = reader.GetSafeDouble(startingIndex++);

            return aAddress;
        }
    }
}


