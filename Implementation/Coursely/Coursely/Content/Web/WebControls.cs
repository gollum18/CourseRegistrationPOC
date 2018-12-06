using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI.WebControls;

using Coursely.Content.Classes;
using Coursely.Content.Managers;

namespace Coursely.Content.Web
{
    public static class WebControls
    {
        //
        // CONSTANTS
        //

        // Constants used by the schedule table
        public static readonly string[] SCHEDULE_HEADERS = 
        {
            "Section", "Instructor(s)", "Room", "Dates", "Times", "Days", "Enrollment"
        };
        
        public static readonly char[] SCHEDULE_TRIM_CHARS =
        {
            ' ', ',', ';'
        };

        // Color constants used by the web controls
        public static readonly Color RED = Color.Red;
        public static readonly Color GREEN = Color.Green;
        public static readonly Color DARK_ORANGE = Color.DarkOrange;

        // Constants used by the various dropdown selectors
        public static readonly string DEFAULT_DROPDOWN_VALUE = "-1";
        public static readonly ListItem DEFAULT_LISTITEM_SECTION = new ListItem("-Section-", DEFAULT_DROPDOWN_VALUE);
        public static readonly ListItem DEFAULT_LISTITEM_SCHOOL = new ListItem("-School-", DEFAULT_DROPDOWN_VALUE);
        public static readonly ListItem DEFAULT_LISTITEM_DEPARTMENT = new ListItem("-Department-", DEFAULT_DROPDOWN_VALUE);
        public static readonly ListItem DEFAULT_LISTITEM_COURSE = new ListItem("-Course-", DEFAULT_DROPDOWN_VALUE);
        public static readonly ListItem DEFAULT_LISTITEM_INSTRUCTOR = new ListItem("-Instructor", DEFAULT_DROPDOWN_VALUE);

        // Constants used by the course management page
        public static readonly string[] DAYS = {
            "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
        };

        public static readonly string[] SEMESTERS = { "Fall", "Spring", "Summer" };

        // Fields
        private static int[] YEARS = null;

        //
        // PUBLIC METHODS
        //

        /// <summary>
        /// Creates the rows for a table display of a schedule.
        /// </summary>
        /// <param name="sections">The sections in the schedule.</param>
        /// <returns>A tuple where the first item is the table header, and the second item is a list
        /// of table rows.</returns>
        public static List<TableRow> CreateScheduleRows(List<Section> sections)
        {
            // Generate the header
            TableHeaderRow headerRow = new TableHeaderRow();
            foreach (var header in SCHEDULE_HEADERS)
            {
                headerRow.Cells.Add(new TableCell() { Text = header });
            }
            // Generate the rows
            List<TableRow> rows = new List<TableRow>
            {
                headerRow
            };
            try
            {
                foreach (var section in sections)
                {
                    TableRow row = new TableRow();
                    row.Cells.Add(new TableCell() { Text = section.ToString()});
                    string instructorsContent = "";
                    IEnumerator<string> instructors = section.GetInstructors();
                    User user = null;
                    while (instructors.MoveNext())
                    {
                        user = UserManager.InstanceOf().GetUser(instructors.Current);
                        instructorsContent += $"{user.LastName}, {user.FirstName}; ";
                    }
                    instructorsContent = instructorsContent.TrimEnd(SCHEDULE_TRIM_CHARS);
                    row.Cells.Add(new TableCell() { Text = instructorsContent });
                    row.Cells.Add(new TableCell()
                    {
                        Text =
                        $"{UniversityManager.InstanceOf().GetBuilding(section.BuildingID).Abbreviation}{section.Room}"
                    });
                    row.Cells.Add(new TableCell()
                    {
                        Text = $"{section.StartDateAndTime.ToShortDateString()} - " +
                        $"{section.EndDateAndTime.ToShortDateString()}"
                    });
                    row.Cells.Add(new TableCell()
                    {
                        Text = $"{section.StartDateAndTime.ToShortTimeString()} - " +
                        $"{section.EndDateAndTime.ToShortTimeString()}"
                    });
                    string days = "";
                    IEnumerator<string> enumerator = section.GetDays();
                    while (enumerator.MoveNext())
                    {
                        days += $"{enumerator.Current}, ";
                    }
                    days = days.TrimEnd(SCHEDULE_TRIM_CHARS);
                    row.Cells.Add(new TableCell() { Text = days });
                    row.Cells.Add(new TableCell() { Text = $"{section.CurrentEnrollment} / {section.MaxEnrollment}" });
                    rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rows;
        }

        /// <summary>
        /// Generates the table for a students grades.
        /// </summary>
        /// <param name="studentName">The students name formatted as "LastName, FirstName".</param>
        /// <param name="record">The students record.</param>
        /// <returns>A list of rows for a table that displays a students avademic record.</returns>
        public static List<TableRow> GenerateRecordRows(string studentName, List<string> record)
        {
            List<TableRow> rows = new List<TableRow>();
            TableHeaderRow header = new TableHeaderRow();
            header.Cells.Add(new TableHeaderCell() {
                Text = $"Record for: {studentName}",
                Scope = TableHeaderScope.Column
            });
            foreach (var grade in record)
            {
                TableRow row = new TableRow();
                row.Cells.Add(new TableCell()
                {
                    Text = grade
                });
                rows.Add(row);
            }
            return rows;
        }

        /// <summary>
        /// Gets all of the selected items from a list control.
        /// </summary>
        /// <param name="list">The control to retrieve the selected items for.</param>
        /// <returns>A list of ListItem objects.</returns>
        public static List<ListItem> GetSelectedItems(ListControl list)
        {
            if (list == null)
            {
                throw new Exception("Cannot get items for the indicated list! List is null.");
            }
            return list.Items.Cast<ListItem>()
                .Where(li => li.Selected)
                .ToList();
        }

        /// <summary>
        /// Gets the years [currentYear, currentYear + 2] for a dropdown control as a list of integers..
        /// </summary>
        /// <returns>An array of years.</returns>
        public static int[] GetYearsForDropDown()
        {
            // Lazily initialize years
            if (YEARS == null) {
                int currentYear = DateTime.Now.Year;
                YEARS = new int[] {
                    currentYear, currentYear + 1, currentYear + 2
                };
            }
            // Return it as it's content is likely not to change
            return YEARS;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int MinYear() => YEARS.First();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int MaxYear() => YEARS.Last();

        /// <summary>
        /// Resets a DropDown control to it's default item.
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="defaultListItem"></param>
        public static void ResetSelector(DropDownList selector, ListItem defaultListItem)
        {
            selector.Items.Clear();
            selector.Items.Add(defaultListItem);
        }

        /// <summary>
        /// Sets a label control to the indicated color and message.
        /// </summary>
        /// <param name="label">The label control.</param>
        /// <param name="color">The color to set to.</param>
        /// <param name="message">THe message to set to.</param>
        public static void SetLabel(Label label, Color color, string message)
        {
            label.ForeColor = color;
            label.Text = message;
        }
    }
}