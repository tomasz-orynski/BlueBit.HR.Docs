using System;
using System.Collections.Generic;
using System.Linq;
using System.DirectoryServices;
using NHibernate;
using NHibernate.Cfg;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using System.Configuration;

namespace BlueBit.HR.Docs.BL.DataLayer.Cfg
{
    public static class Defines
    {
        public const int FLD_LEN_IDENTIFIER = 100;
        public const int FLD_LEN_PESEL = 11;
        public const int FLD_LEN_PIN = 10;
        public const int FLD_LEN_PIN_MIN = 4;
        public const int FLD_LEN_PIN_MAX = FLD_LEN_PIN;

        private class Item
        {
            public Type EntityType;
            public Type MapType;
            public string TableName;

            public static Item Create<TEntity, TMap>(string tableName)
                where TEntity: Entities.Commons.IObjectInDBWithID
                where TMap: Mappings.Commons.ObjectInDBWithIDMap<TEntity>
            {
                return new Item() { EntityType = typeof(TEntity), MapType = typeof(TMap), TableName = tableName };
            }
        }

        private static readonly IList<Item> items = new List<Item>() 
        {
            Item.Create<Entities.Employee,                          Mappings.EmployeeMap>                       ("T_Employees"),
            Item.Create<Entities.DocumentWithData,                  Mappings.DocumentWithDataMap>               ("T_Documents"),
            Item.Create<Entities.DocumentWithoutDataAndLastVer,     Mappings.DocumentWithoutDataAndLastVerMap>  ("V_DocumentsLastVer"),
            Item.Create<Entities.DocumentsLoad,                     Mappings.DocumentsLoadMap>                  ("T_DocumentsLoads"),
            Item.Create<Entities.Session,                           Mappings.SessionMap>                        ("T_Sessions"),
            Item.Create<Entities.SessionDocumentGet,                Mappings.SessionDocumentGetMap>             ("T_SessionDocumentGets"),
        };
        private static readonly IDictionary<Type, string> entityType2TableName = items.ToDictionary(i => i.EntityType, i => i.TableName);

        private static void ApplyMappings(MappingConfiguration mappingConfiguration)
        {
            var fm = mappingConfiguration.FluentMappings;
            foreach (var item in items)
                fm.Add(item.MapType);
        }

        public static ISessionFactory CreateSessionFactory()
        {
            var configuration = Fluently
                .Configure()
                .Database(MsSqlConfiguration.MsSql2008
                        .UseOuterJoin()
                        .ConnectionString(x => x.FromConnectionStringWithKey("HRDOCS_DB")))
                .Mappings(ApplyMappings)
                .BuildConfiguration();

            return configuration.BuildSessionFactory();
        }

        public static IEnumerable<DirectorySearcher> CreateDirectorySearchers()
        {
            for (var i = 0; i < ConfigurationManager.ConnectionStrings.Count; ++i)
            {
                var conStr = ConfigurationManager.ConnectionStrings[i];
                if (conStr.Name.StartsWith("HRDOCS_AD"))
                    yield return new DirectorySearcher(new DirectoryEntry(conStr.ConnectionString));
            }
        }

        internal static string GetTableName<T>() where T : Entities.Commons.IObjectInDB { return entityType2TableName[typeof(T)]; }
    }
}
