using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain.Customers;
using Sabio.Models.Domain.Friends;
using Sabio.Models.Domain.Friends1;
using Sabio.Models.Domain.Images;
using Sabio.Models.Domain.Skills;
using Sabio.Models.Domain.Users;
using Sabio.Models.Requests.Customers;
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
    public class CustomerService : ICustomerService
    {
        //initializing the variable type IDataProvider as null;
        IDataProvider _data = null;

        //Creating the constructor of the class
        public CustomerService(IDataProvider data)
        {
            //reassigning the data coming from the DB to a variable so the methods are be able to access to it
            _data = data;
        }

        //DELETE
        public void Delete(int id)
        {
            string procName = "[dbo].[Customers_Delete]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection paramCol)
            {
                paramCol.AddWithValue("@Id", id);
            }
            , returnParameters: null);
        }

        //UPDATE
        public void Update(CustomerUpdateRequest model, int UserId)
        {
            string procName = "[dbo].[Customers_Update]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection paramCol)
            {
                AddCommonParams(model, paramCol);
                //paramCol.AddWithValue("@DateModified", model.DateModified);//dont need to put this since is going to be provided in SQL

                paramCol.AddWithValue("@UserId", UserId);
                paramCol.AddWithValue("@Id", model.Id);

            }, returnParameters: null
            );
        }

        //ADD/INSERT
        public int Add(CustomerAddRequest model, int UserId)
        {
            int id = 0;
            string procName = "[dbo].[Customers_Insert]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection paramCol)
            {
                AddCommonParams(model, paramCol);

                paramCol.AddWithValue("@UserId", UserId);
                paramCol.AddWithValue("@DateCreated", model.DateCreated);
                paramCol.AddWithValue("@DateModified", model.DateModified);

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                paramCol.Add(idOut);

            }, returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object objId = returnCollection["@Id"].Value;

                int.TryParse(objId.ToString(), out id);
            }

            );
            return id;
        }

        private static void AddCommonParams(CustomerAddRequest model, SqlParameterCollection paramCol)
        {
            paramCol.AddWithValue("@Title", model.Title);
            paramCol.AddWithValue("@Bio", model.Bio);
            paramCol.AddWithValue("@Summary", model.Summary);
            paramCol.AddWithValue("@Headline", model.Headline);
            paramCol.AddWithValue("@Slug", model.Slug);
            paramCol.AddWithValue("@StatusId", model.StatusId);
            paramCol.AddWithValue("@PrimaryImageId", model.PrimaryImageId);           

        }

        //GET BY ID
        public Customer GetById(int id)
        {
            Customer customer = null;

            string procName = "[dbo].[Customers_SelectById]";

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCol)
            {
                paramCol.AddWithValue("@Id", id);
            }
            , singleRecordMapper: delegate (IDataReader reader, short set)
            {
                customer = SingleMapper(reader);

            });

            return customer;
        }

        //GET ALL
        public List<Customer> GetAll()
        {
            List<Customer> list = null;

            string procName = "[dbo].[Customers_SelectAll]";

            _data.ExecuteCmd(procName, inputParamMapper: null
            , singleRecordMapper: delegate (IDataReader reader, short set)
            {
                Customer aCustomer = SingleMapper(reader);

                if (list == null)
                {
                    list = new List<Customer>();
                }

                list.Add(aCustomer);
            });

            return list;
        }

        private static Customer SingleMapper(IDataReader reader)
        {
            Customer aCustomer = new Customer();

            int index = 0;

            aCustomer.Id = reader.GetSafeInt32(index++);
            aCustomer.Title = reader.GetSafeString(index++);
            aCustomer.Bio = reader.GetSafeString(index++);
            aCustomer.Summary = reader.GetSafeString(index++);
            aCustomer.Headline = reader.GetSafeString(index++);
            aCustomer.Slug = reader.GetSafeString(index++);
            aCustomer.StatusId = reader.GetInt32(index++);
            aCustomer.PrimaryImageId = reader.GetInt32(index++);
            aCustomer.UserId = reader.GetInt32(index++);
            aCustomer.DateCreated = reader.GetDateTime(index++);
            aCustomer.DateModified = reader.GetDateTime(index++);

            return aCustomer;
        }

        //-------------------------------------

        //SEARCH PAGINATE V3
        public Paged<CustomerV3> SearchPaginatedV3(int pageIndex, int pageSize, string query)
        {
            Paged<CustomerV3> pagedListV3 = null;
            List<CustomerV3> customerListV3 = null;
            int totalCount = 0;

            string procName = "[dbo].[Customers_Search_PaginationV3]";

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCol)
            {
                paramCol.AddWithValue("@PageIndex", pageIndex);
                paramCol.AddWithValue("@PageSize", pageSize);
                paramCol.AddWithValue("@Query", query);

            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                CustomerV3 customerV3 = SingleCustomerMapper(reader);
                totalCount = reader.GetSafeInt32(14);

                if (customerListV3 == null)
                {
                    customerListV3 = new List<CustomerV3>();
                }

                customerListV3.Add(customerV3);
            });

            if (customerListV3 != null)
            {
                pagedListV3 = new Paged<CustomerV3>(customerListV3, pageIndex, pageSize, totalCount);

            }

            return pagedListV3;
        }


        //PAGINATE V3
        public Paged<CustomerV3> PaginationV3(int pageIndex, int pageSize) 
        {
            Paged<CustomerV3> pagedListV3 = null;
            List<CustomerV3> customerListV3 = null;
            int totalCount = 0;

            string procName = "[dbo].[Customers_PaginationV3]";

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCol)
            {
                paramCol.AddWithValue("@PageIndex", pageIndex);
                paramCol.AddWithValue("@PageSize", pageSize);

            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                CustomerV3 customerV3 = SingleCustomerMapper(reader);
                totalCount = reader.GetSafeInt32(14);

                if (customerListV3 == null)
                {
                    customerListV3 = new List<CustomerV3>();
                }

                customerListV3.Add(customerV3);
            });

            if (customerListV3 != null)
            {
                pagedListV3 = new Paged<CustomerV3>(customerListV3, pageIndex, pageSize, totalCount);

            }

            return pagedListV3;
        }

        //GET BY ID
        public CustomerV3 GetByIdV3(int id)
        {
            CustomerV3 customerV3 = null;

            string procName = "[dbo].[Customers_SelectByIdV3]";

            _data.ExecuteCmd(procName, inputParamMapper: delegate(SqlParameterCollection paramCol)
            {
                paramCol.AddWithValue("@Id", id);
            }
            , singleRecordMapper: delegate (IDataReader reader, short set)
            {
                customerV3 = SingleCustomerMapper(reader);

            });

            return customerV3;
        }

        //GET ALL V3
        public List<CustomerV3> GetAllV3()
        {
            List<CustomerV3> listV3 = null;

            string procName = "[dbo].[Customers_SelectAllV3]";

            _data.ExecuteCmd(procName, inputParamMapper: null
            , singleRecordMapper: delegate (IDataReader reader, short set)
            {
                CustomerV3 customerV3 = SingleCustomerMapper(reader);

                if (listV3 == null)
                {
                    listV3 = new List<CustomerV3>();
                }

                listV3.Add(customerV3);
            });

            return listV3;
        }

        private static CustomerV3 SingleCustomerMapper(IDataReader reader)
        {
            CustomerV3 customerV3 = new CustomerV3();
            customerV3.PrimaryImage = new ImageV3();
            customerV3.Skills = new List<SkillV3>();

            int index = 0;

            customerV3.Id = reader.GetSafeInt32(index++);
            customerV3.Title = reader.GetSafeString(index++);
            customerV3.Bio = reader.GetSafeString(index++);
            customerV3.Summary = reader.GetSafeString(index++);
            customerV3.Headline = reader.GetSafeString(index++);
            customerV3.Slug = reader.GetSafeString(index++);
            customerV3.StatusId = reader.GetSafeInt32(index++);
            customerV3.PrimaryImage.Id = reader.GetSafeInt32(index++);
            customerV3.PrimaryImage.TypeId = reader.GetSafeInt32(index++);
            customerV3.PrimaryImage.Url = reader.GetSafeString(index++);
            customerV3.Skills = reader.DeserializeObject<List<SkillV3>>(index++);
            customerV3.UserId = reader.GetSafeInt32(index++);
            customerV3.DateCreated = reader.GetSafeDateTime(index++);
            customerV3.DateModified = reader.GetSafeDateTime(index++);

            return customerV3;
        }
    }
}
