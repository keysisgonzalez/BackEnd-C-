using Newtonsoft.Json;
using Sabio.Data;
using Sabio.Data.Extensions;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain.Friends;
using Sabio.Models.Domain.Friends1;
using Sabio.Models.Domain.Images;
using Sabio.Models.Domain.Skills;
using Sabio.Models.Domain.Users;
using Sabio.Models.Requests.Friends;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public class FriendService : IFriendService
    {
        IDataProvider _data = null;

        public FriendService(IDataProvider data)
        {
            _data = data;
        }

        #region - FriendV1 Services -
        //DELETE METHOD
        public void Delete(int id)
        {
            string procName = "[dbo].[Friends_Delete]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);

            }, returnParameters: null);
        }

        //UPDATE METHOD REFACTORED
        public void Update(FriendUpdateRequest model, int UserId)
        {
            string procName = "[dbo].[Friends_Update]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@UserId", UserId);
                col.AddWithValue("@Title", model.Title);
                col.AddWithValue("@Bio", model.Bio);
                col.AddWithValue("@Summary", model.Summary);
                col.AddWithValue("@Headline", model.Headline);
                col.AddWithValue("@Slug", model.Slug);
                col.AddWithValue("@StatusId", model.StatusId);
                col.AddWithValue("@PrimaryImageUrl", model.PrimaryImageUrl);
                col.AddWithValue("@Id", model.Id);

            }, returnParameters: null);

        }

        //ADD/INSERT METHOD REFACTORED
        public int Add(FriendAddRequest model, int UserId)
        {
            int id = 0;

            string procName = "[dbo].[Friends_Insert]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);

                col.AddWithValue("@UserId", UserId);

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                col.Add(idOut);

            }, returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object objId = returnCollection["@Id"].Value;

                Int32.TryParse(objId.ToString(), out id);
            });

            return id;
        }

        private static void AddCommonParams(FriendAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("Title", model.Title);
            col.AddWithValue("Bio", model.Bio);
            col.AddWithValue("Summary", model.Summary);
            col.AddWithValue("Headline", model.Headline);
            col.AddWithValue("Slug", model.Slug);
            col.AddWithValue("StatusId", model.StatusId);
            col.AddWithValue("PrimaryImageUrl", model.PrimaryImageUrl);

        }

        //GET/SELECT BY ID METHOD REFACTORED
        public Friend Get(int id)
        {
            string procName = "[dbo].[Friends_SelectById]";

            Friend friend = null;

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);

            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                friend = MapSingleFriend(reader);
            }
            );
            return friend;
        }

        private static Friend MapSingleFriend(IDataReader reader)
        {
            Friend friend = new Friend();

            int startingIndex = 0;

            friend.Id = reader.GetSafeInt32(startingIndex++);
            friend.Title = reader.GetSafeString(startingIndex++);
            friend.Bio = reader.GetSafeString(startingIndex++);
            friend.Summary = reader.GetSafeString(startingIndex++);
            friend.Headline = reader.GetSafeString(startingIndex++);
            friend.Slug = reader.GetSafeString(startingIndex++);
            friend.StatusId = reader.GetSafeInt32(startingIndex++);
            friend.PrimaryImageUrl = reader.GetSafeString(startingIndex++);
            friend.UserId = reader.GetSafeInt32(startingIndex++);
            friend.DateCreated = reader.GetSafeDateTime(startingIndex++);
            friend.DateModified = reader.GetSafeDateTime(startingIndex++);
            
            return friend;
        }    

        //GET/SELECT ALL METHOD NON REFACTORED
        public List<Friend> GetAll()
        {
            List<Friend> list = null;

            string procName = "[dbo].[Friends_SelectAll]";

            _data.ExecuteCmd(procName, inputParamMapper: null
           , singleRecordMapper: delegate (IDataReader reader, short set)
           {
               Friend aFriend = new Friend();

               int startingIndex = 0;

               aFriend.Id = reader.GetSafeInt32(startingIndex++);
               aFriend.Title = reader.GetSafeString(startingIndex++);
               aFriend.Bio = reader.GetSafeString(startingIndex++);
               aFriend.Summary = reader.GetSafeString(startingIndex++);
               aFriend.Headline = reader.GetSafeString(startingIndex++);
               aFriend.Slug = reader.GetSafeString(startingIndex++);
               aFriend.StatusId = reader.GetSafeInt32(startingIndex++);
               aFriend.PrimaryImageUrl = reader.GetSafeString(startingIndex++);
               aFriend.DateCreated = reader.GetSafeDateTime(startingIndex++);
               aFriend.DateModified = reader.GetSafeDateTime(startingIndex++);
               aFriend.UserId = reader.GetSafeInt32(startingIndex++);

               if (list == null)
               {
                   list = new List<Friend>();
               }

               list.Add(aFriend);
           });

            return list;
        }

        //PAGINATE 
        public Paged<Friend> GetPage(int pageIndex, int pageSize)
        {
            Paged<Friend> pagedList = null;
            List<Friend> list = null;
            int totalCount = 0;

            string procName = "[dbo].[Friends_Pagination]";

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@PageIndex", (SqlDbType)pageIndex);
                paramCollection.AddWithValue("@PageSize", (SqlDbType)pageSize);

            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                Friend friend = MapSingleFriend(reader);
                totalCount = reader.GetSafeInt32(11);

                if (list == null)
                {
                    list = new List<Friend>();
                }

                list.Add(friend);
            });

            if (list != null)
            {
                pagedList = new Paged<Friend>(list, pageIndex, pageSize, totalCount);

            }
            return pagedList;
        }

        #endregion

        #region FriendV3 Services

        private static void AddCommonParamsV3(FriendAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("Title", model.Title);
            col.AddWithValue("Bio", model.Bio);
            col.AddWithValue("Summary", model.Summary);
            col.AddWithValue("Headline", model.Headline);
            col.AddWithValue("Slug", model.Slug);
            col.AddWithValue("StatusId", model.StatusId);
            col.AddWithValue("PrimaryImageUrl", model.PrimaryImageUrl);

        }

        //PAGINATE V3
        public Paged<FriendV3> PaginationV3(int pageIndex, int pageSize)
        {
            Paged<FriendV3> pagedListV3 = null;
            List<FriendV3> friendListV3 = null;
            int totalCount = 0;

            string procName = "[dbo].[Friends_PaginationV3]";

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@PageIndex", pageIndex);
                paramCollection.AddWithValue("@PageSize", pageSize);

            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                FriendV3 friendV3 = MapSingleFriendV3(reader);
                totalCount = reader.GetSafeInt32(6);

                if (friendListV3 == null)
                {
                    friendListV3 = new List<FriendV3>();
                }

                friendListV3.Add(friendV3);
            });

            if (friendListV3 != null)
            {
                pagedListV3 = new Paged<FriendV3>(friendListV3, pageIndex, pageSize, totalCount);

            }
            return pagedListV3;
        }

        //SEARCH PAGINATE V3
        public Paged<FriendV3> SearchPaginatedV3(int pageIndex, int pageSize, string query)
        {
            Paged<FriendV3> pagedListV3 = null;
            List<FriendV3> friendListV3 = null;
            int totalCount = 0;

            string procName = "[dbo].[Friends_Search_PaginationV3]";

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@PageIndex", pageIndex);
                paramCollection.AddWithValue("@PageSize", pageSize);
                paramCollection.AddWithValue("@Query", query);

            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                FriendV3 friendV3 = MapSingleFriendV3(reader);
                totalCount = reader.GetSafeInt32(6);

                if (friendListV3 == null)
                {
                    friendListV3 = new List<FriendV3>();
                }

                friendListV3.Add(friendV3);
            });

            if (friendListV3 != null)
            {
                pagedListV3 = new Paged<FriendV3>(friendListV3, pageIndex, pageSize, totalCount);
            }
            return pagedListV3;
        }

        //GET/SELECT BY ID V3
        public FriendV3 GetByIdV3(int id)
        {
            FriendV3 friendV3 = null;

            string procName = "[dbo].[Friends_SelectByIdV3]";

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            }
           , singleRecordMapper: delegate (IDataReader reader, short set)
           {
               friendV3 = MapSingleFriendV3(reader);

           });

            return friendV3;
        }

        //GET/SELECT ALL V3
        public List<FriendV3> GetAllV3()
        {
            List<FriendV3> friendListV3 = null;


            string procName = "[dbo].[Friends_SelectAllV3]";

            _data.ExecuteCmd(procName, inputParamMapper: null
           , singleRecordMapper: delegate (IDataReader reader, short set)
           {
               FriendV3 friendV3 = MapSingleFriendV3(reader);

               if (friendListV3 == null)
               {
                   friendListV3 = new List<FriendV3>();
               }

               friendListV3.Add(friendV3);
           });

            return friendListV3;
        }

        private static FriendV3 MapSingleFriendV3(IDataReader reader)
        {
            FriendV3 friendV3 = new FriendV3();
            friendV3.PrimaryImage = new Image();
            friendV3.Skills = new List<Skill>();

            int startingIndex = 0;

            friendV3.Id = reader.GetSafeInt32(startingIndex++);
            friendV3.Title = reader.GetSafeString(startingIndex++);
            friendV3.Bio = reader.GetSafeString(startingIndex++);
            friendV3.Summary = reader.GetSafeString(startingIndex++);
            friendV3.Headline = reader.GetSafeString(startingIndex++);
            friendV3.Slug = reader.GetSafeString(startingIndex++);
            friendV3.StatusId = reader.GetSafeInt32(startingIndex++);
            friendV3.PrimaryImage.Id = reader.GetSafeInt32(startingIndex++);
            friendV3.PrimaryImage.TypeId = reader.GetSafeInt32(startingIndex++);
            friendV3.PrimaryImage.Url = reader.GetSafeString(startingIndex++);
            friendV3.Skills = reader.DeserializeObject<List<Skill>>(startingIndex++);
            friendV3.UserId = reader.GetSafeInt32(startingIndex++);
            friendV3.DateCreated = reader.GetSafeDateTime(startingIndex++);
            friendV3.DateModified = reader.GetSafeDateTime(startingIndex++);

            return friendV3;
        }
    }

    #endregion


}
