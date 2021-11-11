using ExpressionTreeTest.DataAccess.MSSQL;
using ExpressionTreeTest.DataAccess.MSSQL.Models;
using ExpressionTreeTest.DataAccess.MSSQL.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ExpressionTreeTest.Tests
{
    public class PhoneRepositoryTest
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
            FilterType filterType = FilterType.NotNull;
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
                filterConditions = "0",
                OrderParams = null,
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
                        FilterType = FilterType.NotNull,
                        FieldValue = null
                    },
                    new FilterParam() {
                        FieldName = "ReleaseYear",
                        FilterType = FilterType.LessThan,
                        FieldValue = "2021"
                    },
                    new FilterParam() {
                        FieldName = "Name",
                        FilterType = FilterType.Contains,
                        FieldValue = "DEXP"
                    }
                },
                filterConditions = "0 & (1 | 2)",
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
            FilterType filterType = FilterType.NotNull;
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
                filterConditions = "0",
                OrderParams = null,
                PageNumber = 1,
                PageSize = 10
            };

            var result = phoneRepository.GetAllInformationByParams(queryParams).Result;

            Assert.AreEqual(1, 2);
        }
    }
}
