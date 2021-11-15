using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTreeTest.DataAccess.MSSQL.Models
{
    /// <summary>
    /// Параметр фильтрации.
    /// </summary>
    public class FilterParam
    {
        /// <summary>
        /// Имя поля для фильтрации.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Тип фильтра.
        ///   В зависимости от типа принимаемого полем, можно выбирать определённый фильтр.
        ///     
        ///   равенство       :    equals
        ///   неравенство     :    !equals
        ///   пустая строка   :    blank
        ///   не пустая строка:    !blank
        ///   null            :    null
        ///   не null         :    !null
        ///   содержит        :    contains
        ///   не содержит     :    !contains
        ///   начинается      :    starts
        ///   не начинается   :    !starts
        ///   заканчивается   :    ends
        ///   не заканчивается:    !ends
        /// </summary>
        public IFilter FilterType { get; set; }

        /// <summary>
        /// Значение фильтра в зависимости от поля ( его типа ).
        /// </summary>
        public string FieldValue { get; set; }
    }
}
