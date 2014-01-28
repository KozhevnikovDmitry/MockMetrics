using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GU.Archive.DataModel
{
    public enum RequestType
    {
        /// <summary>
        /// социально-правовые
        /// </summary>
        SocialLegal = 1,

        /// <summary>
        /// тематические
        /// </summary>
        Thematic = 2,

        /// <summary>
        /// генеалогические
        /// </summary>
        Genealogical = 3,

        /// <summary>
        /// иностранные граждане
        /// (????) здсь ли они или отдельным флагом в Post
        /// </summary>
        Aliens = 4,

        /// <summary>
        /// жалобы
        /// </summary>
        Complaint = 5,

        /// <summary>
        /// прочие
        /// </summary>
        Other = 6
    }
}
