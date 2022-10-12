﻿using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
namespace OrmTest
{
    public class UCustom22
    {

        public static void Init()
        {
            var db = NewUnitTest.Db;



            //建表 

            if (!db.DbMaintenance.IsAnyTable("UnitTest0011", false))

            {

                db.CodeFirst.InitTables<Test0011>();

            }

            db.Insertable(new List<Test0011>() { new Test0011 { id = 1, age = 1 }, new Test0011 { id = 2, age = 2 } }).ExecuteCommand();



            //用例代码        

            Search<Test0011>(db);
            db.CurrentConnectionConfig.MoreSettings = new ConnMoreSettings()
            {
                 IsWithNoLockQuery=true
            };
            db.Queryable<Test0011>()
                .Select<Test0011>()
                .MergeTable().LeftJoin<Test0011>((x, y) => true)
                .ToList();


            db.CodeFirst.InitTables(typeof(Tree));
            db.DbMaintenance.TruncateTable("tree");
            db.Insertable(new Tree() { Id = 1, Name = "root" }).ExecuteCommand();
            db.Insertable(new Tree() { Id = 11, Name = "child1", ParentId = 1 }).ExecuteCommand();
            db.Insertable(new Tree() { Id = 12, Name = "child2", ParentId = 1 }).ExecuteCommand();
            db.Insertable(new Tree() { Id = 2, Name = "root" }).ExecuteCommand();
            db.Insertable(new Tree() { Id = 22, Name = "child3", ParentId = 2 }).ExecuteCommand();

            var list = db.Queryable<Tree>().ToChildList(it => it.ParentId, 1);
            if (list.Count != 3) 
            {
                throw new Exception("unit error");
            }

            var list2 = db.Queryable<Tree>().ToChildList(it => it.ParentId, 1,false);
            if (list2.Count !=2)
            {
                throw new Exception("unit error");
            }

        }



        public static List<T> Search<T>(SqlSugarClient db) where T : ISearch

        {

            var searchReq = new { age = 1 };

            return db.Queryable<T>().Where(u => u.age == searchReq.age).ToList();

        }



        //用例实体
        [SugarTable("UnitTest0011")]
        public class Test0011 : ISearch

        {

            public int id { get; set; }

            public int age { get; set; }

        }



        public interface ISearch

        {

            int age { get; set; }

        }
    }


}
