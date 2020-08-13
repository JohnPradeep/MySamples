using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependentTFSTracking
{
    public class TfsItem
    {
        int m_tfsId;
        List<ImpactFileInfo> m_fileInfo;
        List<DependentTfs> m_dependentTfs;
        public int TfsID
        {
            get { return m_tfsId; }
            set { m_tfsId = value; }
        }
        public List<ImpactFileInfo> FilesInfo
        {
            get { return m_fileInfo; }
            set { m_fileInfo = value; }
        }

        public List<DependentTfs> DependentTfsList
        {
            get
            {
                if (m_dependentTfs == null)
                    m_dependentTfs = new List<DependentTfs>();
                return m_dependentTfs;
            }
            set { m_dependentTfs = value; }
        }
    }

    public class ImpactFileInfo
    {
        string m_fileName = string.Empty;
        DateTime m_checkInDate = DateTime.MinValue;
        string m_checkedInBy = string.Empty;

        public string FileName
        {
            get { return m_fileName; }
            set { m_fileName = value.ToLower().Replace(" ", ""); }
        }
        public DateTime CheckInDate
        {
            get { return m_checkInDate; }
            set { m_checkInDate = value; }
        }
        public string CheckedInBy
        {
            get { return m_checkedInBy; }
            set { m_checkedInBy = value; }
        }
    }

    public class DependentTfs
    {
        int m_tfsId;
        List<ImpactFileInfo> m_intersectFileInfo;
        public int TfsID
        {
            get { return m_tfsId; }
            set { m_tfsId = value; }
        }
        public List<ImpactFileInfo> IntersectFiles
        {
            get { return m_intersectFileInfo; }
            set { m_intersectFileInfo = value; }
        }
    }
}
