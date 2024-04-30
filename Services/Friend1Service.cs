using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models.Domain.Cars;
using Sabio.Models.Domain.Friends1;
using Sabio.Models.Requests.Friends1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services
{
    // Class declared as public
    public class Friend1Service
    {
        // Declaring a private variable _data of type IDataProvider and initializing it to null
        IDataProvider _data = null;

        // Constructor for Friend1Service that takes an IDataProvider parameter and assigns it to the _data variable
        public Friend1Service(IDataProvider data) 
        {
            _data = data;
        }

        //DELETE METHOD 
        // Method for deleting a friend record by ID
        public void Delete(int id)
        {
            // Declaring a string variable procName and assigning a stored procedure name to it
            string procName = "[dbo].[Friends_Delete]";

            // Invoking a method ExecuteNonQuery on the _data object with specified parameters
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection paramCol)
            {
                // Adding a parameter "@Id" with the value of the id parameter to the paramCol collection
                paramCol.AddWithValue("@Id", id);

            }, returnParameters: null);

        }

        //UPDATE METHOD NON REFACTORED
        // Method for updating a friend record
        public void Update(Friend1UpdateRequest model)
        {
            // Declaring a string variable procName and assigning a stored procedure name to it
            string procName = "[dbo].[Friends_Update]";

            // Invoking a method ExecuteNonQuery on the _data object with specified parameters
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection paramCol)
            {
                // Adding parameters with values from the model object to the paramCol collection
                // These parameters represent the fields of the friend record to be updated
                paramCol.AddWithValue("Title", model.Title);
                paramCol.AddWithValue("Bio", model.Bio);
                paramCol.AddWithValue("Summary", model.Summary);
                paramCol.AddWithValue("Headline", model.Headline);
                paramCol.AddWithValue("Slug", model.Slug);
                paramCol.AddWithValue("StatusId", model.StatusId);
                paramCol.AddWithValue("PrimaryImageUrl", model.PrimaryImageUrl);
                paramCol.AddWithValue("UserId", model.UserId);
                paramCol.AddWithValue("Id", model.Id);

            }, returnParameters:null );           
        }

        //ADD/INSERT METHOD NON REFACTORED
        // Method for adding a new friend record
        public int Add(Friend1AddRequest model)
        {
            // Declaring an integer variable id and initializing it to 0
            int id = 0;

            // Declaring a string variable procName and assigning a stored procedure name to it
            string procName = "[dbo].[Friends_Insert]";

            // Invoking a method ExecuteNonQuery on the _data object with specified parameters
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection paramCol)
            {
                // Adding parameters with values from the model object to the paramCol collection
                // These parameters represent the fields of the new friend record to be added
                paramCol.AddWithValue("Title", model.Title);
                paramCol.AddWithValue("Bio", model.Bio);
                paramCol.AddWithValue("Summary", model.Summary);
                paramCol.AddWithValue("Headline", model.Headline);
                paramCol.AddWithValue("Slug", model.Slug);
                paramCol.AddWithValue("StatusId", model.StatusId);
                paramCol.AddWithValue("PrimaryImageUrl", model.PrimaryImageUrl);
                paramCol.AddWithValue("UserId", model.UserId);

                // Adding an output parameter "@Id" to retrieve the ID of the newly added friend record
                SqlParameter idParamOut = new SqlParameter("@Id", SqlDbType.Int);
                idParamOut.Direction = ParameterDirection.Output;

                paramCol.Add(idParamOut);

            }, returnParameters: delegate(SqlParameterCollection paramCol)
            {
                // Retrieving the value of the output parameter "@Id" and parsing it to an integer
                // Assigning the parsed value to the id variable
                object objId = paramCol["@Id"].Value;

                Int32.TryParse(objId.ToString(), out id);
            }
            );
            // Returning the id of the newly added friend record
            return id;
        }

        //GET/SELECT BY ID METHOD NON REFACTORED
        // Method for retrieving a friend record by ID
        public Friend1 GetById(int id)
        {
            // Declaring a Friend1 object variable aFriend and initializing it to null
            Friend1 aFriend = null;

            // Declaring a string variable procName and assigning a stored procedure name to it
            string procName = "[dbo].[Friends_SelectById]";

            // Invoking a method ExecuteCmd on the _data object with specified parameters
            _data.ExecuteCmd(procName, inputParamMapper: delegate(SqlParameterCollection paramCol)
                {

                    paramCol.AddWithValue("@Id", id);

                },
                // Mapping the data from the reader to the aFriend object.
                singleRecordMapper: delegate (IDataReader reader, short set)
                {  
                    aFriend = new Friend1();

                    // Incrementing the index for each column retrieved from the reader
                    int index = 0;

                    // Setting the properties of the aFriend object with values from the reader
                    aFriend.Id = reader.GetSafeInt32(index++);
                    aFriend.Title = reader.GetSafeString(index++);
                    aFriend.Bio = reader.GetSafeString(index++);
                    aFriend.Summary = reader.GetSafeString(index++);
                    aFriend.Headline = reader.GetSafeString(index++);
                    aFriend.Slug = reader.GetSafeString(index++);
                    aFriend.StatusId = reader.GetSafeInt32(index++);
                    aFriend.PrimaryImageUrl = reader.GetSafeString(index++);
                    aFriend.UserId = reader.GetSafeInt32(index++);
                    aFriend.DateCreated = reader.GetSafeDateTime(index++);
                    aFriend.DateModified = reader.GetSafeDateTime(index++);                    
                    
                });

            // Returning the retrieved friend record
            return aFriend;
        }

        //GET/SELECT ALL METHOD NON REFACTORED
        // Method for retrieving all friend records
        public List <Friend1> GetAll()
        {
            // Declaring a List of Friend1 objects variable friendList and initializing it to null
            List<Friend1> friendList = null;

            // Declaring a string variable procName and assigning a stored procedure name to it
            string procName = "[dbo].[Friends_SelectAll]";

            // Invoking a method ExecuteCmd on the _data object with specified parameters
            _data.ExecuteCmd(procName, inputParamMapper: null,
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    // Creating a new Friend1 object aFriend
                    Friend1 aFriend = new Friend1();

                    // Incrementing the index for each column retrieved from the reader
                    int index = 0;

                    // Setting the properties of the aFriend object with values from the reader
                    aFriend.Id = reader.GetSafeInt32(index++);
                    aFriend.Title = reader.GetSafeString(index++);
                    aFriend.Bio = reader.GetSafeString(index++);
                    aFriend.Summary = reader.GetSafeString(index++);
                    aFriend.Headline = reader.GetSafeString(index++);
                    aFriend.Slug = reader.GetSafeString(index++);
                    aFriend.StatusId = reader.GetSafeInt32(index++);                    
                    aFriend.PrimaryImageUrl = reader.GetSafeString(index++);
                    aFriend.DateCreated = reader.GetSafeDateTime(index++);
                    aFriend.DateModified = reader.GetSafeDateTime(index++);
                    aFriend.UserId = reader.GetSafeInt32(index++);

                    // If friendList is null, initializing it as a new List of Friend1 objects
                    if (friendList == null) 
                    {
                        friendList = new List <Friend1>();
                    }
                    // Adding the aFriend object to the friendList
                    friendList.Add(aFriend);
                });
            // Returning the retrieved friendList record
            return friendList;
        }
    }
}
