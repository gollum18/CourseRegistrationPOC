<%@ Page Title="Coursely - Schedule" Language="C#" MasterPageFile="~/MasterForm.Master" AutoEventWireup="true" CodeBehind="Schedule.aspx.cs" Inherits="Coursely.Content.Web.Schedule" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="server">
    <div class="container">
        <div class="row">
            <div class="col">
                <label for="SemesterDropDown" class="col-form-label">Semester: </label>
                <select id="SemesterDropDown" class="form-control" runat="server">
                    <option value="Fall">Fall</option>
                    <option value="Spring">Spring</option>
                    <option value="Summer">Summer</option>
                </select>
            </div>
            <div class="col">
                <label for="YearDropDown" class="col-form-label">Year: </label>
                <asp:DropDownList ID="YearDropDown" CssClass="form-control" ClientIDMode="Static" runat="server"></asp:DropDownList>
            </div>
        </div>
        <div class="row mt-2 mb-2">
            <div class="col justify-content-center">
                <asp:Button ID="ViewScheduleButton" CssClass="btn btn-primary col-3" ClientIDMode="Static" OnClick="OnViewScheduleClicked" Text="View Schedule" runat="server"/>
            </div>
        </div>
        
    </div>
    <section>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container">
        <div class="row">
            <div class="col">
                <asp:Table ID="ScheduleView" CssClass="table table-bordered table-striped" ClientIDMode="Static" runat="server"></asp:Table>
            </div>
        </div>
        <div class="row">
            <div class="col">
                <asp:RadioButtonList ID="UnenrollSelector" CssClass="form-control" RepeatDirection="Horizontal" ClientIDMode="Static" runat="server"></asp:RadioButtonList>
            </div>
        </div>
        <div class="row mt-2 mb-2">
            <div class="col justify-content-center">
                <asp:Button Text="Unenroll" CssClass="btn btn-danger col-3" ID="UnenrollButton" Visible="false" ClientIDMode="Static" OnClick="OnUnenrollButtonClicked" runat="server" />
            </div>
        </div>
        <div class="row">
            <div class="col">
                <asp:Label ID="StatusLabel" CssClass="alert" Font-Bold="true" Font-Size="Large" ClientIDMode="Static" runat="server"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
