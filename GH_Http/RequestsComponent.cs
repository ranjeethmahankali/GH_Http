using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

namespace GH_Http
{
    //This component sends the request
    public class RequestsComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public RequestsComponent()
          : base("Request", "Request","Can send the http requests","Data", "Data")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("URL", "U", "Url to which to send the request", GH_ParamAccess.item);
            pManager.AddTextParameter("Method", "M", "Method of the http request", GH_ParamAccess.item, "POST");
            pManager.AddGenericParameter("Data","D", "Data to send with the request", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Response", "R", "Response Text from the server", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string URL = null;
            string method = null;
            Dictionary<string, string> data = null;

            if(!DA.GetData(0, ref URL))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, "No Url received !");
                return;
            }

            if(!DA.GetData(1, ref method))
            {
                method = "POST";
            }

            if(!DA.GetData(2, ref data))
            {
                data = new Dictionary<string, string>();
            }

            string response = null;
            if(method == "POST")
            {
                //send a post request
                response = Http_Request.POST(URL, data);
            }
            else if (method == "GET")
            {
                //send a get request
                response = Http_Request.GET(URL, data);
            }
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Your method is not supported !");
                return;
            }

            DA.SetData(0, response);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{b112c7e1-fa07-4e0c-b514-fa1ea596255b}"); }
        }
    }

    //This component builds a dictionary
    public class BuildDataComponent : GH_Component, IGH_VariableParameterComponent
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public BuildDataComponent()
          : base("Build Data", "Build", "Builds a Dictionary", "Data", "Data")
        {
            Params.ParameterChanged += new GH_ComponentParamServer.ParameterChangedEventHandler(OnParametersChanged);
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("field_1", "F1", "Data fields to add to the dictionary", GH_ParamAccess.item);
            Params.Input[0].MutableNickName = true;
            Params.Input[0].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Dictionary", "D", "Dictionary build from data supplied", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();
            for(int i = 0; i < Params.Input.Count; i++)
            {
                string value = null;
                if(DA.GetData(i, ref value))
                {
                    output.Add(Params.Input[i].NickName, value);
                }
            }

            DA.SetData(0, output);
        }

        //attached to the parameter change event - triggered when nickname updated
        protected virtual void OnParametersChanged(object sender, GH_ParamServerEventArgs e)
        {
            ExpireSolution(true);
        }

        //these are for IGH_VariableParameterComponent Interface
        public bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            return (side == GH_ParameterSide.Input);
        }

        public bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            return (side == GH_ParameterSide.Input && Params.Input.Count > 1);
        }

        public IGH_Param CreateParameter(GH_ParameterSide side, int index)
        {
            Param_String newParam = new Param_String();
            int n = Params.Input.Count;
            newParam.Name = "field_" + (n + 1).ToString();
            newParam.NickName = "F" + (n + 1).ToString();
            newParam.MutableNickName = true;
            newParam.Optional = true;

            return newParam;
        }

        public bool DestroyParameter(GH_ParameterSide side, int index)
        {
            return true;
        }

        public void VariableParameterMaintenance()
        {
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{7cc30343-2848-4585-acea-b7403741551e}"); }
        }
    }
}