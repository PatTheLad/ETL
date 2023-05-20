using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;
using VereinsVerwaltung.Model;

namespace VereinsVerwaltung.Service
{
    public class Reader
    {
        public List<SingleData> XML(MemoryStream ms, string fileName, List<SingleData> data)   //converts stream from xml file to list of "singledata"
        {
            int interneId;
            try { interneId = data.Max(x => x.InterneId)+1; } catch { interneId = 1; }   //Gets first free id
            XmlDocument xmlDoc = new XmlDocument();
            ms.Flush();
            if (ms.Position > 0)
            {
                ms.Position = 0;
            }
            xmlDoc.Load(ms);
            XmlNodeList nodeList = xmlDoc.DocumentElement.ChildNodes;

            foreach (XmlNode node in nodeList)
            {
                foreach (XmlNode tmpNode in node.ChildNodes)
                {
                    SingleData dataType = new();
                    dataType.Header = tmpNode.Name.ToUpper();
                    dataType.Inhalt = tmpNode.InnerText;
                    dataType.InterneId = interneId;
                    dataType.FileName = fileName;
                    data.Add(dataType);
                }
                interneId++;
            }
            return data;
        }

        public List<SingleData> CSV(MemoryStream ms, string fileName, List<SingleData> data)   //converts stream from csv file to list of "singledata"
        {
            int interneId;
            try { interneId = data.Max(x => x.InterneId) + 1; } catch { interneId = 1; }     //Gets first free id
            var outputFileString = Encoding.UTF8.GetString(ms.ToArray());
            List<string> tmpHeaderList = new List<string>();
            bool firstLine = true;
            foreach (var item in outputFileString.Split(Environment.NewLine))
            {
                var line = item.Split(";").ToList().Select((v, i) => new { ColName = v, ColIndex = i });
                if(firstLine == true)
                {
                    foreach (var header in line)
                    {
                        tmpHeaderList.Add(header.ColName.ToUpper());
                    }
                    firstLine = false;
                }
                else if(item != "")
                {
                    int index = 0;
                    foreach (var value in line)
                    {
                        SingleData singleData = new();
                        singleData.Inhalt = value.ColName;
                        singleData.Header = tmpHeaderList[index];
                        singleData.InterneId = interneId;
                        singleData.FileName = fileName;
                        data.Add(singleData);
                        index++;
                    }
                    interneId++;
                }
            }
            return data;
        }

        public List<SingleData> JSON(MemoryStream ms, string fileName, List<SingleData> data)  //converts stream from json file to list of "singledata"
        {
            int interneId;
            try { interneId = data.Max(x => x.InterneId) + 1; } catch { interneId = 1; }    //Gets first free id
            var jsonString = Encoding.UTF8.GetString(ms.ToArray());
            var jsonObjects = JsonConvert.DeserializeObject<List<JObject>>(jsonString);
            foreach (var jsonObject in jsonObjects)
            {
                foreach (var property in jsonObject.Properties())
                {
                    SingleData singleData = new();
                    singleData.Inhalt = property.Value.ToString();
                    singleData.Header = property.Name;
                    singleData.InterneId = interneId;
                    singleData.FileName = fileName;
                    data.Add(singleData);
                }
                interneId++;
            }
            return data;
        }
    }
}
