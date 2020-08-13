using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace DependentTFSTracking
{
    public class Program
    {
        static void Main(string[] args)
        {
            List<ImpactFile> HotFixImpactFiles = GetImpactFilesList(@"../../Data/Day0Day1.txt");
            List<ImpactFile> RecentChangesImpactFiles =
                 GetImpactFilesList(@"../../Data/After60ImpactFiles.txt");
            GenericEqualityComparer<ImpactFileInfo, string> comparer = new GenericEqualityComparer<ImpactFileInfo, string>(x => x.FileName);
            var imapctFilesByIdHF = HotFixImpactFiles.GroupBy(f => f.TfsID).Select(g =>
                                    new TfsItem
                                    {
                                        TfsID = g.Key,
                                        FilesInfo = g.Select(info => new ImpactFileInfo
                                        {
                                            FileName = info.FileName,
                                            CheckedInBy = info.CheckedInBy,
                                            CheckInDate = info.CheckInDate
                                        }).OrderByDescending(date => date.CheckInDate)
                                        .Distinct(comparer).ToList()
                                    });
            var recentTFSItems = RecentChangesImpactFiles.GroupBy(f => f.TfsID).Select(g =>
                                new TfsItem
                                {
                                    TfsID = g.Key,
                                    FilesInfo = g.Select(info => new ImpactFileInfo
                                    {
                                        FileName = info.FileName,
                                        CheckedInBy = info.CheckedInBy,
                                        CheckInDate = info.CheckInDate
                                    }).OrderByDescending(date => date.CheckInDate)
                                    .Distinct(comparer).ToList()
                                });
            DependencyList<TfsItem> dependencyList = new DependencyList<TfsItem>();
            GetDeepDependency(imapctFilesByIdHF, recentTFSItems, dependencyList);
            DependencyList<TfsItem> sortedTfsList = Sort(dependencyList, x => x.DependentTfsList, (x) => { return dependencyList.Where(y => y.TfsID == x.TfsID).FirstOrDefault(); }, x => x.TfsID);
            List<DependencyList<TfsItem>> groupDepList = Group(dependencyList, x => x.DependentTfsList, (x) => { return dependencyList.Where(y => y.TfsID == x.TfsID).FirstOrDefault(); }, new GenericEqualityComparer<TfsItem, int>(x => x.TfsID));
            string xml = GetXMLFromObject<DependencyList<TfsItem>>(dependencyList);
            Console.Read();
        }

        static void GetDeepDependency(IEnumerable<TfsItem> inputList, IEnumerable<TfsItem> compareList, DependencyList<TfsItem> dependencyList)
        {
            List<TfsItem> nextIterationInput = new List<TfsItem>();
            foreach (var input in inputList)
            {
                foreach (var compare in compareList)
                {
                    IEnumerable<ImpactFileInfo> intersectFiles = compare.FilesInfo.Intersect(input.FilesInfo, new GenericEqualityComparer<ImpactFileInfo, string>(x => x.FileName));
                    if (intersectFiles.Count() > 0)
                    {
                        input.DependentTfsList.Add(new DependentTfs() { TfsID = compare.TfsID, IntersectFiles = intersectFiles.ToList() });
                        nextIterationInput.Add(compare);
                    }
                }
                dependencyList.Add(input);
            }
            if (nextIterationInput.Count > 0)
            { 
                GenericEqualityComparer<TfsItem, int> genericComparer = new GenericEqualityComparer<TfsItem, int>(x => x.TfsID);
                nextIterationInput = nextIterationInput.Distinct(genericComparer).ToList();
                GetDeepDependency(nextIterationInput, compareList.Except(nextIterationInput, genericComparer), dependencyList);
            }
        }
        static List<ImpactFile> GetImpactFilesList(string fileName)
        {
            List<ImpactFile> impactFiles = new List<ImpactFile>();
            using (StreamReader reader = new StreamReader(fileName))
            {
                string line = string.Empty;
                bool isFirstLine = true;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] values = line.Split(new string[] { "\t" }, StringSplitOptions.None);
                    if (!isFirstLine && values.Length == 7)
                    {
                        impactFiles.Add(new ImpactFile(values));
                    }
                    isFirstLine = false;
                }
            }
            return impactFiles;
        }

        public static string GetXMLFromObject<T>(DependencyList<TfsItem> depndencyList)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter tw = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                tw = new XmlTextWriter(sw);
                serializer.Serialize(tw, depndencyList);
            }
            catch (Exception ex)
            {
                //Handle Exception Code
            }
            finally
            {
                sw.Close();
                if (tw != null)
                {
                    tw.Close();
                }
            }
            return sw.ToString();
        }

        #region Topological Sorting

        public static DependencyList<T> Sort<T,D, TKey>(DependencyList<T> source, Func<T, IEnumerable<D>> getDependencies, Func<D,T> getDependecyDetail, Func<T, TKey> getKey)
        {
            return Sort(source, getDependencies, getDependecyDetail, new GenericEqualityComparer<T, TKey>(getKey));
        }

        public static DependencyList<T> Sort<T, D, TKey>(DependencyList<T> source, Func<T, IEnumerable<D>> getDependencies, Func<D, T> getDependecyDetail, GenericEqualityComparer<T,TKey> comparer)
        {
            var sorted = new DependencyList<T>();
            var visited = new Dictionary<T, bool>(comparer);

            foreach (var item in source)
            {
                Visit(item, getDependencies, getDependecyDetail, sorted, visited);
            }

            return sorted;
        }

        public static void Visit<T,D>(T item, Func<T, IEnumerable<D>> getDependencies, Func<D,T> getDependecyDetail, DependencyList<T> sorted, Dictionary<T, bool> visited)
        {
            bool inProcess;
            var alreadyVisited = visited.TryGetValue(item, out inProcess);

            if (alreadyVisited)
            {
                if (inProcess)
                {
                    throw new ArgumentException("Cyclic dependency found.");
                }
            }
            else
            {
                visited[item] = true;

                var dependencies = getDependencies(item);
                if (dependencies != null)
                {
                    foreach (var dependency in dependencies)
                    {
                        Visit(getDependecyDetail(dependency), getDependencies, getDependecyDetail, sorted, visited);
                    }
                }

                visited[item] = false;
                sorted.Add(item);
            }
        }
        #endregion

        #region Group
        public static List<DependencyList<T>> Group<T,D,TKey>(DependencyList<T> depList,Func<T,IEnumerable<D>> getDependencies, Func<D,T> getDependencyDetail, GenericEqualityComparer<T,TKey> comparer = null)
        {
            Dictionary<T,int> visited = new Dictionary<T, int>(comparer);
            List<DependencyList<T>> sorted = new List<DependencyList<T>>();
            foreach (var v in depList)
            {
                Visit(v, getDependencies, getDependencyDetail, visited, sorted);
            }
            return sorted;
        }

        public static int Visit<T,D>(T item, Func<T, IEnumerable<D>> getDependencies, Func<D, T> getDependencyDetail, Dictionary<T, int> visited, List<DependencyList<T>> sorted)
        {
            const int inProcess = -1;
            int level;
            var alreadyVisited = visited.TryGetValue(item, out level);
            if(alreadyVisited)
            {
                if (level == inProcess)
                    throw new ArgumentException("Circular dependency detected");
            }
            else
            {
                visited[item] = (level = inProcess);
                var dependencies = getDependencies(item);
                foreach (var dependency in dependencies)
                {
                    var depLevel = Visit(getDependencyDetail(dependency), getDependencies, getDependencyDetail, visited, sorted);
                    level = Math.Max(depLevel, level);
                }
                visited[item] = ++level;
                while (level >= sorted.Count)
                {
                    sorted.Add(new DependencyList<T>());
                }
                sorted[level].Add(item);
            }

            return level;
        }
        #endregion
    }

}
