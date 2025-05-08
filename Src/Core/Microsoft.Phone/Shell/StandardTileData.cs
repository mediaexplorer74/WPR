using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Phone.Shell
{
    public class StandardTileData
    {
        public void set_Count(ShellTileData data)
        {
            Debug.WriteLine("[i] set_Count data : " + data);
        }

        public void set_Count(string text)
        {
            Debug.WriteLine("[i] set_Count text : " + text);
        }

        public void set_Count(int? count)
        {
            Debug.WriteLine("[i] set_Count count : " + count);
        }

        public void set_BackBackgroundImage(System.Uri uri)
        {
            Debug.WriteLine("[i] set_BackBackgroundImage uri : " + uri);
        }



        public void set_BackContent(string text)
        {
            Debug.WriteLine("[i] set_BackContent text : " + text);
        }

        public void set_BackTitle(string text)
        {
            Debug.WriteLine("[i] set_BackTitle text : " + text);
        }

    }
}
