using VereinsVerwaltung.Model;

namespace VereinsVerwaltung.Service
{
    public class Writer
    {
        public MemoryStream XML(List<SingleData> data)  //Uses our data list to create a memory stream for a xml file
        {
            MemoryStream memoryStream = new();
            StreamWriter streamWriter = new(memoryStream);
            streamWriter.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            streamWriter.WriteLine("<etl>");
            for (int counter = 1; counter < (data.Max(x => x.InterneId) + 1); counter++)
            {
                streamWriter.WriteLine("  <object>");
                foreach (string? header in data.Select(x => x.Header).Distinct())
                {
                    var tmp = data.Where(x => x.Header == header && x.InterneId == counter).FirstOrDefault();
                    if (tmp != null)
                    {
                        streamWriter.WriteLine("    <" + header + ">"  + tmp.Inhalt + "</" + header + ">");
                    }
                    else
                    {
                        streamWriter.WriteLine("    <" + header + ">" + "" + "</" + header + ">");
                    }
                }
                streamWriter.WriteLine("  </object>");
            }
            streamWriter.WriteLine("<etl>");
            streamWriter.Flush();
            memoryStream.Position = 0;
            return memoryStream;
        }

        public MemoryStream CSV(List<SingleData> data)  //Uses our data list to create a memory stream for a csv file
        {
            bool rowStart = true;
            MemoryStream memoryStream = new();
            StreamWriter streamWriter = new(memoryStream);
            string headerString = "";
            foreach (string? header in data.Select(x => x.Header).Distinct())
            {
                if (!rowStart)
                {
                    headerString += ";";
                }
                else
                {
                    rowStart = false;
                }
                headerString += header;
            }
            streamWriter.WriteLine(headerString);

            for (int counter = 1; counter < (data.Max(x => x.InterneId) + 1); counter++)
            {
                string rowString = "";
                rowStart = true;
                foreach (string? header in data.Select(x => x.Header).Distinct())
                {
                    if (!rowStart)
                    {
                        rowString += ";";
                    }
                    else
                    {
                        rowStart = false;
                    }
                    var tmp = data.Where(x => x.Header == header && x.InterneId == counter).FirstOrDefault();
                    if(tmp != null)
                    {
                        rowString += tmp.Inhalt;
                    }
                    else
                    {
                        rowString += "";
                    }
                    
                }
                streamWriter.WriteLine(rowString);
            }
            streamWriter.Flush();
            memoryStream.Position = 0;
            return memoryStream;
        }

        public MemoryStream JSON(List<SingleData> data) //Uses our data list to create a memory stream for a json file
        {
            bool rowStart = true;
            MemoryStream memoryStream = new();
            StreamWriter streamWriter = new(memoryStream);
            streamWriter.WriteLine("[");
            for (int counter = 1; counter < (data.Max(x => x.InterneId) + 1); counter++)
            {
                string rowString = "{";
                rowStart = true;
                foreach (string? header in data.Select(x => x.Header).Distinct())
                {
                    if (!rowStart)
                    {
                        rowString += ",";
                    }
                    else
                    {
                        rowStart = false;
                    }
                    var tmp = data.Where(x => x.Header == header && x.InterneId == counter).FirstOrDefault();
                    if (tmp != null)
                    {
                        rowString += "\""+header + "\"" + ":" + "\"" + tmp.Inhalt + "\"";
                    }
                    else
                    {
                        rowString += "\"" + header + "\"" + ":" + "\"" + "" + "\"";
                    }
                }
                rowString += "}";
                if(data.Max(x => x.InterneId) != counter)
                {
                    rowString += ",";
                }
                streamWriter.WriteLine(rowString);
            }
            streamWriter.WriteLine("]");
            streamWriter.Flush();
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
