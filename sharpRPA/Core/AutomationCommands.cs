﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using SHDocVw;
using System.Data;
using MSHTML;
using System.Windows.Automation;
using System.Net.Mail;

namespace sharpRPA.Core.AutomationCommands
{
    #region Base Command
    [XmlInclude(typeof(SendKeysCommand))]
    [XmlInclude(typeof(SendMouseMoveCommand))]
    [XmlInclude(typeof(PauseCommand))]
    [XmlInclude(typeof(ActivateWindowCommand))]
    [XmlInclude(typeof(MoveWindowCommand))]
    [XmlInclude(typeof(CommentCommand))]
    [XmlInclude(typeof(ThickAppClickItemCommand))]
    [XmlInclude(typeof(ThickAppGetTextCommand))]
    [XmlInclude(typeof(ResizeWindowCommand))]
    [XmlInclude(typeof(MessageBoxCommand))]
    [XmlInclude(typeof(StopProcessCommand))]
    [XmlInclude(typeof(StartProcessCommand))]
    [XmlInclude(typeof(VariableCommand))]
    [XmlInclude(typeof(RunScriptCommand))]
    [XmlInclude(typeof(CloseWindowCommand))]
    [XmlInclude(typeof(IEBrowserCreateCommand))]
    [XmlInclude(typeof(IEBrowserNavigateCommand))]
    [XmlInclude(typeof(IEBrowserCloseCommand))]
    [XmlInclude(typeof(IEBrowserElementCommand))]
    [XmlInclude(typeof(IEBrowserFindBrowserCommand))]
    [XmlInclude(typeof(SetWindowStateCommand))]
    [XmlInclude(typeof(BeginLoopCommand))]
    [XmlInclude(typeof(EndLoopCommand))]
    [XmlInclude(typeof(ClipboardGetTextCommand))]
    [XmlInclude(typeof(ScreenshotCommand))]
    [XmlInclude(typeof(ExcelOpenWorkbookCommand))]
    [XmlInclude(typeof(ExcelCreateApplicationCommand))]
    [XmlInclude(typeof(ExcelAddWorkbookCommand))]
    [XmlInclude(typeof(ExcelGoToCellCommand))]
    [XmlInclude(typeof(ExcelSetCellCommand))]
    [XmlInclude(typeof(ExcelCloseApplicationCommand))]
    [XmlInclude(typeof(ExcelGetCellCommand))]
    [XmlInclude(typeof(ExcelRunMacroCommand))]
    [XmlInclude(typeof(SeleniumBrowserCreateCommand))]
    [XmlInclude(typeof(SeleniumBrowserNavigateURLCommand))]
    [XmlInclude(typeof(SeleniumBrowserNavigateForwardCommand))]
    [XmlInclude(typeof(SeleniumBrowserNavigateBackCommand))]
    [XmlInclude(typeof(SeleniumBrowserRefreshCommand))]
    [XmlInclude(typeof(SeleniumBrowserCloseCommand))]
    [XmlInclude(typeof(SeleniumBrowserElementActionCommand))]
    [XmlInclude(typeof(SMTPSendEmailCommand))]
    [XmlInclude(typeof(ErrorHandlingCommand))]
    [Serializable]
    public abstract class ScriptCommand
    {
        [XmlAttribute]
        public string CommandName { get; set; }
        [XmlAttribute]
        public bool IsCommented { get; set; }
        [XmlAttribute]
        public string SelectionName { get; set; }
        [XmlAttribute]
        public int DefaultPause { get; set; }
        [XmlAttribute]
        public int LineNumber { get; set; }
        [XmlAttribute]
        public bool PauseBeforeExeucution { get; set; }
        [XmlIgnore]
        public System.Drawing.Color DisplayForeColor { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Comment Field (Optional)")]
        public string v_Comment { get; set; }
        [XmlAttribute]
        public bool CommandEnabled {  get; set; }

        public ScriptCommand()
        {
            this.DisplayForeColor = System.Drawing.Color.Black;
            this.CommandEnabled = false;
            this.DefaultPause = 250;
            this.IsCommented = false;
        }

       

        public virtual void RunCommand(object sender)
        {
            System.Threading.Thread.Sleep(DefaultPause);
        }
        public virtual void RunCommand(object sender, Core.Script.ScriptAction command, System.ComponentModel.BackgroundWorker bgw)
        {
            System.Threading.Thread.Sleep(DefaultPause);
        }

        public virtual string GetDisplayValue()
        {
            return SelectionName;
        }

       
    }
    #endregion

    #region Legacy IE Web Commands
    [Serializable]
    [Attributes.ClassAttributes.Group("IE Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to create a new IE web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements the 'InternetExplorer' application object from SHDocVw.dll to achieve automation.")]
    public class IEBrowserCreateCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }

        public IEBrowserCreateCommand()
        {
            this.CommandName = "IEBrowserCreateCommand";
            this.SelectionName = "IE Browser - Create Browser";
            this.v_InstanceName = "default";
            this.CommandEnabled = false;
        }


        public override void RunCommand(object sender)
        {

            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            var newBrowserSession = new SHDocVw.InternetExplorer();
            newBrowserSession.Visible = true;
            sendingInstance.appInstances.Add(v_InstanceName, newBrowserSession);

          
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }

    }
    [Serializable]
    [Attributes.ClassAttributes.Group("IE Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to find and attach to an existing IE web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements the 'InternetExplorer' application object from SHDocVw.dll to achieve automation.")]
    public class IEBrowserFindBrowserCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select or Enter the Browser Name")]
        public string v_IEBrowserName { get; set; }


        public IEBrowserFindBrowserCommand()
        {
            this.CommandName = "IEBrowserFindBrowserCommand";
            this.SelectionName = "IE Browser - Find Browser";
            this.v_InstanceName = "default";
            this.CommandEnabled = false;
        }


        public override void RunCommand(object sender)
        {

            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            bool browserFound = false;
            var shellWindows = new ShellWindows();
            foreach (IWebBrowser2 shellWindow in shellWindows)
            {

                if ((shellWindow.Document is MSHTML.HTMLDocument) && (shellWindow.Document.Title == v_IEBrowserName))
                {
                    sendingInstance.appInstances.Add(v_InstanceName, shellWindow.Application);
                    browserFound = true;
                    break;
                }

            }

            //try partial match
            if (!browserFound)
            {
          
                foreach (IWebBrowser2 shellWindow in shellWindows)
                {

                    if ((shellWindow.Document is MSHTML.HTMLDocument) && (shellWindow.Document.Title.Contains(v_IEBrowserName)))
                    {
                        sendingInstance.appInstances.Add(v_InstanceName, shellWindow.Application);
                        browserFound = true;
                        break;
                    }

                }

              
            }

            if (!browserFound)
            {
                throw new Exception("Browser was not found!");
            }

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Browser Name: '" + v_IEBrowserName + "', Instance Name: '" + v_InstanceName + "']";
        }

    }
    [Serializable]
    [Attributes.ClassAttributes.Group("IE Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to navigate the associated IE web browser to a URL.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements the 'InternetExplorer' application object from SHDocVw.dll to achieve automation.")]
    public class IEBrowserNavigateCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the URL to navigate to")]
        public string v_URL { get; set; }

        public IEBrowserNavigateCommand()
        {
            this.CommandName = "WebBrowserNavigateCommand";
            this.SelectionName = "IE Browser - Navigate";
            this.v_InstanceName = "default";
            this.CommandEnabled = false;
        }


        public override void RunCommand(object sender)
        {

            object browserObject = null;
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            if (sendingInstance.appInstances.TryGetValue(v_InstanceName, out browserObject))
            {
                var browserInstance = (SHDocVw.InternetExplorer)browserObject;

                VariableCommand variableCommand = new VariableCommand();
                var variablizedURL = variableCommand.VariablizeString(sender, v_URL);

                browserInstance.Navigate(variablizedURL);
                WaitForReadyState(browserInstance);

            }
            else
            {
                throw new Exception("Session Instance was not found");
            }


        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [URL: '" + v_URL + "', Instance Name: '" + v_InstanceName + "']";
        }
        private void WaitForReadyState(SHDocVw.InternetExplorer ieInstance)
        {

            DateTime waitExpires = DateTime.Now.AddSeconds(15);

            do

            {
                System.Threading.Thread.Sleep(500);
            }

            while ((ieInstance.ReadyState != SHDocVw.tagREADYSTATE.READYSTATE_COMPLETE) && (waitExpires > DateTime.Now));
        }

    }
    [Serializable]
    [Attributes.ClassAttributes.Group("IE Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to close the associated IE web browser")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements the 'InternetExplorer' application object from SHDocVw.dll to achieve automation.")]
    public class IEBrowserCloseCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }

        public IEBrowserCloseCommand()
        {
            this.CommandName = "IEBrowserCloseCommand";
            this.SelectionName = "IE Browser - Close Browser";
            this.CommandEnabled = false;
            this.v_InstanceName = "default";

        }


        public override void RunCommand(object sender)
        {

            object browserObject = null;
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            if (sendingInstance.appInstances.TryGetValue(v_InstanceName, out browserObject))
            {
                var browserInstance = (SHDocVw.InternetExplorer)browserObject;
                browserInstance.Quit();
                sendingInstance.appInstances.Remove(v_InstanceName);
            }
            else
            {
                throw new Exception("Session Instance was not found");
            }


        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }

    }
    [Serializable]
    [Attributes.ClassAttributes.Group("IE Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to manipulate (get or set) elements within the HTML document of the associated IE web browser.  Features an assisting element capture form")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements the 'InternetExplorer' application object from SHDocVw.dll and MSHTML.dll to achieve automation.")]
    public class IEBrowserElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter or capture element search parameters")]
        public System.Data.DataTable v_WebSearchTable { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select an action")]
        public string v_WebAction { get; set; }
        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Action Parameters")]
        public System.Data.DataTable v_WebActionParameterTable { get; set; }

        public IEBrowserElementCommand()
        {
            this.CommandName = "IEBrowserElementCommand";
            this.SelectionName = "IE Browser - Element Action";
            this.CommandEnabled = false;
            this.v_InstanceName = "default";

            this.v_WebSearchTable = new System.Data.DataTable();
            this.v_WebSearchTable.TableName = DateTime.Now.ToString("WebSearchParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));
            this.v_WebSearchTable.Columns.Add("Enabled");
            this.v_WebSearchTable.Columns.Add("Property Name");
            this.v_WebSearchTable.Columns.Add("Property Value");

            this.v_WebActionParameterTable = new System.Data.DataTable();
            this.v_WebActionParameterTable.TableName = DateTime.Now.ToString("WebActionParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));
            this.v_WebActionParameterTable.Columns.Add("Parameter Name");
            this.v_WebActionParameterTable.Columns.Add("Parameter Value");
        }


        public override void RunCommand(object sender)
        {

            object browserObject = null;
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;

            if (sendingInstance.appInstances.TryGetValue(v_InstanceName, out browserObject))
            {
                var browserInstance = (SHDocVw.InternetExplorer)browserObject;

                DataTable searchTable = Core.Common.Clone<DataTable>(v_WebSearchTable);

                DataColumn matchFoundColumn = new DataColumn();
                matchFoundColumn.ColumnName = "Match Found";
                matchFoundColumn.DefaultValue = false;
                searchTable.Columns.Add(matchFoundColumn);


                var elementSearchProperties = from rws in searchTable.AsEnumerable()
                                              where rws.Field<string>("Enabled") == "True"
                                              select rws;

                bool qualifyingElementFound = false;

                foreach (IHTMLElement element in browserInstance.Document.All)
                {

                    qualifyingElementFound = FindQualifyingElement(elementSearchProperties, element);

                    if ((qualifyingElementFound) && (v_WebAction == "Invoke Click"))
                    {
                        element.click();
                        WaitForReadyState(browserInstance);
                        break;
                    }
                    else if ((qualifyingElementFound) && (v_WebAction == "Left Click") || (qualifyingElementFound) && (v_WebAction == "Middle Click") || (qualifyingElementFound) && (v_WebAction == "Right Click"))
                    {
                        int elementXposition = FindElementXPosition(element);
                        int elementYposition = FindElementYPosition(element);

                        //inputs need to be validated

                        int userXAdjust = Convert.ToInt32((from rw in v_WebActionParameterTable.AsEnumerable()
                                                where rw.Field<string>("Parameter Name") == "X Adjustment"
                                                select rw.Field<string>("Parameter Value")).FirstOrDefault());


                        int userYAdjust = Convert.ToInt32((from rw in v_WebActionParameterTable.AsEnumerable()
                                          where rw.Field<string>("Parameter Name") == "Y Adjustment"
                                          select rw.Field<string>("Parameter Value")).FirstOrDefault());


                        var ieClientLocation = User32Functions.GetWindowPosition(new IntPtr(browserInstance.HWND));

                        SendMouseMoveCommand newMouseMove = new SendMouseMoveCommand();
                        newMouseMove.v_XMousePosition = (elementXposition + ieClientLocation.left + 10) + userXAdjust; // + 10 gives extra padding
                        newMouseMove.v_YMousePosition = (elementYposition + ieClientLocation.top + 90) + userYAdjust; // +90 accounts for title bar height
                        newMouseMove.v_MouseClick = v_WebAction;
                        newMouseMove.RunCommand(sender);

                        break;
                    }

                    else if ((qualifyingElementFound) && (v_WebAction == "Set Attribute"))
                    {


                        string attributeName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                                where rw.Field<string>("Parameter Name") == "Attribute Name"
                                                select rw.Field<string>("Parameter Value")).FirstOrDefault();


                        string valueToSet = (from rw in v_WebActionParameterTable.AsEnumerable()
                                             where rw.Field<string>("Parameter Name") == "Value To Set"
                                             select rw.Field<string>("Parameter Value")).FirstOrDefault();

                        Core.AutomationCommands.VariableCommand newVariableCommand = new VariableCommand();
                        valueToSet = newVariableCommand.VariablizeString(sender, valueToSet);

                        element.setAttribute(attributeName, valueToSet);
                        break;


                    }
                    else if ((qualifyingElementFound) && (v_WebAction == "Get Attribute"))
                    {


                        string attributeName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                                where rw.Field<string>("Parameter Name") == "Attribute Name"
                                                select rw.Field<string>("Parameter Value")).FirstOrDefault();


                        string variableName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                               where rw.Field<string>("Parameter Name") == "Variable Name"
                                               select rw.Field<string>("Parameter Value")).FirstOrDefault();


                        var convertedAttribute = Convert.ToString(element.getAttribute(attributeName));

                        VariableCommand newVariableCommand = new VariableCommand();
                        newVariableCommand.v_userVariableName = variableName;
                        newVariableCommand.v_Input = convertedAttribute;
                        newVariableCommand.RunCommand(sender);
                        break;


                    }
                  
                }

                if (!qualifyingElementFound)
                {
                    throw new Exception("Could not find the element!");
                }


            }

            else
            {
                throw new Exception("Session Instance was not found");
            }
        }


        private bool FindQualifyingElement(EnumerableRowCollection<DataRow> elementSearchProperties, IHTMLElement element)
            {


           foreach (DataRow seachCriteria in elementSearchProperties)
            {


                string searchPropertyName = seachCriteria.Field<string>("Property Name");
                string searchPropertyValue = seachCriteria.Field<string>("Property Value");
                string searchPropertyFound = seachCriteria.Field<string>("Match Found");

                            
                searchPropertyFound = "False";

                if (element.GetType().GetProperty(searchPropertyName) == null)
                {
                    return false;
                }

                int searchValue;
                if (int.TryParse(searchPropertyValue, out searchValue))
                {
                    int elementValue = (int)element.GetType().GetProperty(searchPropertyName).GetValue(element, null);
                    if (elementValue == searchValue)
                    {
                        seachCriteria.SetField<string>("Match Found", "True");
                    }
                    else
                    {
                        seachCriteria.SetField<string>("Match Found", "False");
                    }
                }
                else
                {
                    string elementValue = (string)element.GetType().GetProperty(searchPropertyName).GetValue(element, null);
                    if ((elementValue != null) && (elementValue == searchPropertyValue))
                    {
                        seachCriteria.SetField<string>("Match Found", "True");
                    }
                    else
                    {
                        seachCriteria.SetField<string>("Match Found", "False");
                    }
                }

            }

            foreach (var seachCriteria in elementSearchProperties)
            {
                Console.WriteLine (seachCriteria.Field<string>("Property Value"));
            }

              

                return elementSearchProperties.Where(seachCriteria => seachCriteria.Field<string>("Match Found") == "True").Count() == elementSearchProperties.Count();
        }

        private int FindElementXPosition(MSHTML.IHTMLElement obj)
        {
            int curleft = 0;
            if (obj.offsetParent != null)
            {
                while (obj.offsetParent != null)
                {
                    curleft += obj.offsetLeft;
                    obj = obj.offsetParent;
                }
            }

            return curleft;
        }

        public int FindElementYPosition(MSHTML.IHTMLElement obj)
        {
            int curtop = 0;
            if (obj.offsetParent != null)
            {
                while (obj.offsetParent != null)
                {
                    curtop += obj.offsetTop;
                    obj = obj.offsetParent;
                }
            }

            return curtop;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Action: '" + v_WebAction + "', Instance Name: '" + v_InstanceName + "']";
        }

        private void WaitForReadyState(SHDocVw.InternetExplorer ieInstance)
        {

            DateTime waitExpires = DateTime.Now.AddSeconds(15);

            do

            {
                System.Threading.Thread.Sleep(500);
            }

            while ((ieInstance.ReadyState != SHDocVw.tagREADYSTATE.READYSTATE_COMPLETE) && (waitExpires > DateTime.Now));
        }

    }
    #endregion

    #region Web Selenium
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to create a new Selenium web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserCreateCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }

        public SeleniumBrowserCreateCommand()
        {
            this.CommandName = "SeleniumBrowserCreateCommand";
            this.SelectionName = "Web Browser - Create Browser";
            this.v_InstanceName = "default";
            this.CommandEnabled = true;
        }


        public override void RunCommand(object sender)
        {

            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            var driverPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "Resources");       
            OpenQA.Selenium.Chrome.ChromeDriverService driverService = OpenQA.Selenium.Chrome.ChromeDriverService.CreateDefaultService(driverPath);
            //driverService.HideCommandPromptWindow = true;

            var newSeleniumSession = new OpenQA.Selenium.Chrome.ChromeDriver(driverService, new OpenQA.Selenium.Chrome.ChromeOptions());

            if (sendingInstance.appInstances.ContainsKey(v_InstanceName))
            {
                //need to figure out how to handle multiple potential session names
                sendingInstance.appInstances.Remove(v_InstanceName);
            }

            sendingInstance.appInstances.Add(v_InstanceName, newSeleniumSession);

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }

    }

    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to navigate a Selenium web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserNavigateURLCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the URL to navigate to")]
        public string v_URL { get; set; }

        public SeleniumBrowserNavigateURLCommand()
        {
            this.CommandName = "SeleniumBrowserNavigateURLCommand";
            this.SelectionName = "Web Browser - Navigate to URL";
            this.v_InstanceName = "default";
            this.CommandEnabled = true;
        }


        public override void RunCommand(object sender)
        {

            object browserObject = null;
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            if (sendingInstance.appInstances.TryGetValue(v_InstanceName, out browserObject))
            {
                var seleniumInstance = (OpenQA.Selenium.Chrome.ChromeDriver)browserObject;

                VariableCommand variableCommand = new VariableCommand();
                var variablizedURL = variableCommand.VariablizeString(sender, v_URL);

                seleniumInstance.Navigate().GoToUrl(variablizedURL);
    

            }
            else
            {
                throw new Exception("Session Instance was not found");
            }


        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [URL: '" + v_URL + "', Instance Name: '" + v_InstanceName + "']";
        }
        private void WaitForReadyState(SHDocVw.InternetExplorer ieInstance)
        {

            DateTime waitExpires = DateTime.Now.AddSeconds(15);

            do

            {
                System.Threading.Thread.Sleep(500);
            }

            while ((ieInstance.ReadyState != SHDocVw.tagREADYSTATE.READYSTATE_COMPLETE) && (waitExpires > DateTime.Now));
        }

    }

    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to navigate forward a Selenium web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserNavigateForwardCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }

        public SeleniumBrowserNavigateForwardCommand()
        {
            this.CommandName = "WebBrowserNavigateCommand";
            this.SelectionName = "Web Browser - Navigate Forward";
            this.v_InstanceName = "default";
            this.CommandEnabled = true;
        }


        public override void RunCommand(object sender)
        {

            object browserObject = null;
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            if (sendingInstance.appInstances.TryGetValue(v_InstanceName, out browserObject))
            {
                var seleniumInstance = (OpenQA.Selenium.Chrome.ChromeDriver)browserObject;
                seleniumInstance.Navigate().Forward();
            }
            else
            {
                throw new Exception("Session Instance was not found");
            }


        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
       

    }

    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to navigate backwards in a Selenium web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserNavigateBackCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }

        public SeleniumBrowserNavigateBackCommand()
        {
            this.CommandName = "SeleniumBrowserNavigateBackCommand";
            this.SelectionName = "Web Browser - Navigate Back";
            this.v_InstanceName = "default";
            this.CommandEnabled = true;
        }


        public override void RunCommand(object sender)
        {

            object browserObject = null;
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            if (sendingInstance.appInstances.TryGetValue(v_InstanceName, out browserObject))
            {
                var seleniumInstance = (OpenQA.Selenium.Chrome.ChromeDriver)browserObject;
                seleniumInstance.Navigate().Back();
            }
            else
            {
                throw new Exception("Session Instance was not found");
            }


        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }


    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to refresh a Selenium web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserRefreshCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }

        public SeleniumBrowserRefreshCommand()
        {
            this.CommandName = "SeleniumBrowserRefreshCommand";
            this.SelectionName = "Web Browser - Refresh";
            this.v_InstanceName = "default";
            this.CommandEnabled = true;
        }


        public override void RunCommand(object sender)
        {

            object browserObject = null;
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            if (sendingInstance.appInstances.TryGetValue(v_InstanceName, out browserObject))
            {
                var seleniumInstance = (OpenQA.Selenium.Chrome.ChromeDriver)browserObject;
                seleniumInstance.Navigate().Refresh();
            }
            else
            {
                throw new Exception("Session Instance was not found");
            }


        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }


    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to close a Selenium web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserCloseCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }

        public SeleniumBrowserCloseCommand()
        {
            this.CommandName = "SeleniumBrowserCloseCommand";
            this.SelectionName = "Web Browser - Close Browser";
            this.v_InstanceName = "default";
            this.CommandEnabled = true;
        }


        public override void RunCommand(object sender)
        {

            object browserObject = null;
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            if (sendingInstance.appInstances.TryGetValue(v_InstanceName, out browserObject))
            {
                var seleniumInstance = (OpenQA.Selenium.Chrome.ChromeDriver)browserObject;
                seleniumInstance.Quit();
                seleniumInstance.Dispose();
            }
            else
            {
                throw new Exception("Session Instance was not found");
            }

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }

    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to close a Selenium web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserElementActionCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Element Search Method")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Element By XPath")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Element By ID")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Element By Name")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Element By Tag Name")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Element By Class Name")]
        public string v_SeleniumSearchType { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Element Search Parameter")]
        public string v_SeleniumSearchParameter { get; set; }
        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Element Action")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Invoke Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Left Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Right Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Middle Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Set Text")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Get Text")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Get Attribute")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Wait For Element To Exist")]
       
        public string v_SeleniumElementAction { get; set; }
        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Additional Parameters")]
        public DataTable v_WebActionParameterTable { get; set; }
        public SeleniumBrowserElementActionCommand()
        {
            this.CommandName = "SeleniumBrowserCreateCommand";
            this.SelectionName = "Web Browser - Element Action";
            this.v_InstanceName = "default";
            this.CommandEnabled = true;

            this.v_WebActionParameterTable = new System.Data.DataTable();
            this.v_WebActionParameterTable.TableName = DateTime.Now.ToString("WebActionParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));
            this.v_WebActionParameterTable.Columns.Add("Parameter Name");
            this.v_WebActionParameterTable.Columns.Add("Parameter Value");

        }


        public override void RunCommand(object sender)
        {
          
            object browserObject = null;
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            if (sendingInstance.appInstances.TryGetValue(v_InstanceName, out browserObject))
            {
                var seleniumInstance = (OpenQA.Selenium.Chrome.ChromeDriver)browserObject;


             

                OpenQA.Selenium.IWebElement element = null;


                if (v_SeleniumElementAction == "Wait For Element To Exist")
                {
                    int timeOut = Convert.ToInt32((from rw in v_WebActionParameterTable.AsEnumerable()
                                                   where rw.Field<string>("Parameter Name") == "Timeout (Seconds)"
                                                   select rw.Field<string>("Parameter Value")).FirstOrDefault());

                    var timeToEnd = DateTime.Now.AddSeconds(timeOut);

                    while (timeToEnd >= DateTime.Now)
                    {
                        try
                        {
                            element = FindElement(seleniumInstance); 
                            break;
                        }
                        catch (Exception)
                        {
                            sendingInstance.ReportProgress("Element Not Yet Found... " + (timeToEnd - DateTime.Now).Seconds + "s remain");
                            System.Threading.Thread.Sleep(1000);
                        }
                    }

                    if (element == null)
                    {
                        throw new Exception("Element Not Found");
                    }

                    return;

                }
                else
                {
                    element = FindElement(seleniumInstance);
                }

         
               
                switch (v_SeleniumSearchType)
                {
                    case "Find Element By XPath":
                        element = seleniumInstance.FindElementByXPath(v_SeleniumSearchParameter);
                        break;
                    case "Find Element By ID":
                        element = seleniumInstance.FindElementById(v_SeleniumSearchParameter);
                        break;
                    case "Find Element By Name":
                        element = seleniumInstance.FindElementByName(v_SeleniumSearchParameter);
                        break;
                    case "Find Element By Tag Name":
                        element = seleniumInstance.FindElementByTagName(v_SeleniumSearchParameter);
                        break;
                    case "Find Element By Class Name":
                        element = seleniumInstance.FindElementByClassName(v_SeleniumSearchParameter);
                        break;
                    default:
                        throw new Exception("Search Type was not found");
                }


                switch (v_SeleniumElementAction)
                {
                    case "Invoke Click":
                        element.Click();
                        break;
                    case "Left Click":
                    case "Right Click":
                    case "Middle Click":

                        int userXAdjust = Convert.ToInt32((from rw in v_WebActionParameterTable.AsEnumerable()
                                                           where rw.Field<string>("Parameter Name") == "X Adjustment"
                                                           select rw.Field<string>("Parameter Value")).FirstOrDefault());


                        int userYAdjust = Convert.ToInt32((from rw in v_WebActionParameterTable.AsEnumerable()
                                                           where rw.Field<string>("Parameter Name") == "Y Adjustment"
                                                           select rw.Field<string>("Parameter Value")).FirstOrDefault());

                        var elementLocation = element.Location;                 
                        SendMouseMoveCommand newMouseMove = new SendMouseMoveCommand();
                        var seleniumWindowPosition = seleniumInstance.Manage().Window.Position;
                        newMouseMove.v_XMousePosition = (seleniumWindowPosition.X + elementLocation.X  + 30 + userXAdjust); // added 30 for offset
                        newMouseMove.v_YMousePosition = (seleniumWindowPosition.Y + elementLocation.Y +  130 + userYAdjust); //added 130 for offset
                        newMouseMove.v_MouseClick = v_SeleniumElementAction;
                        newMouseMove.RunCommand(sender);
                        break;
                    case "Set Text":

                        string textToSet = (from rw in v_WebActionParameterTable.AsEnumerable()
                                           where rw.Field<string>("Parameter Name") == "Text To Set"
                                           select rw.Field<string>("Parameter Value")).FirstOrDefault();

                        string[] potentialKeyPresses = textToSet.Split('{', '}');

                       

  
                        Type seleniumKeys = typeof(OpenQA.Selenium.Keys);
                        System.Reflection.FieldInfo[] fields = seleniumKeys.GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

                        //check if chunked string contains a key press command like {ENTER}
                        foreach (string chunkedString in potentialKeyPresses)
                        {
                            if (chunkedString == "")
                                continue;

                            if (fields.Any(f => f.Name == chunkedString))
                            {
                                string keyPress = (string)fields.Where(f => f.Name == chunkedString).FirstOrDefault().GetValue(null);
                                seleniumInstance.Keyboard.PressKey(keyPress);
                            }
                            else
                            {
                                element.SendKeys(chunkedString);
                            }
                        }

                        break;
                    case "Get Text":
                    case "Get Attribute":


                        string variableName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                               where rw.Field<string>("Parameter Name") == "Variable Name"
                                               select rw.Field<string>("Parameter Value")).FirstOrDefault();

                        string attributeName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                                where rw.Field<string>("Parameter Name") == "Attribute Name"
                                                select rw.Field<string>("Parameter Value")).FirstOrDefault();


                        string elementValue;
                        if (v_SeleniumElementAction == "Get Text")
                        {
                            elementValue = element.Text;
                        }
                        else
                        {
                            elementValue = element.GetAttribute(attributeName);
                        }

                        VariableCommand newVariableCommand = new VariableCommand();
                        newVariableCommand.v_userVariableName = variableName;
                        newVariableCommand.v_Input = elementValue;
                        newVariableCommand.RunCommand(sender);
                           
                        break;
                    default:
                        throw new Exception("Element Action was not found");
                }



            }
            else
            {
                throw new Exception("Session Instance was not found");
            }

        }

        private OpenQA.Selenium.IWebElement FindElement(OpenQA.Selenium.Chrome.ChromeDriver seleniumInstance)
        {
            OpenQA.Selenium.IWebElement element = null;

            switch (v_SeleniumSearchType)
            {
                case "Find Element By XPath":
                    element = seleniumInstance.FindElementByXPath(v_SeleniumSearchParameter);
                    break;
                case "Find Element By ID":
                    element = seleniumInstance.FindElementById(v_SeleniumSearchParameter);
                    break;
                case "Find Element By Name":
                    element = seleniumInstance.FindElementByName(v_SeleniumSearchParameter);
                    break;
                case "Find Element By Tag Name":
                    element = seleniumInstance.FindElementByTagName(v_SeleniumSearchParameter);
                    break;
                case "Find Element By Class Name":
                    element = seleniumInstance.FindElementByClassName(v_SeleniumSearchParameter);
                    break;
                default:
                    throw new Exception("Search Type was not found");
            }
            
            return element;

        }




        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [" + v_SeleniumSearchType + " and " + v_SeleniumElementAction + ", Instance Name: '" + v_InstanceName + "']";
        }

    }

    #endregion

    #region Misc Commands

    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.Description("This command pauses the script for a set amount of time in milliseconds.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Thread.Sleep' to achieve automation.")]
    public class PauseCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Amount of time to pause for (in milliseconds).")]
        public int v_PauseLength { get; set; }

        public PauseCommand()
        {
            this.CommandName = "PauseCommand";
            this.SelectionName = "Pause - Pause Script";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            System.Threading.Thread.Sleep(v_PauseLength);
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Wait for " + v_PauseLength + "ms]";
        }


    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.Description("This command pauses the script for a set amount of time in milliseconds.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Thread.Sleep' to achieve automation.")]
    public class ErrorHandlingCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Action On Error")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Stop Processing")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Continue Processing")]
        public string v_ErrorHandlingAction { get; set; }


        public ErrorHandlingCommand()
        {
            this.CommandName = "ErrorHandlingCommand";
            this.SelectionName = "Error Handling - On Error";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            UI.Forms.frmScriptEngine engineForm = (UI.Forms.frmScriptEngine)sender;
            engineForm.errorHandling = this;
        }


        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Action: " + v_ErrorHandlingAction + "]";
        }


    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.Description("This command activates a window and brings it to the front.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'FindWindowNative', 'SetForegroundWindow', 'ShowWindow' from user32.dll to achieve automation.")]
    public class ActivateWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select or Type a window Name")]
        public string v_WindowName { get; set; }

        public ActivateWindowCommand()
        {
            this.CommandName = "ActivateWindowCommand";
            this.SelectionName = "Window - Activate Window";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {

            IntPtr hWnd = User32Functions.FindWindow(v_WindowName);
            if (hWnd != IntPtr.Zero)
            {
                User32Functions.SetWindowState(hWnd, User32Functions.WindowState.SW_SHOWNORMAL);
                User32Functions.SetForegroundWindow(hWnd); 
            }
            else
            {
                throw new Exception("Window not found");
            }


        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Window: " + v_WindowName + "]";
        }

    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.Description("This command moves a window to a specified location on screen.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'FindWindowNative', 'SetWindowPos' from user32.dll to achieve automation.")]
    public class MoveWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select or Type a window Name")]
        public string v_WindowName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the X position to move the window to.")]
        public int v_XWindowPosition { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the Y position to move the window to.")]
        public int v_YWindowPosition { get; set; }

        public MoveWindowCommand()
        {
            this.CommandName = "MoveWindowCommand";
            this.SelectionName = "Window - Move Window";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {

   
            IntPtr hWnd = User32Functions.FindWindow(v_WindowName);

            if (hWnd != IntPtr.Zero)
            {
                User32Functions.SetWindowPosition(hWnd, v_XWindowPosition, v_YWindowPosition);
            }
            else
            {
                throw new Exception("Window not found");
            }

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Window: " + v_WindowName + ", Target Coordinates (" + v_XWindowPosition + "," + v_YWindowPosition + ")]";
        }

    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.Description("This command resizes a window to a specified size.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'FindWindowNative', 'SetWindowPos' from user32.dll to achieve automation.")]
    public class ResizeWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select or Type a window Name")]
        public string v_WindowName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the new window width")]
        public int v_XWindowSize { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the new window height")]
        public int v_YWindowSize { get; set; }

        public ResizeWindowCommand()
        {
            this.CommandName = "ResizeWindowCommand";
            this.SelectionName = "Window - Resize Window";

            //not working
            //this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {


            IntPtr hWnd = User32Functions.FindWindow(v_WindowName);

            if (hWnd != IntPtr.Zero)
            {
                User32Functions.SetWindowSize(hWnd,v_XWindowSize, v_YWindowSize);
            }

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Window: " + v_WindowName + ", Target Size (" + v_XWindowSize + "," + v_YWindowSize + "]";
        }

    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.Description("This command closes an open window.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'FindWindowNative', 'SendMessage' from user32.dll to achieve automation.")]
    public class CloseWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select or Type a window Name")]
        public string v_WindowName { get; set; }

        public CloseWindowCommand()
        {
            this.CommandName = "CloseWindowCommand";
            this.SelectionName = "Window - Close Window";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {

            IntPtr hWnd = User32Functions.FindWindow(v_WindowName);

            if (hWnd != IntPtr.Zero)
            {
        
                User32Functions.CloseWindow(hWnd);

            }
            else
            {
                throw new Exception("Window not found");
            }


        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Window: " + v_WindowName + "]";
        }

    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.Description("This command sets a target windows state (minimize, maximize, restore)")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'FindWindowNative', 'ShowWindow' from user32.dll to achieve automation.")]
    public class SetWindowStateCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select or Type a window Name")]
        public string v_WindowName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select a Window State")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Maximize")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Minimize")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Restore")]
        public string v_WindowState { get; set; }

        public SetWindowStateCommand()
        {
            this.CommandName = "SetWindowStateCommand";
            this.SelectionName = "Window - Set Window State";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {

            IntPtr hWnd = User32Functions.FindWindow(v_WindowName);

            if (hWnd != IntPtr.Zero) //If found
            {
                User32Functions.WindowState WINDOW_STATE = User32Functions.WindowState.SW_SHOWNORMAL;
                switch (v_WindowState)
                {
                    case "Maximize":
                        WINDOW_STATE = User32Functions.WindowState.SW_MAXIMIZE;
                        break;
                    case "Minimize":
                        WINDOW_STATE = User32Functions.WindowState.SW_MINIMIZE;
                        break;
                    case "Restore":
                        WINDOW_STATE = User32Functions.WindowState.SW_RESTORE;
                        break;
                    default:
                        break;
                }

                User32Functions.SetWindowState(hWnd, WINDOW_STATE);
         
            }
            else
            {
                throw new Exception("Window not found");
            }


        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Window: " + v_WindowName + ", Window State: " + v_WindowState + "]";
        }

    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to add an in-line comment to the configuration.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command is for visual purposes only")]
    public class CommentCommand : ScriptCommand
    {

        public CommentCommand()
        {
            this.CommandName = "CommentCommand";
            this.SelectionName = "Comment - Add Code Comment";
            this.DisplayForeColor = System.Drawing.Color.ForestGreen;
            this.CommandEnabled = true;
        }

        public override string GetDisplayValue()
        {
            return "// Comment: " + this.v_Comment;
        }


    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to show a MessageBox and supports variables.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'MessageBox' and invokes VariableCommand to find variable data.")]
    public class MessageBoxCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the message to be displayed.")]
        public string v_Message { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Close After X (Seconds) - 0 to bypass")]
        public int v_AutoCloseAfter { get; set; }
        public MessageBoxCommand()
        {
            this.CommandName = "MessageBoxCommand";
            this.SelectionName = "Message Box - Show Message";
            this.CommandEnabled = true;
            this.v_AutoCloseAfter = 0;
        }


        public override void RunCommand(object sender)
        {
            UI.Forms.frmScriptEngine engineForm = (UI.Forms.frmScriptEngine)sender;
            VariableCommand variableCommand = new VariableCommand();
            var variablizedText = variableCommand.VariablizeString(sender, v_Message);
            var result = engineForm.Invoke(new Action(() => 
            {
                engineForm.ShowMessage(variablizedText, "MessageBox Command", UI.Forms.Supplemental.frmDialog.DialogType.OkOnly, v_AutoCloseAfter);
            }
            
            ));
            //System.Windows.Forms.MessageBox.Show(variablizedText, "Message Box Command", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Message: " + v_Message + "]";
        }

    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Programs/Process Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to start a program or a process. You can use short names 'chrome.exe' or fully qualified names 'c:/some.exe'")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Process.Start'.")]
    public class StartProcessCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter the name or path to the program (ex. notepad, calc)")]
        public string v_ProgramName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter any arguments (if applicable)")]
        public string v_ProgramArgs { get; set; }

        public StartProcessCommand()
        {
            this.CommandName = "StartProcessCommand";
            this.SelectionName = "Process - Start Process";
            this.CommandEnabled = true;

        }

        public override void RunCommand(object sender)
        {
            VariableCommand newVariableCommand = new VariableCommand();
            v_ProgramName = newVariableCommand.VariablizeString(sender, v_ProgramName);
            v_ProgramArgs = newVariableCommand.VariablizeString(sender, v_ProgramArgs);

            if (v_ProgramArgs == "")
            {
                System.Diagnostics.Process.Start(v_ProgramName);
            }
            else
            {
                System.Diagnostics.Process.Start(v_ProgramName, v_ProgramArgs);
            }

            System.Threading.Thread.Sleep(2000);


        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Process: " + v_ProgramName + "]";
        }

    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Programs/Process Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to stop a program or a process. You can use the name of the process 'chrome'. Alternatively, you may use the Close Window or Thick App Command instead.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Process.CloseMainWindow'.")]
    public class StopProcessCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Enter the process name to be stopped (calc, notepad)")]
        public string v_ProgramShortName { get; set; }

        public StopProcessCommand()
        {
            this.CommandName = "StopProgramCommand";
            this.SelectionName = "Process - Stop Process";
            this.CommandEnabled = true;

        }

        public override void RunCommand(object sender)
        {

            var processes = System.Diagnostics.Process.GetProcessesByName(v_ProgramShortName);

            foreach (var prc in processes)
                prc.CloseMainWindow();

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Process: " + v_ProgramShortName + "]";
        }

    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Programs/Process Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to run a script or program and wait for it to exit before proceeding.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Process.Start' and waits for the script/program to exit before proceeding.")]
    public class RunScriptCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Enter the path to the script")]
        public string v_ScriptPath { get; set; }

        public RunScriptCommand()
        {
            this.CommandName = "RunScriptCommand";
            this.SelectionName = "Script - Run Script";
            this.CommandEnabled = true;

        }

        public override void RunCommand(object sender)
        {

            {


                System.Diagnostics.Process scriptProc = new System.Diagnostics.Process();
                scriptProc.StartInfo.FileName = v_ScriptPath;
                scriptProc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                scriptProc.Start();
                scriptProc.WaitForExit();

                scriptProc.Close();



            }



        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Script Path: " + v_ScriptPath + "]";
        }

    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Variable Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to modify variables.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    public class VariableCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select a variable to modify")]
        public string v_userVariableName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please define the input to be set to above variable")]
        public string v_Input { get; set; }
        public VariableCommand()
        {
            this.CommandName = "VariableCommand";
            this.SelectionName = "Variable - Set Variable";
            this.CommandEnabled = true;

        }

        public override void RunCommand(object sender)
        {

            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            var requiredVariable = sendingInstance.variableList.Where(var => var.variableName == v_userVariableName).FirstOrDefault();

            if (requiredVariable != null)
            {
                requiredVariable.variableValue = VariablizeString(sender, v_Input);
            }
            else
            {
                throw new Exception("Variable was not found. Enclose variables within brackets, ex. [vVariable]");
            }



        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Apply '" + v_Input + "' to Variable: " + v_userVariableName + "]";
        }

        public string VariablizeString(object sender, string replacementString)
        {
            if (replacementString == null)
                return string.Empty;

            var engineForm = (UI.Forms.frmScriptEngine)sender;

            var variableList = engineForm.variableList;
            var systemVariables = GenerateSystemVariables();

            var searchList = new List<Core.Script.ScriptVariable>();
            searchList.AddRange(variableList);
            searchList.AddRange(systemVariables);


            string[] potentialVariables = replacementString.Split('[', ']');

            foreach (var potentialVariable in potentialVariables)
            {

                var varCheck = (from vars in searchList
                                where vars.variableName == potentialVariable
                                select vars).FirstOrDefault();

                if (varCheck != null)
                {
                    var searchVariable = "[" + potentialVariable + "]";
                    replacementString = replacementString.Replace(searchVariable, varCheck.variableValue);
                }

            }

            //test
            try
            {
                DataTable dt = new DataTable();
                var v = dt.Compute(replacementString, "");
                return v.ToString();
            }
            catch (Exception)
            {
                return replacementString;
            }
           


           


        }

        public List<Core.Script.ScriptVariable> GenerateSystemVariables()
        {
            List<Core.Script.ScriptVariable> systemVariableList = new List<Core.Script.ScriptVariable>();


            systemVariableList.Add(new Core.Script.ScriptVariable { variableName = "Folder.Desktop", variableValue = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)});
            systemVariableList.Add(new Core.Script.ScriptVariable { variableName = "Folder.Documents", variableValue = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)});
            systemVariableList.Add(new Core.Script.ScriptVariable { variableName = "Folder.AppData", variableValue = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) });
            systemVariableList.Add(new Core.Script.ScriptVariable { variableName = "Folder.ScriptPath", variableValue = Core.Common.GetScriptFolderPath()});
            systemVariableList.Add(new Core.Script.ScriptVariable { variableName = "DateTime.Now", variableValue = DateTime.Now.ToString() });
            systemVariableList.Add(new Core.Script.ScriptVariable { variableName = "DateTime.Now.FileSafe", variableValue = DateTime.Now.ToString("MM-dd-yy hh.mm.ss")});
            systemVariableList.Add(new Core.Script.ScriptVariable { variableName = "PC.MachineName", variableValue = Environment.MachineName });
            systemVariableList.Add(new Core.Script.ScriptVariable { variableName = "PC.UserName", variableValue = Environment.UserName });
            systemVariableList.Add(new Core.Script.ScriptVariable { variableName = "PC.DomainName", variableValue = Environment.UserDomainName });

            return systemVariableList;

        }

    }
     [Serializable]
    [Attributes.ClassAttributes.Group("Clipboard Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to get text from the clipboard.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against the VariableList from the scripting engine using System.Windows.Forms.Clipboard.")]
    public class ClipboardGetTextCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select a variable to set clipboard contents")]
        public string v_userVariableName { get; set; }

        public ClipboardGetTextCommand()
        {
            this.CommandName = "ClipboardCommand";
            this.SelectionName = "Clipboard - Get Text";
            this.CommandEnabled = true;

        }

        public override void RunCommand(object sender)
        {
        
                VariableCommand newVariableCommand = new VariableCommand();
                newVariableCommand.v_userVariableName = v_userVariableName;
                newVariableCommand.v_Input = User32Functions.GetClipboardText();
                newVariableCommand.RunCommand(sender);
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Get Text From Clipboard and Apply to Variable: " + v_userVariableName + "]";
        }


    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.Description("This command takes a screenshot and saves it to a location")]
    [Attributes.ClassAttributes.ImplementationDescription("NOT IMPLEMENTED")]
    public class ScreenshotCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Window name")]
        public string v_ScreenshotWindowName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the path to save the image")]
        public string v_FilePath { get; set; }
        public ScreenshotCommand()
        {
            this.CommandName = "ScreenshotCommand";
            this.SelectionName = "Screenshot - Take Screenshot";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
    
            var image = User32Functions.CaptureWindow(v_ScreenshotWindowName);
            VariableCommand newVariableCommand = new VariableCommand();
            string variablizedString = newVariableCommand.VariablizeString(sender, v_FilePath);
            image.Save(variablizedString);

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Window: '" + v_ScreenshotWindowName + "', File Path: '" + v_FilePath + "]";
        }

    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to send email using SMTP.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command is for visual purposes only")]
    public class SMTPSendEmailCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Host Name")]
        public string v_SMTPHost { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Port")]
        public int v_SMTPPort { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Username")]
        public string v_SMTPUserName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Password")]
        public string v_SMTPPassword { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("From Email")]
        public string v_SMTPFromEmail { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("To Email")]
        public string v_SMTPToEmail { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Subject")]
        public string v_SMTPSubject { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Body")]
        public string v_SMTPBody { get; set; }
        public SMTPSendEmailCommand()
        {
            this.CommandName = "SMTPCommand";
            this.SelectionName = "SMTP - Send Email";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {

            //use variable command on strings
            VariableCommand variableCommand = new VariableCommand();
            string varSMTPHost = variableCommand.VariablizeString(sender, v_SMTPHost);
            string varSMTPPort = variableCommand.VariablizeString(sender, v_SMTPPort.ToString());
            string varSMTPUserName = variableCommand.VariablizeString(sender, v_SMTPUserName);
            string varSMTPPassword = variableCommand.VariablizeString(sender, v_SMTPPassword);

            string varSMTPFromEmail = variableCommand.VariablizeString(sender, v_SMTPFromEmail);
            string varSMTPToEmail = variableCommand.VariablizeString(sender, v_SMTPToEmail);
            string varSMTPSubject = variableCommand.VariablizeString(sender, v_SMTPSubject);
            string varSMTPBody = variableCommand.VariablizeString(sender, v_SMTPBody);

            var client = new SmtpClient(varSMTPHost, int.Parse(varSMTPPort));
            client.Credentials = new System.Net.NetworkCredential(varSMTPUserName, varSMTPPassword);
            client.EnableSsl = true;

            client.Send(varSMTPFromEmail, varSMTPToEmail, varSMTPSubject, varSMTPBody);
        }
    
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [To Address: '" + v_SMTPToEmail + "']";
        }


    }
    #endregion

    #region Input Commands
    [Serializable]
    [Attributes.ClassAttributes.Group("Input Commands")]
    [Attributes.ClassAttributes.Description("Use this command to send key strokes to the current or a targeted window.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Windows.Forms.SendKeys' method to achieve automation.")]
    public class SendKeysCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Window name")]
        public string v_WindowName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter text to send")]
        public string v_TextToSend { get; set; }

        public SendKeysCommand()
        {
            this.CommandName = "SendKeysCommand";
            this.SelectionName = "Input - Send Keystrokes";
            this.CommandEnabled = true;
        }


        public override void RunCommand(object sender)
        {

            if (v_WindowName != "Current Window")
            {
                ActivateWindowCommand activateWindow = new ActivateWindowCommand();
                activateWindow.v_WindowName = v_WindowName;
                activateWindow.RunCommand(sender);            
            }

            VariableCommand newVariableCommand = new VariableCommand();
            string variableReplacedText = newVariableCommand.VariablizeString(sender, v_TextToSend);
            System.Windows.Forms.SendKeys.SendWait(variableReplacedText);

            System.Threading.Thread.Sleep(500);

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Send '" + v_TextToSend + " to '" + v_WindowName + "']";
        }

    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Input Commands")]
    [Attributes.ClassAttributes.Description("Use this command to simulate mouse movement and click the mouse on coordinates.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'SetCursorPos' function from user32.dll to achieve automation.")]
    public class SendMouseMoveCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter the X position to move the mouse to")]
        public int v_XMousePosition { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter the Y position to move the mouse to")]
        public int v_YMousePosition { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate mouse click type if required")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("None")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Left Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Middle Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Right Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Left Down")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Middle Down")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Right Down")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Left Up")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Middle Up")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Right Up")]
        public string v_MouseClick { get; set; }

        public SendMouseMoveCommand()
        {
            this.CommandName = "SendMouseMoveCommand";
            this.SelectionName = "Input - Send Mouse Move";
            this.CommandEnabled = true;
        }



        public override void RunCommand(object sender)
        {

            User32Functions.SetCursorPosition(v_XMousePosition, v_YMousePosition);
            User32Functions.SendMouseClick(v_MouseClick, v_XMousePosition, v_YMousePosition);

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Coordinates (" + v_XMousePosition + "," + v_YMousePosition + ") Click: " + v_MouseClick + "]";
        }

    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Input Commands")]
    [Attributes.ClassAttributes.Description("This command clicks an item in a Thick Application window.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Windows UI Automation' to find elements and invokes a SendMouseMove Command to click and achieve automation")]
    public class ThickAppClickItemCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the Window to Automate")]
        public string v_AutomationWindowName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the Appropriate Item")]
        public string v_AutomationHandleName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate mouse click type if required")]
        public string v_MouseClick { get; set; }

        public ThickAppClickItemCommand()
        {
            this.CommandName = "ThickAppClickItemCommand";
            this.SelectionName = "Thick App - Click Item";
            this.CommandEnabled = true;
            this.DefaultPause = 3000;
        }

        public override void RunCommand(object sender)
        {
            var searchItem = AutomationElement.RootElement.FindFirst
            (TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty,
            v_AutomationWindowName));

            if (searchItem == null)
            {
                throw new Exception("Window not found");
            }

            var requiredItem = searchItem.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, v_AutomationHandleName));



            var newActivateWindow = new ActivateWindowCommand();
            newActivateWindow.v_WindowName = v_AutomationWindowName;
            newActivateWindow.RunCommand(sender);

            //get newpoint for now
            var newPoint = requiredItem.GetClickablePoint();

            //send mousemove command
            var newMouseMove = new SendMouseMoveCommand();
            newMouseMove.v_XMousePosition = (int)newPoint.X;
            newMouseMove.v_YMousePosition = (int)newPoint.Y;
            newMouseMove.v_MouseClick = v_MouseClick;
            newMouseMove.RunCommand(sender);

        }

        public List<string> FindHandleObjects(string windowTitle)
        {

            var automationElement = AutomationElement.RootElement.FindFirst
    (TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty,
    windowTitle));

            var searchItems = automationElement.FindAll(TreeScope.Descendants, PropertyCondition.TrueCondition);

            List<String> handleList = new List<String>();
            foreach (AutomationElement item in searchItems)
            {
                if (item.Current.Name.Trim() != string.Empty)
                    handleList.Add(item.Current.Name);

            }
            // handleList = handleList.OrderBy(x => x).ToList();

            return handleList;


        }

        public override string GetDisplayValue()
        {


            return base.GetDisplayValue() + " [Perform " + v_MouseClick + " on '" + v_AutomationHandleName + "' in Window '" + v_AutomationWindowName + "']";
        }

    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Input Commands")]
    [Attributes.ClassAttributes.Description("This command gets text from a Thick Application window and assigns it to a variable.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Windows UI Automation' to find elements and invokes a Variable Command to assign data and achieve automation")]
    public class ThickAppGetTextCommand : ScriptCommand
    {
        [Attributes.PropertyAttributes.PropertyDescription("Please select the Window to Automate")]
        public string v_AutomationWindowName { get; set; }
        [Attributes.PropertyAttributes.PropertyDescription("Please select the Appropriate Item")]
        public string v_AutomationHandleDisplayName { get; set; }
        [Attributes.PropertyAttributes.PropertyDescription("Automation ID of the Item")]
        public string v_AutomationID { get; set; }
        [Attributes.PropertyAttributes.PropertyDescription("Assign to Variable")]
        public string v_userVariableName { get; set; }

        public ThickAppGetTextCommand()
        {
            this.CommandName = "ThickAppGetTextCommand";
            this.SelectionName = "Thick App - Get Text";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            var searchItem = AutomationElement.RootElement.FindFirst
            (TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty,
            v_AutomationWindowName));

            if (searchItem == null)
            {
                throw new Exception("Window not found");
            }

            var requiredItem = searchItem.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, v_AutomationID));

            var newVariableCommand = new Core.AutomationCommands.VariableCommand();
            newVariableCommand.v_userVariableName = v_userVariableName;
            newVariableCommand.v_Input = requiredItem.Current.Name;
            newVariableCommand.RunCommand(sender);

        }

        public string FindHandleID(string windowTitle, string nameProperty)
        {

            var automationElement = AutomationElement.RootElement.FindFirst
    (TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty,
    windowTitle));

            var requiredItem = automationElement.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, nameProperty));

            return requiredItem.Current.AutomationId;

        }

        public override string GetDisplayValue()
        {


            return base.GetDisplayValue() + " [Set Variable [" + v_userVariableName + "] From ID " + v_AutomationID + " (" + v_AutomationHandleDisplayName + ") in Window '" + v_AutomationWindowName + "']";
        }


    }
    #endregion

    #region Loop Commands

    [Serializable]
    [Attributes.ClassAttributes.Group("Loop Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to repeat actions several times (loop).  Any 'Begin Loop' command must have a following 'End Loop' command.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command recursively calls the underlying 'BeginLoop' Command to achieve automation.")]
    public class BeginLoopCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Enter the amount of times to loop")]
        public string v_LoopParameter { get; set; }
    

        public BeginLoopCommand()
        {
            this.CommandName = "BeginLoopCommand";
            this.SelectionName = "Loop - Begin Loop";
            this.CommandEnabled = true;
        }


        public override void RunCommand(object sender, Core.Script.ScriptAction parentCommand, System.ComponentModel.BackgroundWorker bgw)
        {
            Core.AutomationCommands.BeginLoopCommand loopCommand = (Core.AutomationCommands.BeginLoopCommand)parentCommand.ScriptCommand;
            var engineForm = (UI.Forms.frmScriptEngine)sender;
            var loopTimes = int.Parse(loopCommand.v_LoopParameter);

            
            for (int i = 0; i < loopTimes; i++)
            {
                bgw.ReportProgress(0, new object[] { loopCommand.LineNumber, "Starting Loop Number " + (i + 1) + "/" + loopTimes + " From Line " + loopCommand.LineNumber});

                foreach (var cmd in parentCommand.AdditionalScriptCommands)
                {
                    if (bgw.CancellationPending)
                        return;
                    engineForm.ExecuteCommand(cmd, bgw);
                }

                bgw.ReportProgress(0, new object[] { loopCommand.LineNumber, "Finished Loop From Line " + loopCommand.LineNumber });
            }




        }

        public override string GetDisplayValue()
        {
            return "Loop " + v_LoopParameter + " Times";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Loop Commands")]
    [Attributes.ClassAttributes.Description("This command signifies the exit point of looped (repeated) actions.  Required for all loops.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command is used by the serializer to signify the end point of a loop.")]
    public class EndLoopCommand : ScriptCommand
    {
   
        public EndLoopCommand()
        {
            this.DefaultPause = 0;
            this.CommandName = "EndLoopCommand";
            this.SelectionName = "Loop - End Loop";
            this.CommandEnabled = true;
        }


        public override string GetDisplayValue()
        {
            return "End Loop";
        }
    }

    #endregion

    #region Excel Commands
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to open the Excel Application.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelCreateApplicationCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
      
        public ExcelCreateApplicationCommand()
        {
            this.CommandName = "ExcelOpenApplicationCommand";
            this.SelectionName = "Excel - Create Excel Application";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            var newExcelSession = new Microsoft.Office.Interop.Excel.Application();
            newExcelSession.Visible = true;
            sendingInstance.appInstances.Add(v_InstanceName, newExcelSession);

        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to open an existing Excel Workbook.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelOpenWorkbookCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the workbook file path")]
        public string v_FilePath { get; set; }
        public ExcelOpenWorkbookCommand()
        {
            this.CommandName = "ExcelOpenWorkbookCommand";
            this.SelectionName = "Excel - Open Workbook";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            object excelObject;
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            if (sendingInstance.appInstances.TryGetValue(v_InstanceName, out excelObject))
            {
                Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                excelInstance.Workbooks.Open(v_FilePath);           
            }

        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Open from '" + v_FilePath + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to add a new Excel Workbook.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelAddWorkbookCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }

        public ExcelAddWorkbookCommand()
        {
            this.CommandName = "ExcelAddWorkbookCommand";
            this.SelectionName = "Excel - Add Workbook";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            object excelObject;
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            if (sendingInstance.appInstances.TryGetValue(v_InstanceName, out excelObject))
            {
                Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                excelInstance.Workbooks.Add();
            }


        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to move to a specific cell.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelGoToCellCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Cell Location (ex. A1 or B2)")]
        public string v_CellLocation { get; set; }
        public ExcelGoToCellCommand()
        {
            this.CommandName = "ExcelGoToCellCommand";
            this.SelectionName = "Excel - Go To Cell";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            object excelObject;
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            if (sendingInstance.appInstances.TryGetValue(v_InstanceName, out excelObject))
            {
                Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                Microsoft.Office.Interop.Excel.Worksheet excelSheet = excelInstance.ActiveSheet;
                excelSheet.Range[v_CellLocation].Select();
            }


        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Go To: '" + v_CellLocation + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to set the value of a specific cell.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelSetCellCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter text to set")]
        public string v_TextToSet { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Cell Location (ex. A1 or B2)")]
        public string v_ExcelCellAddress { get; set; }
        public ExcelSetCellCommand()
        {
            this.CommandName = "ExcelSetCellCommand";
            this.SelectionName = "Excel - Set Cell";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            object excelObject;
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            if (sendingInstance.appInstances.TryGetValue(v_InstanceName, out excelObject))
            {
                VariableCommand newVariableCommand = new VariableCommand();
                string excelRange = newVariableCommand.VariablizeString(sender, v_ExcelCellAddress);
                string textToSet = newVariableCommand.VariablizeString(sender, v_TextToSet);

                Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                Microsoft.Office.Interop.Excel.Worksheet excelSheet = excelInstance.ActiveSheet;
                excelSheet.Range[excelRange].Value = textToSet;
            }


        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Set Cell '" + v_ExcelCellAddress + "' to '" + v_TextToSet + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command gets text from a specified Excel Cell and assigns it to a variable.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Excel Interop' to achieve automation.")]
    public class ExcelGetCellCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Cell Location (ex. A1 or B2)")]
        public string v_ExcelCellAddress { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Assign to Variable")]
        public string v_userVariableName { get; set; }

        public ExcelGetCellCommand()
        {
            this.CommandName = "ExcelGetCellCommand";
            this.SelectionName = "Excel - Get Cell";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            object excelObject;
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            if (sendingInstance.appInstances.TryGetValue(v_InstanceName, out excelObject))
            {
                VariableCommand newVariableCommand = new VariableCommand();
                string excelRange = newVariableCommand.VariablizeString(sender, v_ExcelCellAddress);

                Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                Microsoft.Office.Interop.Excel.Worksheet excelSheet = excelInstance.ActiveSheet;
                var cellValue = excelSheet.Range[excelRange].Value;


                newVariableCommand.v_userVariableName = v_userVariableName;
                newVariableCommand.v_Input = cellValue.ToString();
                newVariableCommand.RunCommand(sender);

            }


        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Get Value From '" + v_ExcelCellAddress + "' and apply to variable '" + v_userVariableName + "', Instance Name: '" + v_InstanceName + "']";
        }


    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to run a macro in an Excel Workbook.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelRunMacroCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the macro name")]
        public string v_MacroName { get; set; }
        public ExcelRunMacroCommand()
        {
            this.CommandName = "ExcelAddWorkbookCommand";
            this.SelectionName = "Excel - Run Macro";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            object excelObject;
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            if (sendingInstance.appInstances.TryGetValue(v_InstanceName, out excelObject))
            {
                Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                excelInstance.Run(v_MacroName);
            }


        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to close Excel.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelCloseApplicationCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Indicate if the Workbook should be saved")]
        public bool v_ExcelSaveOnExit { get; set; }
        public ExcelCloseApplicationCommand()
        {
            this.CommandName = "ExcelCloseApplicationCommand";
            this.SelectionName = "Excel - Close Application";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            object excelObject;
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            if (sendingInstance.appInstances.TryGetValue(v_InstanceName, out excelObject))
            {
                Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                excelInstance.ActiveWorkbook.Close(v_ExcelSaveOnExit);
                excelInstance.Quit();
            }


        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Save On Close: " + v_ExcelSaveOnExit + ", Instance Name: '" + v_InstanceName + "']";
        }
    }
    
    #endregion
}



