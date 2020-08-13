using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependentTFSTracking
{
    public class ImpactFile
    {
        string m_fileName = string.Empty;
        string m_changeItemType = string.Empty;
        DateTime m_checkInDate = DateTime.MinValue;
        string m_checkedInBy = string.Empty;
        int m_tfsId;
        int m_changeSetID;
        int m_mergeReqID;
        public string FileName
        {
            get { return m_fileName; }
        }
        public string ChangeItemType
        {
            get { return m_changeItemType; }
        }
        public DateTime CheckInDate
        {
            get { return m_checkInDate; }
        }
        public string CheckedInBy
        {
            get { return m_checkedInBy; }
        }
        public int TfsID
        {
            get { return m_tfsId; }
        }
        public int ChangeSetID
        {
            get { return m_changeSetID; }
        }
        public int MergeRequestID
        {
            get { return m_mergeReqID; }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileDetails"></param>
        public ImpactFile(string[] fileDetails)
        {
            m_fileName = fileDetails[0];
            m_changeItemType = fileDetails[1];
            m_checkInDate = Convert.ToDateTime(fileDetails[2]);
            m_checkedInBy = fileDetails[3];
            m_tfsId = Convert.ToInt32(fileDetails[4]);
            m_changeSetID = Convert.ToInt32(fileDetails[5]);
            m_mergeReqID = Convert.ToInt32(fileDetails[6]);

        }

    }
}
