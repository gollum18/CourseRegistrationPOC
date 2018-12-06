using System;
using System.Web.UI.WebControls;

using Coursely.Content.Classes;
using Coursely.Content.Managers;

namespace Coursely.Content.Web
{
    public partial class UniversityAdmin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // User must be logged in to see this page
            if (Session["UnivID"] == null || Session["Role"] == null ||
                !UserManager.InstanceOf().IsUserAuthenticated(Session["UnivID"].ToString()))
            {
                Response.Redirect("~/Content/Web/Login.aspx");
            }
            
            // The user must be an administrator
            if (!Session["Role"].ToString().Equals(Classes.User.ADMINISTRATOR))
            {
                Response.Redirect("~/Content/Web/Home.aspx");
            }

            // Setup page information
            Label title = Master.FindControl("LblTitle") as Label;
            title.Text = "Coursely - Admin - Manage Users";

            LinkButton homelink = Master.FindControl("HomeLink") as LinkButton;
            homelink.Text = "Home";
            homelink.PostBackUrl = "~/Content/Web/Home.aspx";

            LinkButton acctlink = Master.FindControl("AccountLink") as LinkButton;
            acctlink.Visible = true;

            LinkButton loglink = Master.FindControl("LogLink") as LinkButton;
            loglink.Text = "Logout";
            loglink.Click += OnLogButtonClicked;

            // Load data into the selector controls
            if (BuildingSelector.Items.Count == 1)
            {
                try
                {
                    foreach (var building in UniversityManager.InstanceOf().GetBuildings())
                    {
                        BuildingSelector.Items.Add(new ListItem(building.Name, building.BuildingID.ToString()));
                    }
                }
                catch (Exception ex)
                {
                    WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
                }
            }

            if (SchoolSelector.Items.Count == 1)
            {
                try
                {
                    foreach (var school in UniversityManager.InstanceOf().GetSchools())
                    {
                        foreach (var department in UniversityManager.InstanceOf().GetDepartments(school.SchoolID))
                        {
                            foreach (var major in UniversityManager.InstanceOf().GetMajors(department.DepartmentID))
                            {
                                MajorSelector.Items.Add(new ListItem(major.Name, major.MajorID.ToString()));
                            }
                            DepartmentSelector.Items.Add(new ListItem(department.Name, department.DepartmentID.ToString()));
                        }
                        SchoolSelector.Items.Add(new ListItem(school.Name, school.SchoolID.ToString()));
                    }
                }
                catch (Exception ex)
                {
                    WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
                }
            }
        }

        protected void OnLogButtonClicked(object sender, EventArgs e)
        {
            UserManager.InstanceOf().Logout(Session["UnivID"].ToString());
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Content/Web/Login.aspx");
        }

        protected void OnBuildingChanged(object sender, EventArgs e)
        {
            if (BuildingSelector.SelectedValue.Equals(WebControls.DEFAULT_DROPDOWN_VALUE))
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, "Error: No building selected!");
                return;
            }
            try
            {
                Building building = UniversityManager.InstanceOf().GetBuilding(
                    int.Parse(BuildingSelector.SelectedValue));
                BuildingName.Text = building.Name;
                BuildingAbbreviation.Text = building.Abbreviation;
                if (!StatusLabel.Text.Equals(""))
                {
                    StatusLabel.Text = "";
                }
            }
            catch (Exception ex)
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
            }
        }

        protected void OnDepartmentChanged(object sender, EventArgs e)
        {
            if (DepartmentSelector.SelectedValue.Equals(WebControls.DEFAULT_DROPDOWN_VALUE))
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, "Error: No department selected!");
                return;
            }
            try
            {
                Department department = UniversityManager.InstanceOf().GetDepartment(
                    int.Parse(DepartmentSelector.SelectedValue));
                DepartmentName.Text = department.Name;
                DepartmentAbbreviation.Text = department.Abbreviation;
                if (!StatusLabel.Text.Equals(""))
                {
                    StatusLabel.Text = "";
                }
            }
            catch (Exception ex)
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
            }
        }

        protected void OnMajorChanged(object sender, EventArgs e)
        {
            if (MajorSelector.SelectedValue.Equals(WebControls.DEFAULT_DROPDOWN_VALUE))
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, "Error: No major selected!");
                return;
            }
            try
            {
                MajorName.Text = UniversityManager.InstanceOf().GetMajor(
                    int.Parse(MajorSelector.SelectedValue)).Name;
                if (!StatusLabel.Text.Equals(""))
                {
                    StatusLabel.Text = "";
                }
            }
            catch (Exception ex)
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
            }
        }

        protected void OnSchoolChanged(object sender, EventArgs e)
        {
            if (SchoolSelector.SelectedValue.Equals(WebControls.DEFAULT_DROPDOWN_VALUE))
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, "Error: No school selected!");
                return;
            }
            try
            {
                School school = UniversityManager.InstanceOf().GetSchool(
                    int.Parse(SchoolSelector.SelectedValue));
                SchoolName.Text = school.Name;
                SchoolAbbreviation.Text = school.Abbreviation;
                if (!StatusLabel.Text.Equals(""))
                {
                    StatusLabel.Text = "";
                }
            }
            catch (Exception ex)
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
            }
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            switch (eventArgument)
            {
                case "WindowClosed":
                    OnLogButtonClicked("PageClosedEvent", null);
                    break;
                case "CreateBuilding":
                    ModifyBuilding(true);
                    break;
                case "ModifyBuilding":
                    ModifyBuilding();
                    break;
                case "CreateDepartment":
                    ModifyDepartment(true);
                    break;
                case "ModifyDepartment":
                    ModifyDepartment();
                    break;
                case "CreateMajor":
                    ModifyMajor(true);
                    break;
                case "ModifyMajor":
                    ModifyMajor();
                    break;
                case "CreateSchool":
                    ModifySchool(true);
                    break;
                case "ModifySchool":
                    ModifySchool();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Creates/modifies a building.
        /// </summary>
        /// <param name="create">Whether we are creating a building or modifying a building.
        /// The default is false (modifying a building).</param>
        protected void ModifyBuilding(bool create = false)
        {
            try
            {
                if (ValidateBuilding()) {
                    if (create)
                    {
                        if (!UniversityManager.InstanceOf().AddBuilding(BuildingName.Text, 
                                BuildingAbbreviation.Text))
                        {
                            WebControls.SetLabel(StatusLabel, WebControls.RED, "There was an issue creating the building.");
                        }
                        else
                        {
                            BuildingSelector.Items.Add(new ListItem(BuildingName.Text, 
                                UniversityManager.InstanceOf().GetBuildingID(BuildingName.Text).ToString()));
                            WebControls.SetLabel(StatusLabel, WebControls.GREEN, "Building created successfully.");
                        }
                    }
                    else
                    {
                        if (Validation.ValidateBuilding(BuildingSelector.SelectedValue, out string error))
                        {
                            if (!UniversityManager.InstanceOf().ModifyBuilding(int.Parse(
                                BuildingSelector.SelectedValue), BuildingName.Text, BuildingAbbreviation.Text))
                            {
                                WebControls.SetLabel(StatusLabel, WebControls.RED, "There was an issue modifying the building.");
                            }
                            else
                            {
                                WebControls.SetLabel(StatusLabel, WebControls.GREEN, "Building was modified successfully.");
                            }
                        }
                        else
                        {
                            WebControls.SetLabel(StatusLabel, WebControls.RED, error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
            }
        }

        /// <summary>
        /// Creates/modifies a department.
        /// </summary>
        /// <param name="create">Whether we are creating a department or modifying a department.
        /// The default is false (modifying a department).</param>
        protected void ModifyDepartment(bool create = false)
        {
            try
            {
                if (ValidateDepartment()) {
                    if (create)
                    {
                        if (Validation.ValidateSchool(SchoolSelector.SelectedValue, out string error)) {
                            if (!UniversityManager.InstanceOf().AddDepartment(int.Parse(
                                    SchoolSelector.SelectedValue), 
                                    DepartmentName.Text, DepartmentAbbreviation.Text))
                            {
                                WebControls.SetLabel(StatusLabel, WebControls.RED, "There was an issue creating the department.");
                            }
                            else
                            {
                                DepartmentSelector.Items.Add(new ListItem(DepartmentName.Text, 
                                    UniversityManager.InstanceOf().GetDepartmentID(DepartmentName.Text).ToString()));
                                WebControls.SetLabel(StatusLabel, WebControls.GREEN, "Department created successfully.");
                            }
                        }
                        else
                        {
                            WebControls.SetLabel(StatusLabel, WebControls.RED, error);
                        }
                    }
                    else
                    {
                        if (Validation.ValidateDepartment(DepartmentSelector.SelectedValue, out string error) &&
                            Validation.ValidateSchool(SchoolSelector.SelectedValue, out error)) {
                            if (!UniversityManager.InstanceOf().ModifyDepartment(int.Parse(
                                    DepartmentSelector.SelectedValue), int.Parse(SchoolSelector.SelectedValue), 
                                    DepartmentAbbreviation.Text))
                            {
                                WebControls.SetLabel(StatusLabel, WebControls.RED, "There was an issue modifying the specified department.");
                            }
                            else
                            {
                                WebControls.SetLabel(StatusLabel, WebControls.GREEN, "Department modified successfully.");
                            }
                        }
                        else
                        {
                            WebControls.SetLabel(StatusLabel, WebControls.RED, error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
            }
        }

        /// <summary>
        /// Creates/modifies a major.
        /// </summary>
        /// <param name="create">Whether we are creating a major or modifying a major.
        /// The default is false (modifying a major).</param>
        protected void ModifyMajor(bool create = false)
        {
            try
            {
                if (ValidateMajor())
                {
                    if (create)
                    {
                        if (Validation.ValidateDepartment(DepartmentSelector.SelectedValue, out string error)) {
                            if (!UniversityManager.InstanceOf().AddMajor(int.Parse(
                                DepartmentSelector.SelectedValue), MajorName.Text))
                            {
                                WebControls.SetLabel(StatusLabel, WebControls.RED, "There was an error creating the major.");
                            }
                            else
                            {
                                MajorSelector.Items.Add(new ListItem(MajorName.Text, 
                                    UniversityManager.InstanceOf().GetMajorID(MajorName.Text).ToString()));
                                WebControls.SetLabel(StatusLabel, WebControls.GREEN, "Major created successfully.");
                            }
                        }
                        else
                        {
                            WebControls.SetLabel(StatusLabel, WebControls.RED, error);
                        }
                    }
                    else
                    {
                        if (Validation.ValidateMajor(MajorSelector.SelectedValue, out string error) && 
                                Validation.ValidateDepartment(DepartmentSelector.SelectedValue, out error))
                        {
                            if (!UniversityManager.InstanceOf().ModifyMajor(int.Parse(
                                    MajorSelector.SelectedValue), int.Parse(
                                        DepartmentSelector.SelectedValue)))
                            {
                                WebControls.SetLabel(StatusLabel, WebControls.RED, "There was an error modifying the major.");
                            }
                            else
                            {
                                WebControls.SetLabel(StatusLabel, WebControls.GREEN, "Major modified successfully.");
                            }
                        }
                        else
                        {
                            WebControls.SetLabel(StatusLabel, WebControls.RED, error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
            }
        }

        /// <summary>
        /// Creates/modifies a school.
        /// </summary>
        /// <param name="create">Whether we are creating a school or modifying a school.
        /// The default is false (modifying a school).</param>
        protected void ModifySchool(bool create = false)
        {
            try
            {
                if (ValidateSchool()) {
                    if (create)
                    {
                        if (!UniversityManager.InstanceOf().AddSchool(SchoolName.Text, SchoolAbbreviation.Text))
                        {
                            WebControls.SetLabel(StatusLabel, WebControls.RED, "There was an issue creating the school, please try again.");
                        }
                        else
                        {
                            SchoolSelector.Items.Add(new ListItem(SchoolName.Text, 
                                UniversityManager.InstanceOf().GetSchoolID(SchoolName.Text).ToString()));
                            WebControls.SetLabel(StatusLabel, WebControls.GREEN, "School created successfully.");
                        }
                    }
                    else
                    {
                        if (Validation.ValidateSchool(SchoolSelector.SelectedValue, out string error)) {
                            if (!UniversityManager.InstanceOf().ModifySchool(int.Parse(SchoolSelector.SelectedValue), SchoolAbbreviation.Text))
                            {
                                WebControls.SetLabel(StatusLabel, WebControls.RED, "There was an issue modifying the school, please try again.");
                            }
                            else
                            {
                                WebControls.SetLabel(StatusLabel, WebControls.GREEN, "School modified succssfully.");
                            }
                        }
                        else
                        {
                            WebControls.SetLabel(StatusLabel, WebControls.RED, error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, ex.Message);
            }
        }

        protected bool ValidateBuilding()
        {
            // Validate the name and abbreviation
            if (!Validation.ValidateBuildingName(BuildingName.Text, out string error))
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, error);
                return false;
            }
            if (!Validation.ValidateBuildingAbbreviation(BuildingAbbreviation.Text, out error))
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, error);
                return false;
            }
            return true;
        }

        protected bool ValidateDepartment()
        {
            // Validate the school, department name, and abbreviation
            if (!Validation.ValidateSchool(SchoolSelector.SelectedValue, out string error))
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, error);
                return false;
            }
            if (!Validation.ValidateDepartmentName(DepartmentName.Text, out error))
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, error);
                return false;
            }
            if (!Validation.ValidateDepartmentAbbreviation(DepartmentAbbreviation.Text, out error))
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, error);
                return false;
            }
            return true;
        }

        protected bool ValidateMajor()
        {
            // Validates the department, and major name
            if (!Validation.ValidateMajorName(MajorName.Text, out string error))
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, error);
                return false;
            }
            return true;
        }

        protected bool ValidateSchool()
        {
            if (!Validation.ValidateSchoolName(SchoolName.Text, out string error))
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, error);
                return false;
            }
            if (!Validation.ValidateSchoolAbbreviation(SchoolAbbreviation.Text, out error))
            {
                WebControls.SetLabel(StatusLabel, WebControls.RED, error);
                return false;
            }
            return true;
        }
    }
}