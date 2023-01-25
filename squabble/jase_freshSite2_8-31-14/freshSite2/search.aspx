<%@ Page Title="Search" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="search.aspx.cs" Inherits="freshSite2.search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <h2><%: Title %>.</h2>
    <div class="col-lg-12">
    <div class="row">
        
        <div class="col-md-4">
            <section id="searchForm">
                <div class="form-horizontal">
                    <h4>Let's find your book</h4>
                    <hr />
                      <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="FailureText" />
                        </p>
                    </asp:PlaceHolder>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="UniversityList" CssClass="col-md-2 control-label">University</asp:Label>
                        <div class="col-md-10">
                           <asp:DropDownList ID="UniversityList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="UniversityList_SelectedIndexChanged" Width="100%"></asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="UniversityList"
                                CssClass="text-danger" ErrorMessage="You must select a university" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="TermList" CssClass="col-md-2 control-label">Term</asp:Label>
                        <div class="col-md-10">
                           <asp:DropDownList ID="TermList" runat="server" OnSelectedIndexChanged="TermList_SelectedIndexChanged" Enabled="False" AutoPostBack="True" Width="100%"></asp:DropDownList>
                            
                        </div>
                    </div>
                     <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="DepartmentList" CssClass="col-md-2 control-label">Department</asp:Label>
                        <div class="col-md-10">
                           <asp:DropDownList ID="DepartmentList" runat="server" OnSelectedIndexChanged="DepartmentList_SelectedIndexChanged" AutoPostBack="True" Enabled="False" Width="100%"></asp:DropDownList>
                            
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="CourseList" CssClass="col-md-2 control-label">Course</asp:Label>
                        <div class="col-md-10">
                           <asp:DropDownList ID="CourseList" runat="server" OnSelectedIndexChanged="CourseList_SelectedIndexChanged" Enabled="False" AutoPostBack="True" Width="100%"></asp:DropDownList>

                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="SectionList" CssClass="col-md-2 control-label">Section</asp:Label>
                        <div class="col-md-10">
                           <asp:DropDownList ID="SectionList" runat="server" OnSelectedIndexChanged="SectionList_SelectedIndexChanged" AutoPostBack="True" Enabled="False" Width="100%"></asp:DropDownList>

                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="BookList" CssClass="col-md-2 control-label">Book</asp:Label>
                        <div class="col-md-10">
                           <asp:DropDownList ID="BookList" runat="server" OnSelectedIndexChanged="BookList_SelectedIndexChanged" Enabled="False" Width="100%"></asp:DropDownList>

                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
     
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <asp:Button runat="server" OnClick="doNext" Text="Next" CssClass="btn btn-default" />
                        </div>
                    </div>
                </div>
             
            </section>
        </div>

        <div class="col-md-8">
            <section id="searchResult">
                
                <div class="form-horizontal">
                     <asp:Panel runat="server" ID="pnlResults"></asp:Panel>
                </div>
             
            </section>
        </div>
    </div>
    </div>

    




</asp:Content>
