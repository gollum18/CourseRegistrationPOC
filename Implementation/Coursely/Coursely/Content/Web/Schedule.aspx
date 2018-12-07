<%@ Page Title="Coursely - Schedule" Language="C#" MasterPageFile="~/MasterForm.Master" AutoEventWireup="true" CodeBehind="Schedule.aspx.cs" Inherits="Coursely.Content.Web.Schedule" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="server">
    <section><label for="SemesterDropDown">Semester: </label>
        <select id="SemesterDropDown" runat="server">
            <option value="Fall">Fall</option>
            <option value="Spring">Spring</option>
            <option value="Summer">Summer</option>
        </select>
        <label for="YearDropDown">Year: </label>
        <asp:DropDownList ID="YearDropDown" ClientIDMode="Static" runat="server"></asp:DropDownList>
        <asp:Button ID="ViewScheduleButton" ClientIDMode="Static" OnClick="OnViewScheduleClicked" Text="View Schedule" runat="server"/>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <section>
        <asp:Table ID="ScheduleView" CssClass="table table-bordered table-striped" ClientIDMode="Static" runat="server"></asp:Table>
        <asp:RadioButtonList ID="UnenrollSelector" ClientIDMode="Static" runat="server"></asp:RadioButtonList>
        <asp:Button Text="Unenroll" ID="UnenrollButton" Visible="false" ClientIDMode="Static" OnClick="OnUnenrollButtonClicked" runat="server" />
    </section>
    <section>
        <asp:Label ID="StatusLabel" Font-Bold="true" Font-Size="Large" ClientIDMode="Static" runat="server"></asp:Label>
    </section>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
