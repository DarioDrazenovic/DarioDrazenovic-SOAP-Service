using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Xml.Linq;
using static DarioDrazenovicSOAP_IIS.EsportsTeamArray;

namespace DarioDrazenovicSOAP_IIS
{
    /// <summary>
    /// Summary description for ServiceSOAP
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ServiceSOAP : System.Web.Services.WebService
    {
        EsportsTeamArray esportsTeamArray = new EsportsTeamArray();
        List<EsportsTeam> esportsTeamList = new List<EsportsTeam>();
        EsportsTeam et1 = new EsportsTeam("1", "League of Legends", "G2 Esports", 320000);
        EsportsTeam et2 = new EsportsTeam("2", "League of Legends", "Fnatic", 204000);
        EsportsTeam et3 = new EsportsTeam("3", "League of Legends", "Vitality", 105300);
        EsportsTeam et4 = new EsportsTeam("4", "CS:GO", "Faze Clan", 850500);
        EsportsTeam et5 = new EsportsTeam("5", "CS:GO", "G2 Esports", 250850);


        [WebMethod]
        public List<EsportsTeam> EntitySearch(string word)
        {
            esportsTeamList.Add(et1);
            esportsTeamList.Add(et2);
            esportsTeamList.Add(et3);
            esportsTeamList.Add(et4);
            esportsTeamList.Add(et5);

            esportsTeamArray.EsportsTeamList = esportsTeamList;

            DataContractSerializer dcSerializer = new DataContractSerializer(typeof(EsportsTeamArray));
            string filePath = @"D:\DarioDrazenovicIIS Projekt\EsportsTeamSOAP.xml";

            using (FileStream writer = new FileStream(filePath, FileMode.Create))
            {
                dcSerializer.WriteObject(writer, esportsTeamArray);
                writer.Close();
            }

            XmlDocument documentXML = new XmlDocument();
            // loads an XML document from a file at filePath, and creates an XmlDocument object "documentXML" from it.
            documentXML.Load(filePath);
            XmlNode root = documentXML.DocumentElement;

            XmlNamespaceManager xmlns = new XmlNamespaceManager(documentXML.NameTable);
            xmlns.AddNamespace("ns", "http://schemas.datacontract.org/2004/07/XSD-DarioDrazenovicIIS.Model");

            // Used to select nodes from the XML document using an XPath expression.
            // Xmlns -> passed as a second argument to the SelectNodes method, to provide the namespace information for the XPath expression.
            XmlNodeList xmlNodeList = root.SelectNodes("/ns:EsportsTeamArray/ns:EsportsTeamList/ns:EsportsTeam[ns:Type='" + word + "']", xmlns);

            List<EsportsTeam> result = new List<EsportsTeam>();
            for (int i = 0; i < xmlNodeList.Count; i++)
            {
                var currentNodeXml = "<EsportsTeam xmlns=\"http://schemas.datacontract.org/2004/07/XSD-DarioDrazenovicIIS.Model\">" + xmlNodeList[i].InnerXml + "</EsportsTeam>";
                Stream ms = new MemoryStream(Encoding.UTF8.GetBytes(currentNodeXml));

                DataContractSerializer ds = new DataContractSerializer(typeof(EsportsTeam));
                result.Add((EsportsTeam)ds.ReadObject(ms));
            }

            return result;
        }
    }
}