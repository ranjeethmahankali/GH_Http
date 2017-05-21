using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace GH_Http
{
    public class GH_HttpInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "GHHttp";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("bf886fb1-0d71-48bb-ad10-6016a51a378d");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
