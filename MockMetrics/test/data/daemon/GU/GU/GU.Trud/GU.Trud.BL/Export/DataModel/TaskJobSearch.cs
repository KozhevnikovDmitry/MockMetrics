using System;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using Common.DA.Interface;
using Common.Types;

namespace GU.Trud.BL.Export.DataModel
{
    /// <summary>
    /// Класс, предназначенный для хранения экпортных данных для интеграции с Катарсисом.
    /// </summary>
    [TableName("gu_trud.task_new_job_search_catharsis_v")]
    internal class TaskJobSearch : IToItemArray, IDomainObject
    {
        [MapField("agency_id")]
        public int AgencyId { get; set; }

        [MapField("task_id")]
        public int TaskId { get; set; }

        #region Export Data

        public int kpy_id { get; set; }
        public string namb { get; set; }
        public string regud { get; set; }
        public string raz { get; set; }
        public string old_raz { get; set; }
        public DateTime dobr { get; set; }
        public string fam { get; set; }
        public string ima { get; set; }
        public string otch { get; set; }
        public string v_p { get; set; }
        public DateTime dr { get; set; }
        public string region { get; set; }
        public string p_mra { get; set; }
        public string p_ra { get; set; }
        public string adres { get; set; }
        public string tele { get; set; }
        public string p_okz { get; set; }
        public string txt_obr { get; set; }
        public string txt_prof1 { get; set; }
        public string txt_prof { get; set; }
        public string txt_prof5 { get; set; }
        public decimal staj1 { get; set; }
        public decimal staj5 { get; set; }
        public decimal staj { get; set; }
        public string p_onv { get; set; }
        public string txt_prof0 { get; set; }
        public int zp_tr { get; set; }
        public string prof_tr { get; set; }
        public DateTime daz { get; set; }
        public string p_pz { get; set; }
        public string note { get; set; }
        public string email { get; set; }
        public string p_trv { get; set; }
        public DateTime date_add { get; set; }
        public DateTime datrep { get; set; }
        public DateTime dat_zr { get; set; }
        public int soct { get; set; }
        public int soct1 { get; set; }
        public int soct2 { get; set; }
        public string p_ouk { get; set; }
        public int gr_inv { get; set; }
        public string p_obr { get; set; }
        public string txt_uz_1 { get; set; }
        public DateTime do_1 { get; set; }

        #endregion
        
        #region IToItemArray

        public object[] ToItemArray()
        {
            return new object[] { kpy_id, namb, regud, raz, old_raz, dobr, fam, ima, otch, v_p, 
                                  dr, region, p_mra, p_ra, adres, tele, p_okz, txt_obr, txt_prof1, 
                                  txt_prof, txt_prof5, staj1, staj5, staj, p_onv, txt_prof0, zp_tr, 
                                  prof_tr, daz, p_pz, email, p_trv, date_add, datrep, dat_zr, soct,
                                  soct1, soct2, p_ouk, gr_inv, p_obr, txt_uz_1, do_1};
        }

        public string[] ToItemNameArray()
        {
            return new[] { Util.GetPropertyName(() => kpy_id), 
                           Util.GetPropertyName(() => namb),
                           Util.GetPropertyName(() => regud),
                           Util.GetPropertyName(() => raz),
                           Util.GetPropertyName(() => old_raz),
                           Util.GetPropertyName(() => dobr),
                           Util.GetPropertyName(() => fam),
                           Util.GetPropertyName(() => ima),
                           Util.GetPropertyName(() => otch),
                           Util.GetPropertyName(() => v_p),
                           Util.GetPropertyName(() => dr),
                           Util.GetPropertyName(() => region),
                           Util.GetPropertyName(() => p_mra),
                           Util.GetPropertyName(() => p_ra),
                           Util.GetPropertyName(() => adres),
                           Util.GetPropertyName(() => tele),
                           Util.GetPropertyName(() => p_okz),
                           Util.GetPropertyName(() => txt_obr),
                           Util.GetPropertyName(() => txt_prof1),
                           Util.GetPropertyName(() => txt_prof),
                           Util.GetPropertyName(() => txt_prof5),
                           Util.GetPropertyName(() => staj1),
                           Util.GetPropertyName(() => staj5),
                           Util.GetPropertyName(() => staj),
                           Util.GetPropertyName(() => p_onv),
                           Util.GetPropertyName(() => txt_prof0),
                           Util.GetPropertyName(() => zp_tr),
                           Util.GetPropertyName(() => prof_tr),
                           Util.GetPropertyName(() => daz),
                           Util.GetPropertyName(() => p_pz),
                           Util.GetPropertyName(() => email),
                           Util.GetPropertyName(() => p_trv),
                           Util.GetPropertyName(() => date_add),
                           Util.GetPropertyName(() => datrep),
                           Util.GetPropertyName(() => dat_zr),
                           Util.GetPropertyName(() => soct),
                           Util.GetPropertyName(() => soct1),
                           Util.GetPropertyName(() => soct2),
                           Util.GetPropertyName(() => p_ouk),
                           Util.GetPropertyName(() => gr_inv),
                           Util.GetPropertyName(() => p_obr),
                           Util.GetPropertyName(() => txt_uz_1),
                           Util.GetPropertyName(() => do_1)};
        }

        #endregion
        
        #region IDomainObject

        public string GetKeyValue()
        {
            throw new NotImplementedException();
        }

        public void SetKeyValue(object val)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
