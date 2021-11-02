using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpressionTreeTest.DataAccess.MSSQL.Entities
{
    public class Phone
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Наименование модели телефона.
        /// </summary>
        [Column(TypeName = "nvarchar(1000)")]
        public string Name { get; set; }

        /// <summary>
        /// Год релиза.
        /// </summary>
        public int? ReleaseYear { get; set; }

        /// <summary>
        /// Количество сим карт.
        /// </summary>
        public int? SimCardCount { get; set; }

        /// <summary>
        /// Формат сим карт. 
        /// Значение из справочника SimCardFormat.
        /// </summary>
        public int? SimCardFormatId { get; set; }

        /// <summary>
        /// Цвет телефона.
        /// </summary>
        [Column(TypeName = "nvarchar(100)")]
        public string Color { get; set; }

        /// <summary>
        /// Диагональ экрана в дюймах.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ScreenDiagonal { get; set; }

        ///и ещё куча полей которые можно взять отсюда - https://www.dns-shop.ru/catalog/17a8a01d16404e77/smartfony/
    }
}
