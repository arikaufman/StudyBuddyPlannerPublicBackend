using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using plannerBackEnd.Common.sqlTools;
using System.Collections.Generic;
using System.Data;
using plannerBackEnd.Common;

namespace plannerBackEnd.Faculties
{
    [EntryFilter]
    [ApiController]
    [Route("api/[controller]")]
    public class FacultiesController : Controller
    {
        private readonly IMapper mapper;
        private SqlConnection sqlConnection;
        private SqlTools sqlTools;

        // -----------------------------------------------------------------------------

        public FacultiesController(
            IMapper mapper,
            SqlConnection sqlConnection,
            SqlTools sqlTools
        )
        {
            this.mapper = mapper;
            this.sqlConnection = sqlConnection;
            this.sqlTools = sqlTools;
        }

        // -----------------------------------------------------------------------------
        // POST: api/faculties/list
        [HttpPost("list")]
        public List<Faculty> GetList()
        {
            return getList();
        }

        // -----------------------------------------------------------------------------

        private List<Faculty> getList()
        {
            string query = @"SELECT *, (SELECT COUNT(*) FROM userprofile WHERE facultyId = F.id) AS `numberofstudents`
                                 FROM faculties F";

            List<Faculty> filterResponse = new List<Faculty>();


            DataTable dataTable = sqlTools.GetTable(query);

            if (dataTable != null)
            {
                filterResponse = sqlTools.ConvertDataTable<Faculty>(dataTable);
            }

            return filterResponse;
        }
    }
}