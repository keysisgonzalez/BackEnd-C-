using Sabio.Models;
using Sabio.Models.Domain.Friends;
using Sabio.Models.Requests.Friends;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface IFriendService
    {
        //FriendV1
        int Add(FriendAddRequest model, int UserId);
        void Delete(int id);
        Friend Get(int id);
        List<Friend> GetAll();
        Paged<Friend> GetPage(int pageIndex, int pageSize);     
        void Update(FriendUpdateRequest model, int UserId);

        //FriendV3
       List<FriendV3> GetAllV3();
       FriendV3 GetByIdV3(int id);
       Paged<FriendV3> PaginationV3(int pageIndex, int pageSize);
       Paged<FriendV3> SearchPaginatedV3(int pageIndex, int pageSize, string query);
      
    }
}