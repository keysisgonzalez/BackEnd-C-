using Sabio.Data.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Models.Domain.Users;
using System.Data.SqlClient;
using System.Data;
using Sabio.Data;
using Sabio.Models.Requests;
using Sabio.Models.Requests.Users;
using System.Security.Cryptography;
using Sabio.Services.Interfaces;

namespace Sabio.Services
{
    public class UserServiceV1 : IUserServiceV1

    {   //declaring the "_data" variable of type interface and assigning it the value to null to be reassigned later.
        IDataProvider _data = null;

        //creating the constructor of the class "UserServiceV1", passing the param of type IDataProvider
        //and assigning its value to the already created variable "_data".
        public UserServiceV1(IDataProvider data)
        {
            _data = data;
        }

        //DELETE METHOD
        public void Delete(int id)
        {
            string procName = "[dbo].[Users_Delete]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);

            }, returnParameters: null);
        }

        //UPDATE METHOD REFACTORED
        public void Update(UserUpdateRequest model)
        {
            string procName = "[dbo].[Users_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@FirstName", model.FirstName);
                col.AddWithValue("@LastName", model.LastName);
                col.AddWithValue("@Email", model.Email);
                col.AddWithValue("@Password", model.Password);
                col.AddWithValue("@AvatarUrl", model.AvatarUrl);
                col.AddWithValue("@TenantId", model.TenantId);
                col.AddWithValue("@Id", model.Id);

            }, returnParameters: null);
        }

        //ADD/INSERT METHOD REFACTORED
        public int Add(UserAddRequest model)
        {
            int id = 0;

            string procName = "[dbo].[Users_Insert]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                col.Add(idOut);

            }, returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object objectId = returnCollection["@Id"].Value;

                Int32.TryParse(objectId.ToString(), out id);
            }
            );

            return id;
        }

        private static void AddCommonParams(UserAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@FirstName", model.FirstName);
            col.AddWithValue("@LastName", model.LastName);
            col.AddWithValue("@Email", model.Email);
            col.AddWithValue("@Password", model.Password);
            col.AddWithValue("@AvatarUrl", model.AvatarUrl);
            col.AddWithValue("@TenantId", model.TenantId);
        }


        //GET/SELECT BY ID METHOD REFACTORED
        public User Get(int id) //creating the "Get" method that will return a User type object and will received an id param of type int
        {
            string procName = "[dbo].[Users_SelectById]"; //storing the proc name into a string that will be sent to the DB

            User user = null; //creating a variable type User and  ing from the DB

            //since this is a select method and the data variable is inheriting the IDataProvider innterface, we need to stick to its contract and
            //use the ExecuteCmd method once we received the data from the DB along with its inline functions
            //then I am passing the procName param, the second param is a function that received a param of type "SqlParameterCollection"
            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
             {
                 //accesing to the "AddWithValue" method of the param (originaly inside of the "SqlParameterCollection" function/class??) which is expecting
                 //two params, on of type string and the second of type object which is going to be the Id that we pass to the "Get" method
                 paramCollection.AddWithValue("@Id", id);

                 //siengle Record Mapper
             }, singleRecordMapper: delegate (IDataReader reader, short set)
             {

                 user = MapSingleUser(reader); //invoking the mapper function an assigning its value to the variable "user" of type User
             }
             );

            return user; //returning the "user" variable with the data from the DB

        }
        //mapper function 
        private static User MapSingleUser(IDataReader reader)  //mapper function to map a single IDataRecord to a User object
        {
            User user = new User(); //creating a new User object to store the mapped data

            int startingIndex = 0; //initializing the starting index for reading data from the IDataRecord

            //mapping data from the IDataRecord to the User object
            user.Id = reader.GetSafeInt32(startingIndex++);
            user.FirstName = reader.GetSafeString(startingIndex++);
            user.LastName = reader.GetSafeString(startingIndex++);
            user.Email = reader.GetSafeString(startingIndex++);
            user.AvatarUrl = reader.GetSafeString(startingIndex++);
            user.TenantId = reader.GetSafeString(startingIndex++);
            user.DateCreated = reader.GetSafeDateTime(startingIndex++);
            user.DateModified = reader.GetSafeDateTime(startingIndex++);

            return user;
        }

        //GET/SELECT ALL METHOD REFACTORED
        public List<User> GetAll()
        {
            List<User> list = null;

            string procName = "[dbo].[Users_SelectAll]";

            _data.ExecuteCmd(procName, inputParamMapper: null
           , singleRecordMapper: delegate (IDataReader reader, short set) //single record mapper
           {

               User aUser = MapUser(reader);

               if (list == null)
               {
                   list = new List<User>();
               }

               list.Add(aUser);

           });

            return list;
        }

        private static User MapUser(IDataReader reader)
        {
            User aUser = new User();

            int startingIndex = 0;

            aUser.Id = reader.GetSafeInt32(startingIndex++);
            aUser.FirstName = reader.GetSafeString(startingIndex++);
            aUser.LastName = reader.GetSafeString(startingIndex++);
            aUser.Email = reader.GetSafeString(startingIndex++);
            aUser.AvatarUrl = reader.GetSafeString(startingIndex++);
            aUser.TenantId = reader.GetSafeString(startingIndex++);
            aUser.DateCreated = reader.GetSafeDateTime(startingIndex++);
            aUser.DateModified = reader.GetSafeDateTime(startingIndex++);

            return aUser;
        }
    }
}


////SELECT ALL METHOD NON REFACTORED SAMPLE 
//public List<User> GetUsers()
//{
//    List<User> list = null;

//    string procName = "[dbo].[Users_SelectAll]";

//    _data.ExecuteCmd(procName, inputParamMapper: null
//   , singleRecordMapper: delegate (IDataReader reader, short set) //single record mapper
//   {
//       User aUser = new User();

//       int startingIndex = 0;

//       aUser.Id = reader.GetSafeInt32(startingIndex++);
//       aUser.FirstName = reader.GetSafeString(startingIndex++);
//       aUser.LastName = reader.GetSafeString(startingIndex++);
//       aUser.Email = reader.GetSafeString(startingIndex++);
//       aUser.AvatarUrl = reader.GetSafeString(startingIndex++);
//       aUser.Password = reader.GetSafeString(startingIndex++);
//       aUser.TenantId = reader.GetSafeString(startingIndex++);
//       aUser.DateCreated = reader.GetSafeDateTime(startingIndex++);
//       aUser.DateModified = reader.GetSafeDateTime(startingIndex++);

//       if (list != null)
//       {
//           list = new List<User>();
//       }

//       list.Add(aUser);

//   });

//    return list;
//}
