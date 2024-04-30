using Sabio.Models;
using Sabio.Models.Domain.Computers;
using Sabio.Models.Requests.Computers;
using System.Collections.Generic;

namespace Sabio.Services
{
    public interface IComputerService
    {
        List<Computer> GetAll();
        Computer GetById(int id);
        int Add(ComputerAddRequest model);
        void Update(ComputerUpdateRequest model);
        void Delete(int id);
        List<ComputerV3> GetAllV3();
        ComputerV3 GetByIdV3(int id);
        Paged<ComputerV3> PaginationV3(int pageIndex, int pageSize);
        Paged<ComputerV3> SearchPaginatedV3(int pageIndex, int pageSize, string query);
    }
}