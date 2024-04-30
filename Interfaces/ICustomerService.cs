using Sabio.Models;
using Sabio.Models.Domain.Customers;
using Sabio.Models.Requests.Customers;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface ICustomerService
    {
        int Add(CustomerAddRequest model, int UserId);
        void Delete(int id);
        List<Customer> GetAll();
        Customer GetById(int id);
        void Update(CustomerUpdateRequest model, int UserId);

        //Customer V3
        List<CustomerV3> GetAllV3();
        CustomerV3 GetByIdV3(int id);
        Paged<CustomerV3> PaginationV3(int pageIndex, int pageSize);

        Paged<CustomerV3> SearchPaginatedV3(int pageIndex, int pageSize, string query);
    }
} 