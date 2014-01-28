using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using Common.DA;
using Common.DA.Interface;
using Common.Types;

namespace GU.Archive.DataModel
{
    /// <summary>
    /// Корреспонденция
    /// </summary>
    [TableName("gu_archive.author")]
    public abstract class Author : IdentityDomainObject<Author>, IPersistentObject
    {
        [PrimaryKey]
        [MapField("author_id")]
        public abstract override int Id { get; set; }

        [MapField("name")]
        public abstract string Name { get; set; }
    }
}
