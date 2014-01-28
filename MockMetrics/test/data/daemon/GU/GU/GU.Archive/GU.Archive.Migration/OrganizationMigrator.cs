using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using GU.Archive.BL;
using GU.Archive.DataModel;

namespace GU.Archive.Migration
{
    public class OrganizationMigrator
    {
        public void Migrate()
        {
            try
            {
                var list = ImportData();
                var errlist = SaveData(list);

                string str = string.Empty;
                foreach (var err in errlist)
                {
                    str += string.Format("Наименование: {0}; Адрес: {1}", err.Key.FullName, err.Key.Address.ToString());
                    str += Environment.NewLine;
                }

                int c = errlist.Count;
            }
            catch (Exception ex)
            {
                string exstr = ex.ToString();
            }
        }

        private List<Organization> ImportData()
        {
            var fi = new FileInfo(@"C:\Work\gu\archive\organization_data\org-gakk.xls");
            var dt = ExcelHelper.ImportData(fi);

            var list = (from DataRow row in dt.Rows
                        select ExtractOrganization(row)).ToList();

            fi = new FileInfo(@"C:\Work\gu\archive\organization_data\org.xls");
            dt = ExcelHelper.ImportData(fi);

            var list2 = (from DataRow row in dt.Rows
                         select ExtractOrganization(row)).ToList();

            foreach (var o in list2)
            {
                if (list.Count(x => x.FullName == o.FullName) == 0)
                {
                    list.Add(o);
                }
            }

            return list;
        }

        private Dictionary<Organization, Exception> SaveData(List<Organization> list)
        {
            var errList = new Dictionary<Organization, Exception>();
            var mapper = ArchiveFacade.GetDataMapper<Organization>();

            using (var db = new ArchiveDbManager())
            {
                //db.BeginDomainTransaction();

                foreach (var o in list)
                {
                    try
                    {
                        mapper.Save(o);
                    }
                    catch(Exception ex)
                    {
                        string str = ex.ToString();
                        errList.Add(o, ex);
                    }
                }

                //db.CommitDomainTransaction();
            }

            return errList;
        }

        private Organization ExtractOrganization(DataRow dr)
        {
            var org = Organization.CreateInstance();
            org.Email = dr["email"].ToString().Trim();
            org.Fax = dr["fax"].ToString().Trim();
            org.FullName = dr["name"].ToString().Trim();
            org.ShortName = org.FullName.Length > 150 ? org.FullName.Substring(0, 150) : org.FullName;
            org.HeadName = dr["who"].ToString().Trim();
            org.Phone = dr["tel"].ToString().Trim();
            org.Address = ExtractAddress(dr);

            return org;
        }

        private Address ExtractAddress(DataRow dr)
        {
            var addr = Address.CreateInstance();
            var arr = dr["addr"].ToString().Split(',');
            addr.Country = arr[0].Trim();
            addr.CountryRegion = arr[1].Trim();
            addr.Area = arr[2].Trim();
            addr.City = arr[3].Trim();
            addr.Street = arr[5].Trim();
            addr.House = arr[6].Trim();
            addr.Flat = arr[8].Trim();
            addr.Zip = arr[9].Trim();
            addr.Note = string.Format("Миграция от {0}", DateTime.Now.ToShortDateString());

            return addr;
        }
    }
}
