using ExpressionTreeTest.DataAccess.MSSQL;
using ExpressionTreeTest.DataAccess.MSSQL.Entities;
using ExpressionTreeTest.DataAccess.MSSQL.Models;
using ExpressionTreeTest.DataAccess.MSSQL.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;

namespace ExpressionTreeTest.Tests
{
    public class PhoneRepositoryInMemoryTest
    {
        public PhonesContext _phonesContext { get; set; }

        [OneTimeSetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PhonesContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .Options;
             
            _phonesContext = new PhonesContext(options);

            List<SimCardFormat> simCardFormats = new List<SimCardFormat>() {
                new SimCardFormat() { Id = 1, Name = "Полноразмерные (1FF)", Height = 85.6m, Width = 53.98m },
                new SimCardFormat() { Id = 2, Name = "Mini-SIM (2FF)", Height = 25m, Width = 15m },
                new SimCardFormat() { Id = 3, Name = "Micro-SIM (3FF)", Height = 15m, Width = 12m },
                new SimCardFormat() { Id = 4, Name = "Nano-SIM (4FF)", Height = 12.3m, Width = 8.8m },
                new SimCardFormat() { Id = 5, Name = "Встроенные SIM (Embedded-SIM)", Height = 6m, Width = 5m }
            };

            List<Phone> phones = new List<Phone>() {
                new Phone {Id=1, Name="DEXP A440", ReleaseYear =2021, SimCardCount = 2, SimCardFormatId =4, Color= "розовый", ScreenDiagonal = 4 },
                new Phone {Id=2, Name="Samsung Galaxy A72", ReleaseYear =2020, SimCardCount = 1, SimCardFormatId =3, Color= "лаванда", ScreenDiagonal = 6.7m },
                new Phone {Id=3, Name="POCO X3 Pro", ReleaseYear =2023, SimCardCount = 2, SimCardFormatId =5, Color= "бежевый", ScreenDiagonal = 6.67m },
                };

            _phonesContext.SimCardFormats.AddRange(simCardFormats);
            _phonesContext.Phones.AddRange(phones);
            _phonesContext.SaveChanges();
        }

        [Test]
        public void GetAllInformationByParams_ShouldReturnFilteredResult()
        {
            var phoneRepository = new PhoneRepository(_phonesContext, null);

            string fieldName = "Name";
            string filterType = "!contains";
            string fieldValue = "DEXP";

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

            Assert.NotNull(result);
        }

        [Test]
        public void GetAllInformationByParams_ShouldReturnFilteredResult2()
        {
            var phoneRepository = new PhoneRepository(_phonesContext, null);

            var queryParams = new QueryParams() {
                FilterParams = new List<FilterParam>()
                {
                     new FilterParam() {
                         FieldName = "Color",
                         FilterType = "equals",
                         FieldValue = "розовый"
                     },
                     new FilterParam() {
                         FieldName = "Color",
                         FilterType = "equals",
                         FieldValue = "лаванда"
                     },
                     new FilterParam() {
                         FieldName = "ScreenDiagonal",
                         FilterType = ">",
                         FieldValue = "6"
                     }
                 },
                FilterRule = "2 & (0 | 1)",
                OrderParams = null,
                PageNumber = 1,
                PageSize = 10
            };

            var result = phoneRepository.GetAllInformationByParams(queryParams).Result;

            Assert.NotNull(result);
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

            Assert.NotNull(result);
        }
    }
}
