﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                        select EmployeeId,EmployeeName,Department,
                        DATE_FORMAT(DateOfJoining,'%Y-%m-%d') as DateOfJoining,
                        PhotoFileName
                        from 
                        Employee
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("ConnectionString");
            MySqlDataReader myReader;
            //using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            using (MySqlConnection mycon = new MySqlConnection("server=127.0.0.1;port=3306;database=mytestdb;uid=root;password=Haidv1234*;"))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult(table);
        }


        [HttpPost]
        public JsonResult Post(Employee emp)
        {
            string query = @"
                        insert into Employee 
                        (EmployeeName,Department,DateOfJoining,PhotoFileName) 
                        values
                         (@EmployeeName,@Department,@DateOfJoining,@PhotoFileName) ;
                        
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("ConnectionString");
            MySqlDataReader myReader;
            //using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            using (MySqlConnection mycon = new MySqlConnection("server=127.0.0.1;port=3306;database=mytestdb;uid=root;password=Haidv1234*;"))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
                    myCommand.Parameters.AddWithValue("@Department", emp.Department);
                    myCommand.Parameters.AddWithValue("@DateOfJoining", emp.DateOfJoining);
                    myCommand.Parameters.AddWithValue("@PhotoFileName", emp.PhotoFileName);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }


        [HttpPut]
        public JsonResult Put(Employee emp)
        {
            string query = @"
                        update Employee set 
                        EmployeeName =@EmployeeName,
                        Department =@Department,
                        DateOfJoining =@DateOfJoining,
                        PhotoFileName =@PhotoFileName
                        where EmployeeId=@EmployeeId;
                        
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("ConnectionString");
            MySqlDataReader myReader;
            //using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            using (MySqlConnection mycon = new MySqlConnection("server=127.0.0.1;port=3306;database=mytestdb;uid=root;password=Haidv1234*;"))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeId", emp.EmployeeId);
                    myCommand.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
                    myCommand.Parameters.AddWithValue("@Department", emp.Department);
                    myCommand.Parameters.AddWithValue("@DateOfJoining", emp.DateOfJoining);
                    myCommand.Parameters.AddWithValue("@PhotoFileName", emp.PhotoFileName);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Updated Successfully");
        }



        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                        delete from Employee 
                        where EmployeeId=@EmployeeId;
                        
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("ConnectionString");
            MySqlDataReader myReader;
            //using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            using (MySqlConnection mycon = new MySqlConnection("server=127.0.0.1;port=3306;database=mytestdb;uid=root;password=Haidv1234*;"))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeId", id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Deleted Successfully");
        }


        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {

                var httpReuest = Request.Form;
                var postedFile = httpReuest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception)
            {

                return new JsonResult("haind.png");
            }
        }
    }
}