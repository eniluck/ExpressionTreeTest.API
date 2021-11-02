using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTreeTest.DataAccess.MSSQL.Entities
{
    /// <summary>
    /// https://www.protarif.info/news/new?id=3565
    /// https://ru.wikipedia.org/wiki/%D0%A1%D0%B8%D0%BC-%D0%BA%D0%B0%D1%80%D1%82%D0%B0
    /// </summary>
    public class SimCardFormat
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Название формата.
        /// </summary>
        [Column(TypeName = "nvarchar(1000)")]
        public string Name { get; set; }

        /// <summary>
        /// Длина в милиметрах.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Height { get; set; }

        /// <summary>
        /// Ширина в милиметрах.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Width { get; set; }
    }
}
