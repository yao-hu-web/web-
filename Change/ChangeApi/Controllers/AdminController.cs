using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChangeApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChangeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<AdminUser> Get()
        {
            string connString = "server=.;database=OLSite;Uid=sa;pwd=123456";
            List<AdminUser> AdminUserList = new List<AdminUser>();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string sql = "select * from Product";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    AdminUserList.Add(new AdminUser
                    {
                        AdminUserId = reader.GetInt32(0),
                        UserName = reader.GetString(50),
                        Pwd = reader.GetString(50),
                        Aurole = reader.GetInt32(2)
                    });
                }
                cmd.Connection.Close();
            }
            return AdminUserList;
        }
    }
}