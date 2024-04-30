using Sabio.Data.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using Sabio.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Models.Domain.Players;
using System.Data.SqlClient;
using Sabio.Models.Domain.Friends;
using System.Numerics;
using Sabio.Models.Requests.Players;
using Sabio.Models.Domain.Users;

namespace Sabio.Services
{

    public class PlayerService
    {
        //declaring the "_data" variable of type interface and assigning it the value to null to be reassigned later.
        IDataProvider _data = null;
        public PlayerService(IDataProvider data) 
        {
            _data = data;        
        }

        //DELETE METHOD 
        public void Delete(int id)
        {
            string procName = "[dbo].[Players_Delete]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection paramCol)
            {
                paramCol.AddWithValue("Id", id);

            }, returnParameters: null);
        }

        //UPDATE METHOD NON REFACTORED
        public void Update(PlayerUpdateRequest model)
        {
            string procName = "[dbo].[Players_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection updateCol)
            {
                updateCol.AddWithValue("@Title", model.Title);
                updateCol.AddWithValue("@StatusId", model.StatusId);
                updateCol.AddWithValue("@PrimaryImageUrl", model.PrimaryImageUrl);
                updateCol.AddWithValue("@UserId", model.UserId);
                updateCol.AddWithValue("@Id", model.Id);

            }, returnParameters: null
            );
        }

        //ADD/INSERT METHOD NON REFACTORED
        public int Add(PlayerAddRequest model)
        {
            int id = 0;

            string procName = "[dbo].[Players_Insert]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection insertCol)
            {
                insertCol.AddWithValue("@Title", model.Title);
                insertCol.AddWithValue("@StatusId", model.@StatusId);
                insertCol.AddWithValue("@PrimaryImageUrl", model.@PrimaryImageUrl);
                insertCol.AddWithValue("@UserId", model.@UserId);

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                insertCol.Add(idOut);

            }, returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object objId = returnCollection["@Id"].Value;

                Int32.TryParse(objId.ToString(), out id);
            }
            );
            
            return id;
        }

        //GET/SELECT BY ID METHOD NON REFACTORED
        public Player Get(int id)
        {
            string procName = "[dbo].[Players_SelectById]";

            Player player = null;

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCol)
            {
                paramCol.AddWithValue("Id", id);

            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                player = new Player();

                int startingIndex = 0;

                player.Id = reader.GetSafeInt32(startingIndex++);
                player.Title = reader.GetSafeString(startingIndex++);
                player.StatusId = reader.GetSafeInt32(startingIndex++);
                player.PrimaryImageUrl = reader.GetSafeString(startingIndex++);                
                player.UserId = reader.GetSafeInt32(startingIndex++);

            }
            );

            return player;
 
        }

        //GET/SELECT ALL METHOD NON REFACTORED
        public List<Player> GetAll() 
        {
            List<Player> list = null;

            string procName = "[dbo].[Players_SelectAll]";

            _data.ExecuteCmd(procName, inputParamMapper: null,
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                Player aPlayer = new Player();

                int startingIndex = 0;

                aPlayer.Id = reader.GetSafeInt32(startingIndex++);
                aPlayer.Title = reader.GetSafeString(startingIndex++);
                aPlayer.StatusId = reader.GetSafeInt32(startingIndex++);
                aPlayer.PrimaryImageUrl = reader.GetSafeString(startingIndex++);
                aPlayer.UserId = reader.GetSafeInt32(startingIndex++);

                if(list == null)
                {
                    list = new List<Player>();
                }

                list.Add(aPlayer);
            }
            );

            return list;
        }
    }
}
