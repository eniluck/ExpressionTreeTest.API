using ExpressionTreeTest.DataAccess.MSSQL;
using ExpressionTreeTest.DataAccess.MSSQL.Models;
using ExpressionTreeTest.DataAccess.MSSQL.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ExpressionTreeTest.Tests
{
    public class PhoneRepositoryIntegrationTest
    {
         public const string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PhonesDB;Integrated Security=True;";
         public PhonesContext _phonesContext { get; set; }

         [SetUp]
         public void Setup()
         {
             // тесты в памяти ?
             // https://jimmybogard.com/avoid-in-memory-databases-for-tests/
             var optionsBuilder = new DbContextOptionsBuilder<PhonesContext>();
             var options = optionsBuilder.UseSqlServer(ConnectionString).Options;

             _phonesContext = new PhonesContext(options);
         }

         [Test]
         public void GetAllInformationByParams_ShouldReturnFilteredResult()
         {
             var phoneRepository = new PhoneRepository(_phonesContext, null);

             string fieldName = "Name";
             string filterType = "!null";
             string fieldValue = null;

             var queryParams = new QueryParams() {
                 FilterParams = new List<FilterParam>()
                 {
                     new FilterParam() {
                         FieldName = fieldName,
                         FilterType = filterType,
                         FieldValue = fieldValue
                     }
                 },
                 FilterRule = "0",
                 OrderParams = new List<OrderParam>() 
                 {
                     new OrderParam() {
                         FieldName = "ScreenDiagonal",
                         Order = OrderType.Asc
                     }
                 },
                 PageNumber = 1,
                 PageSize = 10
             };

             var result = phoneRepository.GetAllInformationByParams(queryParams).Result;

             Assert.AreEqual(1, 2);
         }

         [Test]
         public void GetAllInformationByParams_ShouldReturnFilteredResult2()
         {
             var phoneRepository = new PhoneRepository(_phonesContext, null);

             var queryParams = new QueryParams() {
                 FilterParams = new List<FilterParam>()
                 {
                     new FilterParam() {
                         FieldName = "Name",
                         FilterType = "!null",
                         FieldValue = null
                     },
                     new FilterParam() {
                         FieldName = "ReleaseYear",
                         FilterType = "<",
                         FieldValue = "2021"
                     },
                     new FilterParam() {
                         FieldName = "Name",
                         FilterType = "contains",
                         FieldValue = "DEXP"
                     }
                 },
                 FilterRule = "0 & (1 | 2)",
                 OrderParams = null,
                 PageNumber = 1,
                 PageSize = 10
             };

             var result = phoneRepository.GetAllInformationByParams(queryParams).Result;

             Assert.AreEqual(1, 2);
         }


         [Test]
         public void GetAllInformationByParams_FotTest()
         {
             var phoneRepository = new PhoneRepository(_phonesContext, null);

             string fieldName = "Name";
             string filterType = "!null";
             string fieldValue = null;

             var queryParams = new QueryParams() {
                 FilterParams = new List<FilterParam>()
                 {
                     new FilterParam() {
                         FieldName = fieldName,
                         FilterType = filterType,
                         FieldValue = fieldValue
                     }
                 },
                 FilterRule = "0",
                 OrderParams = null,
                 PageNumber = 1,
                 PageSize = 10
             };

             var result = phoneRepository.GetAllInformationByParams(queryParams).Result;

             Assert.AreEqual(1, 2);
         }
    }
}
