using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanillaForKonata.Util
{
    public class PicHTMLBuilder
    {
        /*
         * template
         <html><body>
            <p>%VAR:valueA%</p>
            <p>%VAR:valueB%</p>
            <div>
                %HTML:contentA%
            </div>
         </body><html>
         */
        string HTML="<html></html>";
        string css = "<style></style>";
        public PicHTMLBuilder(string html)
        {
            if (!html.Contains("<html>")&& !html.Contains("<style>"))
            {
                HTML = html;
                css = "";
            }
            else
            {
                HTML = "<html>" + Text.TextGainCenter("<html>", "</html>", html) + "</html>";
                css = "<style>" + Text.TextGainCenter("<style>", "</style>", html) + "</style>";
            }
           

        }
        public void setValue(string key,string value) {
            HTML=HTML.Replace($"%VAR:{key}%",value);
        }
        public void putHTML(string key,string content) {
            HTML=HTML.Replace($"%HTML:{key}%",content);
        }
        public string getHTML()
            => HTML + css;
        public static string pathConverter(string path)
            =>"file:///" + path.Replace("\\", "/");
        public static string getContent(string Html)
            => Text.TextGainCenter("<html>", "</html>", Html);
    }
}
