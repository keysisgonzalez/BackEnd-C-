using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain.ComputerImages;
using Sabio.Models.Domain.Computers;
using Sabio.Models.Domain.Customers;
using Sabio.Models.Domain.Friends;
using Sabio.Models.Domain.Friends1;
using Sabio.Models.Domain.Monitors;
using Sabio.Models.Domain.Skills;
using Sabio.Models.Domain.Users;
using Sabio.Models.Requests.Computers;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Monitor = Sabio.Models.Domain.Monitors.Monitor;

namespace Sabio.Services
{
    public class ComputerService : IComputerService
    {
        IDataProvider _data = null;
        public ComputerService(IDataProvider data)
        {
            _data = data;
        }

        //DELETE
        public void Delete(int id)
        {
            string procName = "[dbo].[Computers_Delete]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection paramCol)
            {
                paramCol.AddWithValue("@Id", id);
            }
            , returnParameters: null);
        }

        //UPDATE
        public void Update(ComputerUpdateRequest model)
        {           
            string procName = "[dbo].[Computers_Update]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection paramCol)
            {
                AddCommomParams(model, paramCol);

                paramCol.AddWithValue("@Id", model.Id);

            }, returnParameters: null);
           
        }

        //ADD/INSERT
        public int Add(ComputerAddRequest model)
        {
            int id = 0;
            string procName = "[dbo].[Computers_Insert]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection paramCol)
            {
                AddCommomParams(model, paramCol);

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                paramCol.Add(idOut);

            }, returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object objId = returnCollection["@Id"].Value;

                int.TryParse(objId.ToString(), out id);

            });

            return id;        
        }

        private static void AddCommomParams(ComputerAddRequest model, SqlParameterCollection paramCol)
        {
            paramCol.AddWithValue("@Name", model.Name);
            paramCol.AddWithValue("@Model", model.Model);
            paramCol.AddWithValue("@Storage", model.Storage);
            paramCol.AddWithValue("@PrimaryImageUrl", model.PrimaryImageUrl);
            paramCol.AddWithValue("@Year", model.Year);
            paramCol.AddWithValue("@IsUsed", model.IsUsed);
        }

        //GET BY ID
        public Computer GetById(int id)
        {
            Computer computer = null;

            string procName = "[dbo].[Computers_SelectById]";

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCol)
            {
                paramCol.AddWithValue("@Id", id);
            },
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                computer = SingleComputerMapper(reader);                               
            });

            return computer;
        }

        //GET ALL
        public List<Computer> GetAll()
        {
            List<Computer> computerList = null;

            string procName = "[dbo].[Computers_SelectAll]";

            _data.ExecuteCmd(procName, inputParamMapper: null,
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                Computer aComputer = SingleComputerMapper(reader);

                if (computerList == null)
                {
                    computerList = new List<Computer>();
                }

                computerList.Add(aComputer);
            });

            return computerList;
        }

        private static Computer SingleComputerMapper(IDataReader reader)
        {
            Computer aComputer = new Computer();

            int index = 0;

            aComputer.Id = reader.GetSafeInt32(index++);
            aComputer.Name = reader.GetSafeString(index++);
            aComputer.Model = reader.GetSafeString(index++);
            aComputer.Storage = reader.GetSafeString(index++);
            aComputer.PrimaryImageUrl = reader.GetSafeString(index++);
            aComputer.Year = reader.GetSafeInt32(index++);
            aComputer.IsUsed = reader.GetSafeBool(index++);
            aComputer.DateCreated = reader.GetSafeDateTime(index++);
            aComputer.DateModified = reader.GetSafeDateTime(index++);
            return aComputer;
        }

        //--------COMPUTER V3

        //SEARCH PAGINATE V3
        public Paged<ComputerV3> SearchPaginatedV3(int pageIndex, int pageSize, string query)
        {
            Paged<ComputerV3> pagedListV3 = null;
            List<ComputerV3> computerListV3 = null;
            int totalCount = 0;

            string procName = "[dbo].[Computers_Search_PaginationV3]";

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@PageIndex", pageIndex);
                paramCollection.AddWithValue("@PageSize", pageSize);
                paramCollection.AddWithValue("@Query", query);
            }
            , singleRecordMapper: delegate (IDataReader reader, short set)
            {
                ComputerV3 computerV3 = ComputerMapper(reader);
                totalCount = reader.GetSafeInt32(13);

                if (computerListV3 == null)
                {
                    computerListV3 = new List<ComputerV3>();
                }

                computerListV3.Add(computerV3);
            });

            if (computerListV3 != null)
            {
                pagedListV3 = new Paged<ComputerV3>(computerListV3, pageIndex, pageSize, totalCount);

            }
            return pagedListV3;
        }

        //PAGINATE V3
        public Paged<ComputerV3> PaginationV3(int pageIndex, int pageSize)
        { 
            Paged<ComputerV3> pagedListV3 = null;
            List<ComputerV3> computerListV3 = null;
            int totalCount = 0;

            string procName = "[dbo].[Computers_PaginationV3]";

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@PageIndex", pageIndex);
                paramCollection.AddWithValue("@PageSize", pageSize);
            }
            , singleRecordMapper: delegate (IDataReader reader, short set)
            {
                ComputerV3 computerV3 = ComputerMapper(reader);
                totalCount = reader.GetSafeInt32(13);

                if (computerListV3 == null)
                {
                    computerListV3 = new List<ComputerV3>();
                }

                computerListV3.Add(computerV3);
            });

            if (computerListV3 != null)
            {
                pagedListV3 = new Paged<ComputerV3>(computerListV3, pageIndex, pageSize, totalCount);

            }
            return pagedListV3;
        }

        //GET/SELECT BY ID V3
        public ComputerV3 GetByIdV3(int id)
        {
           ComputerV3 computerV3 = null;

            string procName = "[dbo].[Computers_GetByIdV3]";

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            }
            , singleRecordMapper: delegate (IDataReader reader, short set)
            {
                computerV3 = ComputerMapper(reader);
            });

            return computerV3;
        }

        //GET/SELECT ALL V3
        public List<ComputerV3> GetAllV3() 
        {
            List<ComputerV3> computerListV3 = null;

            string procName = "[dbo].[Computers_SelectAllV3]";

            _data.ExecuteCmd(procName, inputParamMapper: null
            , singleRecordMapper: delegate (IDataReader reader, short set)
            {
                ComputerV3 computerV3 = ComputerMapper(reader);

                if (computerListV3 == null)
                {
                    computerListV3 = new List<ComputerV3>();
                }

                computerListV3.Add(computerV3);
            });

            return computerListV3;
        }

        private static ComputerV3 ComputerMapper(IDataReader reader)
        {
            ComputerV3 computerV3 = new ComputerV3();
            computerV3.Image = new ComputerImage();
            computerV3.Monitors = new List<Monitor>();

            int index = 0;

            computerV3.Id = reader.GetSafeInt32(index++);
            computerV3.Name = reader.GetSafeString(index++);
            computerV3.Model = reader.GetSafeString(index++);
            computerV3.Storage = reader.GetSafeString(index++);
            computerV3.Image.Id = reader.GetSafeInt32(index++);
            computerV3.Year = reader.GetSafeInt32(index++);
            computerV3.IsUsed = reader.GetSafeBool(index++);
            computerV3.UserId = reader.GetSafeInt32(index++);
            computerV3.Image.TypeId = reader.GetSafeInt32(index++);
            computerV3.Image.Url = reader.GetSafeString(index++);

            computerV3.Monitors = reader.DeserializeObject<List<Monitor>>(index++);
            computerV3.DateCreated = reader.GetSafeDateTime(index++);
            computerV3.DateModified = reader.GetSafeDateTime(index++);

            return computerV3;
        }
    }

}
