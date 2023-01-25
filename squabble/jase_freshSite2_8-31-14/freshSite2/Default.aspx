<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="freshSite2._Default" %>
<%@ MasterType virtualpath="~/Site.Master" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">



    <div class="jumbotron">
   
    <div class="row">
        <div class="col-md-4">
            <a href="Search/SearchBooks">
                <img src="Content/images/squabbleBook_transparent.png" style="width: 273px; height: 276px; display: block; margin: 0 auto;" />
            </a>
        </div>
        <div class="col-md-4">
            <a href="Search/SearchBooks">
                <img src="Content/images/SquabbleLogo_noText.png" style="width: 291px; height: 218px; display: block; margin: 0 auto;" />
            </a>
        </div>
        <div class="col-md-4">
            <a href="Search/SearchBooks">
                <img src="Content/images/thumbsUp.png" style="width: 276px; height: 262px; display: block; margin: 0 auto;" />
            </a>
        </div>
    </div>
         <div class="row">
        <div class="col-md-4">
            <a href="Search/SearchBooks">
                <p class="text-center">BUY</p>
            </a>
        </div>
        <div class="col-md-4">
            <a href="Search/SearchBooks">
                 <p class="text-center">SQUABBLE</p>
            </a>
        </div>
        <div class="col-md-4">
            <a href="Search/SearchBooks">
                  <p class="text-center">SELL</p>
            </a>
        </div>
    </div>
    </div>

    <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />

 
</asp:Content>

       

